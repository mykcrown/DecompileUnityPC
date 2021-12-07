// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICharacterAnimationComponent
{
	string AnimationSuffix
	{
		get;
	}

	bool PreventActionStateAnimations
	{
		get;
	}

	CharacterAnimation[] GetCharacterAnimations();

	bool IsOverridingActionStateAnimation(ActionState actionState, HorizontalDirection facing, ref string animationName);
}
