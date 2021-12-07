// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class CharacterEquipViewAPI : ICharacterEquipViewAPI
{
	public static string UPDATED = "CharacterEquipViewAPI.UPDATED";

	private CharacterID[] characters;

	private EquipmentTypes[] validEquipmentTypes;

	private CharacterID selectedChar;

	private int selectedCharIndex;

	[Inject]
	public IUIAdapter uiAdapter
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
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
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IUserInventory userInventory
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel userEquippedModel
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel
	{
		get;
		set;
	}

	[Inject]
	public IUserProAccountUnlockedModel userProAccountUnlockModel
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

	[Inject]
	public IUnlockCharacter unlockCharacter
	{
		get;
		set;
	}

	[Inject]
	public IUnlockProAccount unlockProAccount
	{
		get;
		set;
	}

	[Inject]
	public ICharactersTabAPI charactersTabAPI
	{
		get;
		set;
	}

	[Inject]
	public GameEnvironmentData environmentData
	{
		get;
		set;
	}

	[Inject]
	public IStoreAPI storeAPI
	{
		get;
		set;
	}

	[Inject("CharacterEquipView")]
	public IEquipModuleAPI equipModuleAPI
	{
		get;
		set;
	}

	public CharacterID SelectedCharacter
	{
		get
		{
			if (this.selectedChar == CharacterID.None)
			{
				this.selectedChar = this.getDefaultCharacter();
			}
			return this.selectedChar;
		}
		set
		{
			if (this.selectedChar != value)
			{
				this.selectedChar = value;
				this.selectedCharIndex = 0;
				this.equipModuleAPI.SelectedEquipment = null;
				this.userInventory.MarkAsNotNewCharacter(value, false);
				this.signalBus.Dispatch(CharacterEquipViewAPI.UPDATED);
			}
		}
	}

	public CharacterDefinition[] SelectedCharacterLinkedCharacters
	{
		get
		{
			CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(this.SelectedCharacter);
			return this.characterDataHelper.GetLinkedCharacters(characterDefinition);
		}
	}

	public int SelectedCharacterIndex
	{
		get
		{
			return this.selectedCharIndex;
		}
		set
		{
			if (this.selectedCharIndex != value)
			{
				this.selectedCharIndex = value;
				this.signalBus.Dispatch(CharacterEquipViewAPI.UPDATED);
			}
		}
	}

	public SkinDefinition EquippedSkin
	{
		get
		{
			if (this.SelectedCharacter == CharacterID.None)
			{
				return null;
			}
			return this.userEquippedModel.GetEquippedSkin(this.SelectedCharacter, this.storeAPI.Port);
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.configureCharacters();
		this.configureEquipment();
		this.signalBus.AddListener("CharactersTabAPI.UPDATED", new Action(this.onCharactersTabUpdated));
		this.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.onScreenClosed));
	}

	private void configureCharacters()
	{
		List<CharacterID> list = new List<CharacterID>();
		CharacterDefinition[] nonRandomCharacters = this.characterLists.GetNonRandomCharacters();
		for (int i = 0; i < nonRandomCharacters.Length; i++)
		{
			CharacterDefinition characterDefinition = nonRandomCharacters[i];
			list.Add(characterDefinition.characterID);
		}
		this.characters = list.ToArray();
	}

	private void onCharactersTabUpdated()
	{
		if (this.charactersTabAPI.GetState() == CharactersTabState.EquipView)
		{
			this.userInventory.MarkAsNotNewCharacter(this.SelectedCharacter, true);
		}
	}

	private void onScreenClosed()
	{
		if (this.uiAdapter.PreviousScreen == ScreenType.StoreScreen)
		{
			this.SelectedCharacterIndex = 0;
			this.equipModuleAPI.SelectedEquipType = this.getDefaultEquipType();
			this.signalBus.Dispatch(CharacterEquipViewAPI.UPDATED);
		}
	}

	private void configureEquipment()
	{
		this.validEquipmentTypes = new List<EquipmentTypes>
		{
			EquipmentTypes.SKIN,
			EquipmentTypes.VICTORY_POSE,
			EquipmentTypes.EMOTE,
			EquipmentTypes.VOICE_TAUNT,
			EquipmentTypes.HOLOGRAM,
			EquipmentTypes.PLATFORM
		}.ToArray();
	}

	private CharacterID getDefaultCharacter()
	{
		return this.characters[0];
	}

	private EquipmentTypes getDefaultEquipType()
	{
		return this.validEquipmentTypes[0];
	}

	public CharacterID[] GetCharacters()
	{
		return this.characters;
	}

	public EquippableItem[] GetItems(CharacterID characterId, EquipmentTypes type)
	{
		return this.equipmentModel.GetCharacterItems(characterId, type);
	}

	public string GetCharacterDisplayName(CharacterID characterId)
	{
		return this.characterDataHelper.GetDisplayName(characterId);
	}

	public int GetTotalItemsPossible(CharacterID characterId, EquipmentTypes type)
	{
		return this.equipmentModel.GetCharacterItems(characterId, type).Length;
	}

	public int GetItemOwnedCount(CharacterID characterId, EquipmentTypes type)
	{
		return this.userInventory.GetOwnedCharacterItemCount(characterId, type);
	}

	public SkinDefinition GetSkinFromItem(EquippableItem item, CharacterID characterId)
	{
		return this.equipmentModel.GetSkinFromItem(item.id);
	}

	public CustomPlatform GetRespawnPlatformDataFromItem(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		return this.equipmentModel.GetRespawnPlatformFromItem(item.id);
	}

	public string GetCharacterPriceString(CharacterID characterId)
	{
		return this.unlockCharacter.GetHardPriceString(characterId);
	}

	public string GetProAccountPriceString()
	{
		return this.unlockProAccount.GetPriceString();
	}

	public bool IsCharacterUnlocked(CharacterID characterId)
	{
		return this.userCharacterUnlockModel.IsUnlocked(characterId);
	}

	public bool IsProAccountUnlocked()
	{
		return this.userProAccountUnlockModel.IsUnlocked();
	}

	public EquipmentTypes[] GetValidEquipTypes()
	{
		return this.validEquipmentTypes;
	}
}
