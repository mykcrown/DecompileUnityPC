using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200029F RID: 671
public class AudioManager : IAudioHandler, ITickable
{
	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000E57 RID: 3671 RVA: 0x000596FE File Offset: 0x00057AFE
	// (set) Token: 0x06000E58 RID: 3672 RVA: 0x00059706 File Offset: 0x00057B06
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0005970F File Offset: 0x00057B0F
	// (set) Token: 0x06000E5A RID: 3674 RVA: 0x00059717 File Offset: 0x00057B17
	[Inject]
	public IAudioHandler AudioHandler { get; set; }

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00059720 File Offset: 0x00057B20
	// (set) Token: 0x06000E5C RID: 3676 RVA: 0x00059728 File Offset: 0x00057B28
	[Inject]
	public ISoundFileManager soundFileManager { get; set; }

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000E5D RID: 3677 RVA: 0x00059731 File Offset: 0x00057B31
	// (set) Token: 0x06000E5E RID: 3678 RVA: 0x00059739 File Offset: 0x00057B39
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00059742 File Offset: 0x00057B42
	// (set) Token: 0x06000E60 RID: 3680 RVA: 0x0005974A File Offset: 0x00057B4A
	public AudioListener UIAudioListener { get; private set; }

	// Token: 0x06000E61 RID: 3681 RVA: 0x00059754 File Offset: 0x00057B54
	public void Init(Transform parent)
	{
		GameObject gameObject = new GameObject("Audio");
		gameObject.transform.SetParent(parent);
		(this.AudioHandler as AudioHandler).Init(this.config, gameObject);
		this.UIAudioListener = new GameObject("Audio Listener").AddComponent<AudioListener>();
		this.UIAudioListener.transform.SetParent(gameObject.transform);
		this.events.Subscribe(typeof(PauseSoundCommand), new Events.EventHandler(this.onPauseSounds));
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x000597DB File Offset: 0x00057BDB
	public void Destroy()
	{
		this.events.Unsubscribe(typeof(PauseSoundCommand), new Events.EventHandler(this.onPauseSounds));
		(this.AudioHandler as AudioPlayer).Destroy();
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x00059810 File Offset: 0x00057C10
	public AudioReference PlaySFX(AudioData sound)
	{
		IAudioHandler audioHandler = this.AudioHandler;
		AudioRequest audioRequest = new AudioRequest(sound, null);
		return audioHandler.PlayGameSound(audioRequest.MaintainPitch());
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x00059838 File Offset: 0x00057C38
	public AudioReference PlaySFX(SoundKey key)
	{
		SoundFileData sound = this.soundFileManager.GetSound(key);
		if (sound == null)
		{
			Debug.LogError("SOUND KEY NOT FOUND, this should never happen " + key);
			return new AudioReference(null, -1);
		}
		return this.AudioHandler.PlayGameSound(new AudioRequest(sound, null));
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x00059894 File Offset: 0x00057C94
	private void onPauseSounds(GameEvent message)
	{
		PauseSoundCommand pauseSoundCommand = message as PauseSoundCommand;
		this.AudioHandler.PauseSounds(pauseSoundCommand.type, pauseSoundCommand.paused);
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x000598BF File Offset: 0x00057CBF
	public void UpdateVolume()
	{
		this.AudioHandler.UpdateVolume();
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x000598CC File Offset: 0x00057CCC
	public void PlayMusic(AudioData music)
	{
		this.AudioHandler.PlayMusic(music);
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x000598DC File Offset: 0x00057CDC
	public void PlayMusic(SoundKey key)
	{
		SoundFileData sound = this.soundFileManager.GetSound(key);
		if (sound == null)
		{
			Debug.LogError("SOUND KEY NOT FOUND, this should never happen " + key);
		}
		else
		{
			this.AudioHandler.PlayMusic(new AudioData(sound.sound, sound.volume, sound.syncMode));
		}
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x0005993E File Offset: 0x00057D3E
	public void StopMusic(Action callback = null, float fadeTime = -1f)
	{
		this.AudioHandler.StopMusic(callback, fadeTime);
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x00059950 File Offset: 0x00057D50
	public void PlayMenuSound(AudioData sound, float delay = 0f)
	{
		AudioRequest request = new AudioRequest(sound, null);
		this.PlayMenuSound(request, delay);
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x00059970 File Offset: 0x00057D70
	public void PlayMenuSound(SoundKey key, float delay = 0f)
	{
		SoundFileData sound = this.soundFileManager.GetSound(key);
		if (sound == null)
		{
			Debug.LogError("SOUND KEY NOT FOUND, this should never happen " + key);
		}
		else
		{
			AudioRequest request = new AudioRequest(sound, null);
			this.PlayMenuSound(request, delay);
		}
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x000599C1 File Offset: 0x00057DC1
	public void PlayMenuSound(AudioRequest request, float delay = 0f)
	{
		this.AudioHandler.PlayMenuSound(request, delay);
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x000599D0 File Offset: 0x00057DD0
	public AudioReference PlayGameSound(AudioRequest request)
	{
		return this.AudioHandler.PlayGameSound(request);
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x000599E0 File Offset: 0x00057DE0
	public AudioReference PlayGameSound(SoundKey key, IAudioOwner attachTo)
	{
		SoundFileData sound = this.soundFileManager.GetSound(key);
		if (sound == null)
		{
			Debug.LogError("SOUND KEY NOT FOUND, this should never happen " + key);
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

	// Token: 0x06000E6F RID: 3695 RVA: 0x00059A51 File Offset: 0x00057E51
	public AudioReference PlaySound(IList<AudioData> sounds, Action<AudioReference, bool> onFinish = null)
	{
		if (sounds.Count > 0)
		{
			return this.PlayGameSound(new AudioRequest(sounds[UnityEngine.Random.Range(0, sounds.Count)], onFinish));
		}
		return new AudioReference(null, -1);
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x00059A85 File Offset: 0x00057E85
	public AudioReference PlayLoopingSound(AudioRequest loopingSound)
	{
		return this.AudioHandler.PlayLoopingSound(loopingSound);
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x00059A93 File Offset: 0x00057E93
	public void StopSound(AudioReference audioRef, float fadeTime = 0f)
	{
		this.AudioHandler.StopSound(audioRef, fadeTime);
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x00059AA2 File Offset: 0x00057EA2
	public void PauseSounds(SoundType type, bool paused)
	{
		this.AudioHandler.PauseSounds(type, paused);
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x00059AB1 File Offset: 0x00057EB1
	public void TickFrame()
	{
		this.AudioHandler.TickFrame();
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x00059ABE File Offset: 0x00057EBE
	public void Register(IAudioOwner owner)
	{
		this.AudioHandler.Register(owner);
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x00059ACC File Offset: 0x00057ECC
	public void Unregister(IAudioOwner owner)
	{
		this.AudioHandler.Unregister(owner);
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x00059ADA File Offset: 0x00057EDA
	public void OnGameDestroyed(int frame)
	{
		this.AudioHandler.OnGameDestroyed(frame);
	}

	// Token: 0x0400085B RID: 2139
	private IRollbackStatus rollbackStatus;
}
