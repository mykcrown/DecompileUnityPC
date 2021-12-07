// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public abstract class VFXBehavior : MonoBehaviour
{
	private void Start()
	{
		this.OnVFXInit();
	}

	public abstract void OnVFXInit();

	public abstract void OnVFXStart();
}
