// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class AsyncResourceMono : MonoBehaviour
{
	public Action OnUpdate;

	protected void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}
}
