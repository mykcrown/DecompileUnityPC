using System;

// Token: 0x02000B4A RID: 2890
public interface IResetObjectPool<T> : ICountOwner where T : class, IResetable
{
	// Token: 0x060053E1 RID: 21473
	void Store(T obj);
}
