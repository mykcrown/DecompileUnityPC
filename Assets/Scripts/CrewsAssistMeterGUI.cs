// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrewsAssistMeterGUI : MonoBehaviour
{
	public Image FillImage;

	public float FlashSpeed = 0.02f;

	public TextMeshProUGUI Text;

	private int flashDirection = 1;

	private float flashAmount;

	private float fillAmount;

	private void Awake()
	{
		this.FillImage.material = UnityEngine.Object.Instantiate<Material>(this.FillImage.material);
		this.SetFill(0f);
	}

	public void SetFill(float amt)
	{
		this.fillAmount = amt;
		this.FillImage.fillAmount = amt;
		this.Text.gameObject.SetActive(amt >= 1f);
	}

	private float getTargetFlash()
	{
		return this.fillAmount * this.fillAmount;
	}

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
}
