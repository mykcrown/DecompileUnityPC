// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public struct AudioData
{
	public AudioClip sound;

	public float volume;

	public AudioSyncMode syncMode;

	public bool isVoice;

	public static AudioData Empty = new AudioData(null, 1f, AudioSyncMode.Synchronized);

	public AudioData(AudioClip sound, float volume = 1f, AudioSyncMode syncMode = AudioSyncMode.Synchronized)
	{
		this.sound = sound;
		this.volume = volume;
		this.syncMode = syncMode;
		this.isVoice = false;
	}

	public AudioData MultiplyVolume(float volumeMult)
	{
		AudioData result = new AudioData(this.sound, this.volume * volumeMult, this.syncMode);
		return result;
	}
}
