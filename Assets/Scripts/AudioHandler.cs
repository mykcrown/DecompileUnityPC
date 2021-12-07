// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : IAudioHandler, ITickable
{
	private struct SfxEvent
	{
		public AudioRequest sound
		{
			get;
			private set;
		}

		public int sourceId
		{
			get;
			private set;
		}

		public bool play
		{
			get;
			private set;
		}

		public float fadeTime
		{
			get;
			private set;
		}

		public IAudioOwner attachTo
		{
			get;
			private set;
		}

		public SfxEvent(AudioRequest sound, int sourceId, bool play, IAudioOwner attachTo, float fadeTime = 0f)
		{
			this.sound = sound;
			this.sourceId = sourceId;
			this.play = play;
			this.fadeTime = fadeTime;
			this.attachTo = attachTo;
		}
	}

	private GameObject parentObject;

	private List<AudioHandler.SfxEvent>[] delayedSfxBuffer = new List<AudioHandler.SfxEvent>[RollbackStatePoolContainer.ROLLBACK_FRAMES];

	private List<AudioHandler.SfxEvent>[] delayedLoopingSfxBuffer = new List<AudioHandler.SfxEvent>[RollbackStatePoolContainer.ROLLBACK_FRAMES];

	private int latestConfirmedFrame;

	[Inject]
	public IRollbackStatus rollbackStatus
	{
		get;
		set;
	}

	[Inject]
	public IAudioPlayer AudioPlayer
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public ConfigData configData
	{
		get;
		set;
	}

	private IFrameOwner frameOwner
	{
		get
		{
			return (!(this.gameController.currentGame == null)) ? this.gameController.currentGame : null;
		}
	}

	private bool gameStarted
	{
		get
		{
			return !(this.gameController.currentGame == null) && this.gameController.currentGame.StartedGame;
		}
	}

	private int currentFrame
	{
		get
		{
			return (this.frameOwner != null) ? this.frameOwner.Frame : (-1);
		}
	}

	private int bufferIndex
	{
		get
		{
			return (this.frameOwner != null) ? this.getBufferIndex(this.frameOwner.Frame) : (-1);
		}
	}

	private bool canPlayAudioForFrame(int frame)
	{
		return !this.rollbackStatus.RollbackEnabled || frame <= this.rollbackStatus.FullyConfirmedFrame || !this.gameStarted;
	}

	private int getBufferIndex(int frame)
	{
		return frame % RollbackStatePoolContainer.ROLLBACK_FRAMES;
	}

	public void Init(ConfigData config, GameObject parentObject)
	{
		(this.AudioPlayer as AudioPlayer).Init(parentObject);
		for (int i = 0; i < RollbackStatePoolContainer.ROLLBACK_FRAMES; i++)
		{
			this.delayedSfxBuffer[i] = new List<AudioHandler.SfxEvent>();
			this.delayedLoopingSfxBuffer[i] = new List<AudioHandler.SfxEvent>();
		}
	}

	public void Destroy()
	{
		(this.AudioPlayer as AudioPlayer).Destroy();
	}

	private void stopAllBufferedSounds(List<AudioHandler.SfxEvent> sfxBuffer)
	{
		foreach (AudioHandler.SfxEvent current in sfxBuffer)
		{
			if (current.play)
			{
				this.AudioPlayer.StopSound(new AudioReference(current.attachTo, current.sourceId), 0f);
			}
		}
		sfxBuffer.Clear();
	}

	void ITickable.TickFrame()
	{
		if (this.rollbackStatus.RollbackEnabled)
		{
			if (this.rollbackStatus.IsRollingBack)
			{
				this.stopAllBufferedSounds(this.delayedSfxBuffer[this.bufferIndex]);
				this.stopAllBufferedSounds(this.delayedLoopingSfxBuffer[this.bufferIndex]);
			}
			else
			{
				this.PlaySoundsUntilFrame(this.rollbackStatus.FullyConfirmedFrame);
			}
		}
	}

	private void PlaySoundsUntilFrame(int frame)
	{
		for (int i = this.latestConfirmedFrame + 1; i <= frame; i++)
		{
			int bufferIndex = this.getBufferIndex(i);
			if (this.canPlayAudioForFrame(i))
			{
				foreach (AudioHandler.SfxEvent current in this.delayedSfxBuffer[bufferIndex])
				{
					if (current.play)
					{
						this.AudioPlayer.PlayGameSound(current.sound, current.sourceId, current.attachTo);
					}
					else
					{
						this.AudioPlayer.StopSound(new AudioReference(current.attachTo, current.sourceId), current.fadeTime);
					}
				}
				this.delayedSfxBuffer[bufferIndex].Clear();
				foreach (AudioHandler.SfxEvent current2 in this.delayedLoopingSfxBuffer[bufferIndex])
				{
					if (current2.play)
					{
						this.AudioPlayer.PlayLoopingSound(current2.sound, current2.sourceId);
					}
					else
					{
						this.AudioPlayer.StopSound(new AudioReference(current2.attachTo, current2.sourceId), current2.fadeTime);
					}
				}
				this.delayedLoopingSfxBuffer[bufferIndex].Clear();
			}
		}
		this.latestConfirmedFrame = frame;
	}

	void IAudioHandler.PlayMusic(AudioData music)
	{
		this.AudioPlayer.PlayMusic(music.sound, music.volume);
	}

	void IAudioHandler.StopMusic(Action callback, float fadeTime)
	{
		this.AudioPlayer.StopMusic(callback, fadeTime);
	}

	void IAudioHandler.OnGameDestroyed(int frame)
	{
		this.PlaySoundsUntilFrame(frame);
		for (int i = 0; i < RollbackStatePoolContainer.ROLLBACK_FRAMES; i++)
		{
			if (this.delayedSfxBuffer[i].Count > 0)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					this.latestConfirmedFrame,
					"+",
					i,
					":Delayed sfx buffer isn't empty"
				}));
				foreach (AudioHandler.SfxEvent current in this.delayedSfxBuffer[i])
				{
					UnityEngine.Debug.Log("Unplayed sound " + current.sound);
				}
			}
			this.delayedSfxBuffer[i].Clear();
			if (this.delayedLoopingSfxBuffer[i].Count > 0)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					this.latestConfirmedFrame,
					"+",
					i,
					":Delayed looping sfx buffer isn't empty"
				}));
				foreach (AudioHandler.SfxEvent current2 in this.delayedLoopingSfxBuffer[i])
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Unplayed sound ",
						current2.sound,
						".",
						current2.play,
						".",
						current2.sourceId
					}));
				}
			}
			this.delayedLoopingSfxBuffer[i].Clear();
		}
	}

	private AudioReference playGameSound(AudioRequest request)
	{
		if (request.sound == null)
		{
			return new AudioReference(null, -1);
		}
		if (!request.maintainPitch && !request.ignorePitchVariation)
		{
			float num = 1f + (UnityEngine.Random.value - 0.5f) * this.configData.soundSettings.randomPitchVariation * 2f;
			request.pitch *= num;
		}
		int audioSourceId;
		if (request.usePoint)
		{
			request.attachTo = this.gameController.currentGame.PointAudio.GetOwner();
			audioSourceId = this.gameController.currentGame.PointAudio.GetAudioSourceId(request.point);
		}
		else
		{
			audioSourceId = this.AudioPlayer.GetAudioSourceId(request.attachTo);
		}
		if (this.canPlayAudioForFrame(this.currentFrame))
		{
			this.AudioPlayer.PlayGameSound(request, audioSourceId, request.attachTo);
		}
		else
		{
			this.delayedSfxBuffer[this.bufferIndex].Add(new AudioHandler.SfxEvent(request, audioSourceId, true, request.attachTo, 0f));
		}
		return new AudioReference(request.attachTo, audioSourceId);
	}

	AudioReference IAudioHandler.PlayGameSound(AudioRequest request)
	{
		return this.playGameSound(request);
	}

	void IAudioHandler.PlayMenuSound(AudioRequest request, float delay)
	{
		this.AudioPlayer.PlayMenuSound(request.sound, request.volume, request.pitch, delay);
	}

	public AudioReference PlayLoopingSound(AudioRequest sound)
	{
		int audioSourceId = this.AudioPlayer.GetAudioSourceId(sound.attachTo);
		if (this.canPlayAudioForFrame(this.currentFrame))
		{
			this.AudioPlayer.PlayLoopingSound(sound, audioSourceId);
		}
		else
		{
			this.delayedLoopingSfxBuffer[this.bufferIndex].Add(new AudioHandler.SfxEvent(sound, audioSourceId, true, sound.attachTo, 0f));
		}
		return new AudioReference(sound.attachTo, audioSourceId);
	}

	AudioReference IAudioHandler.PlayLoopingSound(AudioRequest loopingSound)
	{
		return this.PlayLoopingSound(loopingSound);
	}

	void IAudioHandler.StopSound(AudioReference audioRef, float fadeTime)
	{
		if (this.canPlayAudioForFrame(this.currentFrame))
		{
			this.AudioPlayer.StopSound(audioRef, fadeTime);
		}
		else
		{
			this.delayedLoopingSfxBuffer[this.bufferIndex].Add(new AudioHandler.SfxEvent(default(AudioRequest), audioRef.sourceId, false, audioRef.owner, fadeTime));
		}
	}

	void IAudioHandler.PauseSounds(SoundType type, bool paused)
	{
		this.AudioPlayer.PauseSounds(type, paused);
	}

	void IAudioHandler.UpdateVolume()
	{
		this.AudioPlayer.UpdateVolume();
	}

	void IAudioHandler.Register(IAudioOwner owner)
	{
		this.AudioPlayer.Register(owner);
	}

	void IAudioHandler.Unregister(IAudioOwner owner)
	{
		this.AudioPlayer.Unregister(owner);
	}
}
