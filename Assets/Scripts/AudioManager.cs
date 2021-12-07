// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : IAudioHandler, ITickable
{
	private IRollbackStatus rollbackStatus;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IAudioHandler AudioHandler
	{
		get;
		set;
	}

	[Inject]
	public ISoundFileManager soundFileManager
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	public AudioListener UIAudioListener
	{
		get;
		private set;
	}

	public void Init(Transform parent)
	{
		GameObject gameObject = new GameObject("Audio");
		gameObject.transform.SetParent(parent);
		(this.AudioHandler as AudioHandler).Init(this.config, gameObject);
		this.UIAudioListener = new GameObject("Audio Listener").AddComponent<AudioListener>();
		this.UIAudioListener.transform.SetParent(gameObject.transform);
		this.events.Subscribe(typeof(PauseSoundCommand), new Events.EventHandler(this.onPauseSounds));
	}

	public void Destroy()
	{
		this.events.Unsubscribe(typeof(PauseSoundCommand), new Events.EventHandler(this.onPauseSounds));
		(this.AudioHandler as AudioPlayer).Destroy();
	}

	public AudioReference PlaySFX(AudioData sound)
	{
		IAudioHandler arg_16_0 = this.AudioHandler;
		AudioRequest audioRequest = new AudioRequest(sound, null);
		return arg_16_0.PlayGameSound(audioRequest.MaintainPitch());
	}

	public AudioReference PlaySFX(SoundKey key)
	{
		SoundFileData sound = this.soundFileManager.GetSound(key);
		if (sound == null)
		{
			UnityEngine.Debug.LogError("SOUND KEY NOT FOUND, this should never happen " + key);
			return new AudioReference(null, -1);
		}
		return this.AudioHandler.PlayGameSound(new AudioRequest(sound, null));
	}

	private void onPauseSounds(GameEvent message)
	{
		PauseSoundCommand pauseSoundCommand = message as PauseSoundCommand;
		this.AudioHandler.PauseSounds(pauseSoundCommand.type, pauseSoundCommand.paused);
	}

	public void UpdateVolume()
	{
		this.AudioHandler.UpdateVolume();
	}

	public void PlayMusic(AudioData music)
	{
		this.AudioHandler.PlayMusic(music);
	}

	public void PlayMusic(SoundKey key)
	{
		SoundFileData sound = this.soundFileManager.GetSound(key);
		if (sound == null)
		{
			UnityEngine.Debug.LogError("SOUND KEY NOT FOUND, this should never happen " + key);
		}
		else
		{
			this.AudioHandler.PlayMusic(new AudioData(sound.sound, sound.volume, sound.syncMode));
		}
	}

	public void StopMusic(Action callback = null, float fadeTime = -1f)
	{
		this.AudioHandler.StopMusic(callback, fadeTime);
	}

	public void PlayMenuSound(AudioData sound, float delay = 0f)
	{
		AudioRequest request = new AudioRequest(sound, null);
		this.PlayMenuSound(request, delay);
	}

	public void PlayMenuSound(SoundKey key, float delay = 0f)
	{
		SoundFileData sound = this.soundFileManager.GetSound(key);
		if (sound == null)
		{
			UnityEngine.Debug.LogError("SOUND KEY NOT FOUND, this should never happen " + key);
		}
		else
		{
			AudioRequest request = new AudioRequest(sound, null);
			this.PlayMenuSound(request, delay);
		}
	}

	public void PlayMenuSound(AudioRequest request, float delay = 0f)
	{
		this.AudioHandler.PlayMenuSound(request, delay);
	}

	public AudioReference PlayGameSound(AudioRequest request)
	{
		return this.AudioHandler.PlayGameSound(request);
	}

	public AudioReference PlayGameSound(SoundKey key, IAudioOwner attachTo)
	{
		SoundFileData sound = this.soundFileManager.GetSound(key);
		if (sound == null)
		{
			UnityEngine.Debug.LogError("SOUND KEY NOT FOUND, this should never happen " + key);
		}
		else if (sound.sound != null)
		{
			return this.PlayGameSound(new AudioRequest(sound, null)
			{
				attachTo = attachTo
			});
		}
		return new AudioReference(null, 0);
	}

	public AudioReference PlaySound(IList<AudioData> sounds, Action<AudioReference, bool> onFinish = null)
	{
		if (sounds.Count > 0)
		{
			return this.PlayGameSound(new AudioRequest(sounds[UnityEngine.Random.Range(0, sounds.Count)], onFinish));
		}
		return new AudioReference(null, -1);
	}

	public AudioReference PlayLoopingSound(AudioRequest loopingSound)
	{
		return this.AudioHandler.PlayLoopingSound(loopingSound);
	}

	public void StopSound(AudioReference audioRef, float fadeTime = 0f)
	{
		this.AudioHandler.StopSound(audioRef, fadeTime);
	}

	public void PauseSounds(SoundType type, bool paused)
	{
		this.AudioHandler.PauseSounds(type, paused);
	}

	public void TickFrame()
	{
		this.AudioHandler.TickFrame();
	}

	public void Register(IAudioOwner owner)
	{
		this.AudioHandler.Register(owner);
	}

	public void Unregister(IAudioOwner owner)
	{
		this.AudioHandler.Unregister(owner);
	}

	public void OnGameDestroyed(int frame)
	{
		this.AudioHandler.OnGameDestroyed(frame);
	}
}
