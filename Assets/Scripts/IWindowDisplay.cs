// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IWindowDisplay
{
	T Add<T>(GameObject prefab, WindowTransition transition, bool pushToBack = false, bool closeOnScreenChange = false, bool useOverrideOpenSound = false, AudioData overrideOpenSound = default(AudioData)) where T : BaseWindow;

	void Remove(BaseWindow window);

	int GetWindowCount();
}
