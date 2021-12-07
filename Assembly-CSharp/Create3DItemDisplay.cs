using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A1C RID: 2588
public class Create3DItemDisplay : ICreate3DItemDisplay
{
	// Token: 0x170011E7 RID: 4583
	// (get) Token: 0x06004B2A RID: 19242 RVA: 0x00141126 File Offset: 0x0013F526
	// (set) Token: 0x06004B2B RID: 19243 RVA: 0x0014112E File Offset: 0x0013F52E
	[Inject]
	public ConfigData config { private get; set; }

	// Token: 0x170011E8 RID: 4584
	// (get) Token: 0x06004B2C RID: 19244 RVA: 0x00141137 File Offset: 0x0013F537
	// (set) Token: 0x06004B2D RID: 19245 RVA: 0x0014113F File Offset: 0x0013F53F
	[Inject]
	public IDependencyInjection injector { private get; set; }

	// Token: 0x170011E9 RID: 4585
	// (get) Token: 0x06004B2E RID: 19246 RVA: 0x00141148 File Offset: 0x0013F548
	// (set) Token: 0x06004B2F RID: 19247 RVA: 0x00141150 File Offset: 0x0013F550
	[Inject]
	public IEquipmentModel equipmentModel { private get; set; }

	// Token: 0x170011EA RID: 4586
	// (get) Token: 0x06004B30 RID: 19248 RVA: 0x00141159 File Offset: 0x0013F559
	// (set) Token: 0x06004B31 RID: 19249 RVA: 0x00141161 File Offset: 0x0013F561
	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager { private get; set; }

	// Token: 0x170011EB RID: 4587
	// (get) Token: 0x06004B32 RID: 19250 RVA: 0x0014116A File Offset: 0x0013F56A
	// (set) Token: 0x06004B33 RID: 19251 RVA: 0x00141172 File Offset: 0x0013F572
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x170011EC RID: 4588
	// (get) Token: 0x06004B34 RID: 19252 RVA: 0x0014117B File Offset: 0x0013F57B
	// (set) Token: 0x06004B35 RID: 19253 RVA: 0x00141183 File Offset: 0x0013F583
	[Inject]
	public GameDataManager gameDataManager { private get; set; }

	// Token: 0x170011ED RID: 4589
	// (get) Token: 0x06004B36 RID: 19254 RVA: 0x0014118C File Offset: 0x0013F58C
	// (set) Token: 0x06004B37 RID: 19255 RVA: 0x00141194 File Offset: 0x0013F594
	[Inject]
	public IMainThreadTimer timer { private get; set; }

	// Token: 0x170011EE RID: 4590
	// (get) Token: 0x06004B38 RID: 19256 RVA: 0x0014119D File Offset: 0x0013F59D
	// (set) Token: 0x06004B39 RID: 19257 RVA: 0x001411A5 File Offset: 0x0013F5A5
	[Inject]
	public IUserCharacterEquippedModel userEquippedModel { private get; set; }

	// Token: 0x170011EF RID: 4591
	// (get) Token: 0x06004B3A RID: 19258 RVA: 0x001411AE File Offset: 0x0013F5AE
	// (set) Token: 0x06004B3B RID: 19259 RVA: 0x001411B6 File Offset: 0x0013F5B6
	[Inject]
	public IPreviewAnimationHelper previewAnimationHelper { private get; set; }

	// Token: 0x170011F0 RID: 4592
	// (get) Token: 0x06004B3C RID: 19260 RVA: 0x001411BF File Offset: 0x0013F5BF
	// (set) Token: 0x06004B3D RID: 19261 RVA: 0x001411C7 File Offset: 0x0013F5C7
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x170011F1 RID: 4593
	// (get) Token: 0x06004B3E RID: 19262 RVA: 0x001411D0 File Offset: 0x0013F5D0
	// (set) Token: 0x06004B3F RID: 19263 RVA: 0x001411D8 File Offset: 0x0013F5D8
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x170011F2 RID: 4594
	// (get) Token: 0x06004B40 RID: 19264 RVA: 0x001411E1 File Offset: 0x0013F5E1
	// (set) Token: 0x06004B41 RID: 19265 RVA: 0x001411E9 File Offset: 0x0013F5E9
	[Inject]
	public IStoreAPI storeAPI { private get; set; }

	// Token: 0x170011F3 RID: 4595
	// (get) Token: 0x06004B42 RID: 19266 RVA: 0x001411F2 File Offset: 0x0013F5F2
	// (set) Token: 0x06004B43 RID: 19267 RVA: 0x001411FA File Offset: 0x0013F5FA
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x06004B44 RID: 19268 RVA: 0x00141204 File Offset: 0x0013F604
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

	// Token: 0x06004B45 RID: 19269 RVA: 0x00141234 File Offset: 0x0013F634
	public Item3DPreview CreateDefault(EquipmentTypes type)
	{
		EquippableItem defaultItem = this.getDefaultItem(type);
		if (defaultItem == null)
		{
			return null;
		}
		return this.CreateDisplay(defaultItem);
	}

	// Token: 0x06004B46 RID: 19270 RVA: 0x00141258 File Offset: 0x0013F658
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

	// Token: 0x06004B47 RID: 19271 RVA: 0x001412AB File Offset: 0x0013F6AB
	private EquippableItem getDefaultItem(EquipmentTypes type)
	{
		if (type != EquipmentTypes.PLATFORM)
		{
			return null;
		}
		return this.equipmentModel.GetDefaultItem(type);
	}

	// Token: 0x06004B48 RID: 19272 RVA: 0x001412C8 File Offset: 0x0013F6C8
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

	// Token: 0x06004B49 RID: 19273 RVA: 0x0014136C File Offset: 0x0013F76C
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

