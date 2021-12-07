// Decompile from assembly: Assembly-CSharp.dll

using System;

public class StatePool<T> : IStatePool<T>, IStatePool where T : ICopyable<T>, new()
{
	private T[] arr;

	private int index;

	public StatePool(int poolMulti)
	{
		this.arr = new T[poolMulti * RollbackStatePooling.DEFAULT_BUFFER_SIZE];
		for (int i = 0; i < this.arr.Length; i++)
		{
			this.arr[i] = Activator.CreateInstance<T>();
		}
	}

	public T GetNext()
	{
		this.index = (this.index + 1) % this.arr.Length;
		return this.arr[this.index];
	}
}
