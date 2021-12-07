using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200073B RID: 1851
public class MockGetEquippedItemsFromServer : IGetEquippedItemsFromServer
{
	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x000E97B0 File Offset: 0x000E7BB0
	// (set) Token: 0x06002DC6 RID: 11718 RVA: 0x000E97B8 File Offset: 0x000E7BB8
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000B34 RID: 2868
	// (get) Token: 0x06002DC7 RID: 11719 RVA: 0x000E97C1 File Offset: 0x000E7BC1
	// (set) Token: 0x06002DC8 RID: 11720 RVA: 0x000E97C9 File Offset: 0x000E7BC9
	[Inject]
	public IUserTauntsModel userTauntsModel { get; set; }

	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x06002DC9 RID: 11721 RVA: 0x000E97D2 File Offset: 0x000E7BD2
	// (set) Token: 0x06002DCA RID: 11722 RVA: 0x000E97DA File Offset: 0x000E7BDA
	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel { get; set; }

	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x06002DCB RID: 11723 RVA: 0x000E97E3 File Offset: 0x000E7BE3
	// (set) Token: 0x06002DCC RID: 11724 RVA: 0x000E97EB File Offset: 0x000E7BEB
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B37 RID: 2871
	// (get) Token: 0x06002DCD RID: 11725 RVA: 0x000E97F4 File Offset: 0x000E7BF4
	public bool IsComplete
	{
		get
		{
			return this.isComplete;
		}
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x000E97FC File Offset: 0x000E7BFC
	private void setupDemoEquips()
	{
		CharacterID[] values = EnumUtil.GetValues<CharacterID>();
		foreach (CharacterID characterID in values)
		{
			if (characterID != CharacterID.Any && characterID != CharacterID.None)
			{
				Dictionary<TauntSlot, EquipmentID> slotsForCharacter = this.userTauntsModel.GetSlotsForCharacter(characterID, 100);
				switch (characterID)
				{
				case CharacterID.Kidd:
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.UP, EquipmentTypes.EMOTE, "Double Thumb");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.DOWN, EquipmentTypes.VOICE_TAUNT, "Shines Like Me");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.LEFT, EquipmentTypes.HOLOGRAM, "Kidd: Laughing");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.RIGHT, EquipmentTypes.HOLOGRAM, "Kidd: Pixelated");
					this.equipDemoItem(characterID, EquipmentTypes.VICTORY_POSE, "Suave");
					this.equipDemoItem(characterID, EquipmentTypes.PLATFORM, "Guardian");
					break;
				case CharacterID.Ashani:
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.UP, EquipmentTypes.EMOTE, "Met Your Match");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.DOWN, EquipmentTypes.VOICE_TAUNT, "Top Tier");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.LEFT, EquipmentTypes.HOLOGRAM, "Ashani: Wink");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.RIGHT, EquipmentTypes.HOLOGRAM, "Ashani: Shrug");
					this.equipDemoItem(characterID, EquipmentTypes.VICTORY_POSE, "Discharge");
					this.equipDemoItem(characterID, EquipmentTypes.PLATFORM, "Earth Tech");
					break;
				case CharacterID.Xana:
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.UP, EquipmentTypes.EMOTE, "Kiss");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.DOWN, EquipmentTypes.VOICE_TAUNT, "Don't get Grabbed");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.LEFT, EquipmentTypes.HOLOGRAM, "Xana: Smirk");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.RIGHT, EquipmentTypes.HOLOGRAM, "Xana: Sad");
					this.equipDemoItem(characterID, EquipmentTypes.VICTORY_POSE, "Get Some");
					this.equipDemoItem(characterID, EquipmentTypes.PLATFORM, "Arena Champion");
					break;
				case CharacterID.Raymer:
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.UP, EquipmentTypes.EMOTE, "Gun Toss");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.DOWN, EquipmentTypes.VOICE_TAUNT, "Lucky Shot");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.LEFT, EquipmentTypes.HOLOGRAM, "Raymer: Smirk");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.RIGHT, EquipmentTypes.HOLOGRAM, "Raymer: Sneer");
					this.equipDemoItem(characterID, EquipmentTypes.VICTORY_POSE, "Without Fail");
					this.equipDemoItem(characterID, EquipmentTypes.PLATFORM, "Merciless");
					break;
				case CharacterID.Zhurong:
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.UP, EquipmentTypes.EMOTE, "Yawn");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.DOWN, EquipmentTypes.VOICE_TAUNT, "All Will Bow before Me");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.LEFT, EquipmentTypes.HOLOGRAM, "Zhurong: Smug");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.RIGHT, EquipmentTypes.HOLOGRAM, "Zhurong: Laughing");
					this.equipDemoItem(characterID, EquipmentTypes.VICTORY_POSE, "Conductor");
					this.equipDemoItem(characterID, EquipmentTypes.PLATFORM, "Ceremony");
					break;
				case CharacterID.AfiGalu:
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.UP, EquipmentTypes.EMOTE, "Helicopter");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.DOWN, EquipmentTypes.VOICE_TAUNT, "Hello");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.LEFT, EquipmentTypes.HOLOGRAM, "Afigalu: Chibi");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.RIGHT, EquipmentTypes.HOLOGRAM, "A&G: Pixelated");
					this.equipDemoItem(characterID, EquipmentTypes.VICTORY_POSE, "Friendship");
					this.equipDemoItem(characterID, EquipmentTypes.PLATFORM, "Gravity Orb");
					break;
				case CharacterID.Weishan:
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.UP, EquipmentTypes.EMOTE, "Chest Pound");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.DOWN, EquipmentTypes.VOICE_TAUNT, "Bring me a Stronger Foe");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.LEFT, EquipmentTypes.HOLOGRAM, "Weishan: Beckon");
					this.mapTaunt(characterID, slotsForCharacter, TauntSlot.RIGHT, EquipmentTypes.HOLOGRAM, "Weishan: Thumbs Up");
					this.equipDemoItem(characterID, EquipmentTypes.VICTORY_POSE, "Dragon Stance");
					this.equipDemoItem(characterID, EquipmentTypes.PLATFORM, "Relic");
					break;
				}
			}
		}
	}

	// Token: 0x06002DCF RID: 11727 RVA: 0x000E9B10 File Offset: 0x000E7F10
	private void mapTaunt(CharacterID characterId, Dictionary<TauntSlot, EquipmentID> tauntMapping, TauntSlot slot, EquipmentTypes type, string backupNameText)
	{
		List<EquippableItem> list = new List<EquippableItem>(this.equipmentModel.GetAllCharacterItems(characterId));
		list.AddRange(this.equipmentModel.GetAllCharacterItems(CharacterID.Any));
		EquippableItem equippableItem = this.findItem(list, type, backupNameText);
		if (equippableItem != null)
		{
			tauntMapping[slot] = equippableItem.id;
		}
	}

	// Token: 0x06002DD0 RID: 11728 RVA: 0x000E9B64 File Offset: 0x000E7F64
	private void equipDemoItem(CharacterID characterId, EquipmentTypes type, string backupNameText)
	{
		List<EquippableItem> list = new List<EquippableItem>(this.equipmentModel.GetAllCharacterItems(characterId));
		list.AddRange(this.equipmentModel.GetAllCharacterItems(CharacterID.Any));
		EquippableItem equippableItem = this.findItem(list, type, backupNameText);
		if (equippableItem != null)
		{
			this.userCharacterEquippedModel.Equip(equippableItem, characterId, 100, false);
		}
	}

	// Token: 0x06002DD1 RID: 11729 RVA: 0x000E9BB8 File Offset: 0x000E7FB8
	private EquippableItem findItem(List<EquippableItem> items, EquipmentTypes type, string backupNameText)
	{
		foreach (EquippableItem equippableItem in items)
		{
			if (equippableItem.backupNameText == backupNameText && type == equippableItem.type)
			{
				return equippableItem;
			}
		}
		Debug.LogError("Failed to find " + type.ToString() + " item: " + backupNameText);
		return null;
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x000E9C54 File Offset: 0x000E8054
	public void MakeRequest()
	{
		if (!this.IsComplete)
		{
			this.setupDemoEquips();
			this.isComplete = true;
			this.signalBus.Dispatch(MockGetEquippedItemsFromServer.UPDATED);
		}
	}

	// Token: 0x04002062 RID: 8290
	public static string UPDATED = "GetEquippedItemsFromServer.UPDATE";

	// Token: 0x04002067 RID: 8295
	private bool isComplete;
}
