using System;

// Token: 0x020002E1 RID: 737
public interface ISoundFileManager
{
	// Token: 0x06000F5C RID: 3932
	SoundFileData GetSound(SoundKey key);

	// Token: 0x06000F5D RID: 3933
	AudioData GetSoundAsAudioData(SoundKey key);

	// Token: 0x06000F5E RID: 3934
	void PreloadSound(SoundKey key);

	// Token: 0x06000F5F RID: 3935
	void PreloadBundle(SoundBundleKey key, bool preloadIndividualSounds);
}
