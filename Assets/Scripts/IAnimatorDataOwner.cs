// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatorDataOwner
{
	string CurrentAnimationName
	{
		get;
	}

	int CurrentFrame
	{
		get;
	}

	void SetHiddenProps(List<Transform> hiddenProps);
}
