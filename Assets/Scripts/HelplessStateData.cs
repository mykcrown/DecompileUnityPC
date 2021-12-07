// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class HelplessStateData
{
	public AnimationClipFile animationClipFile = new AnimationClipFile();

	public AnimationClipFile leftAnimationClipFile = new AnimationClipFile();

	public Fixed airMobilityMulti = 1;

	public Fixed maxHAirVelocityMulti = 1;

	public AnimationClip animationClip
	{
		get
		{
			return this.animationClipFile.obj;
		}
	}

	public AnimationClip leftAnimationClip
	{
		get
		{
			return this.leftAnimationClipFile.obj;
		}
	}

	public string GetClipName(MoveData move, bool left = false)
	{
		return move.name + "_helplessState" + ((!left) ? string.Empty : "_left");
	}
}
