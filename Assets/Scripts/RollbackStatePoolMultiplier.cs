// Decompile from assembly: Assembly-CSharp.dll

using System;

[AttributeUsage(AttributeTargets.Class)]
public class RollbackStatePoolMultiplier : Attribute
{
	public int PoolSizeMultiplier
	{
		get;
		private set;
	}

	public RollbackStatePoolMultiplier(int poolSizeMultiplier)
	{
		this.PoolSizeMultiplier = poolSizeMultiplier;
	}
}
