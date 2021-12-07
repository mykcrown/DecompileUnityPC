// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class DebugUIElement : MonoBehaviour
{
	private void Start()
	{
	}

	public void Init(float duration)
	{
		UnityEngine.Object.Destroy(base.gameObject, duration);
	}
}
