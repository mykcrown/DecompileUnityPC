// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class AshaniChargeSprite : FlashSprite, IChargeListSprite
{
	public Color UnfilledColor = Color.gray;

	private bool isActive = true;

	private FlickerSprite flicker;

	bool IChargeListSprite.IsActive
	{
		get
		{
			return this.isActive;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.flicker = base.GetComponentInChildren<FlickerSprite>();
		this.Image.fillMethod = Image.FillMethod.Horizontal;
		this.Image.type = Image.Type.Filled;
		this.setFill(0f);
	}

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

	private void setFill(float amt)
	{
		if (this.flicker != null)
		{
			this.flicker.StopFlicker();
		}
		this.Image.fillAmount = amt;
	}

	void IChargeListSprite.SetMaxCharge(bool maxCharge)
	{
		this.loopFlash = maxCharge;
		if (maxCharge)
		{
			base.Flash();
		}
	}

	void IChargeListSprite.WarnImminentLoss(float durationTillLoss)
	{
		if (this.flicker != null)
		{
			this.flicker.Flicker(durationTillLoss, this.flicker.FlickerInterval);
		}
	}
}
