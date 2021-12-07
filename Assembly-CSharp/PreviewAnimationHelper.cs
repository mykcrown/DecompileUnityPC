using System;
using System.Collections.Generic;

// Token: 0x02000A1F RID: 2591
public class PreviewAnimationHelper : IPreviewAnimationHelper
{
	// Token: 0x170011F5 RID: 4597
	// (get) Token: 0x06004B5F RID: 19295 RVA: 0x00141A7C File Offset: 0x0013FE7C
	// (set) Token: 0x06004B60 RID: 19296 RVA: 0x00141A84 File Offset: 0x0013FE84
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x06004B61 RID: 19297 RVA: 0x00141A90 File Offset: 0x0013FE90
	public void PlayVictoryPose(IUISceneCharacter characterDisplay, CharacterDefinition characterDef, EquippableItem item)
	{
		if (characterDisplay == null || characterDef == null || item == null)
		{
			return;
		}
		List<UISceneCharacterAnimRequest> allAnimationRequestsFromItem = this.characterDataHelper.GetAllAnimationRequestsFromItem(characterDef, item);
		for (int i = 0; i < allAnimationRequestsFromItem.Count; i++)
		{
			UISceneCharacterAnimRequest value = allAnimationRequestsFromItem[i];
			if (value.type == UISceneCharacterAnimRequest.AnimRequestType.AnimData && value.loopingAnimData == null)
			{
				value.loopingAnimData = value.animData;
			}
			else if (value.type == UISceneCharacterAnimRequest.AnimRequestType.MoveData && value.loopingMoveData == null)
			{
				value.loopingMoveData = value.moveData;
			}
			allAnimationRequestsFromItem[i] = value;
		}
		characterDisplay.PlayVictoryPose(allAnimationRequestsFromItem);
	}

	// Token: 0x06004B62 RID: 19298 RVA: 0x00141B4C File Offset: 0x0013FF4C
	public void PlayBasicAnimation(IUISceneCharacter characterDisplay, CharacterDefinition characterDef, EquippableItem item, int selectedCharacterIndex)
	{
		if (characterDisplay == null || characterDef == null || item == null)
		{
			return;
		}
		List<UISceneCharacterAnimRequest> allAnimationRequestsFromItem = this.characterDataHelper.GetAllAnimationRequestsFromItem(characterDef, item);
		for (int i = 0; i < allAnimationRequestsFromItem.Count; i++)
		{
			if (i > 0)
			{
				allAnimationRequestsFromItem[i] = default(UISceneCharacterAnimRequest);
			}
		}
		characterDisplay.PlayAnimations(allAnimationRequestsFromItem);
	}
}
