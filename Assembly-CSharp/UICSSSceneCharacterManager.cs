using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008FD RID: 2301
public class UICSSSceneCharacterManager : IUICSSSceneCharacterManager, IStartupLoader
{
	// Token: 0x17000E4A RID: 3658
	// (get) Token: 0x06003B9B RID: 15259 RVA: 0x001156D1 File Offset: 0x00113AD1
	// (set) Token: 0x06003B9C RID: 15260 RVA: 0x001156D9 File Offset: 0x00113AD9
	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager { get; set; }

	// Token: 0x17000E4B RID: 3659
	// (get) Token: 0x06003B9D RID: 15261 RVA: 0x001156E2 File Offset: 0x00113AE2
	// (set) Token: 0x06003B9E RID: 15262 RVA: 0x001156EA File Offset: 0x00113AEA
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000E4C RID: 3660
	// (get) Token: 0x06003B9F RID: 15263 RVA: 0x001156F3 File Offset: 0x00113AF3
	// (set) Token: 0x06003BA0 RID: 15264 RVA: 0x001156FB File Offset: 0x00113AFB
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000E4D RID: 3661
	// (get) Token: 0x06003BA1 RID: 15265 RVA: 0x00115704 File Offset: 0x00113B04
	// (set) Token: 0x06003BA2 RID: 15266 RVA: 0x0011570C File Offset: 0x00113B0C
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x17000E4E RID: 3662
	// (get) Token: 0x06003BA3 RID: 15267 RVA: 0x00115715 File Offset: 0x00113B15
	// (set) Token: 0x06003BA4 RID: 15268 RVA: 0x0011571D File Offset: 0x00113B1D
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x17000E4F RID: 3663
	// (get) Token: 0x06003BA5 RID: 15269 RVA: 0x00115726 File Offset: 0x00113B26
	// (set) Token: 0x06003BA6 RID: 15270 RVA: 0x0011572E File Offset: 0x00113B2E
	[Inject]
	public GameEnvironmentData environmentData { get; set; }

	// Token: 0x17000E50 RID: 3664
	// (get) Token: 0x06003BA7 RID: 15271 RVA: 0x00115737 File Offset: 0x00113B37
	// (set) Token: 0x06003BA8 RID: 15272 RVA: 0x0011573F File Offset: 0x00113B3F
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x06003BA9 RID: 15273 RVA: 0x00115748 File Offset: 0x00113B48
	[PostConstruct]
	public void Init()
	{
		this.container = new GameObject("UICSSCharacterManager");
		this.container.SetActive(false);
		UnityEngine.Object.DontDestroyOnLoad(this.container);
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x00115771 File Offset: 0x00113B71
	public void StartupLoad(Action callback)
	{
		if (SystemBoot.mode != SystemBoot.Mode.StagePreview)
		{
			this.Preload();
		}
		callback();
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x0011578C File Offset: 0x00113B8C
	public void Preload()
	{
		this.cachePrefab();
		foreach (CharacterDefinition characterDefinition in this.characterLists.GetLegalCharacters())
		{
			UICSSSceneCharacter character = this.GetCharacter(characterDefinition.characterID, this.skinDataManager.GetDefaultSkin(characterDefinition.characterID));
			this.ReleaseCharacter(character);
		}
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x001157E8 File Offset: 0x00113BE8
	private void cachePrefab()
	{
		if (this.characterPrefab == null)
		{
			this.characterPrefab = Resources.Load<UICSSSceneCharacter>("GUI/Menu/CharacterSelect/UICSSSceneChar");
		}
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x0011580C File Offset: 0x00113C0C
	private UICSSSceneCharacter getFromPool(CharacterID id)
	{
		if (this.pool.ContainsKey(id) && this.pool[id].Count > 0)
		{
			UICSSSceneCharacter result = this.pool[id][0];
			this.pool[id].RemoveAt(0);
			return result;
		}
		return null;
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x0011586C File Offset: 0x00113C6C
	private void addToPool(UICSSSceneCharacter display)
	{
		CharacterID characterID = display.CharacterID;
		if (!this.pool.ContainsKey(characterID))
		{
			this.pool[characterID] = new List<UICSSSceneCharacter>();
		}
		this.pool[characterID].Add(display);
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x001158B4 File Offset: 0x00113CB4
	public UICSSSceneCharacter GetCharacter(CharacterID characterId, SkinDefinition initWithSkin)
	{
		UICSSSceneCharacter uicsssceneCharacter = this.getFromPool(characterId);
		if (uicsssceneCharacter == null)
		{
			CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterId);
			uicsssceneCharacter = UnityEngine.Object.Instantiate<UICSSSceneCharacter>(this.characterPrefab);
			uicsssceneCharacter.name = "UICSSChar-" + characterDefinition.characterName;
			UnityEngine.Object.DontDestroyOnLoad(uicsssceneCharacter.gameObject);
			uicsssceneCharacter.transform.SetParent(this.container.transform, false);
			this.injector.Inject(uicsssceneCharacter);
			uicsssceneCharacter.Init(this.characterMenusDataLoader.GetData(characterDefinition));
			if (!characterDefinition.isRandom)
			{
				uicsssceneCharacter.SetCharacterDisplay(this.uiSceneCharacterManager.GetCharacter(characterId, initWithSkin));
			}
		}
		return uicsssceneCharacter;
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x00115964 File Offset: 0x00113D64
	public void ReleaseCharacter(UICSSSceneCharacter characterDisplay)
	{
		if (this.container != null)
		{
			characterDisplay.transform.SetParent(this.container.transform, false);
			this.addToPool(characterDisplay);
		}
	}

	// Token: 0x0400290D RID: 10509
	private Dictionary<CharacterID, List<UICSSSceneCharacter>> pool = new Dictionary<CharacterID, List<UICSSSceneCharacter>>();

	// Token: 0x0400290E RID: 10510
	private GameObject container;

	// Token: 0x0400290F RID: 10511
	private UICSSSceneCharacter characterPrefab;
}
