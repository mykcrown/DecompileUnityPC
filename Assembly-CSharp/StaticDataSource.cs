using System;
using IconsTool;
using UnityEngine;

// Token: 0x02000735 RID: 1845
public class StaticDataSource : IStaticDataSource
{
	// Token: 0x17000B22 RID: 2850
	// (get) Token: 0x06002D8F RID: 11663 RVA: 0x000E93BC File Offset: 0x000E77BC
	// (set) Token: 0x06002D90 RID: 11664 RVA: 0x000E93C4 File Offset: 0x000E77C4
	[Inject]
	public IServerConnectionManager serverConnectionManager { get; set; }

	// Token: 0x06002D91 RID: 11665 RVA: 0x000E93CD File Offset: 0x000E77CD
	public ulong GetChecksum()
	{
		this.ValidateStaticData();
		if (this.cache == null)
		{
			Debug.LogError("STATIC DB ERROR, local checksum failed");
			return 0UL;
		}
		return this.cache.StaticChecksum;
	}

	// Token: 0x06002D92 RID: 11666 RVA: 0x000E93FC File Offset: 0x000E77FC
	public bool ValidateStaticData()
	{
		if (this.cache == null)
		{
			TextAsset textAsset = (TextAsset)Resources.Load("StaticDb");
			StaticDataReadOnly.StaticDataReader staticDataReader = new StaticDataReadOnly.StaticDataReader();
			if (!staticDataReader.LoadFromText(textAsset.text, true))
			{
				return false;
			}
			this.cache = staticDataReader;
		}
		return true;
	}

	// Token: 0x06002D93 RID: 11667 RVA: 0x000E9448 File Offset: 0x000E7848
	private StaticDataReadOnly.StaticDataReader getStaticData()
	{
		this.ValidateStaticData();
		return this.cache;
	}

	// Token: 0x06002D94 RID: 11668 RVA: 0x000E9458 File Offset: 0x000E7858
	public StaticDataReadOnly.StaticItemBase GetLocalItemData(int id)
	{
		StaticDataReadOnly.StaticDataReader staticData = this.getStaticData();
		StaticDataReadOnly.StaticItemBase result;
		staticData.ItemMap.TryGetValue((ulong)((long)id), out result);
		return result;
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x000E9480 File Offset: 0x000E7880
	public StaticDataReadOnly.StaticPackageData GetLocalPackageData(int packageId)
	{
		StaticDataReadOnly.StaticDataReader staticData = this.getStaticData();
		StaticDataReadOnly.StaticPackageData result;
		staticData.PackageMap.TryGetValue((ulong)((long)packageId), out result);
		return result;
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x000E94A8 File Offset: 0x000E78A8
	public ulong GetSpectraInPackage(int packageId)
	{
		if (packageId == 0)
		{
			return 0UL;
		}
		StaticDataReadOnly.StaticPackageData localPackageData = this.GetLocalPackageData(packageId);
		if (localPackageData == null)
		{
			return 0UL;
		}
		ulong num = 0UL;
		foreach (StaticDataReadOnly.StaticPackageItemData staticPackageItemData in localPackageData.m_packageItemList)
		{
			StaticDataReadOnly.StaticItemCurrency staticItemCurrency = this.GetLocalItemData((int)staticPackageItemData.m_staticId) as StaticDataReadOnly.StaticItemCurrency;
			if (staticItemCurrency != null)
			{
				num += staticItemCurrency.m_currencyAmount;
			}
		}
		return num;
	}

	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x06002D97 RID: 11671 RVA: 0x000E9540 File Offset: 0x000E7940
	public ulong CharacterUnlockTokenId
	{
		get
		{
			StaticDataReadOnly.StaticDataReader staticData = this.getStaticData();
			return staticData.CharacterUnlockTokenId;
		}
	}

	// Token: 0x04002052 RID: 8274
	private StaticDataReadOnly.StaticDataReader cache;
}
