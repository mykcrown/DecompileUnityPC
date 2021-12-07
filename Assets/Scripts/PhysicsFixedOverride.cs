// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public struct PhysicsFixedOverride : IPhysicsValueOverride<Fixed>, IPhysicsValueOverride
{
	public bool isOverriden;

	public Fixed value;

	public bool IsOverriden
	{
		get
		{
			return this.isOverriden;
		}
		set
		{
			this.isOverriden = value;
		}
	}

	public Fixed Value
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	public PhysicsFixedOverride(bool isOverriden = false, Fixed value = default(Fixed))
	{
		this.isOverriden = isOverriden;
		this.value = value;
	}

	public Fixed GetValueOrDefault(Fixed defaultValue)
	{
		return (!this.isOverriden) ? defaultValue : this.value;
	}
}
