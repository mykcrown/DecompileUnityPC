// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IBackgroundLoader
{
	bool BakedAnimationDataLoaded
	{
		get;
	}

	void LoadBakedAnimations(MonoBehaviour host);

	void WaitForSetup(Action callback);

	void OnApplicationQuit();
}
