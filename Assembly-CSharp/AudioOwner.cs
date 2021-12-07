using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002B4 RID: 692
public class AudioOwner : IAudioOwner
{
	// Token: 0x06000F16 RID: 3862 RVA: 0x0005BA08 File Offset: 0x00059E08
	public void Init(GameObject container, bool separateGameObjects)
	{
		for (int i = 0; i < this.poolSize; i++)
		{
			AudioSource audioSource;
			if (separateGameObjects)
			{
				audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
				audioSource.transform.SetParent(container.transform, false);
			}
			else
			{
				audioSource = container.AddComponent<AudioSource>();
			}
			AudioPlayer.PooledAudioSource item = new AudioPlayer.PooledAudioSource
			{
				Source = audioSource,
				Active = true,
				Id = i,
				FadeStartTime = 0f,
				FadeStartVolume = 0f
			};
			this.list.Add(item);
		}
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x0005BA9F File Offset: 0x00059E9F
	public AudioPlayer.PooledAudioSource GetSource(int sourceId)
	{
		return this.list[sourceId];
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x0005BAB0 File Offset: 0x00059EB0
	public void TickTimeDelta(float deltaTime)
	{
		foreach (AudioPlayer.PooledAudioSource pooledAudioSource in this.list)
		{
			if (pooledAudioSource.Active && !pooledAudioSource.Source.loop && pooledAudioSource.TickTimeDelta(Time.deltaTime))
			{
				this.stopSound(pooledAudioSource, pooledAudioSource.FadeStartTime == 0f);
			}
		}
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x0005BB44 File Offset: 0x00059F44
	private void stopSound(AudioPlayer.PooledAudioSource source, bool clipCompleted)
	{
		if (source.Source != null && !source.Source.Equals(null))
		{
			try
			{
				source.Source.Stop();
				if (source.OnFinish != null)
				{
					source.OnFinish(new AudioReference(this, source.Id), clipCompleted);
					source.OnFinish = null;
				}
			}
			finally
			{
				source.Active = false;
			}
		}
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x0005BBC8 File Offset: 0x00059FC8
	public void StopSound(int sourceID, float fadeTime)
	{
		AudioPlayer.PooledAudioSource pooledAudioSource = this.list[sourceID];
		if (fadeTime <= 0f)
		{
			this.stopSound(pooledAudioSource, false);
		}
		else
		{
			pooledAudioSource.InitializeFadeOut(fadeTime);
		}
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x0005BC04 File Offset: 0x0005A004
	public AudioPlayer.PooledAudioSource New()
	{
		AudioPlayer.PooledAudioSource result = this.list[this.currentIndex];
		this.currentIndex = (this.currentIndex + 1) % this.poolSize;
		return result;
	}

	// Token: 0x040008B1 RID: 2225
	private int poolSize = 8;

	// Token: 0x040008B2 RID: 2226
	private int currentIndex;

	// Token: 0x040008B3 RID: 2227
	private List<AudioPlayer.PooledAudioSource> list = new List<AudioPlayer.PooledAudioSource>();
}
