using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002A7 RID: 679
public class AudioHandler : IAudioHandler, ITickable
{
	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00059D1B File Offset: 0x0005811B
	// (set) Token: 0x06000E8E RID: 3726 RVA: 0x00059D23 File Offset: 0x00058123
	[Inject]
	public IRollbackStatus rollbackStatus { get; set; }

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000E8F RID: 3727 RVA: 0x00059D2C File Offset: 0x0005812C
	// (set) Token: 0x06000E90 RID: 3728 RVA: 0x00059D34 File Offset: 0x00058134
	[Inject]
	public IAudioPlayer AudioPlayer { get; set; }

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000E91 RID: 3729 RVA: 0x00059D3D File Offset: 0x0005813D
	// (set) Token: 0x06000E92 RID: 3730 RVA: 0x00059D45 File Offset: 0x00058145
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000E93 RID: 3731 RVA: 0x00059D4E File Offset: 0x0005814E
	// (set) Token: 0x06000E94 RID: 3732 RVA: 0x00059D56 File Offset: 0x00058156
	[Inject]
	public ConfigData configData { get; set; }

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000E95 RID: 3733 RVA: 0x00059D5F File Offset: 0x0005815F
	private IFrameOwner frameOwner
	{
		get
		{
			return (!(this.gameController.currentGame == null)) ? this.gameController.currentGame : null;
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000E96 RID: 3734 RVA: 0x00059D88 File Offset: 0x00058188
	private bool gameStarted
	{
		get
		{
			return !(this.gameController.currentGame == null) && this.gameController.currentGame.StartedGame;
		}
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x00059DB6 File Offset: 0x000581B6
	private bool canPlayAudioForFrame(int frame)
	{
		return !this.rollbackStatus.RollbackEnabled || frame <= this.rollbackStatus.FullyConfirmedFrame || !this.gameStarted;
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000E98 RID: 3736 RVA: 0x00059DE5 File Offset: 0x000581E5
	private int currentFrame
	{
		get
		{
			return (this.frameOwner != null) ? this.frameOwner.Frame : -1;
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000E99 RID: 3737 RVA: 0x00059E03 File Offset: 0x00058203
	private int bufferIndex
	{
		get
		{
			return (this.frameOwner != null) ? this.getBufferIndex(this.frameOwner.Frame) : -1;
		}
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x00059E27 File Offset: 0x00058227
	private int getBufferIndex(int frame)
	{
		return frame % RollbackStatePoolContainer.ROLLBACK_FRAMES;
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x00059E30 File Offset: 0x00058230
	public void Init(ConfigData config, GameObject parentObject)
	{
		(this.AudioPlayer as AudioPlayer).Init(parentObject);
		for (int i = 0; i < RollbackStatePoolContainer.ROLLBACK_FRAMES; i++)
		{
			this.delayedSfxBuffer[i] = new List<AudioHandler.SfxEvent>();
			this.delayedLoopingSfxBuffer[i] = new List<AudioHandler.SfxEvent>();
		}
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x00059E7E File Offset: 0x0005827E
	public void Destroy()
	{
		(this.AudioPlayer as AudioPlayer).Destroy();
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x00059E90 File Offset: 0x00058290
	private void stopAllBufferedSounds(List<AudioHandler.SfxEvent> sfxBuffer)
	{
		foreach (AudioHandler.SfxEvent sfxEvent in sfxBuffer)
		{
			if (sfxEvent.play)
			{
				this.AudioPlayer.StopSound(new AudioReference(sfxEvent.attachTo, sfxEvent.sourceId), 0f);
			}
		}
		sfxBuffer.Clear();
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x00059F18 File Offset: 0x00058318
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

	// Token: 0x06000E9F RID: 3743 RVA: 0x00059F84 File Offset: 0x00058384
	private void PlaySoundsUntilFrame(int frame)
	{
		for (int i = this.latestConfirmedFrame + 1; i <= frame; i++)
		{
			int bufferIndex = this.getBufferIndex(i);
			if (this.canPlayAudioForFrame(i))
			{
				foreach (AudioHandler.SfxEvent sfxEvent in this.delayedSfxBuffer[bufferIndex])
				{
					if (sfxEvent.play)
					{
						this.AudioPlayer.PlayGameSound(sfxEvent.sound, sfxEvent.sourceId, sfxEvent.attachTo);
					}
					else
					{
						this.AudioPlayer.StopSound(new AudioReference(sfxEvent.attachTo, sfxEvent.sourceId), sfxEvent.fadeTime);
					}
				}
				this.delayedSfxBuffer[bufferIndex].Clear();
				foreach (AudioHandler.SfxEvent sfxEvent2 in this.delayedLoopingSfxBuffer[bufferIndex])
				{
					if (sfxEvent2.play)
					{
						this.AudioPlayer.PlayLoopingSound(sfxEvent2.sound, sfxEvent2.sourceId);
					}
					else
					{
						this.AudioPlayer.StopSound(new AudioReference(sfxEvent2.attachTo, sfxEvent2.sourceId), sfxEvent2.fadeTime);
					}
				}
				this.delayedLoopingSfxBuffer[bufferIndex].Clear();
			}
		}
		this.latestConfirmedFrame = frame;
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x0005A118 File Offset: 0x00058518
	void IAudioHandler.PlayMusic(AudioData music)
	{
		this.AudioPlayer.PlayMusic(music.sound, music.volume);
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0005A133 File Offset: 0x00058533
	void IAudioHandler.StopMusic(Action callback, float fadeTime)
	{
		this.AudioPlayer.StopMusic(callback, fadeTime);
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x0005A144 File Offset: 0x00058544
	void IAudioHandler.OnGameDestroyed(int frame)
	{
		this.PlaySoundsUntilFrame(frame);
		for (int i = 0; i < RollbackStatePoolContainer.ROLLBACK_FRAMES; i++)
		{
			if (this.delayedSfxBuffer[i].Count > 0)
			{
				Debug.LogError(string.Concat(new object[]
				{
					this.latestConfirmedFrame,
					"+",
					i,
					":Delayed sfx buffer isn't empty"
				}));
				foreach (AudioHandler.SfxEvent sfxEvent in this.delayedSfxBuffer[i])
				{
					Debug.Log("Unplayed sound " + sfxEvent.sound);
				}
			}
			this.delayedSfxBuffer[i].Clear();
			if (this.delayedLoopingSfxBuffer[i].Count > 0)
			{
				Debug.LogError(string.Concat(new object[]
				{
					this.latestConfirmedFrame,
					"+",
					i,
					":Delayed looping sfx buffer isn't empty"
				}));
				foreach (AudioHandler.SfxEvent sfxEvent2 in this.delayedLoopingSfxBuffer[i])
				{
					Debug.Log(string.Concat(new object[]
					{
						"Unplayed sound ",
						sfxEvent2.sound,
						".",
						sfxEvent2.play,
						".",
						sfxEvent2.sourceId
					}));
				}
			}
			this.delayedLoopingSfxBuffer[i].Clear();
		}
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x0005A320 File Offset: 0x00058720
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

	// Token: 0x06000EA4 RID: 3748 RVA: 0x0005A451 File Offset: 0x00058851
	AudioReference IAudioHandler.PlayGameSound(AudioRequest request)
	{
		return this.playGameSound(request);
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x0005A45A File Offset: 0x0005885A
	void IAudioHandler.PlayMenuSound(AudioRequest request, float delay)
	{
		this.AudioPlayer.PlayMenuSound(request.sound, request.volume, request.pitch, delay);
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x0005A480 File Offset: 0x00058880
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

	// Token: 0x06000EA7 RID: 3751 RVA: 0x0005A4F6 File Offset: 0x000588F6
	AudioReference IAudioHandler.PlayLoopingSound(AudioRequest loopingSound)
	{
		return this.PlayLoopingSound(loopingSound);
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x0005A500 File Offset: 0x00058900
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

	// Token: 0x06000EA9 RID: 3753 RVA: 0x0005A560 File Offset: 0x00058960
	void IAudioHandler.PauseSounds(SoundType type, bool paused)
	{
		this.AudioPlayer.PauseSounds(type, paused);
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x0005A56F File Offset: 0x0005896F
	void IAudioHandler.UpdateVolume()
	{
		this.AudioPlayer.UpdateVolume();
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x0005A57C File Offset: 0x0005897C
	void IAudioHandler.Register(IAudioOwner owner)
	{
		this.AudioPlayer.Register(owner);
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x0005A58A File Offset: 0x0005898A
	void IAudioHandler.Unregister(IAudioOwner owner)
	{
		this.AudioPlayer.Unregister(owner);
	}

	// Token: 0x0400087B RID: 2171
	private GameObject parentObject;

	// Token: 0x0400087C RID: 2172
	private List<AudioHandler.SfxEvent>[] delayedSfxBuffer = new List<AudioHandler.SfxEvent>[RollbackStatePoolContainer.ROLLBACK_FRAMES];

	// Token: 0x0400087D RID: 2173
	private List<AudioHandler.SfxEvent>[] delayedLoopingSfxBuffer = new List<AudioHandler.SfxEvent>[RollbackStatePoolContainer.ROLLBACK_FRAMES];

	// Token: 0x0400087E RID: 2174
	private int latestConfirmedFrame;

	// Token: 0x020002A8 RID: 680
	private struct SfxEvent
	{
		// Token: 0x06000EAD RID: 3757 RVA: 0x0005A598 File Offset: 0x00058998
		public SfxEvent(AudioRequest sound, int sourceId, bool play, IAudioOwner attachTo, float fadeTime = 0f)
		{
			this.sound = sound;
			this.sourceId = sourceId;
			this.play = play;
			this.fadeTime = fadeTime;
			this.attachTo = attachTo;
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0005A5BF File Offset: 0x000589BF
		// (set) Token: 0x06000EAF RID: 3759 RVA: 0x0005A5C7 File Offset: 0x000589C7
		public AudioRequest sound { get; private set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0005A5D0 File Offset: 0x000589D0
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x0005A5D8 File Offset: 0x000589D8
		public int sourceId { get; private set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x0005A5E1 File Offset: 0x000589E1
		// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x0005A5E9 File Offset: 0x000589E9
		public bool play { get; private set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0005A5F2 File Offset: 0x000589F2
		// (set) Token: 0x06000EB5 RID: 3765 RVA: 0x0005A5FA File Offset: 0x000589FA
		public float fadeTime { get; private set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0005A603 File Offset: 0x00058A03
		// (set) Token: 0x06000EB7 RID: 3767 RVA: 0x0005A60B File Offset: 0x00058A0B
		public IAudioOwner attachTo { get; private set; }
	}
}
