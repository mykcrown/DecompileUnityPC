// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IUISceneCharacter
{
	bool IsCharacterSwapping
	{
		get;
	}

	CharacterID CharacterID
	{
		get;
	}

	void Init(CharacterMenusData characterMenusData, SkinDefinition initWithSkin);

	void Activate(Transform setParent);

	void Reinitialize();

	void Attach(Transform attachTo, Camera usingCamera);

	void AddAligner(CharacterMenusData.UICharacterAdjustments aligner);

	void RemoveAligners();

	void SetSkin(SkinDefinition skinDefinition);

	void PlayVictoryPose(List<UISceneCharacterAnimRequest> animations);

	void PlayAnimations(List<UISceneCharacterAnimRequest> animations);

	void SetDefaultAnimations(List<UISceneCharacterAnimRequest> animation);

	void ChangeFrontCharIndex(int value);

	void PlayTransition(List<UISceneCharacterAnimRequest> animations);

	void InstantSyncFrontCharacter();

	void SetMode(UIAssetDisplayMode mode);

	void ResetRotation();

	int GetClickedCharacterIndex(Vector2 clickPosition, Camera theCamera);

	GameObject GetCharacterObject(int index);
}
