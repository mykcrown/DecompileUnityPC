// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class PreviewAnimationHelper : IPreviewAnimationHelper
{
	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

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
