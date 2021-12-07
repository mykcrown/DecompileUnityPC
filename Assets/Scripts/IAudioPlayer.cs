// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IAudioPlayer
{
	void PlayMusic(AudioClip music, float volume = 1f);

	void PlayGameSound(AudioRequest request, int sourceId, IAudioOwner attachTo);

	void PlayMenuSound(AudioClip sound, float volume = 1f, float pitch = 1f, float delay = 0f);

	void StopMusic(Action callback, float fadeTime = -1f);

	void UpdateVolume();

	int GetAudioSourceId(IAudioOwner owner);

	void PlayLoopingSound(AudioRequest loopingSound, int sourceId);

	void StopSound(AudioReference audioRef, float fadeTime = 0f);

	void PauseSounds(SoundType type, bool paused);

	void Register(IAudioOwner owner);

	void Unregister(IAudioOwner owner);
}
