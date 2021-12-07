using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009A7 RID: 2471
public class MainMenuAPI : IMainMenuAPI, IStartupLoader
{
	// Token: 0x17001004 RID: 4100
	// (get) Token: 0x060043CB RID: 17355 RVA: 0x0012BC48 File Offset: 0x0012A048
	// (set) Token: 0x060043CC RID: 17356 RVA: 0x0012BC50 File Offset: 0x0012A050
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x17001005 RID: 4101
	// (get) Token: 0x060043CD RID: 17357 RVA: 0x0012BC59 File Offset: 0x0012A059
	// (set) Token: 0x060043CE RID: 17358 RVA: 0x0012BC61 File Offset: 0x0012A061
	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager { private get; set; }

	// Token: 0x17001006 RID: 4102
	// (get) Token: 0x060043CF RID: 17359 RVA: 0x0012BC6A File Offset: 0x0012A06A
	// (set) Token: 0x060043D0 RID: 17360 RVA: 0x0012BC72 File Offset: 0x0012A072
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x17001007 RID: 4103
	// (get) Token: 0x060043D1 RID: 17361 RVA: 0x0012BC7B File Offset: 0x0012A07B
	// (set) Token: 0x060043D2 RID: 17362 RVA: 0x0012BC83 File Offset: 0x0012A083
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17001008 RID: 4104
	// (get) Token: 0x060043D3 RID: 17363 RVA: 0x0012BC8C File Offset: 0x0012A08C
	// (set) Token: 0x060043D4 RID: 17364 RVA: 0x0012BC94 File Offset: 0x0012A094
	[Inject]
	public UserAudioSettings userAudioSettings { private get; set; }

	// Token: 0x17001009 RID: 4105
	// (get) Token: 0x060043D5 RID: 17365 RVA: 0x0012BC9D File Offset: 0x0012A09D
	// (set) Token: 0x060043D6 RID: 17366 RVA: 0x0012BCA5 File Offset: 0x0012A0A5
	public CharacterID characterHighlight { get; set; }

	// Token: 0x1700100A RID: 4106
	// (get) Token: 0x060043D7 RID: 17367 RVA: 0x0012BCAE File Offset: 0x0012A0AE
	// (set) Token: 0x060043D8 RID: 17368 RVA: 0x0012BCB6 File Offset: 0x0012A0B6
	public bool ShowedLoginBonus { get; set; }

	// Token: 0x060043D9 RID: 17369 RVA: 0x0012BCC0 File Offset: 0x0012A0C0
	public void RandomizeCharacter()
	{
		List<CharacterID> list = new List<CharacterID>();
		list.Add(CharacterID.Ashani);
		list.Add(CharacterID.Raymer);
		list.Add(CharacterID.Zhurong);
		list.Add(CharacterID.Weishan);
		list.Add(CharacterID.Xana);
		list.Add(CharacterID.Kidd);
		list.Add(CharacterID.CHARACTER_8);
		int index;
		if (list.Count == 1)
		{
			index = 0;
		}
		else
		{
			index = UnityEngine.Random.Range(0, list.Count);
		}
		this.characterHighlight = list[index];
	}

	// Token: 0x060043DA RID: 17370 RVA: 0x0012BD34 File Offset: 0x0012A134
	public string GetCurrentStage()
	{
		return this.characterStages[this.characterHighlight].scene;
	}

	// Token: 0x060043DB RID: 17371 RVA: 0x0012BD5C File Offset: 0x0012A15C
	public SoundKey GetCurrentMusic()
	{
		if (this.userAudioSettings.UseAltMenuMusic())
		{
			return this.characterStages[this.characterHighlight].altMusic;
		}
		return SoundKey.mainMenu_music;
	}

	// Token: 0x060043DC RID: 17372 RVA: 0x0012BD98 File Offset: 0x0012A198
	public SkinDefinition GetCharacterSkin()
	{
		CharacterMenusData data = this.characterMenusDataLoader.GetData(this.characterLists.GetCharacterDefinition(this.characterHighlight));
		if (data.mainMenuSkin.obj != null)
		{
			return data.mainMenuSkin.obj;
		}
		return this.skinDataManager.GetDefaultSkin(this.characterHighlight);
	}

	// Token: 0x060043DD RID: 17373 RVA: 0x0012BDF8 File Offset: 0x0012A1F8
	public void StartupLoad(Action callback)
	{
		this.RandomizeCharacter();
		UISceneCharacterManager.CharacterWithSkin item = default(UISceneCharacterManager.CharacterWithSkin);
		item.characterDef = this.characterLists.GetCharacterDefinition(this.characterHighlight);
		item.skinDefinition = this.GetCharacterSkin();
		List<UISceneCharacterManager.CharacterWithSkin> list = new List<UISceneCharacterManager.CharacterWithSkin>();
		list.Add(item);
		this.uiSceneCharacterManager.VRAMPreload(list, callback);
	}

	// Token: 0x04002D2D RID: 11565
	private Dictionary<CharacterID, MainMenuAPI.MenuTheme> characterStages = new Dictionary<CharacterID, MainMenuAPI.MenuTheme>
	{
		{
			CharacterID.Weishan,
			new MainMenuAPI.MenuTheme("MainMenuScene-Shrine", SoundKey.forbiddenShrineAlt_music)
		},
		{
			CharacterID.Zhurong,
			new MainMenuAPI.MenuTheme("MainMenuScene-Shrine", SoundKey.forbiddenShrineAlt_music)
		},
		{
			CharacterID.Raymer,
			new MainMenuAPI.MenuTheme("MainMenuScene-Cryo", SoundKey.maluMaluAlt_music)
		},
		{
			CharacterID.Ashani,
			new MainMenuAPI.MenuTheme("MainMenuScene-Cryo", SoundKey.combatLabAlt_music)
		},
		{
			CharacterID.Xana,
			new MainMenuAPI.MenuTheme("MainMenuScene-Cryo", SoundKey.wavedashArena_music)
		},
		{
			CharacterID.Kidd,
			new MainMenuAPI.MenuTheme("MainMenuScene-Cryo", SoundKey.cryoStation_music)
		},
		{
			CharacterID.AfiGalu,
			new MainMenuAPI.MenuTheme("MainMenuScene-Cryo", SoundKey.wavedashArena_music)
		},
		{
			CharacterID.CHARACTER_8,
			new MainMenuAPI.MenuTheme("MainMenuScene-Stage", SoundKey.shadowbriarAlt_music)
		}
	};

	// Token: 0x020009A8 RID: 2472
	private struct MenuTheme
	{
		// Token: 0x060043DE RID: 17374 RVA: 0x0012BE52 File Offset: 0x0012A252
		public MenuTheme(string scene, SoundKey altMusic)
		{
			this.scene = scene;
			this.altMusic = altMusic;
		}

		// Token: 0x04002D2E RID: 11566
		public string scene;

		// Token: 0x04002D2F RID: 11567
		public SoundKey altMusic;
	}
}
