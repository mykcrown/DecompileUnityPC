// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IGamewideOverlayDisplay
{
	T AddGamewideOverlay<T>(GameObject prefab, WindowTransition transition) where T : BaseGamewideOverlay;

	void Remove(BaseGamewideOverlay overlay);

	int GetGamewideOverlayCount();
}
