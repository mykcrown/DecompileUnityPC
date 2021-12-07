// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class PointAudio : MonoBehaviour, IAudioOwner
{
	private AudioOwner audioOwner = new AudioOwner();

	public void Init()
	{
		this.audioOwner.Init(base.gameObject, true);
	}

	public IAudioOwner GetOwner()
	{
		return this.audioOwner;
	}

	public int GetAudioSourceId(Vector3 point)
	{
		AudioPlayer.PooledAudioSource pooledAudioSource = this.audioOwner.New();
		pooledAudioSource.Source.transform.position = point;
		return pooledAudioSource.Id;
	}

	public AudioPlayer.PooledAudioSource GetSource(int sourceId)
	{
		return ((IAudioOwner)this.audioOwner).GetSource(sourceId);
	}

	public AudioPlayer.PooledAudioSource New()
	{
		return ((IAudioOwner)this.audioOwner).New();
	}

	public void StopSound(int sourceID, float fadeTime)
	{
		((IAudioOwner)this.audioOwner).StopSound(sourceID, fadeTime);
	}

	public void TickTimeDelta(float deltaTime)
	{
		((IAudioOwner)this.audioOwner).TickTimeDelta(deltaTime);
	}
}
