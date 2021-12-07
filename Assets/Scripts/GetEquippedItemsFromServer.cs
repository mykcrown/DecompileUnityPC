// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;

public class GetEquippedItemsFromServer : IGetEquippedItemsFromServer
{
	public static string UPDATED = "GetEquippedItemsFromServer.UPDATE";

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IUserTauntsModel userTauntsModel
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel
	{
		get;
		set;
	}

	[Inject]
	public IUserGlobalEquippedModel userGlobalEquippedModel
	{
		get;
		set;
	}

	[Inject]
	public IStaticDataSource staticDataSource
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

	[Inject]
	public IEquipMethodMap equipMethodMap
	{
		get;
		set;
	}

	public bool IsComplete
	{
		get
		{
			return true;
		}
	}

	[PostConstruct]
	public void Init()
	{
	}

	public void MakeRequest()
	{
	}
}
