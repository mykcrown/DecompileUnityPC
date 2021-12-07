using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009AB RID: 2475
public class MainMenuScene : UIScene
{
	// Token: 0x17001017 RID: 4119
	// (get) Token: 0x0600441D RID: 17437 RVA: 0x0012C5B9 File Offset: 0x0012A9B9
	// (set) Token: 0x0600441E RID: 17438 RVA: 0x0012C5C1 File Offset: 0x0012A9C1
	[Inject]
	public IMainMenuAPI api { private get; set; }

	// Token: 0x17001018 RID: 4120
	// (get) Token: 0x0600441F RID: 17439 RVA: 0x0012C5CA File Offset: 0x0012A9CA
	// (set) Token: 0x06004420 RID: 17440 RVA: 0x0012C5D2 File Offset: 0x0012A9D2
	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager { private get; set; }

	// Token: 0x17001019 RID: 4121
	// (get) Token: 0x06004421 RID: 17441 RVA: 0x0012C5DB File Offset: 0x0012A9DB
	// (set) Token: 0x06004422 RID: 17442 RVA: 0x0012C5E3 File Offset: 0x0012A9E3
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x1700101A RID: 4122
	// (get) Token: 0x06004423 RID: 17443 RVA: 0x0012C5EC File Offset: 0x0012A9EC
	// (set) Token: 0x06004424 RID: 17444 RVA: 0x0012C5F4 File Offset: 0x0012A9F4
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x1700101B RID: 4123
	// (get) Token: 0x06004425 RID: 17445 RVA: 0x0012C5FD File Offset: 0x0012A9FD
	// (set) Token: 0x06004426 RID: 17446 RVA: 0x0012C605 File Offset: 0x0012AA05
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x1700101C RID: 4124
	// (get) Token: 0x06004427 RID: 17447 RVA: 0x0012C60E File Offset: 0x0012AA0E
	// (set) Token: 0x06004428 RID: 17448 RVA: 0x0012C616 File Offset: 0x0012AA16
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x06004429 RID: 17449 RVA: 0x0012C61F File Offset: 0x0012AA1F
	[PostConstruct]
	public void Init()
	{
		this.Debug.SetActive(false);
	}

	// Token: 0x0600442A RID: 17450 RVA: 0x0012C62D File Offset: 0x0012AA2D
	public bool isStageScene()
	{
		return true;
	}

	// Token: 0x0600442B RID: 17451 RVA: 0x0012C630 File Offset: 0x0012AA30
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

	// Token: 0x0600442C RID: 17452 RVA: 0x0012C6DF File Offset: 0x0012AADF
	public void Exit()
	{
		if (this.characterDisplay != null)
		{
			this.uiSceneCharacterManager.ReleaseCharacter(this.characterDisplay);
		}
		this.characterDisplay = null;
	}

	// Token: 0x04002D52 RID: 11602
	public Transform CharacterContainer;

	// Token: 0x04002D53 RID: 11603
	public GameObject Debug;

	// Token: 0x04002D54 RID: 11604
	public List<Camera> Cameras = new List<Camera>();

	// Token: 0x04002D55 RID: 11605
	private IUISceneCharacter characterDisplay;
}
