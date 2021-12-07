// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public struct PhysicsIntOverride : IPhysicsValueOverride<int>, IPhysicsValueOverride
{
	public bool isOverriden;

	public int value;

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

	public int Value
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

	public PhysicsIntOverride(bool isOverriden = false, int value = 0)
	{
		this.isOverriden = isOverriden;
		this.value = value;
	}

	public int GetValueOrDefault(int defaultValue)
	{
		return (!this.isOverriden) ? defaultValue : this.value;
	}
}
