// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UICSSSceneCharacterManager : IUICSSSceneCharacterManager, IStartupLoader
{
	private Dictionary<CharacterID, List<UICSSSceneCharacter>> pool = new Dictionary<CharacterID, List<UICSSSceneCharacter>>();

	private GameObject container;

	private UICSSSceneCharacter characterPrefab;

	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
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
	public ICharacterLists characterLists
	{
		get;
		set;
	}

	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader
	{
		private get;
		set;
	}

	[Inject]
	public GameEnvironmentData environmentData
	{
		get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.container = new GameObject("UICSSCharacterManager");
		this.container.SetActive(false);
		UnityEngine.Object.DontDestroyOnLoad(this.container);
	}

	public void StartupLoad(Action callback)
	{
		if (SystemBoot.mode != SystemBoot.Mode.StagePreview)
		{
			this.Preload();
		}
		callback();
	}

	public void Preload()
	{
		this.cachePrefab();
		CharacterDefinition[] legalCharacters = this.characterLists.GetLegalCharacters();
		for (int i = 0; i < legalCharacters.Length; i++)
		{
			CharacterDefinition characterDefinition = legalCharacters[i];
			UICSSSceneCharacter character = this.GetCharacter(characterDefinition.characterID, this.skinDataManager.GetDefaultSkin(characterDefinition.characterID));
			this.ReleaseCharacter(character);
		}
	}

	private void cachePrefab()
	{
		if (this.characterPrefab == null)
		{
			this.characterPrefab = Resources.Load<UICSSSceneCharacter>("GUI/Menu/CharacterSelect/UICSSSceneChar");
		}
	}

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

	private void addToPool(UICSSSceneCharacter display)
	{
		CharacterID characterID = display.CharacterID;
		if (!this.pool.ContainsKey(characterID))
		{
			this.pool[characterID] = new List<UICSSSceneCharacter>();
		}
		this.pool[characterID].Add(display);
	}

	public UICSSSceneCharacter GetCharacter(CharacterID characterId, SkinDefinition initWithSkin)
	{
		UICSSSceneCharacter uICSSSceneCharacter = this.getFromPool(characterId);
		if (uICSSSceneCharacter == null)
		{
			CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterId);
			uICSSSceneCharacter = UnityEngine.Object.Instantiate<UICSSSceneCharacter>(this.characterPrefab);
			uICSSSceneCharacter.name = "UICSSChar-" + characterDefinition.characterName;
			UnityEngine.Object.DontDestroyOnLoad(uICSSSceneCharacter.gameObject);
			uICSSSceneCharacter.transform.SetParent(this.container.transform, false);
			this.injector.Inject(uICSSSceneCharacter);
			uICSSSceneCharacter.Init(this.characterMenusDataLoader.GetData(characterDefinition));
			if (!characterDefinition.isRandom)
			{
				uICSSSceneCharacter.SetCharacterDisplay(this.uiSceneCharacterManager.GetCharacter(characterId, initWithSkin));
			}
		}
		return uICSSSceneCharacter;
	}

	public void ReleaseCharacter(UICSSSceneCharacter characterDisplay)
	{
		if (this.container != null)
		{
			characterDisplay.transform.SetParent(this.container.transform, false);
			this.addToPool(characterDisplay);
		}
	}
}
