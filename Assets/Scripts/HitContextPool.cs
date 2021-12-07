// Decompile from assembly: Assembly-CSharp.dll

using System;

public class HitContextPool : IHitContextPool
{
	private HitContext[] arr = new HitContext[256];

	private int index;

	public HitContextPool()
	{
		for (int i = 0; i < this.arr.Length; i++)
		{
			this.arr[i] = new HitContext();
		}
	}

	public HitContext GetNext()
	{
		this.index = (this.index + 1) % this.arr.Length;
		this.arr[this.index].Clear();
		return this.arr[this.index];
	}
}
