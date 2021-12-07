using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
[RequireComponent(typeof(AudioSource))]
public class FlockChildSound : MonoBehaviour
{
	// Token: 0x0600002A RID: 42 RVA: 0x00003790 File Offset: 0x00001B90
	public void Start()
	{
		this._flockChild = base.GetComponent<FlockChild>();
		this._audio = base.GetComponent<AudioSource>();
		base.InvokeRepeating("PlayRandomSound", UnityEngine.Random.value + 1f, 1f);
		if (this._scareSounds.Length > 0)
		{
			base.InvokeRepeating("ScareSound", 1f, 0.01f);
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000037F4 File Offset: 0x00001BF4
	public void PlayRandomSound()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (!this._audio.isPlaying && this._flightSounds.Length > 0 && this._flightSoundRandomChance > UnityEngine.Random.value && !this._flockChild._landing)
			{
				this._audio.clip = this._flightSounds[UnityEngine.Random.Range(0, this._flightSounds.Length)];
				this._audio.pitch = UnityEngine.Random.Range(this._pitchMin, this._pitchMax);
				this._audio.volume = UnityEngine.Random.Range(this._volumeMin, this._volumeMax);
				this._audio.Play();
			}
			else if (!this._audio.isPlaying && this._idleSounds.Length > 0 && this._idleSoundRandomChance > UnityEngine.Random.value && this._flockChild._landing)
			{
				this._audio.clip = this._idleSounds[UnityEngine.Random.Range(0, this._idleSounds.Length)];
				this._audio.pitch = UnityEngine.Random.Range(this._pitchMin, this._pitchMax);
				this._audio.volume = UnityEngine.Random.Range(this._volumeMin, this._volumeMax);
				this._audio.Play();
				this._hasLanded = true;
			}
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00003960 File Offset: 0x00001D60
	public void ScareSound()
	{
		if (base.gameObject.activeInHierarchy && this._hasLanded && !this._flockChild._landing && this._idleSoundRandomChance * 2f > UnityEngine.Random.value)
		{
			this._audio.clip = this._scareSounds[UnityEngine.Random.Range(0, this._scareSounds.Length)];
			this._audio.volume = UnityEngine.Random.Range(this._volumeMin, this._volumeMax);
			this._audio.PlayDelayed(UnityEngine.Random.value * 0.2f);
			this._hasLanded = false;
		}
	}

	// Token: 0x04000038 RID: 56
	public AudioClip[] _idleSounds;

	// Token: 0x04000039 RID: 57
	public float _idleSoundRandomChance = 0.05f;

	// Token: 0x0400003A RID: 58
	public AudioClip[] _flightSounds;

	// Token: 0x0400003B RID: 59
	public float _flightSoundRandomChance = 0.05f;

	// Token: 0x0400003C RID: 60
	public AudioClip[] _scareSounds;

	// Token: 0x0400003D RID: 61
	public float _pitchMin = 0.85f;

	// Token: 0x0400003E RID: 62
	public float _pitchMax = 1f;

	// Token: 0x0400003F RID: 63
	public float _volumeMin = 0.6f;

	// Token: 0x04000040 RID: 64
	public float _volumeMax = 0.8f;

	// Token: 0x04000041 RID: 65
	private FlockChild _flockChild;

	// Token: 0x04000042 RID: 66
	private AudioSource _audio;

	// Token: 0x04000043 RID: 67
	private bool _hasLanded;
}
