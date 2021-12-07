// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DummyAnimationDataOwner : IAnimatorDataOwner
{
	public string CurrentAnimationName
	{
		get;
		private set;
	}

	public int CurrentFrame
	{
		get;
		private set;
	}

	public DummyAnimationDataOwner(string animationName, int currentFrame)
	{
		this.CurrentAnimationName = animationName;
		this.CurrentFrame = currentFrame;
	}

	public void SetHiddenProps(List<Transform> hiddenProps)
	{
	}
}
