// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPhysicsValueOverride<T> : IPhysicsValueOverride
{
	T Value
	{
		get;
		set;
	}

	T GetValueOrDefault(T defaultValue);
}
