using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008FB RID: 2299
public class CharacterSelectScene3D : UIScene
{
	// Token: 0x17000E43 RID: 3651
	// (get) Token: 0x06003B71 RID: 15217 RVA: 0x00114E13 File Offset: 0x00113213
	// (set) Token: 0x06003B72 RID: 15218 RVA: 0x00114E1B File Offset: 0x0011321B
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x17000E44 RID: 3652
	// (get) Token: 0x06003B73 RID: 15219 RVA: 0x00114E24 File Offset: 0x00113224
	// (set) Token: 0x06003B74 RID: 15220 RVA: 0x00114E2C File Offset: 0x0011322C
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x17000E45 RID: 3653
	// (get) Token: 0x06003B75 RID: 15221 RVA: 0x00114E35 File Offset: 0x00113235
	// (set) Token: 0x06003B76 RID: 15222 RVA: 0x00114E3D File Offset: 0x0011323D
	[Inject]
	public IUICSSSceneCharacterManager cssCharacterManager { private get; set; }

	// Token: 0x06003B77 RID: 15223 RVA: 0x00114E46 File Offset: 0x00113246
	public UICSSSceneCharacter GetCharacterDisplay(PlayerNum playerNum)
	{
		if (this.characterIndex.ContainsKey(playerNum))
		{
			return this.characterIndex[playerNum];
		}
		return null;
	}

	// Token: 0x06003B78 RID: 15224 RVA: 0x00114E68 File Offset: 0x00113268
	public int GetClickedCharacterIndex(PlayerNum playerNum, Vector2 position)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			return characterDisplay.GetClickedCharacterIndex(position, this.myCamera);
		}
		return -1;
	}

	// Token: 0x06003B79 RID: 15225 RVA: 0x00114E98 File Offset: 0x00113298
	private bool alreadyExists(PlayerNum playerNum, CharacterID characterId)
	{
		return this.index.ContainsKey(playerNum) && this.index[playerNum] == characterId;
	}

	// Token: 0x06003B7A RID: 15226 RVA: 0x00114EC0 File Offset: 0x001132C0
	public void ResetAllPlayers()
	{
		foreach (UICSSSceneCharacter characterDisplay in this.characterIndex.Values)
		{
			this.cssCharacterManager.ReleaseCharacter(characterDisplay);
		}
		this.characterIndex.Clear();
		this.index.Clear();
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x00114F3C File Offset: 0x0011333C
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

	// Token: 0x06003B7C RID: 15228 RVA: 0x00114F84 File Offset: 0x00113384
	public void SetReadyState(PlayerNum playerNum, bool enabled)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			characterDisplay.SetGlowState(enabled);
		}
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x00114FAC File Offset: 0x001133AC
	public void PlayTransition(PlayerNum playerNum, List<UISceneCharacterAnimRequest> animations)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			characterDisplay.PlayTransition(animations);
		}
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x00114FD4 File Offset: 0x001133D4
	public void SetDefaultAnimations(PlayerNum playerNum, List<WavedashAnimationData> animations)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			characterDisplay.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(animations));
		}
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x00115004 File Offset: 0x00113404
	public void ChangeFrontCharIndex(PlayerNum playerNum, int index)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		if (characterDisplay != null)
		{
			characterDisplay.ChangeFrontCharIndex(index);
		}
	}

	// Token: 0x06003B80 RID: 15232 RVA: 0x0011502C File Offset: 0x0011342C
	public bool IsCharacterSwapping(PlayerNum playerNum)
	{
		UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
		return characterDisplay != null && characterDisplay.IsCharacterSwapping;
	}

	// Token: 0x06003B81 RID: 15233 RVA: 0x00115058 File Offset: 0x00113458
	public void SetCharacter(PlayerNum playerNum, CharacterID characterId, SkinDefinition skin, float positionIndex, Transform attachTo, bool useCameraLensingCompensation)
	{
		CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterId);
		UICSSSceneCharacter uicsssceneCharacter;
		if (this.alreadyExists(playerNum, characterId))
		{
			uicsssceneCharacter = this.GetCharacterDisplay(playerNum);
		}
		else
		{
			UICSSSceneCharacter characterDisplay = this.GetCharacterDisplay(playerNum);
			if (characterDisplay != null)
			{
				this.cssCharacterManager.ReleaseCharacter(characterDisplay);
			}
			uicsssceneCharacter = this.cssCharacterManager.GetCharacter(characterId, skin);
			this.characterIndex[playerNum] = uicsssceneCharacter;
			uicsssceneCharacter.Activate(this.CharacterContainer.transform);
			if (!characterDefinition.isRandom)
			{
				List<WavedashAnimationData> allDefaultAnimations = this.characterDataHelper.GetAllDefaultAnimations(characterDefinition, CharacterDefaultAnimationKey.CHARACTER_SELECT_IDLE);
				uicsssceneCharacter.CharacterDisplay.ChangeFrontCharIndex(0);
				uicsssceneCharacter.CharacterDisplay.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations));
				uicsssceneCharacter.CharacterDisplay.SetMode(UIAssetDisplayMode.OffsetRotate);
				uicsssceneCharacter.CharacterDisplay.ResetRotation();
				this.LoadAdjustments(uicsssceneCharacter);
			}
			this.setScaleAndPosition(uicsssceneCharacter, useCameraLensingCompensation);
			uicsssceneCharacter.Attach(attachTo, this.myCamera);
			this.index[playerNum] = characterId;
		}
		if (!characterDefinition.isRandom)
		{
			uicsssceneCharacter.CharacterDisplay.SetSkin(skin);
		}
		uicsssceneCharacter.SetIndex(positionIndex);
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x00115174 File Offset: 0x00113574
	public void LoadAdjustments(UICSSSceneCharacter characterDisplay)
	{
		bool flag = this is OnlineBlindPickScene3D;
		characterDisplay.LoadAdjustments((!flag) ? characterDisplay.characterMenusData.characterSelectAdjustments : characterDisplay.characterMenusData.onlinePickAdjustments);
	}

	// Token: 0x06003B83 RID: 15235 RVA: 0x001151B4 File Offset: 0x001135B4
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

	// Token: 0x040028F1 RID: 10481
	public GameObject CharacterContainer;

	// Token: 0x040028F5 RID: 10485
	private Dictionary<PlayerNum, CharacterID> index = new Dictionary<PlayerNum, CharacterID>();

	// Token: 0x040028F6 RID: 10486
	private Dictionary<PlayerNum, UICSSSceneCharacter> characterIndex = new Dictionary<PlayerNum, UICSSSceneCharacter>(default(PlayerNumComparer));
}
