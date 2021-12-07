// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public struct PhysicsBoolOverride : IPhysicsValueOverride<bool>, IPhysicsValueOverride
{
	public bool isOverriden;

	public bool value;

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

	public bool Value
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

	public PhysicsBoolOverride(bool isOverriden = false, bool value = false)
	{
		this.isOverriden = isOverriden;
		this.value = value;
	}

	public bool GetValueOrDefault(bool defaultValue)
	{
		return (!this.isOverriden) ? defaultValue : this.value;
	}
}
