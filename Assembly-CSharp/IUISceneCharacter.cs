using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A6D RID: 2669
public interface IUISceneCharacter
{
	// Token: 0x06004DA0 RID: 19872
	void Init(CharacterMenusData characterMenusData, SkinDefinition initWithSkin);

	// Token: 0x06004DA1 RID: 19873
	void Activate(Transform setParent);

	// Token: 0x06004DA2 RID: 19874
	void Reinitialize();

	// Token: 0x06004DA3 RID: 19875
	void Attach(Transform attachTo, Camera usingCamera);

	// Token: 0x06004DA4 RID: 19876
	void AddAligner(CharacterMenusData.UICharacterAdjustments aligner);

	// Token: 0x06004DA5 RID: 19877
	void RemoveAligners();

	// Token: 0x06004DA6 RID: 19878
	void SetSkin(SkinDefinition skinDefinition);

	// Token: 0x06004DA7 RID: 19879
	void PlayVictoryPose(List<UISceneCharacterAnimRequest> animations);

	// Token: 0x06004DA8 RID: 19880
	void PlayAnimations(List<UISceneCharacterAnimRequest> animations);

	// Token: 0x06004DA9 RID: 19881
	void SetDefaultAnimations(List<UISceneCharacterAnimRequest> animation);

	// Token: 0x06004DAA RID: 19882
	void ChangeFrontCharIndex(int value);

	// Token: 0x06004DAB RID: 19883
	void PlayTransition(List<UISceneCharacterAnimRequest> animations);

	// Token: 0x06004DAC RID: 19884
	void InstantSyncFrontCharacter();

	// Token: 0x06004DAD RID: 19885
	void SetMode(UIAssetDisplayMode mode);

	// Token: 0x1700125D RID: 4701
	// (get) Token: 0x06004DAE RID: 19886
	bool IsCharacterSwapping { get; }

	// Token: 0x06004DAF RID: 19887
	void ResetRotation();

	// Token: 0x06004DB0 RID: 19888
	int GetClickedCharacterIndex(Vector2 clickPosition, Camera theCamera);

	// Token: 0x06004DB1 RID: 19889
	GameObject GetCharacterObject(int index);

	// Token: 0x1700125E RID: 4702
	// (get) Token: 0x06004DB2 RID: 19890
	CharacterID CharacterID { get; }
}
