using System;
using Commerce;
using IconsServer;
using IconsTool;
using UnityEngine;

// Token: 0x02000767 RID: 1895
public class PackageApplier
{
	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x06002EDF RID: 11999 RVA: 0x000EC80C File Offset: 0x000EAC0C
	// (set) Token: 0x06002EE0 RID: 12000 RVA: 0x000EC814 File Offset: 0x000EAC14
	[Inject]
	public IStaticDataSource staticDataSource { get; set; }

	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x06002EE1 RID: 12001 RVA: 0x000EC81D File Offset: 0x000EAC1D
	// (set) Token: 0x06002EE2 RID: 12002 RVA: 0x000EC825 File Offset: 0x000EAC25
	[Inject]
	public IUserCurrencyModel userCurrencyModel { get; set; }

	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x06002EE3 RID: 12003 RVA: 0x000EC82E File Offset: 0x000EAC2E
	// (set) Token: 0x06002EE4 RID: 12004 RVA: 0x000EC836 File Offset: 0x000EAC36
	[Inject]
	public IUserLootboxesModel userLootboxesModel { get; set; }

	// Token: 0x06002EE5 RID: 12005 RVA: 0x000EC840 File Offset: 0x000EAC40
	public void ApplyPackageToUser(ulong packageId)
	{
		StaticDataReadOnly.StaticPackageData localPackageData = this.staticDataSource.GetLocalPackageData((int)packageId);
		if (localPackageData != null)
		{
			foreach (StaticDataReadOnly.StaticPackageItemData staticPackageItemData in localPackageData.m_packageItemList)
			{
				if (staticPackageItemData.m_classificationType == EClassification.Item)
				{
					StaticDataReadOnly.StaticItemBase localItemData = this.staticDataSource.GetLocalItemData((int)staticPackageItemData.m_staticId);
					if (localItemData is StaticDataReadOnly.StaticItemCurrency)
					{
						StaticDataReadOnly.StaticItemCurrency staticItemCurrency = localItemData as StaticDataReadOnly.StaticItemCurrency;
						ECurrencyType currencyType = staticItemCurrency.m_currencyType;
						if (currencyType != ECurrencyType.Soft)
						{
							if (currencyType != ECurrencyType.SteamMicro)
							{
							}
						}
						else
						{
							this.userCurrencyModel.Spectra += (int)staticItemCurrency.m_currencyAmount;
						}
					}
					else
					{
						Debug.LogError("Package item class `" + localItemData.GetType() + "` has not had an apply implemented yet. Add it in this function.");
					}
				}
				else if (staticPackageItemData.m_classificationType == EClassification.LootBox)
				{
					this.userLootboxesModel.Add((int)staticPackageItemData.m_staticId, 1);
				}
				else
				{
					Debug.LogError("Package item type `" + staticPackageItemData.m_classificationType + "` has not had an apply implemented yet. Add it in this function.");
				}
			}
		}
		else
		{
			Debug.LogError("Could not apply this package since no package was found with the package Id: " + packageId);
		}
	}
}
