// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public interface ICoroutineStarter
{
	Coroutine StartCoroutine(IEnumerator routine);
}
