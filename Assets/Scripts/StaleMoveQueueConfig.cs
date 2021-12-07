// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class StaleMoveQueueConfig
{
	public int[] queueReductionPercent = new int[0];

	public int queueSize
	{
		get
		{
			return this.queueReductionPercent.Length;
		}
	}

	public void Resize(int size)
	{
		List<int> list = new List<int>(this.queueReductionPercent);
		if (list.Count < size)
		{
			for (int i = list.Count; i < size; i++)
			{
				list.Add(0);
			}
		}
		else if (list.Count > size)
		{
			list.RemoveRange(size, list.Count - size);
		}
		this.queueReductionPercent = list.ToArray();
	}
}
