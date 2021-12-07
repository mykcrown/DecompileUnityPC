using System;
using System.Collections.Generic;

// Token: 0x020009D6 RID: 2518
public class CharacterEquipViewAPI : ICharacterEquipViewAPI
{
	// Token: 0x170010F9 RID: 4345
	// (get) Token: 0x060046E8 RID: 18152 RVA: 0x0013582F File Offset: 0x00133C2F
	// (set) Token: 0x060046E9 RID: 18153 RVA: 0x00135837 File Offset: 0x00133C37
	[Inject]
	public IUIAdapter uiAdapter { get; set; }

	// Token: 0x170010FA RID: 4346
	// (get) Token: 0x060046EA RID: 18154 RVA: 0x00135840 File Offset: 0x00133C40
	// (set) Token: 0x060046EB RID: 18155 RVA: 0x00135848 File Offset: 0x00133C48
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170010FB RID: 4347
	// (get) Token: 0x060046EC RID: 18156 RVA: 0x00135851 File Offset: 0x00133C51
	// (set) Token: 0x060046ED RID: 18157 RVA: 0x00135859 File Offset: 0x00133C59
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x170010FC RID: 4348
	// (get) Token: 0x060046EE RID: 18158 RVA: 0x00135862 File Offset: 0x00133C62
	// (set) Token: 0x060046EF RID: 18159 RVA: 0x0013586A File Offset: 0x00133C6A
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x170010FD RID: 4349
	// (get) Token: 0x060046F0 RID: 18160 RVA: 0x00135873 File Offset: 0x00133C73
	// (set) Token: 0x060046F1 RID: 18161 RVA: 0x0013587B File Offset: 0x00133C7B
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170010FE RID: 4350
	// (get) Token: 0x060046F2 RID: 18162 RVA: 0x00135884 File Offset: 0x00133C84
	// (set) Token: 0x060046F3 RID: 18163 RVA: 0x0013588C File Offset: 0x00133C8C
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170010FF RID: 4351
	// (get) Token: 0x060046F4 RID: 18164 RVA: 0x00135895 File Offset: 0x00133C95
	// (set) Token: 0x060046F5 RID: 18165 RVA: 0x0013589D File Offset: 0x00133C9D
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x17001100 RID: 4352
	// (get) Token: 0x060046F6 RID: 18166 RVA: 0x001358A6 File Offset: 0x00133CA6
	// (set) Token: 0x060046F7 RID: 18167 RVA: 0x001358AE File Offset: 0x00133CAE
	[Inject]
	public IUserCharacterEquippedModel userEquippedModel { get; set; }

	// Token: 0x17001101 RID: 4353
	// (get) Token: 0x060046F8 RID: 18168 RVA: 0x001358B7 File Offset: 0x00133CB7
	// (set) Token: 0x060046F9 RID: 18169 RVA: 0x001358BF File Offset: 0x00133CBF
	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel { get; set; }

	// Token: 0x17001102 RID: 4354
	// (get) Token: 0x060046FA RID: 18170 RVA: 0x001358C8 File Offset: 0x00133CC8
	// (set) Token: 0x060046FB RID: 18171 RVA: 0x001358D0 File Offset: 0x00133CD0
	[Inject]
	public IUserProAccountUnlockedModel userProAccountUnlockModel { get; set; }

	// Token: 0x17001103 RID: 4355
	// (get) Token: 0x060046FC RID: 18172 RVA: 0x001358D9 File Offset: 0x00133CD9
	// (set) Token: 0x060046FD RID: 18173 RVA: 0x001358E1 File Offset: 0x00133CE1
	[Inject]
	public IEquipMethodMap equipMethodMap { get; set; }

	// Token: 0x17001104 RID: 4356
	// (get) Token: 0x060046FE RID: 18174 RVA: 0x001358EA File Offset: 0x00133CEA
	// (set) Token: 0x060046FF RID: 18175 RVA: 0x001358F2 File Offset: 0x00133CF2
	[Inject]
	public IUnlockCharacter unlockCharacter { get; set; }

