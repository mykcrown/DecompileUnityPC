// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
public class DissolveShapeMask : MonoBehaviour
{
	[SerializeField]
	private Material _material;

	private void OnDrawGizmos()
	{
		Matrix4x4 matrix = Gizmos.matrix;
		Color color = Gizmos.color;
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.localScale);
		Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.75f);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		Gizmos.color = color;
		Gizmos.matrix = matrix;
	}

	private void Update()
	{
		if (this._material == null)
		{
			return;
		}
		Quaternion rotation = base.transform.rotation;
		Vector3 vector = base.transform.localToWorldMatrix.GetColumn(3);
		Vector3 v = new Vector3(base.transform.localScale.x * 0.5f, base.transform.localScale.y * 0.5f, base.transform.localScale.z * 0.5f);
		this._material.SetVector("_DissolveShapeMaskPosition", new Vector4(vector.x, vector.y, vector.z, 0f));
		this._material.SetVector("_DissolveShapeMaskSize", v);
		this._material.SetMatrix("_DissolveShapeMaskRotation", Matrix4x4.Rotate(base.transform.rotation).inverse);
	}
}
