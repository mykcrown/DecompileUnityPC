// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MockGetEquippedItemsFromServer : IGetEquippedItemsFromServer
{
	public static string UPDATED = "GetEquippedItemsFromServer.UPDATE";

	private bool isComplete;

	[Inject]
	public IEquipmentModel equipmentModel
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
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public bool IsComplete
	{
		get
		{
			return this.isComplete;
		}
	}

	private void setupDemoEquips()
	{
		CharacterID[] values = EnumUtil.GetValues<CharacterID>();
		CharacterID[] array = values;
		for (int i = 0; i < array.Length; i++)
		{
			CharacterID characterID = array[i];
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

	private EquippableItem findItem(List<EquippableItem> items, EquipmentTypes type, string backupNameText)
	{
		foreach (EquippableItem current in items)
		{
			if (current.backupNameText == backupNameText && type == current.type)
			{
				return current;
			}
		}
		UnityEngine.Debug.LogError("Failed to find " + type.ToString() + " item: " + backupNameText);
		return null;
	}

	public void MakeRequest()
	{
		if (!this.IsComplete)
		{
			this.setupDemoEquips();
			this.isComplete = true;
			this.signalBus.Dispatch(MockGetEquippedItemsFromServer.UPDATED);
		}
	}
}
