// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class LoginTextInputField : WavedashTextEntry
{
	public TextMeshProUGUI Title;

	public GameObject RedBorder;

	public GameObject GreenBorder;

	public TextMeshProUGUI ErrorText;

	public FlashableText ErrorTextFlash;

	private LoginEntryType type;

	private bool useValidation = true;

	private bool isErrorState;

	private bool isSuccessState;

	private bool isCursorOver;

	[Inject]
	public ILoginValidator loginValidator
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public LoginEntryType Type
	{
		get
		{
			return this.type;
		}
		set
		{
			this.type = value;
			if (this.type == LoginEntryType.PASSWORD)
			{
				this.TargetInput.inputType = TMP_InputField.InputType.Password;
				this.AutoCapitalize = false;
			}
			else if (this.type == LoginEntryType.USERNAME)
			{
				this.TargetInput.characterLimit = this.getMaxNameLength();
			}
		}
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

	[PostConstruct]
	public void Init()
	{
		this.TargetInput.ValueChangedCallback = new Action(this.onValueChanged);
		this.TargetInput.CursorOverCallback = new Action(this.onCursorOver);
		this.TargetInput.CursorOutCallback = new Action(this.onCursorOut);
	}

	private void onCursorOver()
	{
		this.isCursorOver = true;
		this.UpdateHighlightState();
	}

	private void onCursorOut()
	{
		this.isCursorOver = false;
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
			LoginValidationResult loginValidationResult = this.loginValidator.Validate(this.Type, this.TargetInput.text);
			if (loginValidationResult.state != LoginValidationState.OK)
			{
				this.ShowError(loginValidationResult);
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
		this.HighlightGameObject.SetActive(false);
		if (this.isCursorOver)
		{
			this.HighlightGameObject.SetActive(true);
		}
		if (this.isErrorState)
		{
			this.ErrorText.gameObject.SetActive(true);
			this.RedBorder.SetActive(true);
		}
		else if (this.isSuccessState)
		{
			this.GreenBorder.SetActive(true);
		}
		else if (!this.isCursorOver)
		{
			base.UpdateHighlightState();
		}
	}

	public void ShowError(LoginValidationResult error)
	{
		string text = this.localization.GetText("ui.login.validationError." + error.state, error.value + string.Empty);
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

	private int getMaxNameLength()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.maxNameLength;
	}
}
