using System;
using System.Collections.Generic;

// Token: 0x02000760 RID: 1888
public class UserLootboxesModel : IUserLootboxesModel
{
	// Token: 0x17000B59 RID: 2905
	// (get) Token: 0x06002EB4 RID: 11956 RVA: 0x000EBCCB File Offset: 0x000EA0CB
	// (set) Token: 0x06002EB5 RID: 11957 RVA: 0x000EBCD3 File Offset: 0x000EA0D3
	[Inject]
	public IUserLootboxesSource userLootboxesSource { get; set; }

	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x000EBCDC File Offset: 0x000EA0DC
	// (set) Token: 0x06002EB7 RID: 11959 RVA: 0x000EBCE4 File Offset: 0x000EA0E4
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06002EB8 RID: 11960 RVA: 0x000EBCED File Offset: 0x000EA0ED
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserLootboxesModel.SOURCE_UPDATED, new Action(this.onSourceUpdate));
		this.onSourceUpdate();
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x000EBD14 File Offset: 0x000EA114
	private void onSourceUpdate()
	{
		Dictionary<int, int> lootBoxes = this.userLootboxesSource.GetLootBoxes();
		this.cache.Clear();
		foreach (KeyValuePair<int, int> keyValuePair in lootBoxes)
		{
			this.cache[keyValuePair.Key] = keyValuePair.Value;
		}
		this.signalBus.Dispatch(UserLootboxesModel.UPDATED);
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x000EBDA4 File Offset: 0x000EA1A4
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

	// Token: 0x06002EBB RID: 11963 RVA: 0x000EBDF8 File Offset: 0x000EA1F8
	public int GetNextBoxId()
	{
		using (Dictionary<int, int>.Enumerator enumerator = this.cache.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				KeyValuePair<int, int> keyValuePair = enumerator.Current;
				return keyValuePair.Key;
			}
		}
		return 0;
	}

	// Token: 0x06002EBC RID: 11964 RVA: 0x000EBE60 File Offset: 0x000EA260
	public int GetQuantity(int itemId)
	{
		if (this.cache.ContainsKey(itemId))
		{
			return this.cache[itemId];
		}
		return 0;
	}

	// Token: 0x06002EBD RID: 11965 RVA: 0x000EBE84 File Offset: 0x000EA284
	public int GetTotalQuantity()
	{
		int num = 0;
		foreach (KeyValuePair<int, int> keyValuePair in this.cache)
		{
			num += keyValuePair.Value;
		}
		return num;
	}

	// Token: 0x040020C2 RID: 8386
	public static string UPDATED = "UserLootboxesModel.UPDATED";

	// Token: 0x040020C3 RID: 8387
	public static string SOURCE_UPDATED = "UserLootboxesModel.SOURCE_UPDATED";

	// Token: 0x040020C6 RID: 8390
	private Dictionary<int, int> cache = new Dictionary<int, int>();
}
