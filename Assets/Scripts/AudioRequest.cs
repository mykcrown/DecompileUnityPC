// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public struct AudioRequest
{
	public IAudioOwner attachTo;

	public bool usePoint;

	public Vector3 point;

	public bool maintainPitch;

	public Action<AudioReference, bool> onFinish;

	public float pitch;

	public float volumeMultiplier;

	private AudioData data;

	public AudioClip sound
	{
		get
		{
			return this.data.sound;
		}
	}

	public float volume
	{
		get
		{
			return this.data.volume * this.volumeMultiplier;
		}
	}

	public AudioSyncMode syncMode
	{
		get
		{
			return this.data.syncMode;
		}
	}

	public bool ignorePitchVariation
	{
		get
		{
			return this.data.isVoice;
		}
	}

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

	public AudioRequest MaintainPitch()
	{
		this.maintainPitch = true;
		return this;
	}
}
