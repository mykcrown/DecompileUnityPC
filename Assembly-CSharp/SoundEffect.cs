using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020002DE RID: 734
[Serializable]
public class SoundEffect : ICloneable
{
	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06000F4D RID: 3917 RVA: 0x0005BFFE File Offset: 0x0005A3FE
	// (set) Token: 0x06000F4E RID: 3918 RVA: 0x0005C006 File Offset: 0x0005A406
	public bool editorToggle { get; set; }

	// Token: 0x06000F4F RID: 3919 RVA: 0x0005C00F File Offset: 0x0005A40F
	public object Clone()
	{
		return CloneUtil.SlowDeepClone<SoundEffect>(this);
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x0005C017 File Offset: 0x0005A417
	public AudioData GetRandomSound()
	{
		return this.sounds[UnityEngine.Random.Range(0, this.sounds.Length)];
	}

	// Token: 0x04000956 RID: 2390
	[FormerlySerializedAs("castingFrame")]
	public int frame;

	// Token: 0x04000957 RID: 2391
	public MoveEffectCancelCondition cancelCondition;

	// Token: 0x04000958 RID: 2392
	public float volume = 1f;

	// Token: 0x04000959 RID: 2393
	[FormerlySerializedAs("_sounds")]
	public AudioData[] sounds = new AudioData[0];

	// Token: 0x0400095A RID: 2394
	public AudioData sound;

	// Token: 0x0400095B RID: 2395
	public AudioData[] altSounds = new AudioData[0];

	// Token: 0x0400095C RID: 2396
	public float softKillTime = 0.15f;

	// Token: 0x0400095D RID: 2397
	public int noInterruptFrame;
}
