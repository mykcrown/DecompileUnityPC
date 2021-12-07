// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionProfileGUI : MonoBehaviour
{
	public OptionsProfile profile;

	public TextMeshProUGUI NameText;

	public GameObject CheckMark;

	public GameObject DeleteButtonObject;

	public CursorTargetButton MainButton;

	public CursorTargetButton DeleteButton;

	[Inject]
	public IOptionsProfileManager profileManager
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
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

	public void Load(OptionsProfile profile)
	{
		this.profile = profile;
		this.NameText.text = profile.name;
		this.DeleteButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onDelete);
		this.MainButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClick);
	}

	private void onClick(CursorTargetButton button, PointerEventData eventData)
	{
		this.audioManager.PlayMenuSound(SoundKey.characterSelect_ruleSetSelect, 0f);
		this.profileManager.Select(this.profile);
	}

	private void onDelete(CursorTargetButton button, PointerEventData eventData)
	{
		DeleteProfileResult deleteProfileResult = this.profileManager.CanDeleteProfile(this.profile);
		if (deleteProfileResult != DeleteProfileResult.SUCCESS)
		{
			if (deleteProfileResult == DeleteProfileResult.MUST_KEEP_ONE)
			{
				this.mustKeepOneDialog();
			}
		}
		else
		{
			GenericDialog genericDialog = this.dialogController.ShowTwoButtonDialog(this.localization.GetText("dialog.profileDelete.title"), this.localization.GetText("dialog.profileDelete.body", this.profile.name), this.localization.GetText("dialog.delete.confirm"), this.localization.GetText("dialog.delete.cancel"));
			genericDialog.ConfirmCallback = new Action(this.confirmedDelete);
		}
	}

	private void confirmedDelete()
	{
		this.profileManager.DeleteProfile(this.profile, new Action<DeleteProfileResult>(this._confirmedDelete_m__0));
	}

	private void mustKeepOneDialog()
	{
		this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.cannotDeleteLastProfile.title"), this.localization.GetText("dialog.cannotDeleteLastProfile.body"), this.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
	}

	public void Removed()
	{
		this.MainButton.Removed();
		this.DeleteButton.Removed();
	}

	private void _confirmedDelete_m__0(DeleteProfileResult result)
	{
		if (result == DeleteProfileResult.MUST_KEEP_ONE)
		{
			this.mustKeepOneDialog();
		}
		else if (result == DeleteProfileResult.FAILURE)
		{
			this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.saveError.title"), this.localization.GetText("dialog.saveError.body"), this.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
		else
		{
			this.audioManager.PlayMenuSound(SoundKey.characterSelect_ruleSetDelete, 0f);
		}
	}
}
