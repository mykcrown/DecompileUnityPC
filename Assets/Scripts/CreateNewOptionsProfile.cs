// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CreateNewOptionsProfile : MonoBehaviour
{
	public WavedashTMProInput InputField;

	public CreateOptionsProfileTextEntry TextEntry;

	public CursorTargetButton AddButton;

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public IOptionsProfileManager profileManager
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
	public AudioManager audioManager
	{
		get;
		set;
	}

	public void Awake()
	{
		this.InputField.EnterCallback = new Action(this.CreateProfile);
		this.InputField.ValueChangedCallback = new Action(this.onValueChanged);
	}

	[PostConstruct]
	public void Init()
	{
		this.InputField.characterLimit = this.profileManager.GetMaxNameLength();
		this.setErrorDisplay(false);
	}

	private void setErrorDisplay(bool value)
	{
		this.TextEntry.IsErrorState = value;
		this.TextEntry.UpdateHighlightState();
	}

	private void onValueChanged()
	{
		this.setErrorDisplay(false);
	}

	public void Removed()
	{
		this.InputField.Removed();
		this.AddButton.Removed();
	}

	public void CreateProfile()
	{
		if (this.InputField.text.Length < 1)
		{
			this.setErrorDisplay(true);
		}
		else
		{
			this.audioManager.PlayMenuSound(SoundKey.characterSelect_ruleSetAdd, 0f);
			this.profileManager.CreateNewProfile(this.InputField.text, new Action<CreateNewProfileResult>(this.onCreateProfileResponse));
		}
	}

	private void onCreateProfileResponse(CreateNewProfileResult result)
	{
		if (!result.previousRequestInProgress)
		{
			if (result.tooManyExist)
			{
				this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.tooManyProfiles.title"), this.localization.GetText("dialog.tooManyProfiles.body", this.profileManager.GetMaxProfiles().ToString()), this.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			}
			else if (result.nameError == NameValidationResult.INVALID_CHAR)
			{
				this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.invalidChar.title"), this.localization.GetText("dialog.invalidChar.body"), this.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			}
			else if (result.nameError == NameValidationResult.TOO_SHORT)
			{
				this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.tooShort.title"), this.localization.GetText("dialog.tooShort.body", this.profileManager.GetMinNameLength().ToString()), this.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			}
			else if (result.saveResult == SaveOptionsProfileResult.FAILURE)
			{
				this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.saveError.title"), this.localization.GetText("dialog.saveError.body"), this.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
			}
		}
	}
}
