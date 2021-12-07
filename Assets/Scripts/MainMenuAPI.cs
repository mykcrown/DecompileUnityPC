// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAPI : IMainMenuAPI, IStartupLoader
{
	private struct MenuTheme
	{
		public string scene;

		public SoundKey altMusic;

		public MenuTheme(string scene, SoundKey altMusic)
		{
			this.scene = scene;
			this.altMusic = altMusic;
		}
	}

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

	[Inject]
	public ICharacterLists characterLists
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
	public ICharacterMenusDataLoader characterMenusDataLoader
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
	public UserAudioSettings userAudioSettings
	{
		private get;
		set;
	}

	public CharacterID characterHighlight
	{
		get;
		set;
	}

	public bool ShowedLoginBonus
	{
		get;
		set;
	}

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

	public string GetCurrentStage()
	{
		return this.characterStages[this.characterHighlight].scene;
	}

	public SoundKey GetCurrentMusic()
	{
		if (this.userAudioSettings.UseAltMenuMusic())
		{
			return this.characterStages[this.characterHighlight].altMusic;
		}
		return SoundKey.mainMenu_music;
	}

	public SkinDefinition GetCharacterSkin()
	{
		CharacterMenusData data = this.characterMenusDataLoader.GetData(this.characterLists.GetCharacterDefinition(this.characterHighlight));
		if (data.mainMenuSkin.obj != null)
		{
			return data.mainMenuSkin.obj;
		}
		return this.skinDataManager.GetDefaultSkin(this.characterHighlight);
	}

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
}
