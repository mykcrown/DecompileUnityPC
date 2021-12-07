// Decompile from assembly: Assembly-CSharp.dll

using IconsTool;
using System;
using UnityEngine;

public class StaticDataSource : IStaticDataSource
{
	private StaticDataReadOnly.StaticDataReader cache;

	[Inject]
	public IServerConnectionManager serverConnectionManager
	{
		get;
		set;
	}

	public ulong CharacterUnlockTokenId
	{
		get
		{
			StaticDataReadOnly.StaticDataReader staticData = this.getStaticData();
			return staticData.CharacterUnlockTokenId;
		}
	}

	public ulong GetChecksum()
	{
		this.ValidateStaticData();
		if (this.cache == null)
		{
			UnityEngine.Debug.LogError("STATIC DB ERROR, local checksum failed");
			return 0uL;
		}
		return this.cache.StaticChecksum;
	}

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

	private StaticDataReadOnly.StaticDataReader getStaticData()
	{
		this.ValidateStaticData();
		return this.cache;
	}

	public StaticDataReadOnly.StaticItemBase GetLocalItemData(int id)
	{
		StaticDataReadOnly.StaticDataReader staticData = this.getStaticData();
		StaticDataReadOnly.StaticItemBase result;
		staticData.ItemMap.TryGetValue((ulong)((long)id), out result);
		return result;
	}

	public StaticDataReadOnly.StaticPackageData GetLocalPackageData(int packageId)
	{
		StaticDataReadOnly.StaticDataReader staticData = this.getStaticData();
		StaticDataReadOnly.StaticPackageData result;
		staticData.PackageMap.TryGetValue((ulong)((long)packageId), out result);
		return result;
	}

	public ulong GetSpectraInPackage(int packageId)
	{
		if (packageId == 0)
		{
			return 0uL;
		}
		StaticDataReadOnly.StaticPackageData localPackageData = this.GetLocalPackageData(packageId);
		if (localPackageData == null)
		{
			return 0uL;
		}
		ulong num = 0uL;
		foreach (StaticDataReadOnly.StaticPackageItemData current in localPackageData.m_packageItemList)
		{
			StaticDataReadOnly.StaticItemCurrency staticItemCurrency = this.GetLocalItemData((int)current.m_staticId) as StaticDataReadOnly.StaticItemCurrency;
			if (staticItemCurrency != null)
			{
				num += staticItemCurrency.m_currencyAmount;
			}
		}
		return num;
	}
}
