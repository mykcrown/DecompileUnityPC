// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class VFXProjectileOrigin : MonoBehaviour
{
	public GameObject SourceTransform;

	public GameObject Projectile;

	public GameObject MuzzleEffect;

	private Vector3 pos;

	public Vector3 MuzzleOffset;

	private void start()
	{
	}

	private void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Space) && this.Projectile != null)
		{
			this.pos = base.transform.position;
			this.Projectile.transform.position = this.pos;
			this.MuzzleEffect.transform.position = this.pos + this.MuzzleOffset;
		}
	}
}
