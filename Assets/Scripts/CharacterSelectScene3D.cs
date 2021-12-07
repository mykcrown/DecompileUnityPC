// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectScene3D : UIScene
{
	public GameObject CharacterContainer;

	private Dictionary<PlayerNum, CharacterID> index = new Dictionary<PlayerNum, CharacterID>();

	private Dictionary<PlayerNum, UICSSSceneCharacter> characterIndex = new Dictionary<PlayerNum, UICSSSceneCharacter>(default(PlayerNumComparer));

	[Inject]
	public ICharacterDataHelper characterDataHelper
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
	public IUICSSSceneCharacterManager cssCharacterManager
	{
		private get;
		set;
	}

	public UICSSSceneCharacter GetCharacterDisplay(PlayerNum playerNum)
	{
		if (this.characterIndex.ContainsKey(playerNum))
		{
			return this.characterIndex[playerNum];
		}
		return null;
	}

	public int GetClickedCharacterIndex(PlayerNum playerNum, Vector2 position)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			return characterDisplay.GetClickedCharacterIndex(position, this.myCamera);
		}
		return -1;
	}

	private bool alreadyExists(PlayerNum playerNum, CharacterID characterId)
	{
		return this.index.ContainsKey(playerNum) && this.index[playerNum] == characterId;
	}

	public void ResetAllPlayers()
	{
		foreach (UICSSSceneCharacter current in this.characterIndex.Values)
		{
			this.cssCharacterManager.ReleaseCharacter(current);
		}
		this.characterIndex.Clear();
		this.index.Clear();
	}

	public void HideCharacter(PlayerNum playerNum)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			this.cssCharacterManager.ReleaseCharacter(characterDisplay);
		}
		this.characterIndex.Remove(playerNum);
		this.index.Remove(playerNum);
	}

	public void SetReadyState(PlayerNum playerNum, bool enabled)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			characterDisplay.SetGlowState(enabled);
		}
	}

	public void PlayTransition(PlayerNum playerNum, List<UISceneCharacterAnimRequest> animations)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			characterDisplay.PlayTransition(animations);
		}
	}

	public void SetDefaultAnimations(PlayerNum playerNum, List<WavedashAnimationData> animations)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			characterDisplay.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(animations));
		}
	}

	public void ChangeFrontCharIndex(PlayerNum playerNum, int index)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			characterDisplay.ChangeFrontCharIndex(index);
		}
	}

	public bool IsCharacterSwapping(PlayerNum playerNum)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		return characterDisplay != null && characterDisplay.IsCharacterSwapping;
	}

	public void SetCharacter(PlayerNum playerNum, CharacterID characterId, SkinDefinition skin, float positionIndex, Transform attachTo, bool useCameraLensingCompensation)
	{
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterId);
		UICSSSceneCharacter uICSSSceneCharacter;
		if (this.alreadyExists(playerNum, characterId))
		{
			uICSSSceneCharacter = this.GetCharacterDisplay(playerNum);
		}
		else
		{
			UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
			if (characterDisplay != null)
			{
				this.cssCharacterManager.ReleaseCharacter(characterDisplay);
			}
			uICSSSceneCharacter = this.cssCharacterManager.GetCharacter(characterId, skin);
			this.characterIndex[playerNum] = uICSSSceneCharacter;
			uICSSSceneCharacter.Activate(this.CharacterContainer.transform);
			if (!characterDefinition.isRandom)
			{
				List<WavedashAnimationData> allDefaultAnimations = this.characterDataHelper.GetAllDefaultAnimations(characterDefinition, CharacterDefaultAnimationKey.CHARACTER_SELECT_IDLE);
				uICSSSceneCharacter.CharacterDisplay.ChangeFrontCharIndex(0);
				uICSSSceneCharacter.CharacterDisplay.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations));
				uICSSSceneCharacter.CharacterDisplay.SetMode(UIAssetDisplayMode.OffsetRotate);
				uICSSSceneCharacter.CharacterDisplay.ResetRotation();
				this.LoadAdjustments(uICSSSceneCharacter);
			}
			this.setScaleAndPosition(uICSSSceneCharacter, useCameraLensingCompensation);
			uICSSSceneCharacter.Attach(attachTo, this.myCamera);
			this.index[playerNum] = characterId;
		}
		if (!characterDefinition.isRandom)
		{
			uICSSSceneCharacter.CharacterDisplay.SetSkin(skin);
		}
		uICSSSceneCharacter.SetIndex(positionIndex);
	}

	public void LoadAdjustments(UICSSSceneCharacter characterDisplay)
	{
		bool flag = this is OnlineBlindPickScene3D;
		characterDisplay.LoadAdjustments((!flag) ? characterDisplay.characterMenusData.characterSelectAdjustments : characterDisplay.characterMenusData.onlinePickAdjustments);
	}

	private void setScaleAndPosition(UICSSSceneCharacter characterDisplay, bool useCameraLensingCompensation)
	{
		float num = -0.92f;
		float num2 = 1.125f;
		float z;
		float num3;
		if (useCameraLensingCompensation)
		{
			z = num;
			num3 = num2;
		}
		else
		{
			z = 0f;
			num3 = 1f;
		}
		Vector3 localPosition = characterDisplay.transform.localPosition;
		localPosition.z = z;
		characterDisplay.transform.localPosition = localPosition;
		Vector3 localScale = characterDisplay.transform.localScale;
		localScale.y = num3;
		characterDisplay.transform.localScale = localScale;
		characterDisplay.UpdateGlowStateWidth(num3);
	}
}
