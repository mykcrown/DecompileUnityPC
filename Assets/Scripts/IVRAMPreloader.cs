// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IVRAMPreloader
{
	void DebugPreload(Camera myCamera, List<GameObject> preloadObjects, Action callback);

	void Preload(Camera myCamera, List<GameObject> preloadObjects, Action callback);
}
