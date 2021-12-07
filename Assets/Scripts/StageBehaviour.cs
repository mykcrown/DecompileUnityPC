// Decompile from assembly: Assembly-CSharp.dll

using System;

public abstract class StageBehaviour : GameBehavior, ICloneable
{
	public int StartDelay;

	public virtual void Init()
	{
	}

	public abstract void Play(object context = null);

	public object Clone()
	{
		return null;
	}
}
