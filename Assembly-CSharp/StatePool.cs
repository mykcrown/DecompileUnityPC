using System;

// Token: 0x02000886 RID: 2182
public class StatePool<T> : IStatePool<T>, IStatePool where T : ICopyable<T>, new()
{
	// Token: 0x060036E7 RID: 14055 RVA: 0x001006E8 File Offset: 0x000FEAE8
	public StatePool(int poolMulti)
	{
		this.arr = new T[poolMulti * RollbackStatePooling.DEFAULT_BUFFER_SIZE];
		for (int i = 0; i < this.arr.Length; i++)
		{
			this.arr[i] = Activator.CreateInstance<T>();
		}
	}

	// Token: 0x060036E8 RID: 14056 RVA: 0x00100737 File Offset: 0x000FEB37
	public T GetNext()
	{
		this.index = (this.index + 1) % this.arr.Length;
		return this.arr[this.index];
	}

	// Token: 0x0400255B RID: 9563
	private T[] arr;

	// Token: 0x0400255C RID: 9564
	private int index;
}
