// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FixedCapacityList<T> : ICustomList<T>, ICustomList
{
	[SerializeField]
	private List<T> list;

	public List<T> List
	{
		get
		{
			return this.list;
		}
	}

	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

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

	public FixedCapacityList(int capacity)
	{
		this.list = new List<T>(capacity);
	}

	public FixedCapacityList(FixedCapacityList<T> other)
	{
		this.list = new List<T>(other.list);
	}

	public FixedCapacityList(List<T> other)
	{
		this.list = new List<T>(other);
	}

	public FixedCapacityList(IEnumerable<T> collection)
	{
		this.list = new List<T>(collection);
	}

	public List<T> GetEnumerableList()
	{
		return this.list;
	}

	public IEnumerator ManualGetEnumerator()
	{
		return this.list.GetEnumerator();
	}

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

	public void Clear()
	{
		this.list.Clear();
	}

	public bool Contains(T item)
	{
		return this.list.Contains(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		this.list.CopyTo(array, arrayIndex);
	}

	public bool Remove(T item)
	{
		return this.list.Remove(item);
	}

	public int IndexOf(T item)
	{
		return this.list.IndexOf(item);
	}

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
}
