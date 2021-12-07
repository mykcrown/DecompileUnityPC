// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Create3DItemDisplay : ICreate3DItemDisplay
{
	private sealed class _createHologram_c__AnonStorey0
	{
		internal HologramDisplay display;

		internal Create3DItemDisplay _this;

		internal void __m__0()
		{
			this.display.PlayHologram(HologramDisplay.PlayBehavior.Loop, this._this.config.storeSettings.hologramLoopInterval);
		}
	}

	private sealed class _createEmote_c__AnonStorey1
	{
		internal IUISceneCharacter characterDisplay;

		internal CharacterDefinition characterDef;

		internal EquippableItem item;

		internal Create3DItemDisplay _this;

		internal void __m__0()
		{
			this._this.previewAnimationHelper.PlayBasicAnimation(this.characterDisplay, this.characterDef, this.item, 0);
		}
	}

	private sealed class _createVictoryPose_c__AnonStorey2
	{
		internal IUISceneCharacter characterDisplay;

		internal CharacterDefinition characterDef;

		internal EquippableItem item;

		internal Create3DItemDisplay _this;

		internal void __m__0()
		{
			this._this.previewAnimationHelper.PlayVictoryPose(this.characterDisplay, this.characterDef, this.item);
		}
	}

	private sealed class _createVoiceTaunt_c__AnonStorey3
	{
		internal VoiceTauntDisplay display;

		internal VoiceTauntData voiceTaunt;

		internal void __m__0()
		{
			this.display.Play(this.voiceTaunt.primaryAudioData.sound, 0f);
		}
	}

	private static float NETSUKE_SCALE = 0.5f;

	[Inject]
	public ConfigData config
	{
		private get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		private get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		private get;
		set;
	}

	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		private get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		private get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		private get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel userEquippedModel
	{
		private get;
		set;
	}

	[Inject]
	public IPreviewAnimationHelper previewAnimationHelper
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	[Inject]
	public IStoreAPI storeAPI
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader
	{
		private get;
		set;
	}

	public Item3DPreview CreateDisplay(EquippableItem item)
	{
		Item3DPreview item3DPreview = this.createDisplay(item);
		if (item3DPreview != null)
		{
			item3DPreview.type = item.type;
			item3DPreview.item = item;
		}
		return item3DPreview;
	}

	public Item3DPreview CreateDefault(EquipmentTypes type)
	{
		EquippableItem defaultItem = this.getDefaultItem(type);
		if (defaultItem == null)
		{
			return null;
		}
		return this.CreateDisplay(defaultItem);
	}

	public void DestroyPreview(Item3DPreview preview)
	{
		IUISceneCharacter component = preview.obj.GetComponent<UISceneCharacterGroup>();
		if (component != null)
		{
			this.uiSceneCharacterManager.ReleaseCharacter(component);
		}
		else
		{
			preview.obj.transform.SetParent(null, false);
			UnityEngine.Object.DestroyImmediate(preview.obj);
		}
		preview.Cleanup();
	}

	private EquippableItem getDefaultItem(EquipmentTypes type)
	{
		if (type != EquipmentTypes.PLATFORM)
		{
			return null;
		}
		return this.equipmentModel.GetDefaultItem(type);
	}

	private Item3DPreview createDisplay(EquippableItem item)
	{
		if (item == null)
		{
			return null;
		}
		switch (item.type)
		{
		case EquipmentTypes.SKIN:
			return this.createSkin(item);
		case EquipmentTypes.EMOTE:
			return this.createEmote(item);
		case EquipmentTypes.HOLOGRAM:
			return this.createHologram(item);
		case EquipmentTypes.VOICE_TAUNT:
			return this.createVoiceTaunt(item);
		case EquipmentTypes.VICTORY_POSE:
			return this.createVictoryPose(item);
		case EquipmentTypes.PLATFORM:
			return this.createPlatform(item);
		case EquipmentTypes.NETSUKE:
			return this.createNetsuke(item);
		case EquipmentTypes.TOKEN:
			return this.createToken(item);
		case EquipmentTypes.PLAYER_ICON:
			return this.createPlayerIcon(item);
		case EquipmentTypes.CHARACTER:
			return this.createCharacterItem(item);
		}
		return null;
	}

	private Item3DPreview createPlatform(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		CustomPlatform respawnPlatformFromItem = this.equipmentModel.GetRespawnPlatformFromItem(item.id);
		RespawnPlatform component = UnityEngine.Object.Instantiate<GameObject>(this.config.respawnConfig.respawnPlatformPrefab).GetComponent<RespawnPlatform>();
		this.injector.Inject(component);
		GameObject gameObject = (!(respawnPlatformFromItem == null)) ? respawnPlatformFromItem.gameObject : null;
		GameObject gameObject2 = null;
		if (gameObject != null)
		{
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.SetActive(false);
			gameObject2.transform.SetParent(null, false);
		}
		component.AttachCustom(gameObject2);
		item3DPreview.obj = component.gameObject;
		return item3DPreview;
	}

	private Item3DPreview createNetsuke(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		Netsuke netsukeFromItem = this.equipmentModel.GetNetsukeFromItem(item.id);
		PlayerNetsukeDisplay playerNetsukeDisplay = UnityEngine.Object.Instantiate<PlayerNetsukeDisplay>(this.config.netsukeStorePrefab);
		this.injector.Inject(playerNetsukeDisplay);
		playerNetsukeDisplay.AddNetsuke(netsukeFromItem, 0);
		playerNetsukeDisplay.transform.localScale = new Vector3(Create3DItemDisplay.NETSUKE_SCALE, Create3DItemDisplay.NETSUKE_SCALE, Create3DItemDisplay.NETSUKE_SCALE);
		item3DPreview.obj = playerNetsukeDisplay.gameObject;
		return item3DPreview;
	}

	private IUISceneCharacter createCharacter(CharacterDefinition characterDef, SkinDefinition skinDefinition, bool startAnimations = true)
	{
		IUISceneCharacter character = this.uiSceneCharacterManager.GetCharacter(characterDef.characterID, skinDefinition);
		character.Activate(null);
		if (startAnimations)
		{
			List<WavedashAnimationData> allDefaultAnimations = this.characterDataHelper.GetAllDefaultAnimations(characterDef, CharacterDefaultAnimationKey.STORE_IDLE);
			character.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations));
		}
		character.SetMode(UIAssetDisplayMode.OffsetRotate);
		return character;
	}

	private Item3DPreview createCharacterItem(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(item.character);
		SkinDefinition defaultSkin = this.skinDataManager.GetDefaultSkin(item.character);
		IUISceneCharacter iUISceneCharacter = this.createCharacter(characterDefinition, defaultSkin, true);
		item3DPreview.obj = (iUISceneCharacter as Component).gameObject;
		item3DPreview.characterMenusData = this.characterMenusDataLoader.GetData(characterDefinition);
		return item3DPreview;
	}

	private Item3DPreview createSkin(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(item.character);
		SkinDefinition skinFromItem = this.equipmentModel.GetSkinFromItem(item.id);
		IUISceneCharacter iUISceneCharacter = this.createCharacter(characterDefinition, skinFromItem, true);
		item3DPreview.obj = (iUISceneCharacter as Component).gameObject;
		item3DPreview.characterMenusData = this.characterMenusDataLoader.GetData(characterDefinition);
		return item3DPreview;
	}

	private Item3DPreview createHologram(EquippableItem item)
	{
		Create3DItemDisplay._createHologram_c__AnonStorey0 _createHologram_c__AnonStorey = new Create3DItemDisplay._createHologram_c__AnonStorey0();
		_createHologram_c__AnonStorey._this = this;
		Item3DPreview item3DPreview = new Item3DPreview();
		HologramData hologramFromItem = this.equipmentModel.GetHologramFromItem(item.id);
		GameObject prefab = this.config.defaultCharacterEffects.hologram.prefab;
		if (hologramFromItem.hasOverrideVFX && hologramFromItem.overrideVFX != null)
		{
			prefab = hologramFromItem.overrideVFX.prefab;
		}
		item3DPreview.obj = UnityEngine.Object.Instantiate<GameObject>(prefab);
		_createHologram_c__AnonStorey.display = item3DPreview.obj.GetComponent<HologramDisplay>();
		item3DPreview.playPreviewFn = new Action(_createHologram_c__AnonStorey.__m__0);
		_createHologram_c__AnonStorey.display.SetTexture(hologramFromItem.texture);
		item3DPreview.PlayPreview();
		return item3DPreview;
	}

	private SkinDefinition getEquippedSkin(CharacterID characterID, int portId)
	{
		return this.userEquippedModel.GetEquippedSkin(characterID, portId);
	}

	private Item3DPreview createEmote(EquippableItem item)
	{
		Create3DItemDisplay._createEmote_c__AnonStorey1 _createEmote_c__AnonStorey = new Create3DItemDisplay._createEmote_c__AnonStorey1();
		_createEmote_c__AnonStorey.item = item;
		_createEmote_c__AnonStorey._this = this;
		Item3DPreview item3DPreview = new Item3DPreview();
		_createEmote_c__AnonStorey.characterDef = this.characterLists.GetCharacterDefinition(_createEmote_c__AnonStorey.item.character);
		SkinDefinition equippedSkin = this.getEquippedSkin(_createEmote_c__AnonStorey.item.character, this.storeAPI.Port);
		_createEmote_c__AnonStorey.characterDisplay = this.createCharacter(_createEmote_c__AnonStorey.characterDef, equippedSkin, true);
		item3DPreview.obj = (_createEmote_c__AnonStorey.characterDisplay as Component).gameObject;
		item3DPreview.characterMenusData = this.characterMenusDataLoader.GetData(_createEmote_c__AnonStorey.characterDef);
		item3DPreview.playPreviewFn = new Action(_createEmote_c__AnonStorey.__m__0);
		return item3DPreview;
	}

	private Item3DPreview createVictoryPose(EquippableItem item)
	{
		Create3DItemDisplay._createVictoryPose_c__AnonStorey2 _createVictoryPose_c__AnonStorey = new Create3DItemDisplay._createVictoryPose_c__AnonStorey2();
		_createVictoryPose_c__AnonStorey.item = item;
		_createVictoryPose_c__AnonStorey._this = this;
		Item3DPreview item3DPreview = new Item3DPreview();
		_createVictoryPose_c__AnonStorey.characterDef = this.characterLists.GetCharacterDefinition(_createVictoryPose_c__AnonStorey.item.character);
		SkinDefinition equippedSkin = this.getEquippedSkin(_createVictoryPose_c__AnonStorey.item.character, this.storeAPI.Port);
		_createVictoryPose_c__AnonStorey.characterDisplay = this.createCharacter(_createVictoryPose_c__AnonStorey.characterDef, equippedSkin, true);
		_createVictoryPose_c__AnonStorey.characterDisplay.SetMode(UIAssetDisplayMode.Centered);
		item3DPreview.obj = (_createVictoryPose_c__AnonStorey.characterDisplay as Component).gameObject;
		item3DPreview.characterMenusData = this.characterMenusDataLoader.GetData(_createVictoryPose_c__AnonStorey.characterDef);
		item3DPreview.playPreviewFn = new Action(_createVictoryPose_c__AnonStorey.__m__0);
		return item3DPreview;
	}

	private Item3DPreview createToken(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		PlayerToken playerTokenFromItem = this.equipmentModel.GetPlayerTokenFromItem(item.id);
		item3DPreview.obj = UnityEngine.Object.Instantiate<GameObject>(this.config.storeSettings.playerTokenPreviewPrefab);
		PlayerTokenDisplay component = item3DPreview.obj.GetComponent<PlayerTokenDisplay>();
		this.injector.Inject(component);
		if (playerTokenFromItem != null)
		{
			component.SetCustom(playerTokenFromItem.Image.sprite.texture);
		}
		else
		{
			component.SetDefault();
		}
		return item3DPreview;
	}

	private Item3DPreview createPlayerIcon(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		PlayerCardIconData playerIconFromItem = this.equipmentModel.GetPlayerIconFromItem(item.id);
		item3DPreview.obj = UnityEngine.Object.Instantiate<GameObject>(this.config.storeSettings.playerCardIconPreviewPrefab);
		PlayerCardIconDisplay component = item3DPreview.obj.GetComponent<PlayerCardIconDisplay>();
		this.injector.Inject(component);
		if (playerIconFromItem != null)
		{
			component.SetIcon(playerIconFromItem.sprite);
		}
		else
		{
			UnityEngine.Debug.LogError("Failed to find icon for given item.");
		}
		return item3DPreview;
	}

	private Item3DPreview createVoiceTaunt(EquippableItem item)
	{
		Create3DItemDisplay._createVoiceTaunt_c__AnonStorey3 _createVoiceTaunt_c__AnonStorey = new Create3DItemDisplay._createVoiceTaunt_c__AnonStorey3();
		Item3DPreview item3DPreview = new Item3DPreview();
		_createVoiceTaunt_c__AnonStorey.voiceTaunt = this.equipmentModel.GetVoiceTauntFromItem(item.id);
		item3DPreview.obj = UnityEngine.Object.Instantiate<GameObject>(this.config.storeSettings.voiceTauntPreviewPrefab);
		_createVoiceTaunt_c__AnonStorey.display = item3DPreview.obj.GetComponent<VoiceTauntDisplay>();
		this.injector.Inject(_createVoiceTaunt_c__AnonStorey.display);
		_createVoiceTaunt_c__AnonStorey.display.SetCharacter(item.character);
		item3DPreview.playPreviewFn = new Action(_createVoiceTaunt_c__AnonStorey.__m__0);
		return item3DPreview;
	}
}
