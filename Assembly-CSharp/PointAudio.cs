using System;
using UnityEngine;

// Token: 0x020002B5 RID: 693
public class PointAudio : MonoBehaviour, IAudioOwner
{
	// Token: 0x06000F1D RID: 3869 RVA: 0x0005BC4C File Offset: 0x0005A04C
	public void Init()
	{
		this.audioOwner.Init(base.gameObject, true);
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x0005BC60 File Offset: 0x0005A060
	public IAudioOwner GetOwner()
	{
		return this.audioOwner;
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0005BC68 File Offset: 0x0005A068
	public int GetAudioSourceId(Vector3 point)
	{
		AudioPlayer.PooledAudioSource pooledAudioSource = this.audioOwner.New();
		pooledAudioSource.Source.transform.position = point;
		return pooledAudioSource.Id;
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x0005BC98 File Offset: 0x0005A098
	public AudioPlayer.PooledAudioSource GetSource(int sourceId)
	{
		return ((IAudioOwner)this.audioOwner).GetSource(sourceId);
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x0005BCA6 File Offset: 0x0005A0A6
	public AudioPlayer.PooledAudioSource New()
	{
		return ((IAudioOwner)this.audioOwner).New();
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0005BCB3 File Offset: 0x0005A0B3
	public void StopSound(int sourceID, float fadeTime)
	{
		((IAudioOwner)this.audioOwner).StopSound(sourceID, fadeTime);
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x0005BCC2 File Offset: 0x0005A0C2
	public void TickTimeDelta(float deltaTime)
	{
		((IAudioOwner)this.audioOwner).TickTimeDelta(deltaTime);
	}

	// Token: 0x040008B4 RID: 2228
	private AudioOwner audioOwner = new AudioOwner();
}
