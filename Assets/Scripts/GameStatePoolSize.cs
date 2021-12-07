// Decompile from assembly: Assembly-CSharp.dll

using System;

[AttributeUsage(AttributeTargets.Class)]
public class GameStatePoolSize : Attribute
{
	public int PoolSize
	{
		get;
		private set;
	}

	public GameStatePoolSize(int poolSize)
	{
		this.PoolSize = poolSize;
	}
}
