// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataStore<T> where T : IGameDataElement
{
	protected Dictionary<int, T> idMap = new Dictionary<int, T>();

	private ILocalization localization;

	public List<T> DataList
	{
		get;
		private set;
	}

	public GameDataStore(ILocalization localization)
	{
		this.localization = localization;
	}

	public void Load(T[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i] != null && data[i].Enabled)
			{
				this.addDataElement(data[i]);
			}
		}
		this.DataList = data.ToList<T>();
	}

	public void Load(List<T> data)
	{
		for (int i = 0; i < data.Count; i++)
		{
			if (data[i] != null)
			{
				T t = data[i];
				if (t.Enabled)
				{
					this.addDataElement(data[i]);
				}
			}
		}
		this.DataList = data;
	}

	protected virtual void addDataElement(T dataElement)
	{
		if (dataElement == null)
		{
			UnityEngine.Debug.LogError("Null data element detected of type " + typeof(T));
			return;
		}
		if (this.idMap.ContainsKey(dataElement.ID))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Duplicate data element ID ",
				dataElement.ID,
				" Name: ",
				dataElement.Key,
				" type ",
				typeof(T)
			}));
			return;
		}
		this.idMap.Add(dataElement.ID, dataElement);
		if (dataElement.Localization != null && this.localization != null)
		{
			this.localization.AddLocalizationData(dataElement.Localization);
		}
	}

	public T GetDataByID(int ID)
	{
		if (!this.idMap.ContainsKey(ID))
		{
			return default(T);
		}
		return this.idMap[ID];
	}

	public bool ContainsData(int ID)
	{
		return this.idMap.ContainsKey(ID);
	}

	public List<T> GetDataByIDs(List<int> IDs)
	{
		List<T> list = new List<T>();
		this.GetDataByIDs(IDs, list);
		return list;
	}

	public void GetDataByIDs(List<int> IDs, List<T> elements)
	{
		elements.Clear();
		for (int i = 0; i < IDs.Count; i++)
		{
			elements.Add(this.GetDataByID(IDs[i]));
		}
	}

	public void GetAllIDs(List<int> IDs)
	{
		IDs.Clear();
		for (int i = 0; i < this.DataList.Count; i++)
		{
			T t = this.DataList[i];
			IDs.Add(t.ID);
		}
	}
}
