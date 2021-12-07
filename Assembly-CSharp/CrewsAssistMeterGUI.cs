using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008AE RID: 2222
public class CrewsAssistMeterGUI : MonoBehaviour
{
	// Token: 0x060037CD RID: 14285 RVA: 0x00105C2E File Offset: 0x0010402E
	private void Awake()
	{
		this.FillImage.material = UnityEngine.Object.Instantiate<Material>(this.FillImage.material);
		this.SetFill(0f);
	}

	// Token: 0x060037CE RID: 14286 RVA: 0x00105C56 File Offset: 0x00104056
	public void SetFill(float amt)
	{
		this.fillAmount = amt;
		this.FillImage.fillAmount = amt;
		this.Text.gameObject.SetActive(amt >= 1f);
	}

	// Token: 0x060037CF RID: 14287 RVA: 0x00105C86 File Offset: 0x00104086
	private float getTargetFlash()
	{
		return this.fillAmount * this.fillAmount;
	}

	// Token: 0x060037D0 RID: 14288 RVA: 0x00105C98 File Offset: 0x00104098
	private void Update()
	{
		float targetFlash = this.getTargetFlash();
		float num = Time.deltaTime * this.FlashSpeed * targetFlash;
		if (this.flashDirection > 1)
		{
			this.flashAmount += num;
			if (this.flashAmount >= targetFlash)
			{
				this.flashAmount = targetFlash;
				this.flashDirection = -1;
			}
		}
		else
		{
			this.flashAmount -= num;
			if (this.flashAmount <= 0f)
			{
				this.flashAmount = targetFlash;
				this.flashDirection = 1;
			}
		}
		this.FillImage.material.SetFloat("_FlashAmount", this.flashAmount);
	}

	// Token: 0x04002603 RID: 9731
	public Image FillImage;

	// Token: 0x04002604 RID: 9732
	public float FlashSpeed = 0.02f;

	// Token: 0x04002605 RID: 9733
	public TextMeshProUGUI Text;

	// Token: 0x04002606 RID: 9734
	private int flashDirection = 1;

	// Token: 0x04002607 RID: 9735
	private float flashAmount;

	// Token: 0x04002608 RID: 9736
	private float fillAmount;
}
