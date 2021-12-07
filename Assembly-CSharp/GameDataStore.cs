using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class GameDataStore<T> where T : IGameDataElement
{
	// Token: 0x060015D9 RID: 5593 RVA: 0x00077751 File Offset: 0x00075B51
	public GameDataStore(ILocalization localization)
	{
		this.localization = localization;
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x060015DA RID: 5594 RVA: 0x0007776B File Offset: 0x00075B6B
	// (set) Token: 0x060015DB RID: 5595 RVA: 0x00077773 File Offset: 0x00075B73
	public List<T> DataList { get; private set; }

	// Token: 0x060015DC RID: 5596 RVA: 0x0007777C File Offset: 0x00075B7C
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

	// Token: 0x060015DD RID: 5597 RVA: 0x000777E0 File Offset: 0x00075BE0
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

	// Token: 0x060015DE RID: 5598 RVA: 0x00077844 File Offset: 0x00075C44
	protected virtual void addDataElement(T dataElement)
	{
		if (dataElement == null)
		{
			Debug.LogError("Null data element detected of type " + typeof(T));
			return;
		}
		if (this.idMap.ContainsKey(dataElement.ID))
		{
			Debug.LogError(string.Concat(new object[]
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

	// Token: 0x060015DF RID: 5599 RVA: 0x0007793C File Offset: 0x00075D3C
	public T GetDataByID(int ID)
	{
		if (!this.idMap.ContainsKey(ID))
		{
			return default(T);
		}
		return this.idMap[ID];
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x00077970 File Offset: 0x00075D70
	public bool ContainsData(int ID)
	{
		return this.idMap.ContainsKey(ID);
	}

	// Token: 0x060015E1 RID: 5601 RVA: 0x00077980 File Offset: 0x00075D80
	public List<T> GetDataByIDs(List<int> IDs)
	{
		List<T> list = new List<T>();
		this.GetDataByIDs(IDs, list);
		return list;
	}

	// Token: 0x060015E2 RID: 5602 RVA: 0x0007799C File Offset: 0x00075D9C
	public void GetDataByIDs(List<int> IDs, List<T> elements)
	{
		elements.Clear();
		for (int i = 0; i < IDs.Count; i++)
		{
			elements.Add(this.GetDataByID(IDs[i]));
		}
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x000779DC File Offset: 0x00075DDC
	public void GetAllIDs(List<int> IDs)
	{
		IDs.Clear();
		for (int i = 0; i < this.DataList.Count; i++)
		{
			T t = this.DataList[i];
			IDs.Add(t.ID);
		}
	}

	// Token: 0x040010B2 RID: 4274
	protected Dictionary<int, T> idMap = new Dictionary<int, T>();

	// Token: 0x040010B4 RID: 4276
	private ILocalization localization;
}
