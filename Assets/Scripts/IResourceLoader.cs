// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IResourceLoader
{
	void Load<T>(string path, Action<T> callback) where T : UnityEngine.Object;
}
