using System;
using System.Collections.Generic;

// Token: 0x0200075B RID: 1883
public class LootBoxesModel : ILootBoxesModel
{
	// Token: 0x17000B57 RID: 2903
	// (get) Token: 0x06002EA1 RID: 11937 RVA: 0x000EBA47 File Offset: 0x000E9E47
	// (set) Token: 0x06002EA2 RID: 11938 RVA: 0x000EBA4F File Offset: 0x000E9E4F
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B58 RID: 2904
	// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x000EBA58 File Offset: 0x000E9E58
	// (set) Token: 0x06002EA4 RID: 11940 RVA: 0x000EBA60 File Offset: 0x000E9E60
	[Inject]
	public ILootBoxesSource source { get; set; }

	// Token: 0x06002EA5 RID: 11941 RVA: 0x000EBA69 File Offset: 0x000E9E69
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(LootBoxesModel.SOURCE_UPDATED, new Action(this.writeIndeces));
		this.writeIndeces();
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x000EBA90 File Offset: 0x000E9E90
	private void writeIndeces()
	{
		this.list = this.source.GetAllLootBoxes();
		List<LootBoxPackage> list = new List<LootBoxPackage>();
		foreach (LootBoxPackage item in this.list)
		{
			list.Add(item);
		}
		list.Sort(new Comparison<LootBoxPackage>(this.packageSort));
		this.list = list.ToArray();
		foreach (LootBoxPackage lootBoxPackage in this.list)
		{
			this.index[lootBoxPackage.packageId] = lootBoxPackage;
		}
		this.signalBus.Dispatch(LootBoxesModel.UPDATED);
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x000EBB44 File Offset: 0x000E9F44
	private int packageSort(LootBoxPackage a, LootBoxPackage b)
	{
		return (int)a.price - (int)b.price;
	}

	// Token: 0x06002EA8 RID: 11944 RVA: 0x000EBB55 File Offset: 0x000E9F55
	public LootBoxPackage[] GetPackages()
	{
		return this.list;
	}

	// Token: 0x06002EA9 RID: 11945 RVA: 0x000EBB5D File Offset: 0x000E9F5D
	public LootBoxPackage GetBoxToBuy(ulong packageId)
	{
		if (this.index.ContainsKey(packageId))
		{
			return this.index[packageId];
		}
		return null;
	}

	// Token: 0x040020B6 RID: 8374
	public static string SOURCE_UPDATED = "LootBoxesModel.SOURCE_UPDATED";

	// Token: 0x040020B7 RID: 8375
	public static string UPDATED = "LootBoxesModel.UPDATED";

	// Token: 0x040020BA RID: 8378
	private Dictionary<ulong, LootBoxPackage> index = new Dictionary<ulong, LootBoxPackage>();

	// Token: 0x040020BB RID: 8379
	private LootBoxPackage[] list = new LootBoxPackage[0];
}