	// Token: 0x06004B4A RID: 19274 RVA: 0x00141414 File Offset: 0x0013F814
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

	// Token: 0x06004B4B RID: 19275 RVA: 0x0014148C File Offset: 0x0013F88C
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

	// Token: 0x06004B4C RID: 19276 RVA: 0x001414DC File Offset: 0x0013F8DC
	private Item3DPreview createCharacterItem(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(item.character);
		SkinDefinition defaultSkin = this.skinDataManager.GetDefaultSkin(item.character);
		IUISceneCharacter iuisceneCharacter = this.createCharacter(characterDefinition, defaultSkin, true);
		item3DPreview.obj = (iuisceneCharacter as Component).gameObject;
		item3DPreview.characterMenusData = this.characterMenusDataLoader.GetData(characterDefinition);
		return item3DPreview;
	}

	// Token: 0x06004B4D RID: 19277 RVA: 0x00141544 File Offset: 0x0013F944
	private Item3DPreview createSkin(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(item.character);
		SkinDefinition skinFromItem = this.equipmentModel.GetSkinFromItem(item.id);
		IUISceneCharacter iuisceneCharacter = this.createCharacter(characterDefinition, skinFromItem, true);
		item3DPreview.obj = (iuisceneCharacter as Component).gameObject;
		item3DPreview.characterMenusData = this.characterMenusDataLoader.GetData(characterDefinition);
		return item3DPreview;
	}

	// Token: 0x06004B4E RID: 19278 RVA: 0x001415AC File Offset: 0x0013F9AC
	private Item3DPreview createHologram(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		HologramData hologramFromItem = this.equipmentModel.GetHologramFromItem(item.id);
		GameObject prefab = this.config.defaultCharacterEffects.hologram.prefab;
		if (hologramFromItem.hasOverrideVFX && hologramFromItem.overrideVFX != null)
		{
			prefab = hologramFromItem.overrideVFX.prefab;
		}
		item3DPreview.obj = UnityEngine.Object.Instantiate<GameObject>(prefab);
		HologramDisplay display = item3DPreview.obj.GetComponent<HologramDisplay>();
		item3DPreview.playPreviewFn = delegate()
		{
			display.PlayHologram(HologramDisplay.PlayBehavior.Loop, this.config.storeSettings.hologramLoopInterval);
		};
		display.SetTexture(hologramFromItem.texture);
		item3DPreview.PlayPreview();
		return item3DPreview;
	}

	// Token: 0x06004B4F RID: 19279 RVA: 0x0014165D File Offset: 0x0013FA5D
	private SkinDefinition getEquippedSkin(CharacterID characterID, int portId)
	{
		return this.userEquippedModel.GetEquippedSkin(characterID, portId);
	}

	// Token: 0x06004B50 RID: 19280 RVA: 0x0014166C File Offset: 0x0013FA6C
	private Item3DPreview createEmote(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		CharacterDefinition characterDef = this.characterLists.GetCharacterDefinition(item.character);
		SkinDefinition equippedSkin = this.getEquippedSkin(item.character, this.storeAPI.Port);
		IUISceneCharacter characterDisplay = this.createCharacter(characterDef, equippedSkin, true);
		item3DPreview.obj = (characterDisplay as Component).gameObject;
		item3DPreview.characterMenusData = this.characterMenusDataLoader.GetData(characterDef);
		item3DPreview.playPreviewFn = delegate()
		{
			this.previewAnimationHelper.PlayBasicAnimation(characterDisplay, characterDef, item, 0);
		};
		return item3DPreview;
	}

	// Token: 0x06004B51 RID: 19281 RVA: 0x00141720 File Offset: 0x0013FB20
	private Item3DPreview createVictoryPose(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		CharacterDefinition characterDef = this.characterLists.GetCharacterDefinition(item.character);
		SkinDefinition equippedSkin = this.getEquippedSkin(item.character, this.storeAPI.Port);
		IUISceneCharacter characterDisplay = this.createCharacter(characterDef, equippedSkin, true);
		characterDisplay.SetMode(UIAssetDisplayMode.Centered);
		item3DPreview.obj = (characterDisplay as Component).gameObject;
		item3DPreview.characterMenusData = this.characterMenusDataLoader.GetData(characterDef);
		item3DPreview.playPreviewFn = delegate()
		{
			this.previewAnimationHelper.PlayVictoryPose(characterDisplay, characterDef, item);
		};
		return item3DPreview;
	}

	// Token: 0x06004B52 RID: 19282 RVA: 0x001417E0 File Offset: 0x0013FBE0
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

	// Token: 0x06004B53 RID: 19283 RVA: 0x00141868 File Offset: 0x0013FC68
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
			Debug.LogError("Failed to find icon for given item.");
		}
		return item3DPreview;
	}

	// Token: 0x06004B54 RID: 19284 RVA: 0x001418E8 File Offset: 0x0013FCE8
	private Item3DPreview createVoiceTaunt(EquippableItem item)
	{
		Item3DPreview item3DPreview = new Item3DPreview();
		VoiceTauntData voiceTaunt = this.equipmentModel.GetVoiceTauntFromItem(item.id);
		item3DPreview.obj = UnityEngine.Object.Instantiate<GameObject>(this.config.storeSettings.voiceTauntPreviewPrefab);
		VoiceTauntDisplay display = item3DPreview.obj.GetComponent<VoiceTauntDisplay>();
		this.injector.Inject(display);
		display.SetCharacter(item.character);
		item3DPreview.playPreviewFn = delegate()
		{
			display.Play(voiceTaunt.primaryAudioData.sound, 0f);
		};
		return item3DPreview;
	}

	// Token: 0x0400316E RID: 12654
	private static float NETSUKE_SCALE = 0.5f;
}
