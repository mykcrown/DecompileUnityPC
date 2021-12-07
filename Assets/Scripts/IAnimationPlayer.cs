// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationPlayer : IRollbackStateOwner, IAnimatorDataOwner
{
	Dictionary<string, AnimationTimeline> AnimationTimelineMap
	{
		get;
	}

	AnimationData CurrentAnimationData
	{
		get;
	}

	Dictionary<string, bool> AnimationReversesFacingMap
	{
		get;
	}

	ActionState CurrentActionState
	{
		get;
	}

	Fixed CurrentSpeed
	{
		get;
	}

	int CurrentAnimationGameFramelength
	{
		get;
	}

	void Init(AvatarData avatarData, GameObject gameObject, ConfigData config, bool mirror, bool overrideBlendingEnabled = false, bool replaceAnimator = true);

	bool LoadMove(MoveData move, bool replace = false);

	bool LoadMoveSet(CharacterMoveSetData moveSet, bool returnOnNullMove = false);

	void LoadSharedAnimations(CharacterMoveSetData opponentMoveSet, CharacterData owner);

	void LoadAnimation(WavedashAnimationData animationData);

	bool LoadCharacterComponentAnimations(CharacterMoveSetData moveSet, IEnumerable<ICharacterComponent> components);

	bool HasAnimation(string animName);

	void Test(string current, string previous);

	bool PlayAnimation(string animName, bool mirror, int frame, float blendDuration = -1f, float blendOutDuration = -1f);

	bool PlayThrow(string animName, bool mirror);

	bool PlayCharacterAction(CharacterActionData characterAction, string animationName, bool mirror, bool disableBlending = false);

	bool PlayCharacterAction(CharacterActionData characterAction, string animationName, bool mirror, Fixed overrideSpeed, bool disableBlending = false);

	void SetMecanimMirror(bool mirror);

	bool IsAnimationPlaying(string animName);

	float GetAnimationLength(string animName);

	int GetActionGameFramelength(CharacterActionData action);

	void SetPause(bool isPaused);

	void Update(int gameFrame);

	void ChangedAnimationDuringRollback();

	void ForceSyncAnimation();

	Fixed GetAnimationFrameFromGameFrame(string animName, int gameFrame);

	int GetNextGameFrameFromAnimationFrame(string animName, Fixed animFrame);
}
