using System;
using UnityEngine;

// Token: 0x020002DF RID: 735
[Serializable]
public class SoundFileData : ScriptableObject
{
	// Token: 0x0400095F RID: 2399
	public AudioClip sound;

	// Token: 0x04000960 RID: 2400
	public float volume = 1f;

	// Token: 0x04000961 RID: 2401
	public AudioSyncMode syncMode;
}
