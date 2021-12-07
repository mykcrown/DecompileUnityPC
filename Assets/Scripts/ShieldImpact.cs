// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
public class ShieldImpact : MonoBehaviour
{
	public enum MaskType
	{
		Direction,
		Sphere,
		Point
	}

	[SerializeField]
	private Material _shieldMaterial;

	[SerializeField]
	private Transform _shieldTransform;

	[SerializeField]
	private float _impactRadius = 0.25f;

	[SerializeField]
	private ShieldImpact.MaskType _maskType;

	private Vector3 _impactDirection = Vector3.zero;

	private void OnDrawGizmos()
	{
		ShieldImpact.MaskType maskType = this._maskType;
		if (maskType != ShieldImpact.MaskType.Direction)
		{
			if (maskType == ShieldImpact.MaskType.Point || maskType == ShieldImpact.MaskType.Sphere)
			{
				Gizmos.color = new Color(0f, 0f, 1f, 0.1f);
				Gizmos.DrawSphere(base.transform.position, this._impactRadius);
				Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
				Gizmos.DrawWireSphere(base.transform.position, this._impactRadius);
				Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
				Gizmos.DrawRay(base.transform.position, this._impactDirection);
			}
		}
		else
		{
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			Gizmos.DrawRay(base.transform.position, this._impactDirection);
		}
	}

	private void Update()
	{
		if (this._shieldMaterial != null && this._shieldTransform != null)
		{
			this._impactDirection = (base.transform.position - this._shieldTransform.position).normalized;
			this._shieldMaterial.SetVector("_ImpactPosition", base.transform.position);
			this._shieldMaterial.SetFloat("_ImpactRadius", this._impactRadius);
			this._shieldMaterial.SetVector("_ImpactDirection", this._impactDirection);
			float num = this._impactRadius * this._impactRadius;
			float num2 = 0.640000045f * num;
			float num3 = num2 - num;
			float y = 1f / num3;
			float z = -num / num3;
			float x = 25f / num;
			this._shieldMaterial.SetVector("_ImpactRangeProperties", new Vector4(x, y, z, 1f));
			ShieldImpact.MaskType maskType = this._maskType;
			if (maskType != ShieldImpact.MaskType.Direction)
			{
				if (maskType != ShieldImpact.MaskType.Point)
				{
					if (maskType == ShieldImpact.MaskType.Sphere)
					{
						this._shieldMaterial.DisableKeyword("_MASKTYPE_DIRECTION");
						this._shieldMaterial.DisableKeyword("_MASKTYPE_POINT");
						this._shieldMaterial.EnableKeyword("_MASKTYPE_SPHERE");
					}
				}
				else
				{
					this._shieldMaterial.DisableKeyword("_MASKTYPE_DIRECTION");
					this._shieldMaterial.EnableKeyword("_MASKTYPE_POINT");
					this._shieldMaterial.DisableKeyword("_MASKTYPE_SPHERE");
				}
			}
			else
			{
				this._shieldMaterial.EnableKeyword("_MASKTYPE_DIRECTION");
				this._shieldMaterial.DisableKeyword("_MASKTYPE_POINT");
				this._shieldMaterial.DisableKeyword("_MASKTYPE_SPHERE");
			}
		}
	}
}
