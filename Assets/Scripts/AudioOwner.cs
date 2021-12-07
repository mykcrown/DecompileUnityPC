// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioOwner : IAudioOwner
{
	private int poolSize = 8;

	private int currentIndex;

	private List<AudioPlayer.PooledAudioSource> list = new List<AudioPlayer.PooledAudioSource>();

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

	public AudioPlayer.PooledAudioSource GetSource(int sourceId)
	{
		return this.list[sourceId];
	}

	public void TickTimeDelta(float deltaTime)
	{
		foreach (AudioPlayer.PooledAudioSource current in this.list)
		{
			if (current.Active && !current.Source.loop && current.TickTimeDelta(Time.deltaTime))
			{
				this.stopSound(current, current.FadeStartTime == 0f);
			}
		}
	}

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

	public AudioPlayer.PooledAudioSource New()
	{
		AudioPlayer.PooledAudioSource result = this.list[this.currentIndex];
		this.currentIndex = (this.currentIndex + 1) % this.poolSize;
		return result;
	}
}
