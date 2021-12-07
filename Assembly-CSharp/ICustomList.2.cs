using System;

// Token: 0x02000AAC RID: 2732
public interface ICustomList<T> : ICustomList
{
	// Token: 0x06005044 RID: 20548
	int IndexOf(T item);

	// Token: 0x06005045 RID: 20549
	void Insert(int index, T item);

	// Token: 0x170012EC RID: 4844
	T this[int index]
	{
		get;
		set;
	}

	// Token: 0x06005048 RID: 20552
	void Add(T item);

	// Token: 0x06005049 RID: 20553
	bool Contains(T item);

	// Token: 0x0600504A RID: 20554
	void CopyTo(T[] array, int arrayIndex);

	// Token: 0x0600504B RID: 20555
	bool Remove(T item);
}
