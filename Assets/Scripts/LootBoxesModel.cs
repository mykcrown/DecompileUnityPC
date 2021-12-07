// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class LootBoxesModel : ILootBoxesModel
{
	public static string SOURCE_UPDATED = "LootBoxesModel.SOURCE_UPDATED";

	public static string UPDATED = "LootBoxesModel.UPDATED";

	private Dictionary<ulong, LootBoxPackage> index = new Dictionary<ulong, LootBoxPackage>();

	private LootBoxPackage[] list = new LootBoxPackage[0];

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public ILootBoxesSource source
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(LootBoxesModel.SOURCE_UPDATED, new Action(this.writeIndeces));
		this.writeIndeces();
	}

	private void writeIndeces()
	{
		this.list = this.source.GetAllLootBoxes();
		List<LootBoxPackage> list = new List<LootBoxPackage>();
		LootBoxPackage[] array = this.list;
		for (int i = 0; i < array.Length; i++)
		{
			LootBoxPackage item = array[i];
			list.Add(item);
		}
		list.Sort(new Comparison<LootBoxPackage>(this.packageSort));
		this.list = list.ToArray();
		LootBoxPackage[] array2 = this.list;
		for (int j = 0; j < array2.Length; j++)
		{
			LootBoxPackage lootBoxPackage = array2[j];
			this.index[lootBoxPackage.packageId] = lootBoxPackage;
		}
		this.signalBus.Dispatch(LootBoxesModel.UPDATED);
	}

	private int packageSort(LootBoxPackage a, LootBoxPackage b)
	{
		return (int)a.price - (int)b.price;
	}

	public LootBoxPackage[] GetPackages()
	{
		return this.list;
	}

	public LootBoxPackage GetBoxToBuy(ulong packageId)
	{
		if (this.index.ContainsKey(packageId))
		{
			return this.index[packageId];
		}
		return null;
	}
}