	// Token: 0x17001105 RID: 4357
	// (get) Token: 0x06004700 RID: 18176 RVA: 0x001358FB File Offset: 0x00133CFB
	// (set) Token: 0x06004701 RID: 18177 RVA: 0x00135903 File Offset: 0x00133D03
	[Inject]
	public IUnlockProAccount unlockProAccount { get; set; }

	// Token: 0x17001106 RID: 4358
	// (get) Token: 0x06004702 RID: 18178 RVA: 0x0013590C File Offset: 0x00133D0C
	// (set) Token: 0x06004703 RID: 18179 RVA: 0x00135914 File Offset: 0x00133D14
	[Inject]
	public ICharactersTabAPI charactersTabAPI { get; set; }

	// Token: 0x17001107 RID: 4359
	// (get) Token: 0x06004704 RID: 18180 RVA: 0x0013591D File Offset: 0x00133D1D
	// (set) Token: 0x06004705 RID: 18181 RVA: 0x00135925 File Offset: 0x00133D25
	[Inject]
	public GameEnvironmentData environmentData { get; set; }

	// Token: 0x17001108 RID: 4360
	// (get) Token: 0x06004706 RID: 18182 RVA: 0x0013592E File Offset: 0x00133D2E
	// (set) Token: 0x06004707 RID: 18183 RVA: 0x00135936 File Offset: 0x00133D36
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x17001109 RID: 4361
	// (get) Token: 0x06004708 RID: 18184 RVA: 0x0013593F File Offset: 0x00133D3F
	// (set) Token: 0x06004709 RID: 18185 RVA: 0x00135947 File Offset: 0x00133D47
	[Inject("CharacterEquipView")]
	public IEquipModuleAPI equipModuleAPI { get; set; }

