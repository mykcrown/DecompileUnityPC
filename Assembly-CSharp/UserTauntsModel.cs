using System;
using System.Collections.Generic;
using IconsServer;

// Token: 0x0200074D RID: 1869
public class UserTauntsModel : IUserTauntsModel, IStartupLoader
{
	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x06002E58 RID: 11864 RVA: 0x000EADD5 File Offset: 0x000E91D5
	// (set) Token: 0x06002E59 RID: 11865 RVA: 0x000EADDD File Offset: 0x000E91DD
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x06002E5A RID: 11866 RVA: 0x000EADE6 File Offset: 0x000E91E6
	// (set) Token: 0x06002E5B RID: 11867 RVA: 0x000EADEE File Offset: 0x000E91EE
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x06002E5C RID: 11868 RVA: 0x000EADF7 File Offset: 0x000E91F7
	// (set) Token: 0x06002E5D RID: 11869 RVA: 0x000EADFF File Offset: 0x000E91FF
	[Inject]
	public IUserTauntsSource userTauntsSource { get; set; }

	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x06002E5E RID: 11870 RVA: 0x000EAE08 File Offset: 0x000E9208
	// (set) Token: 0x06002E5F RID: 11871 RVA: 0x000EAE10 File Offset: 0x000E9210
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x06002E60 RID: 11872 RVA: 0x000EAE19 File Offset: 0x000E9219
	// (set) Token: 0x06002E61 RID: 11873 RVA: 0x000EAE21 File Offset: 0x000E9221
	[Inject]
	public ISaveFileData saveFileData { get; set; }

	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x06002E62 RID: 11874 RVA: 0x000EAE2A File Offset: 0x000E922A
	// (set) Token: 0x06002E63 RID: 11875 RVA: 0x000EAE32 File Offset: 0x000E9232
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x06002E64 RID: 11876 RVA: 0x000EAE3B File Offset: 0x000E923B
	[PostConstruct]
	public void Init()
	{
		this.index[100] = new UserTaunts(this.userTauntsSource.GetSourceData());
	}

	// Token: 0x06002E65 RID: 11877 RVA: 0x000EAE5C File Offset: 0x000E925C
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

	// Token: 0x06002E66 RID: 11878 RVA: 0x000EAE9B File Offset: 0x000E929B
	public UserTaunts GetDataObject(int portId)
	{
		return this.getMap(portId);
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x000EAEA4 File Offset: 0x000E92A4
	public Dictionary<TauntSlot, EquipmentID> GetSlotsForCharacter(CharacterID characterId, int portId)
	{
		return this.getMap(portId).GetSlotsForCharacter(characterId);
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x000EAEB3 File Offset: 0x000E92B3
	public EquipmentID GetItemInSlot(CharacterID characterId, TauntSlot slot, int portId)
	{
		return this.getMap(portId).GetItemInSlot(characterId, slot);
	}

	// Token: 0x06002E69 RID: 11881 RVA: 0x000EAEC3 File Offset: 0x000E92C3
	public void Save(CharacterID characterId, SerializableDictionary<TauntSlot, EquipmentID> data, int portId, bool alertServer = true)
	{
		this.getMap(portId).Copy(characterId, data, (!alertServer) ? null : this.iconsServerAPI);
		this.saveToDisk();
		this.signalBus.Dispatch(UserTauntsModel.UPDATED);
	}

	// Token: 0x06002E6A RID: 11882 RVA: 0x000EAEFC File Offset: 0x000E92FC
	public bool IsEquipped(EquippableItem item, CharacterID characterId, int portId)
	{
		return this.getMap(portId).IsEquipped(item, characterId);
	}

	// Token: 0x06002E6B RID: 11883 RVA: 0x000EAF0C File Offset: 0x000E930C
	public void StartupLoad(Action callback)
	{
		this.index = this.saveFileData.GetFromFile<UserTauntsIndex>(UserTauntsModel.FILENAME);
		if (this.index == null)
		{
			this.index = new UserTauntsIndex();
		}
		foreach (KeyValuePair<int, UserTaunts> keyValuePair in this.index)
		{
			this.ensureDefaults(keyValuePair.Value);
		}
		callback();
	}

	// Token: 0x06002E6C RID: 11884 RVA: 0x000EAFA0 File Offset: 0x000E93A0
	private void ensureDefaults(UserTaunts userTaunts)
	{
		bool flag = false;
		foreach (CharacterDefinition characterDefinition in this.characterLists.GetNonRandomCharacters())
		{
			CharacterID characterID = characterDefinition.characterID;
			SerializableDictionary<TauntSlot, EquipmentID> serializableDictionary;
			userTaunts.TryGetValue(characterID, out serializableDictionary);
			if (serializableDictionary == null)
			{
				userTaunts[characterID] = new SerializableDictionary<TauntSlot, EquipmentID>();
				serializableDictionary = userTaunts[characterID];
			}
			foreach (KeyValuePair<TauntSlot, EquipmentID> keyValuePair in this.defaultTaunts)
			{
				EquipmentID equipmentID;
				serializableDictionary.TryGetValue(keyValuePair.Key, out equipmentID);
				if (equipmentID.IsNull())
				{
					serializableDictionary[keyValuePair.Key] = keyValuePair.Value;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.saveToDisk();
		}
	}

	// Token: 0x06002E6D RID: 11885 RVA: 0x000EB090 File Offset: 0x000E9490
	private void saveToDisk()
	{
		this.saveFileData.SaveToFile<UserTauntsIndex>(UserTauntsModel.FILENAME, this.index);
	}

	// Token: 0x04002095 RID: 8341
	public static string UPDATED = "UserTauntsModel.UPDATED";

	// Token: 0x04002096 RID: 8342
	private static string FILENAME = "tauntEquips_local";

	// Token: 0x0400209D RID: 8349
	private UserTauntsIndex index = new UserTauntsIndex();

	// Token: 0x0400209E RID: 8350
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
}
