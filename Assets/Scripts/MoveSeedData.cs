// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public struct MoveSeedData
{
	public bool isActive;

	public Fixed damage;

	public override bool Equals(object other)
	{
		return (MoveSeedData)other == this;
	}

	public override int GetHashCode()
	{
		int num = 17;
		num = num * 7 + ((!this.isActive) ? 1 : 0);
		return num * 7 + this.damage.GetHashCode();
	}

	public static bool operator ==(MoveSeedData a, MoveSeedData b)
	{
		return a.isActive == b.isActive && a.damage == b.damage;
	}

	public static bool operator !=(MoveSeedData a, MoveSeedData b)
	{
		return !(a == b);
	}
}
