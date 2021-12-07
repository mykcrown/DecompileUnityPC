// Decompile from assembly: Assembly-CSharp.dll

using System;

public struct UISceneCharacterAnimRequest
{
	public enum AnimRequestType
	{
		AnimData,
		MoveData
	}

	public MoveData moveData;

	public MoveData loopingMoveData;

	public WavedashAnimationData animData;

	public WavedashAnimationData loopingAnimData;

	public UISceneCharacterAnimRequest.AnimRequestType type;

	public UISceneCharacter.AnimationMode mode;

	public override bool Equals(object other)
	{
		return other is UISceneCharacterAnimRequest && (UISceneCharacterAnimRequest)other == this;
	}

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

	public static bool operator ==(UISceneCharacterAnimRequest a, UISceneCharacterAnimRequest b)
	{
		return a.moveData == b.moveData && a.animData == b.animData && a.mode == b.mode && a.loopingMoveData == b.loopingMoveData && a.loopingAnimData == b.loopingAnimData;
	}

	public static bool operator !=(UISceneCharacterAnimRequest a, UISceneCharacterAnimRequest b)
	{
		return !(a == b);
	}
}
