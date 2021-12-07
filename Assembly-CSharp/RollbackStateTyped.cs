using System;

// Token: 0x02000889 RID: 2185
[Serializable]
public abstract class RollbackStateTyped<T> : RollbackState, ICopyable<T>, ICopyable
{
	// Token: 0x060036EB RID: 14059
	public abstract void CopyTo(T target);
}
