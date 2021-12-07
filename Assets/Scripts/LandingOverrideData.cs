// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class LandingOverrideData
{
	public MoveData landMove;

	public int lightLandFrames = 15;

	public int heavyLandFrames = 30;

	public AnimationClipFile landClipFile = new AnimationClipFile();

	public string landClipName;

	public AnimationClipFile leftLandClipFile = new AnimationClipFile();

	public string leftLandClipName;

	public ParticleData landParticle;

	public AnimationClip landClip
	{
		get
		{
			return this.landClipFile.obj;
		}
	}

	public AnimationClip leftLandClip
	{
		get
		{
			return this.leftLandClipFile.obj;
		}
	}

	public string GetClipName(MoveData move, bool left = false)
	{
		return move.name + "_helplessLand" + ((!left) ? string.Empty : "_left");
	}
}
