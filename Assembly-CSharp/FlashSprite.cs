using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008C8 RID: 2248
public class FlashSprite : MonoBehaviour
{
	// Token: 0x060038AA RID: 14506 RVA: 0x0010A0CC File Offset: 0x001084CC
	protected virtual void Awake()
	{
		if (this.Image != null && this.FlashMaterial != null)
		{
			this.Image.material = UnityEngine.Object.Instantiate<Material>(this.FlashMaterial);
		}
	}

	// Token: 0x060038AB RID: 14507 RVA: 0x0010A106 File Offset: 0x00108506
	public void Flash()
	{
		this.flashColor(this.FlashAmount, this.FlashDuration);
	}

	// Token: 0x060038AC RID: 14508 RVA: 0x0010A11C File Offset: 0x0010851C
	private void flashColor(float amount, float duration)
	{
		if (this.isFlashing || this.FlashMaterial == null || this.Image == null)
		{
			return;
		}
		this.isFlashing = true;
		base.StartCoroutine(this.tickFlash(amount, duration));
	}

	// Token: 0x060038AD RID: 14509 RVA: 0x0010A170 File Offset: 0x00108570
	private IEnumerator tickFlash(float amount, float duration)
	{
		float startAmount = amount;
		float delta = amount / duration;
		while (amount > 0f)
		{
			amount -= delta * Time.deltaTime;
			this.Image.material.SetFloat("_FlashAmount", amount);
			yield return null;
		}
		this.Image.material.SetFloat("_FlashAmount", 0f);
		this.isFlashing = false;
		if (this.loopFlash)
		{
			this.flashColor(startAmount, duration);
		}
		yield break;
	}

	// Token: 0x060038AE RID: 14510 RVA: 0x0010A199 File Offset: 0x00108599
	private void OnDisable()
	{
		this.isFlashing = false;
		this.Image.material.SetFloat("_FlashAmount", 0f);
	}

	// Token: 0x060038AF RID: 14511 RVA: 0x0010A1BC File Offset: 0x001085BC
	private void OnDestroy()
	{
		if (this.Image.material != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Image.material);
		}
	}

	// Token: 0x040026FE RID: 9982
	private bool isFlashing;

	// Token: 0x040026FF RID: 9983
	public Material FlashMaterial;

	// Token: 0x04002700 RID: 9984
	public float FlashAmount = 0.7f;

	// Token: 0x04002701 RID: 9985
	public float FlashDuration = 0.3f;

	// Token: 0x04002702 RID: 9986
	public Image Image;

	// Token: 0x04002703 RID: 9987
	protected bool loopFlash;
}
