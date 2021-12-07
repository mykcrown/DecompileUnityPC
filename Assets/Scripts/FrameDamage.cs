// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public struct FrameDamage
{
	public int frame;

	public Fixed damage;

	public FrameDamage(int frame, Fixed damage)
	{
		this.frame = frame;
		this.damage = damage;
	}

	public void Clear()
	{
		this.frame = 0;
		this.damage = 0;
	}
}
