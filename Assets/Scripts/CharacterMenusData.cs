// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CharacterMenusData : ScriptableObject
{
	[Serializable]
	public class UIPortraitData
	{
		public Vector2 offset;

		public float scale;

		public UIPortraitData()
		{
			this.offset = default(Vector2);
			this.scale = 1f;
		}
	}

	[Serializable]
	public class UICharacterAdjustments
	{
		public Vector3 position;

		public Vector3 rotation;

		public float scale = 1f;
	}

	public Sprite generalPortrait;

	public CharacterMenusData.UIPortraitData characterSelectPortraitData = new CharacterMenusData.UIPortraitData();

	public CharacterMenusData.UIPortraitData onlinePickPortraitData = new CharacterMenusData.UIPortraitData();

	public CharacterMenusData.UIPortraitData storeIntroPortraitData = new CharacterMenusData.UIPortraitData();

	public CharacterMenusData.UICharacterAdjustments characterSelectAdjustments = new CharacterMenusData.UICharacterAdjustments();

	public CharacterMenusData.UICharacterAdjustments onlinePickAdjustments = new CharacterMenusData.UICharacterAdjustments();

	public CharacterMenusData.UICharacterAdjustments unboxingAdjustments = new CharacterMenusData.UICharacterAdjustments();

	public CharacterMenusData.UICharacterAdjustments galleryAdjustments = new CharacterMenusData.UICharacterAdjustments();

	public CharacterMenusData.UICharacterAdjustments galleryAdjustmentsEmote = new CharacterMenusData.UICharacterAdjustments();

	public CharacterMenusData.UICharacterAdjustments galleryAdjustmentsVictoryPose = new CharacterMenusData.UICharacterAdjustments();

	public CharacterMenusData.UICharacterAdjustments mainMenuAdjustments = new CharacterMenusData.UICharacterAdjustments();

	public SkinDefinitionFile mainMenuSkin = new SkinDefinitionFile();

	public Sprite smallPortrait;

	public Sprite miniPortrait;

	public Vector3 uiRotateOffset;

	public Vector3 uiCharacterPositionOffset;

	public Vector3 previewCameraOffset = new Vector3(-1.5f, 1f, -7f);

	public Vector3 previewCameraRotation = new Vector3(0f, 0f, 0f);

	public CharacterDefaultAnimationData[] defaultAnimations = new CharacterDefaultAnimationData[0];

	public CharacterBoundsData bounds;

	public Avatar avatar;

	public Avatar genericAvatar;

	public LocalizationData localization = new LocalizationData();

	public CharacterDefinition characterDefinition;

	public AvatarData avatarData
	{
		get
		{
			return new AvatarData(this.avatar, this.genericAvatar);
		}
	}

	public CharacterID characterID
	{
		get
		{
			return this.characterDefinition.characterID;
		}
	}

	public bool isRandom
	{
		get
		{
			return this.characterDefinition.isRandom;
		}
	}

	public string characterName
	{
		get
		{
			return this.characterDefinition.characterName;
		}
	}
}
