// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public abstract class RollbackStateTyped<T> : RollbackState, ICopyable<T>, ICopyable
{
	public abstract void CopyTo(T target);
}
