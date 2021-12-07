// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class PurchasePreviewScene3D : UIScene
{
	public Transform CharacterAnchor;

	private IUISceneCharacter characterDisplay;

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

	public void RenderToTexture(RenderTexture texture, CharacterID characterId, SkinDefinition skin)
	{
		if (this.characterDisplay != null)
		{
			this.uiSceneCharacterManager.ReleaseCharacter(this.characterDisplay);
		}
		base.gameObject.SetActive(true);
		this.myCamera.targetTexture = texture;
		this.characterDisplay = this.uiSceneCharacterManager.GetCharacter(characterId, skin);
		this.characterDisplay.Activate(this.CharacterAnchor);
		CharacterMenusData data = this.characterMenusDataLoader.GetData(characterId);
		List<WavedashAnimationData> allDefaultAnimations = this.characterDataHelper.GetAllDefaultAnimations(data.characterDefinition, CharacterDefaultAnimationKey.STORE_IDLE);
		this.characterDisplay.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations));
		this.characterDisplay.SetMode(UIAssetDisplayMode.OffsetRotate);
		this.myCamera.transform.localPosition = data.previewCameraOffset;
		this.myCamera.transform.rotation = Quaternion.Euler(data.previewCameraRotation);
	}
}
