// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;

public class UserTauntsModel : IUserTauntsModel, IStartupLoader
{
	public static string UPDATED = "UserTauntsModel.UPDATED";

	private static string FILENAME = "tauntEquips_local";

	private UserTauntsIndex index = new UserTauntsIndex();

	private Dictionary<TauntSlot, EquipmentID> defaultTaunts = new Dictionary<TauntSlot, EquipmentID>(default(TauntSlotComparer))
	{
		{
			TauntSlot.UP,
			new EquipmentID(41L)
		},
		{
			TauntSlot.DOWN,
			new EquipmentID(44L)
		},
		{
			TauntSlot.LEFT,
			new EquipmentID(45L)
		},
		{
			TauntSlot.RIGHT,
			new EquipmentID(49L)
		}
	};

	[Inject]
	public IMainThreadTimer timer
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
	public IUserTauntsSource userTauntsSource
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
	public ISaveFileData saveFileData
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		private get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.index[100] = new UserTaunts(this.userTauntsSource.GetSourceData());
	}

	private UserTaunts getMap(int portId)
	{
		UserTaunts result;
		if (!this.index.TryGetValue(portId, out result))
		{
			this.index[portId] = new UserTaunts();
			result = this.index[portId];
		}
		return result;
	}

	public UserTaunts GetDataObject(int portId)
	{
		return this.getMap(portId);
	}

	public Dictionary<TauntSlot, EquipmentID> GetSlotsForCharacter(CharacterID characterId, int portId)
	{
		return this.getMap(portId).GetSlotsForCharacter(characterId);
	}

	public EquipmentID GetItemInSlot(CharacterID characterId, TauntSlot slot, int portId)
	{
		return this.getMap(portId).GetItemInSlot(characterId, slot);
	}

	public void Save(CharacterID characterId, SerializableDictionary<TauntSlot, EquipmentID> data, int portId, bool alertServer = true)
	{
		this.getMap(portId).Copy(characterId, data, (!alertServer) ? null : this.iconsServerAPI);
		this.saveToDisk();
		this.signalBus.Dispatch(UserTauntsModel.UPDATED);
	}

	public bool IsEquipped(EquippableItem item, CharacterID characterId, int portId)
	{
		return this.getMap(portId).IsEquipped(item, characterId);
	}

	public void StartupLoad(Action callback)
	{
		this.index = this.saveFileData.GetFromFile<UserTauntsIndex>(UserTauntsModel.FILENAME);
		if (this.index == null)
		{
			this.index = new UserTauntsIndex();
		}
		foreach (KeyValuePair<int, UserTaunts> current in this.index)
		{
			this.ensureDefaults(current.Value);
		}
		callback();
	}

	private void ensureDefaults(UserTaunts userTaunts)
	{
		bool flag = false;
		CharacterDefinition[] nonRandomCharacters = this.characterLists.GetNonRandomCharacters();
		for (int i = 0; i < nonRandomCharacters.Length; i++)
		{
			CharacterDefinition characterDefinition = nonRandomCharacters[i];
			CharacterID characterID = characterDefinition.characterID;
			SerializableDictionary<TauntSlot, EquipmentID> serializableDictionary;
			userTaunts.TryGetValue(characterID, out serializableDictionary);
			if (serializableDictionary == null)
			{
				userTaunts[characterID] = new SerializableDictionary<TauntSlot, EquipmentID>();
				serializableDictionary = userTaunts[characterID];
			}
			foreach (KeyValuePair<TauntSlot, EquipmentID> current in this.defaultTaunts)
			{
				EquipmentID equipmentID;
				serializableDictionary.TryGetValue(current.Key, out equipmentID);
				if (equipmentID.IsNull())
				{
					serializableDictionary[current.Key] = current.Value;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.saveToDisk();
		}
	}

	private void saveToDisk()
	{
		this.saveFileData.SaveToFile<UserTauntsIndex>(UserTauntsModel.FILENAME, this.index);
	}
}
