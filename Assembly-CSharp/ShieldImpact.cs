using System;
using UnityEngine;

// Token: 0x02000206 RID: 518
[ExecuteInEditMode]
public class ShieldImpact : MonoBehaviour
{
	// Token: 0x060009BE RID: 2494 RVA: 0x00050964 File Offset: 0x0004ED64
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

	// Token: 0x060009BF RID: 2495 RVA: 0x00050A6C File Offset: 0x0004EE6C
	private void Update()
	{
		if (this._shieldMaterial != null && this._shieldTransform != null)
		{
			this._impactDirection = (base.transform.position - this._shieldTransform.position).normalized;
			this._shieldMaterial.SetVector("_ImpactPosition", base.transform.position);
			this._shieldMaterial.SetFloat("_ImpactRadius", this._impactRadius);
			this._shieldMaterial.SetVector("_ImpactDirection", this._impactDirection);
			float num = this._impactRadius * this._impactRadius;
			float num2 = 0.64000005f * num;
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

	// Token: 0x040006E0 RID: 1760
	[SerializeField]
	private Material _shieldMaterial;

	// Token: 0x040006E1 RID: 1761
	[SerializeField]
	private Transform _shieldTransform;

	// Token: 0x040006E2 RID: 1762
	[SerializeField]
	private float _impactRadius = 0.25f;

	// Token: 0x040006E3 RID: 1763
	[SerializeField]
	private ShieldImpact.MaskType _maskType;

	// Token: 0x040006E4 RID: 1764
	private Vector3 _impactDirection = Vector3.zero;

	// Token: 0x02000207 RID: 519
	public enum MaskType
	{
		// Token: 0x040006E6 RID: 1766
		Direction,
		// Token: 0x040006E7 RID: 1767
		Sphere,
		// Token: 0x040006E8 RID: 1768
		Point
	}
}
