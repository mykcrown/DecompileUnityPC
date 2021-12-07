using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008C6 RID: 2246
public class AshaniChargeSprite : FlashSprite, IChargeListSprite
{
	// Token: 0x0600389D RID: 14493 RVA: 0x0010A33F File Offset: 0x0010873F
	protected override void Awake()
	{
		base.Awake();
		this.flicker = base.GetComponentInChildren<FlickerSprite>();
		this.Image.fillMethod = Image.FillMethod.Horizontal;
		this.Image.type = Image.Type.Filled;
		this.setFill(0f);
	}

	// Token: 0x17000DAC RID: 3500
	// (get) Token: 0x0600389E RID: 14494 RVA: 0x0010A376 File Offset: 0x00108776
	bool IChargeListSprite.IsActive
	{
		get
		{
			return this.isActive;
		}
	}

	// Token: 0x0600389F RID: 14495 RVA: 0x0010A37E File Offset: 0x0010877E
	void IChargeListSprite.SetActive(bool isActive)
	{
		this.isActive = isActive;
		if (isActive)
		{
			this.setFill(1f);
			base.Flash();
		}
		else
		{
			this.setFill(0f);
		}
	}

	// Token: 0x060038A0 RID: 14496 RVA: 0x0010A3AE File Offset: 0x001087AE
	void IChargeListSprite.SetPartialValue(float fraction)
	{
		if (!this.isActive)
		{
			this.setFill(fraction);
		}
		if (this.flicker != null)
		{
			this.flicker.StopFlicker();
		}
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x0010A3DE File Offset: 0x001087DE
	private void setFill(float amt)
	{
		if (this.flicker != null)
		{
			this.flicker.StopFlicker();
		}
		this.Image.fillAmount = amt;
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x0010A408 File Offset: 0x00108808
	void IChargeListSprite.SetMaxCharge(bool maxCharge)
	{
		this.loopFlash = maxCharge;
		if (maxCharge)
		{
			base.Flash();
		}
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x0010A41D File Offset: 0x0010881D
	void IChargeListSprite.WarnImminentLoss(float durationTillLoss)
	{
		if (this.flicker != null)
		{
			this.flicker.Flicker(durationTillLoss, this.flicker.FlickerInterval);
		}
	}

	// Token: 0x040026FB RID: 9979
	public Color UnfilledColor = Color.gray;

	// Token: 0x040026FC RID: 9980
	private bool isActive = true;

	// Token: 0x040026FD RID: 9981
	private FlickerSprite flicker;
}
