using System;
using UnityEngine;

// Token: 0x02000588 RID: 1416
public class CharacterMenusData : ScriptableObject
{
	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x000A2367 File Offset: 0x000A0767
	public AvatarData avatarData
	{
		get
		{
			return new AvatarData(this.avatar, this.genericAvatar);
		}
	}

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x000A237A File Offset: 0x000A077A
	public CharacterID characterID
	{
		get
		{
			return this.characterDefinition.characterID;
		}
	}

	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x000A2387 File Offset: 0x000A0787
	public bool isRandom
	{
		get
		{
			return this.characterDefinition.isRandom;
		}
	}

	// Token: 0x170006F9 RID: 1785
	// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x000A2394 File Offset: 0x000A0794
	public string characterName
	{
		get
		{
			return this.characterDefinition.characterName;
		}
	}

	// Token: 0x04001952 RID: 6482
	public Sprite generalPortrait;

	// Token: 0x04001953 RID: 6483
	public CharacterMenusData.UIPortraitData characterSelectPortraitData = new CharacterMenusData.UIPortraitData();

	// Token: 0x04001954 RID: 6484
	public CharacterMenusData.UIPortraitData onlinePickPortraitData = new CharacterMenusData.UIPortraitData();

	// Token: 0x04001955 RID: 6485
	public CharacterMenusData.UIPortraitData storeIntroPortraitData = new CharacterMenusData.UIPortraitData();

	// Token: 0x04001956 RID: 6486
	public CharacterMenusData.UICharacterAdjustments characterSelectAdjustments = new CharacterMenusData.UICharacterAdjustments();

	// Token: 0x04001957 RID: 6487
	public CharacterMenusData.UICharacterAdjustments onlinePickAdjustments = new CharacterMenusData.UICharacterAdjustments();

	// Token: 0x04001958 RID: 6488
	public CharacterMenusData.UICharacterAdjustments unboxingAdjustments = new CharacterMenusData.UICharacterAdjustments();

	// Token: 0x04001959 RID: 6489
	public CharacterMenusData.UICharacterAdjustments galleryAdjustments = new CharacterMenusData.UICharacterAdjustments();

	// Token: 0x0400195A RID: 6490
	public CharacterMenusData.UICharacterAdjustments galleryAdjustmentsEmote = new CharacterMenusData.UICharacterAdjustments();

	// Token: 0x0400195B RID: 6491
	public CharacterMenusData.UICharacterAdjustments galleryAdjustmentsVictoryPose = new CharacterMenusData.UICharacterAdjustments();

	// Token: 0x0400195C RID: 6492
	public CharacterMenusData.UICharacterAdjustments mainMenuAdjustments = new CharacterMenusData.UICharacterAdjustments();

	// Token: 0x0400195D RID: 6493
	public SkinDefinitionFile mainMenuSkin = new SkinDefinitionFile();

	// Token: 0x0400195E RID: 6494
	public Sprite smallPortrait;

	// Token: 0x0400195F RID: 6495
	public Sprite miniPortrait;

	// Token: 0x04001960 RID: 6496
	public Vector3 uiRotateOffset;

	// Token: 0x04001961 RID: 6497
	public Vector3 uiCharacterPositionOffset;

	// Token: 0x04001962 RID: 6498
	public Vector3 previewCameraOffset = new Vector3(-1.5f, 1f, -7f);

	// Token: 0x04001963 RID: 6499
	public Vector3 previewCameraRotation = new Vector3(0f, 0f, 0f);

	// Token: 0x04001964 RID: 6500
	public CharacterDefaultAnimationData[] defaultAnimations = new CharacterDefaultAnimationData[0];

	// Token: 0x04001965 RID: 6501
	public CharacterBoundsData bounds;

	// Token: 0x04001966 RID: 6502
	public Avatar avatar;

	// Token: 0x04001967 RID: 6503
	public Avatar genericAvatar;

	// Token: 0x04001968 RID: 6504
	public LocalizationData localization = new LocalizationData();

	// Token: 0x04001969 RID: 6505
	public CharacterDefinition characterDefinition;

	// Token: 0x02000589 RID: 1417
	[Serializable]
	public class UIPortraitData
	{
		// Token: 0x06001FF9 RID: 8185 RVA: 0x000A23A4 File Offset: 0x000A07A4
		public UIPortraitData()
		{
			this.offset = default(Vector2);
			this.scale = 1f;
		}

		// Token: 0x0400196A RID: 6506
		public Vector2 offset;

		// Token: 0x0400196B RID: 6507
		public float scale;
	}

	// Token: 0x0200058A RID: 1418
	[Serializable]
	public class UICharacterAdjustments
	{
		// Token: 0x0400196C RID: 6508
		public Vector3 position;

		// Token: 0x0400196D RID: 6509
		public Vector3 rotation;

		// Token: 0x0400196E RID: 6510
		public float scale = 1f;
	}
}
