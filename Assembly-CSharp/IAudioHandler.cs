using System;

// Token: 0x020002A6 RID: 678
public interface IAudioHandler : ITickable
{
	// Token: 0x06000E81 RID: 3713
	void PlayMusic(AudioData music);

	// Token: 0x06000E82 RID: 3714
	void PlayMenuSound(AudioRequest request, float delay = 0f);

	// Token: 0x06000E83 RID: 3715
	AudioReference PlayGameSound(AudioRequest request);

	// Token: 0x06000E84 RID: 3716
	void StopMusic(Action callback, float fadeTime = -1f);

	// Token: 0x06000E85 RID: 3717
	AudioReference PlayLoopingSound(AudioRequest sound);

	// Token: 0x06000E86 RID: 3718
	void StopSound(AudioReference audioRef, float fadeTime = 0f);

	// Token: 0x06000E87 RID: 3719
	void UpdateVolume();

	// Token: 0x06000E88 RID: 3720
	void PauseSounds(SoundType type, bool paused);

	// Token: 0x06000E89 RID: 3721
	void OnGameDestroyed(int frame);

	// Token: 0x06000E8A RID: 3722
	void Register(IAudioOwner owner);

	// Token: 0x06000E8B RID: 3723
	void Unregister(IAudioOwner owner);
}
