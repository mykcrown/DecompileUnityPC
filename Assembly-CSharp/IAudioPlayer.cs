using System;
using UnityEngine;

// Token: 0x020002A9 RID: 681
public interface IAudioPlayer
{
	// Token: 0x06000EB8 RID: 3768
	void PlayMusic(AudioClip music, float volume = 1f);

	// Token: 0x06000EB9 RID: 3769
	void PlayGameSound(AudioRequest request, int sourceId, IAudioOwner attachTo);

	// Token: 0x06000EBA RID: 3770
	void PlayMenuSound(AudioClip sound, float volume = 1f, float pitch = 1f, float delay = 0f);

	// Token: 0x06000EBB RID: 3771
	void StopMusic(Action callback, float fadeTime = -1f);

	// Token: 0x06000EBC RID: 3772
	void UpdateVolume();

	// Token: 0x06000EBD RID: 3773
	int GetAudioSourceId(IAudioOwner owner);

	// Token: 0x06000EBE RID: 3774
	void PlayLoopingSound(AudioRequest loopingSound, int sourceId);

	// Token: 0x06000EBF RID: 3775
	void StopSound(AudioReference audioRef, float fadeTime = 0f);

	// Token: 0x06000EC0 RID: 3776
	void PauseSounds(SoundType type, bool paused);

	// Token: 0x06000EC1 RID: 3777
	void Register(IAudioOwner owner);

	// Token: 0x06000EC2 RID: 3778
	void Unregister(IAudioOwner owner);
}
