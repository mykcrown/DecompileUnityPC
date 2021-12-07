// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class UserLootboxesModel : IUserLootboxesModel
{
	public static string UPDATED = "UserLootboxesModel.UPDATED";

	public static string SOURCE_UPDATED = "UserLootboxesModel.SOURCE_UPDATED";

	private Dictionary<int, int> cache = new Dictionary<int, int>();

	[Inject]
	public IUserLootboxesSource userLootboxesSource
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserLootboxesModel.SOURCE_UPDATED, new Action(this.onSourceUpdate));
		this.onSourceUpdate();
	}

	private void onSourceUpdate()
	{
		Dictionary<int, int> lootBoxes = this.userLootboxesSource.GetLootBoxes();
		this.cache.Clear();
		foreach (KeyValuePair<int, int> current in lootBoxes)
		{
			this.cache[current.Key] = current.Value;
		}
		this.signalBus.Dispatch(UserLootboxesModel.UPDATED);
	}

	public void Add(int type, int quantity)
	{
		if (!this.cache.ContainsKey(type))
		{
			this.cache[type] = 0;
		}
		Dictionary<int, int> dictionary;
		(dictionary = this.cache)[type] = dictionary[type] + quantity;
		this.signalBus.Dispatch(UserLootboxesModel.UPDATED);
	}

	public int GetNextBoxId()
	{
		using (Dictionary<int, int>.Enumerator enumerator = this.cache.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				KeyValuePair<int, int> current = enumerator.Current;
				return current.Key;
			}
		}
		return 0;
	}

	public int GetQuantity(int itemId)
	{
		if (this.cache.ContainsKey(itemId))
		{
			return this.cache[itemId];
		}
		return 0;
	}

	public int GetTotalQuantity()
	{
		int num = 0;
		foreach (KeyValuePair<int, int> current in this.cache)
		{
			num += current.Value;
		}
		return num;
	}
}
