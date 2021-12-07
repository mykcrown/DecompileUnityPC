// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICustomList<T> : ICustomList
{
	T this[int index]
	{
		get;
		set;
	}

	int IndexOf(T item);

	void Insert(int index, T item);

	void Add(T item);

	bool Contains(T item);

	void CopyTo(T[] array, int arrayIndex);

	bool Remove(T item);
}
