using System;
using UnityEngine;

// Token: 0x020002AF RID: 687
public struct AudioRequest
{
	// Token: 0x06000EE9 RID: 3817 RVA: 0x0005B3F8 File Offset: 0x000597F8
	public AudioRequest(AudioClip clip, Action<AudioReference, bool> onFinish = null)
	{
		this.data = new AudioData(clip, 1f, AudioSyncMode.Synchronized);
		this.pitch = 1f;
		this.volumeMultiplier = 1f;
		this.onFinish = onFinish;
		this.attachTo = null;
		this.point = Vector3.zero;
		this.usePoint = false;
		this.maintainPitch = false;
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x0005B454 File Offset: 0x00059854
	public AudioRequest(AudioData data, Action<AudioReference, bool> onFinish = null)
	{
		this.data = data;
		this.pitch = 1f;
		this.volumeMultiplier = 1f;
		this.onFinish = onFinish;
		this.attachTo = null;
		this.point = Vector3.zero;
		this.usePoint = false;
		this.maintainPitch = false;
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0005B4A8 File Offset: 0x000598A8
	public AudioRequest(AudioData data, IAudioOwner attachTo, Action<AudioReference, bool> onFinish = null)
	{
		this.data = data;
		this.pitch = 1f;
		this.volumeMultiplier = 1f;
		this.onFinish = onFinish;
		this.attachTo = attachTo;
		this.point = Vector3.zero;
		this.usePoint = false;
		this.maintainPitch = false;
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0005B4FC File Offset: 0x000598FC
	public AudioRequest(AudioData data, Vector3 point, Action<AudioReference, bool> onFinish = null)
	{
		this.data = data;
		this.pitch = 1f;
		this.volumeMultiplier = 1f;
		this.onFinish = onFinish;
		this.attachTo = null;
		this.point = point;
		this.usePoint = true;
		this.maintainPitch = false;
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x0005B54C File Offset: 0x0005994C
	public AudioRequest(SoundFileData data, Action<AudioReference, bool> onFinish = null)
	{
		this.data = new AudioData(data.sound, data.volume, data.syncMode);
		this.pitch = 1f;
		this.volumeMultiplier = 1f;
		this.onFinish = onFinish;
		this.attachTo = null;
		this.point = Vector3.zero;
		this.usePoint = false;
		this.maintainPitch = false;
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000EEE RID: 3822 RVA: 0x0005B5B3 File Offset: 0x000599B3
	public AudioClip sound
	{
		get
		{
			return this.data.sound;
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0005B5C0 File Offset: 0x000599C0
	public float volume
	{
		get
		{
			return this.data.volume * this.volumeMultiplier;
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x0005B5D4 File Offset: 0x000599D4
	public AudioSyncMode syncMode
	{
		get
		{
			return this.data.syncMode;
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0005B5E1 File Offset: 0x000599E1
	public bool ignorePitchVariation
	{
		get
		{
			return this.data.isVoice;
		}
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x0005B5EE File Offset: 0x000599EE
	public AudioRequest MaintainPitch()
	{
		this.maintainPitch = true;
		return this;
	}

	// Token: 0x0400089E RID: 2206
	public IAudioOwner attachTo;

	// Token: 0x0400089F RID: 2207
	public bool usePoint;

	// Token: 0x040008A0 RID: 2208
	public Vector3 point;

	// Token: 0x040008A1 RID: 2209
	public bool maintainPitch;

	// Token: 0x040008A2 RID: 2210
	public Action<AudioReference, bool> onFinish;

	// Token: 0x040008A3 RID: 2211
	public float pitch;

	// Token: 0x040008A4 RID: 2212
	public float volumeMultiplier;

	// Token: 0x040008A5 RID: 2213
	private AudioData data;
}
