// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SoundEffect : ICloneable
{
	[FormerlySerializedAs("castingFrame")]
	public int frame;

	public MoveEffectCancelCondition cancelCondition;

	public float volume = 1f;

	[FormerlySerializedAs("_sounds")]
	public AudioData[] sounds = new AudioData[0];

	public AudioData sound;

	public AudioData[] altSounds = new AudioData[0];

	public float softKillTime = 0.15f;

	public int noInterruptFrame;

	public bool editorToggle
	{
		get;
		set;
	}

	public object Clone()
	{
		return CloneUtil.SlowDeepClone<SoundEffect>(this);
	}

	public AudioData GetRandomSound()
	{
		return this.sounds[UnityEngine.Random.Range(0, this.sounds.Length)];
	}
}
