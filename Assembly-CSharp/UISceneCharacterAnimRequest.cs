using System;

// Token: 0x02000A6A RID: 2666
public struct UISceneCharacterAnimRequest
{
	// Token: 0x06004D9B RID: 19867 RVA: 0x001478F9 File Offset: 0x00145CF9
	public override bool Equals(object other)
	{
		return other is UISceneCharacterAnimRequest && (UISceneCharacterAnimRequest)other == this;
	}

	// Token: 0x06004D9C RID: 19868 RVA: 0x0014791C File Offset: 0x00145D1C
	public override int GetHashCode()
	{
		int num = 17;
		num = num * 7 + ((!(this.moveData == null)) ? this.moveData.GetHashCode() : 0);
		num = num * 7 + ((this.animData != null) ? this.animData.GetHashCode() : 0);
		num = num * 7 + ((!(this.loopingMoveData == null)) ? this.loopingMoveData.GetHashCode() : 0);
		num = num * 7 + ((this.loopingAnimData != null) ? this.loopingAnimData.GetHashCode() : 0);
		num = (int)(num * 7 + this.mode);
		return (int)(num * 7 + this.type);
	}

	// Token: 0x06004D9D RID: 19869 RVA: 0x001479D4 File Offset: 0x00145DD4
	public static bool operator ==(UISceneCharacterAnimRequest a, UISceneCharacterAnimRequest b)
	{
		return a.moveData == b.moveData && a.animData == b.animData && a.mode == b.mode && a.loopingMoveData == b.loopingMoveData && a.loopingAnimData == b.loopingAnimData;
	}

	// Token: 0x06004D9E RID: 19870 RVA: 0x00147A4A File Offset: 0x00145E4A
	public static bool operator !=(UISceneCharacterAnimRequest a, UISceneCharacterAnimRequest b)
	{
		return !(a == b);
	}

	// Token: 0x040032DC RID: 13020
	public MoveData moveData;

	// Token: 0x040032DD RID: 13021
	public MoveData loopingMoveData;

	// Token: 0x040032DE RID: 13022
	public WavedashAnimationData animData;

	// Token: 0x040032DF RID: 13023
	public WavedashAnimationData loopingAnimData;

	// Token: 0x040032E0 RID: 13024
	public UISceneCharacterAnimRequest.AnimRequestType type;

	// Token: 0x040032E1 RID: 13025
	public UISceneCharacter.AnimationMode mode;

	// Token: 0x02000A6B RID: 2667
	public enum AnimRequestType
	{
		// Token: 0x040032E3 RID: 13027
		AnimData,
		// Token: 0x040032E4 RID: 13028
		MoveData
	}
}
