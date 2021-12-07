using System;
using TMPro;
using UnityEngine;

// Token: 0x02000954 RID: 2388
public class ValidatableTextEntry : WavedashTextEntry
{
	// Token: 0x17000F06 RID: 3846
	// (get) Token: 0x06003F6B RID: 16235 RVA: 0x001206A2 File Offset: 0x0011EAA2
	// (set) Token: 0x06003F6C RID: 16236 RVA: 0x001206AA File Offset: 0x0011EAAA
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x06003F6D RID: 16237 RVA: 0x001206B4 File Offset: 0x0011EAB4
	public void Init(ITextValidator textValidator)
	{
		this.textValidator = textValidator;
		this.TargetInput.ValueChangedCallback = new Action(this.onValueChanged);
		this.TargetInput.CursorOverCallback = new Action(this.onCursorOver);
		this.TargetInput.CursorOutCallback = new Action(this.onCursorOut);
	}

	// Token: 0x06003F6E RID: 16238 RVA: 0x0012070D File Offset: 0x0011EB0D
	private void onCursorOver()
	{
		this.UpdateHighlightState();
	}

	// Token: 0x06003F6F RID: 16239 RVA: 0x00120715 File Offset: 0x0011EB15
	private void onCursorOut()
	{
		this.UpdateHighlightState();
	}

	// Token: 0x06003F70 RID: 16240 RVA: 0x00120720 File Offset: 0x0011EB20
	private void onValueChanged()
	{
		if (!this.useValidation)
		{
			this.ShowNeutral();
		}
		else
		{
			TextValidationResult result = this.textValidator.Validate(this.TargetInput.text);
			if (!result.IsOk)
			{
				this.ShowError(result);
			}
			else
			{
				this.ShowNeutral();
			}
		}
	}

	// Token: 0x17000F07 RID: 3847
	// (get) Token: 0x06003F71 RID: 16241 RVA: 0x00120778 File Offset: 0x0011EB78
	// (set) Token: 0x06003F72 RID: 16242 RVA: 0x00120780 File Offset: 0x0011EB80
	public bool UseValidation
	{
		get
		{
			return this.useValidation;
		}
		set
		{
			this.useValidation = value;
			this.onValueChanged();
		}
	}

	// Token: 0x06003F73 RID: 16243 RVA: 0x00120790 File Offset: 0x0011EB90
	public override void UpdateHighlightState()
	{
		this.GreenBorder.SetActive(false);
		this.ErrorText.gameObject.SetActive(false);
		this.RedBorder.SetActive(false);
		if (this.isErrorState)
		{
			this.ErrorText.gameObject.SetActive(true);
			this.RedBorder.SetActive(true);
		}
		else if (this.isSuccessState)
		{
			this.GreenBorder.SetActive(true);
		}
		base.UpdateHighlightState();
	}

	// Token: 0x06003F74 RID: 16244 RVA: 0x00120810 File Offset: 0x0011EC10
	public void ShowError(TextValidationResult result)
	{
		string text = this.localization.GetText(result.DisplayTextKey);
		this.ErrorText.text = text;
		this.isErrorState = true;
		this.isSuccessState = false;
		this.ErrorTextFlash.Flash();
		this.UpdateHighlightState();
	}

	// Token: 0x06003F75 RID: 16245 RVA: 0x0012085B File Offset: 0x0011EC5B
	public void ShowValid()
	{
		this.isSuccessState = true;
		this.isErrorState = false;
		this.UpdateHighlightState();
	}

	// Token: 0x06003F76 RID: 16246 RVA: 0x00120871 File Offset: 0x0011EC71
	public void ShowNeutral()
	{
		this.isSuccessState = false;
		this.isErrorState = false;
		this.UpdateHighlightState();
	}

	// Token: 0x04002B06 RID: 11014
	public TextMeshProUGUI Title;

	// Token: 0x04002B07 RID: 11015
	public GameObject RedBorder;

	// Token: 0x04002B08 RID: 11016
	public GameObject GreenBorder;

	// Token: 0x04002B09 RID: 11017
	public TextMeshProUGUI ErrorText;

	// Token: 0x04002B0A RID: 11018
	public FlashableText ErrorTextFlash;

	// Token: 0x04002B0B RID: 11019
	private bool useValidation = true;

	// Token: 0x04002B0C RID: 11020
	private ITextValidator textValidator;

	// Token: 0x04002B0D RID: 11021
	private bool isErrorState;

	// Token: 0x04002B0E RID: 11022
	private bool isSuccessState;
}
