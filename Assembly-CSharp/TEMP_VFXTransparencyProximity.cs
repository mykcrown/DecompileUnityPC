using System;
using UnityEngine;

// Token: 0x02000B85 RID: 2949
public class TEMP_VFXTransparencyProximity : VFXBehavior
{
	// Token: 0x06005527 RID: 21799 RVA: 0x001B4960 File Offset: 0x001B2D60
	public override void OnVFXInit()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
		GameObject gameObject = GameObject.FindGameObjectWithTag(Tags.GameManager);
		if (gameObject != null)
		{
			this.gameManager = gameObject.GetComponent<GameManager>();
		}
	}

	// Token: 0x06005528 RID: 21800 RVA: 0x001B499C File Offset: 0x001B2D9C
	public override void OnVFXStart()
	{
	}

	// Token: 0x06005529 RID: 21801 RVA: 0x001B49A0 File Offset: 0x001B2DA0
	private void Update()
	{
		float num = float.MaxValue;
		Vector3 b = base.transform.position - Vector3.up * 0.68f;
		foreach (PlayerReference playerReference in this.gameManager.PlayerReferences)
		{
			PlayerController playerController = this.gameManager.GetPlayerController(playerReference.PlayerNum);
			Vector3 vector = (Vector3)playerController.Position - b;
			if (vector.sqrMagnitude < num * num)
			{
				num = vector.magnitude;
			}
		}
		float num2 = Mathf.Max(num - this.minPlayerDistanceDetection, 0f) / Mathf.Max(this.maxPlayerDistanceDetection - this.minPlayerDistanceDetection, 0.0001f);
		num2 = Mathf.Min(num2, 1f);
		float a = num2 * (this.maxAlpha - this.minAlpha) + this.minAlpha;
		foreach (Renderer renderer in this.renderers)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				material.SetColor("_Color", new Color(1f, 1f, 1f, a));
			}
		}
	}

	// Token: 0x0400361A RID: 13850
	[Range(0f, 1f)]
	public float maxAlpha = 1f;

	// Token: 0x0400361B RID: 13851
	[Range(0f, 1f)]
	public float minAlpha;

	// Token: 0x0400361C RID: 13852
	public float maxPlayerDistanceDetection;

	// Token: 0x0400361D RID: 13853
	public float minPlayerDistanceDetection;

	// Token: 0x0400361E RID: 13854
	private Renderer[] renderers;

	// Token: 0x0400361F RID: 13855
	private GameManager gameManager;

	// Token: 0x04003620 RID: 13856
	private const float heightAboveFloor = 0.68f;
}
