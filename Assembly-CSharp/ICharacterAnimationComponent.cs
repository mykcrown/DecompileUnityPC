using System;

// Token: 0x020005BE RID: 1470
public interface ICharacterAnimationComponent
{
	// Token: 0x060020B8 RID: 8376
	CharacterAnimation[] GetCharacterAnimations();

	// Token: 0x17000739 RID: 1849
	// (get) Token: 0x060020B9 RID: 8377
	string AnimationSuffix { get; }

	// Token: 0x060020BA RID: 8378
	bool IsOverridingActionStateAnimation(ActionState actionState, HorizontalDirection facing, ref string animationName);

	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x060020BB RID: 8379
	bool PreventActionStateAnimations { get; }
}
