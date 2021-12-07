using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A21 RID: 2593
public class PurchasePreviewScene3D : UIScene
{
	// Token: 0x170011F6 RID: 4598
	// (get) Token: 0x06004B66 RID: 19302 RVA: 0x00141BBD File Offset: 0x0013FFBD
	// (set) Token: 0x06004B67 RID: 19303 RVA: 0x00141BC5 File Offset: 0x0013FFC5
	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager { private get; set; }

	// Token: 0x170011F7 RID: 4599
	// (get) Token: 0x06004B68 RID: 19304 RVA: 0x00141BCE File Offset: 0x0013FFCE
	// (set) Token: 0x06004B69 RID: 19305 RVA: 0x00141BD6 File Offset: 0x0013FFD6
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x170011F8 RID: 4600
	// (get) Token: 0x06004B6A RID: 19306 RVA: 0x00141BDF File Offset: 0x0013FFDF
	// (set) Token: 0x06004B6B RID: 19307 RVA: 0x00141BE7 File Offset: 0x0013FFE7
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x06004B6C RID: 19308 RVA: 0x00141BF0 File Offset: 0x0013FFF0
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

	// Token: 0x0400318A RID: 12682
	public Transform CharacterAnchor;

	// Token: 0x0400318B RID: 12683
	private IUISceneCharacter characterDisplay;
}
