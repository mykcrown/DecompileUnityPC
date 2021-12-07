using System;

// Token: 0x020002B3 RID: 691
public interface IAudioOwner
{
	// Token: 0x06000F11 RID: 3857
	AudioPlayer.PooledAudioSource GetSource(int sourceId);

	// Token: 0x06000F12 RID: 3858
	AudioPlayer.PooledAudioSource New();

	// Token: 0x06000F13 RID: 3859
	void StopSound(int sourceID, float fadeTime);

	// Token: 0x06000F14 RID: 3860
	void TickTimeDelta(float deltaTime);
}
