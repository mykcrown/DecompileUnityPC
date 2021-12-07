// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IAudioHandler : ITickable
{
	void PlayMusic(AudioData music);

	void PlayMenuSound(AudioRequest request, float delay = 0f);

	AudioReference PlayGameSound(AudioRequest request);

	void StopMusic(Action callback, float fadeTime = -1f);

	AudioReference PlayLoopingSound(AudioRequest sound);

	void StopSound(AudioReference audioRef, float fadeTime = 0f);

	void UpdateVolume();

	void PauseSounds(SoundType type, bool paused);

	void OnGameDestroyed(int frame);

	void Register(IAudioOwner owner);

	void Unregister(IAudioOwner owner);
}
