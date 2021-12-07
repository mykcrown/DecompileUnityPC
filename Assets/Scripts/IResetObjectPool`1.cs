// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IResetObjectPool<T> : ICountOwner where T : class, IResetable
{
	void Store(T obj);
}
