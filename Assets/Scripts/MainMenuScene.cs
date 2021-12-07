// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : UIScene
{
	public Transform CharacterContainer;

	public GameObject Debug;

	public List<Camera> Cameras = new List<Camera>();

	private IUISceneCharacter characterDisplay;

	[Inject]
	public IMainMenuAPI api
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
	public ICharacterMenusDataLoader characterMenusDataLoader
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

	[PostConstruct]
	public void Init()
	{
		this.Debug.SetActive(false);
	}

	public bool isStageScene()
	{
		return true;
	}

	public void SetCharacter(CharacterID characterId)
	{
		if (this.characterDisplay != null)
		{
			this.uiSceneCharacterManager.ReleaseCharacter(this.characterDisplay);
		}
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterId);
		CharacterMenusData data = this.characterMenusDataLoader.GetData(characterDefinition);
		SkinDefinition characterSkin = this.api.GetCharacterSkin();
		this.characterDisplay = this.uiSceneCharacterManager.GetCharacter(characterId, characterSkin);
		List<WavedashAnimationData> allDefaultAnimations = this.characterDataHelper.GetAllDefaultAnimations(characterDefinition, CharacterDefaultAnimationKey.MAIN_MENU);
		this.characterDisplay.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations));
		this.characterDisplay.AddAligner(data.mainMenuAdjustments);
		(this.characterDisplay as MonoBehaviour).transform.SetParent(this.CharacterContainer, false);
	}

	public void Exit()
	{
		if (this.characterDisplay != null)
		{
			this.uiSceneCharacterManager.ReleaseCharacter(this.characterDisplay);
		}
		this.characterDisplay = null;
	}
}
