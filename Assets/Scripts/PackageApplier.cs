// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using IconsServer;
using IconsTool;
using System;
using UnityEngine;

public class PackageApplier
{
	[Inject]
	public IStaticDataSource staticDataSource
	{
		get;
		set;
	}

	[Inject]
	public IUserCurrencyModel userCurrencyModel
	{
		get;
		set;
	}

	[Inject]
	public IUserLootboxesModel userLootboxesModel
	{
		get;
		set;
	}

	public void ApplyPackageToUser(ulong packageId)
	{
		StaticDataReadOnly.StaticPackageData localPackageData = this.staticDataSource.GetLocalPackageData((int)packageId);
		if (localPackageData != null)
		{
			foreach (StaticDataReadOnly.StaticPackageItemData current in localPackageData.m_packageItemList)
			{
				if (current.m_classificationType == EClassification.Item)
				{
					StaticDataReadOnly.StaticItemBase localItemData = this.staticDataSource.GetLocalItemData((int)current.m_staticId);
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
						UnityEngine.Debug.LogError("Package item class `" + localItemData.GetType() + "` has not had an apply implemented yet. Add it in this function.");
					}
				}
				else if (current.m_classificationType == EClassification.LootBox)
				{
					this.userLootboxesModel.Add((int)current.m_staticId, 1);
				}
				else
				{
					UnityEngine.Debug.LogError("Package item type `" + current.m_classificationType + "` has not had an apply implemented yet. Add it in this function.");
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Could not apply this package since no package was found with the package Id: " + packageId);
		}
	}
}
