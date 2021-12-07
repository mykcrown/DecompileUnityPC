// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class ValidatableTextEntry : WavedashTextEntry
{
	public TextMeshProUGUI Title;

	public GameObject RedBorder;

	public GameObject GreenBorder;

	public TextMeshProUGUI ErrorText;

	public FlashableText ErrorTextFlash;

	private bool useValidation = true;

	private ITextValidator textValidator;

	private bool isErrorState;

	private bool isSuccessState;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

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

	public void Init(ITextValidator textValidator)
	{
		this.textValidator = textValidator;
		this.TargetInput.ValueChangedCallback = new Action(this.onValueChanged);
		this.TargetInput.CursorOverCallback = new Action(this.onCursorOver);
		this.TargetInput.CursorOutCallback = new Action(this.onCursorOut);
	}

	private void onCursorOver()
	{
		this.UpdateHighlightState();
	}

	private void onCursorOut()
	{
		this.UpdateHighlightState();
	}

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

	public void ShowError(TextValidationResult result)
	{
		string text = this.localization.GetText(result.DisplayTextKey);
		this.ErrorText.text = text;
		this.isErrorState = true;
		this.isSuccessState = false;
		this.ErrorTextFlash.Flash();
		this.UpdateHighlightState();
	}

	public void ShowValid()
	{
		this.isSuccessState = true;
		this.isErrorState = false;
		this.UpdateHighlightState();
	}

	public void ShowNeutral()
	{
		this.isSuccessState = false;
		this.isErrorState = false;
		this.UpdateHighlightState();
	}
}
