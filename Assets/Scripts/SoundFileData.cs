// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class SoundFileData : ScriptableObject
{
	public AudioClip sound;

	public float volume = 1f;

	public AudioSyncMode syncMode;
}
