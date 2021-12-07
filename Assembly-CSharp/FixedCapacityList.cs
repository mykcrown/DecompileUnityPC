using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AAA RID: 2730
[Serializable]
public class FixedCapacityList<T> : ICustomList<T>, ICustomList
{
	// Token: 0x0600502B RID: 20523 RVA: 0x0014E602 File Offset: 0x0014CA02
	public FixedCapacityList(int capacity)
	{
		this.list = new List<T>(capacity);
	}

	// Token: 0x0600502C RID: 20524 RVA: 0x0014E616 File Offset: 0x0014CA16
	public FixedCapacityList(FixedCapacityList<T> other)
	{
		this.list = new List<T>(other.list);
	}

	// Token: 0x0600502D RID: 20525 RVA: 0x0014E62F File Offset: 0x0014CA2F
	public FixedCapacityList(List<T> other)
	{
		this.list = new List<T>(other);
	}

	// Token: 0x0600502E RID: 20526 RVA: 0x0014E643 File Offset: 0x0014CA43
	public FixedCapacityList(IEnumerable<T> collection)
	{
		this.list = new List<T>(collection);
	}

	// Token: 0x170012E6 RID: 4838
	// (get) Token: 0x0600502F RID: 20527 RVA: 0x0014E657 File Offset: 0x0014CA57
	public List<T> List
	{
		get
		{
			return this.list;
		}
	}

	// Token: 0x06005030 RID: 20528 RVA: 0x0014E65F File Offset: 0x0014CA5F
	public List<T> GetEnumerableList()
	{
		return this.list;
	}

	// Token: 0x06005031 RID: 20529 RVA: 0x0014E667 File Offset: 0x0014CA67
	public IEnumerator ManualGetEnumerator()
	{
		return this.list.GetEnumerator();
	}

	// Token: 0x06005032 RID: 20530 RVA: 0x0014E67C File Offset: 0x0014CA7C
	public void CopyTo(FixedCapacityList<T> targetList)
	{
		if (targetList.List.Capacity < this.List.Capacity)
		{
			throw new UnityException("Insufficient capacity!");
		}
		targetList.Clear();
		int count = this.Count;
		for (int i = 0; i < count; i++)
		{
			targetList.Add(this[i]);
		}
	}

	// Token: 0x06005033 RID: 20531 RVA: 0x0014E6DC File Offset: 0x0014CADC
	public void Add(T item)
	{
		int capacity = this.list.Capacity;
		this.list.Add(item);
		if (this.list.Capacity > capacity)
		{
			GameClient.Log(LogLevel.Error, new object[]
			{
				string.Concat(new object[]
				{
					"FixedCapacityList.Add has allocated! ",
					capacity,
					" -> ",
					this.list.Capacity
				})
			});
		}
	}

	// Token: 0x06005034 RID: 20532 RVA: 0x0014E758 File Offset: 0x0014CB58
	public void Clear()
	{
		this.list.Clear();
	}

	// Token: 0x06005035 RID: 20533 RVA: 0x0014E765 File Offset: 0x0014CB65
	public bool Contains(T item)
	{
		return this.list.Contains(item);
	}

	// Token: 0x06005036 RID: 20534 RVA: 0x0014E773 File Offset: 0x0014CB73
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.list.CopyTo(array, arrayIndex);
	}

	// Token: 0x06005037 RID: 20535 RVA: 0x0014E782 File Offset: 0x0014CB82
	public bool Remove(T item)
	{
		return this.list.Remove(item);
	}

	// Token: 0x170012E7 RID: 4839
	// (get) Token: 0x06005038 RID: 20536 RVA: 0x0014E790 File Offset: 0x0014CB90
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x170012E8 RID: 4840
	// (get) Token: 0x06005039 RID: 20537 RVA: 0x0014E79D File Offset: 0x0014CB9D
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600503A RID: 20538 RVA: 0x0014E7A0 File Offset: 0x0014CBA0
	public int IndexOf(T item)
	{
		return this.list.IndexOf(item);
	}

	// Token: 0x0600503B RID: 20539 RVA: 0x0014E7B0 File Offset: 0x0014CBB0
	public void Insert(int index, T item)
	{
		int capacity = this.list.Capacity;
		this.list.Insert(index, item);
		if (this.list.Capacity > capacity)
		{
			GameClient.Log(LogLevel.Error, new object[]
			{
				string.Concat(new object[]
				{
					"FixedCapacityList.Insert has allocated! ",
					capacity,
					" -> ",
					this.list.Capacity
				})
			});
		}
	}

	// Token: 0x0600503C RID: 20540 RVA: 0x0014E830 File Offset: 0x0014CC30
	public void RemoveAt(int index)
	{
		int capacity = this.list.Capacity;
		this.list.RemoveAt(index);
		if (this.list.Capacity < capacity)
		{
			GameClient.Log(LogLevel.Error, new object[]
			{
				string.Concat(new object[]
				{
					"FixedCapacityList.RemoveAt has shrunk! ",
					capacity,
					" -> ",
					this.list.Capacity
				})
			});
		}
	}

	// Token: 0x170012E9 RID: 4841
	public T this[int index]
	{
		get
		{
			return this.list[index];
		}
		set
		{
			this.list[index] = value;
		}
	}

	// Token: 0x040033B0 RID: 13232
	[SerializeField]
	private List<T> list;
}
