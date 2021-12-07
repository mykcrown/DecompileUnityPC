// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class PlanetRotate : MonoBehaviour
{
	public static float GlobalSpeedup = 1f;

	public Vector3 InitialOrientation = Vector3.zero;

	public Vector3 RotationOrientation = Vector3.zero;

	private Vector3 CurrentOrientation = Vector3.zero;

	private void Start()
	{
	}

	private void Update()
	{
		this.CurrentOrientation += this.RotationOrientation * Time.deltaTime * PlanetRotate.GlobalSpeedup;
		base.transform.localRotation = Quaternion.Euler(this.InitialOrientation) * Quaternion.Euler(this.CurrentOrientation);
	}
}