	// Token: 0x0600470A RID: 18186 RVA: 0x00135950 File Offset: 0x00133D50
	[PostConstruct]
	public void Init()
	{
		this.configureCharacters();
		this.configureEquipment();
		this.signalBus.AddListener("CharactersTabAPI.UPDATED", new Action(this.onCharactersTabUpdated));
		this.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.onScreenClosed));
	}

	// Token: 0x0600470B RID: 18187 RVA: 0x001359A4 File Offset: 0x00133DA4
	private void configureCharacters()
	{
		List<CharacterID> list = new List<CharacterID>();
		foreach (CharacterDefinition characterDefinition in this.characterLists.GetNonRandomCharacters())
		{
			list.Add(characterDefinition.characterID);
		}
		this.characters = list.ToArray();
	}

	// Token: 0x0600470C RID: 18188 RVA: 0x001359F3 File Offset: 0x00133DF3
	private void onCharactersTabUpdated()
	{
		if (this.charactersTabAPI.GetState() == CharactersTabState.EquipView)
		{
			this.userInventory.MarkAsNotNewCharacter(this.SelectedCharacter, true);
		}
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x00135A18 File Offset: 0x00133E18
	private void onScreenClosed()
	{
		if (this.uiAdapter.PreviousScreen == ScreenType.StoreScreen)
		{
			this.SelectedCharacterIndex = 0;
			this.equipModuleAPI.SelectedEquipType = this.getDefaultEquipType();
			this.signalBus.Dispatch(CharacterEquipViewAPI.UPDATED);
		}
	}

	// Token: 0x0600470E RID: 18190 RVA: 0x00135A54 File Offset: 0x00133E54
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

	// Token: 0x0600470F RID: 18191 RVA: 0x00135A9D File Offset: 0x00133E9D
	private CharacterID getDefaultCharacter()
	{
		return this.characters[0];
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x00135AA7 File Offset: 0x00133EA7
	private EquipmentTypes getDefaultEquipType()
	{
		return this.validEquipmentTypes[0];
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x00135AB1 File Offset: 0x00133EB1
	public CharacterID[] GetCharacters()
	{
		return this.characters;
	}

	// Token: 0x1700110A RID: 4362
	// (get) Token: 0x06004712 RID: 18194 RVA: 0x00135AB9 File Offset: 0x00133EB9
	// (set) Token: 0x06004713 RID: 18195 RVA: 0x00135AD8 File Offset: 0x00133ED8
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

	// Token: 0x1700110B RID: 4363
	// (get) Token: 0x06004714 RID: 18196 RVA: 0x00135B28 File Offset: 0x00133F28
	public CharacterDefinition[] SelectedCharacterLinkedCharacters
	{
		get
		{
			CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(this.SelectedCharacter);
			return this.characterDataHelper.GetLinkedCharacters(characterDefinition);
		}
	}

	// Token: 0x1700110C RID: 4364
	// (get) Token: 0x06004715 RID: 18197 RVA: 0x00135B53 File Offset: 0x00133F53
	// (set) Token: 0x06004716 RID: 18198 RVA: 0x00135B5B File Offset: 0x00133F5B
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

	// Token: 0x06004717 RID: 18199 RVA: 0x00135B80 File Offset: 0x00133F80
	public EquippableItem[] GetItems(CharacterID characterId, EquipmentTypes type)
	{
		return this.equipmentModel.GetCharacterItems(characterId, type);
	}

	// Token: 0x06004718 RID: 18200 RVA: 0x00135B8F File Offset: 0x00133F8F
	public string GetCharacterDisplayName(CharacterID characterId)
	{
		return this.characterDataHelper.GetDisplayName(characterId);
	}

	// Token: 0x06004719 RID: 18201 RVA: 0x00135B9D File Offset: 0x00133F9D
	public int GetTotalItemsPossible(CharacterID characterId, EquipmentTypes type)
	{
		return this.equipmentModel.GetCharacterItems(characterId, type).Length;
	}

	// Token: 0x0600471A RID: 18202 RVA: 0x00135BAE File Offset: 0x00133FAE
	public int GetItemOwnedCount(CharacterID characterId, EquipmentTypes type)
	{
		return this.userInventory.GetOwnedCharacterItemCount(characterId, type);
	}

	// Token: 0x1700110D RID: 4365
	// (get) Token: 0x0600471B RID: 18203 RVA: 0x00135BBD File Offset: 0x00133FBD
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

	// Token: 0x0600471C RID: 18204 RVA: 0x00135BE8 File Offset: 0x00133FE8
	public SkinDefinition GetSkinFromItem(EquippableItem item, CharacterID characterId)
	{
		return this.equipmentModel.GetSkinFromItem(item.id);
	}

	// Token: 0x0600471D RID: 18205 RVA: 0x00135BFB File Offset: 0x00133FFB
	public CustomPlatform GetRespawnPlatformDataFromItem(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		return this.equipmentModel.GetRespawnPlatformFromItem(item.id);
	}

	// Token: 0x0600471E RID: 18206 RVA: 0x00135C16 File Offset: 0x00134016
	public string GetCharacterPriceString(CharacterID characterId)
	{
		return this.unlockCharacter.GetHardPriceString(characterId);
	}

	// Token: 0x0600471F RID: 18207 RVA: 0x00135C24 File Offset: 0x00134024
	public string GetProAccountPriceString()
	{
		return this.unlockProAccount.GetPriceString();
	}

	// Token: 0x06004720 RID: 18208 RVA: 0x00135C31 File Offset: 0x00134031
	public bool IsCharacterUnlocked(CharacterID characterId)
	{
		return this.userCharacterUnlockModel.IsUnlocked(characterId);
	}

	// Token: 0x06004721 RID: 18209 RVA: 0x00135C3F File Offset: 0x0013403F
	public bool IsProAccountUnlocked()
	{
		return this.userProAccountUnlockModel.IsUnlocked();
	}

	// Token: 0x06004722 RID: 18210 RVA: 0x00135C4C File Offset: 0x0013404C
	public EquipmentTypes[] GetValidEquipTypes()
	{
		return this.validEquipmentTypes;
	}

	// Token: 0x04002EF8 RID: 12024
	public static string UPDATED = "CharacterEquipViewAPI.UPDATED";

	// Token: 0x04002F0A RID: 12042
	private CharacterID[] characters;

	// Token: 0x04002F0B RID: 12043
	private EquipmentTypes[] validEquipmentTypes;

	// Token: 0x04002F0C RID: 12044
	private CharacterID selectedChar;

	// Token: 0x04002F0D RID: 12045
	private int selectedCharIndex;
}
