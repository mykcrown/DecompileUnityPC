using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000578 RID: 1400
public interface IAnimationPlayer : IRollbackStateOwner, IAnimatorDataOwner
{
	// Token: 0x06001F55 RID: 8021
	void Init(AvatarData avatarData, GameObject gameObject, ConfigData config, bool mirror, bool overrideBlendingEnabled = false, bool replaceAnimator = true);

	// Token: 0x06001F56 RID: 8022
	bool LoadMove(MoveData move, bool replace = false);

	// Token: 0x06001F57 RID: 8023
	bool LoadMoveSet(CharacterMoveSetData moveSet, bool returnOnNullMove = false);

	// Token: 0x06001F58 RID: 8024
	void LoadSharedAnimations(CharacterMoveSetData opponentMoveSet, CharacterData owner);

	// Token: 0x06001F59 RID: 8025
	void LoadAnimation(WavedashAnimationData animationData);

	// Token: 0x06001F5A RID: 8026
	bool LoadCharacterComponentAnimations(CharacterMoveSetData moveSet, IEnumerable<ICharacterComponent> components);

	// Token: 0x06001F5B RID: 8027
	bool HasAnimation(string animName);

	// Token: 0x06001F5C RID: 8028
	void Test(string current, string previous);

	// Token: 0x06001F5D RID: 8029
	bool PlayAnimation(string animName, bool mirror, int frame, float blendDuration = -1f, float blendOutDuration = -1f);

	// Token: 0x06001F5E RID: 8030
	bool PlayThrow(string animName, bool mirror);

	// Token: 0x06001F5F RID: 8031
	bool PlayCharacterAction(CharacterActionData characterAction, string animationName, bool mirror, bool disableBlending = false);

	// Token: 0x06001F60 RID: 8032
	bool PlayCharacterAction(CharacterActionData characterAction, string animationName, bool mirror, Fixed overrideSpeed, bool disableBlending = false);

	// Token: 0x06001F61 RID: 8033
	void SetMecanimMirror(bool mirror);

	// Token: 0x06001F62 RID: 8034
	bool IsAnimationPlaying(string animName);

	// Token: 0x06001F63 RID: 8035
	float GetAnimationLength(string animName);

	// Token: 0x06001F64 RID: 8036
	int GetActionGameFramelength(CharacterActionData action);

	// Token: 0x06001F65 RID: 8037
	void SetPause(bool isPaused);

	// Token: 0x06001F66 RID: 8038
	void Update(int gameFrame);

	// Token: 0x06001F67 RID: 8039
	void ChangedAnimationDuringRollback();

	// Token: 0x06001F68 RID: 8040
	void ForceSyncAnimation();

	// Token: 0x06001F69 RID: 8041
	Fixed GetAnimationFrameFromGameFrame(string animName, int gameFrame);

	// Token: 0x06001F6A RID: 8042
	int GetNextGameFrameFromAnimationFrame(string animName, Fixed animFrame);

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x06001F6B RID: 8043
	Dictionary<string, AnimationTimeline> AnimationTimelineMap { get; }

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06001F6C RID: 8044
	AnimationData CurrentAnimationData { get; }

	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x06001F6D RID: 8045
	Dictionary<string, bool> AnimationReversesFacingMap { get; }

	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06001F6E RID: 8046
	ActionState CurrentActionState { get; }

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06001F6F RID: 8047
	Fixed CurrentSpeed { get; }

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06001F70 RID: 8048
	int CurrentAnimationGameFramelength { get; }
}
