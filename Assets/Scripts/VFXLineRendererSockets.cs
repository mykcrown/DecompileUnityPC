// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class VFXLineRendererSockets : MonoBehaviour
{
	public GameObject lineStart;

	public GameObject lineEnd;

	private void Start()
	{
		LineRenderer component = base.GetComponent<LineRenderer>();
		component.SetPosition(0, this.lineStart.transform.localPosition);
		component.SetPosition(1, this.lineEnd.transform.localPosition);
	}

	private void Update()
	{
	}
}
