// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ISoundFileManager
{
	SoundFileData GetSound(SoundKey key);

	AudioData GetSoundAsAudioData(SoundKey key);

	void PreloadSound(SoundKey key);

	void PreloadBundle(SoundBundleKey key, bool preloadIndividualSounds);
}
