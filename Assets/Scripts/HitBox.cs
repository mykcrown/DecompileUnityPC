// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class HitBox : ICloneable
{
	public BodyPart bodyPart;

	public Vector3 offset;

	public bool isRelativeOffset;

	public float radius = 0.7f;

	public void Rescale(Fixed rescale)
	{
		this.offset *= (float)rescale;
		this.radius *= (float)rescale;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
