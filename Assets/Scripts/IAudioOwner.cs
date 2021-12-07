// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IAudioOwner
{
	AudioPlayer.PooledAudioSource GetSource(int sourceId);

	AudioPlayer.PooledAudioSource New();

	void StopSound(int sourceID, float fadeTime);

	void TickTimeDelta(float deltaTime);
}
