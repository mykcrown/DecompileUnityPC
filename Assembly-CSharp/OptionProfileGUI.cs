using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020008EF RID: 2287
public class OptionProfileGUI : MonoBehaviour
{
	// Token: 0x17000E13 RID: 3603
	// (get) Token: 0x06003A9E RID: 15006 RVA: 0x0011272B File Offset: 0x00110B2B
	// (set) Token: 0x06003A9F RID: 15007 RVA: 0x00112733 File Offset: 0x00110B33
	[Inject]
	public IOptionsProfileManager profileManager { get; set; }

	// Token: 0x17000E14 RID: 3604
	// (get) Token: 0x06003AA0 RID: 15008 RVA: 0x0011273C File Offset: 0x00110B3C
	// (set) Token: 0x06003AA1 RID: 15009 RVA: 0x00112744 File Offset: 0x00110B44
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000E15 RID: 3605
	// (get) Token: 0x06003AA2 RID: 15010 RVA: 0x0011274D File Offset: 0x00110B4D
	// (set) Token: 0x06003AA3 RID: 15011 RVA: 0x00112755 File Offset: 0x00110B55
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000E16 RID: 3606
	// (get) Token: 0x06003AA4 RID: 15012 RVA: 0x0011275E File Offset: 0x00110B5E
	// (set) Token: 0x06003AA5 RID: 15013 RVA: 0x00112766 File Offset: 0x00110B66
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x06003AA6 RID: 15014 RVA: 0x00112770 File Offset: 0x00110B70
	public void Load(OptionsProfile profile)
	{
		this.profile = profile;
		this.NameText.text = profile.name;
		this.DeleteButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onDelete);
		this.MainButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClick);
	}

	// Token: 0x06003AA7 RID: 15015 RVA: 0x001127C3 File Offset: 0x00110BC3
	private void onClick(CursorTargetButton button, PointerEventData eventData)
	{
		this.audioManager.PlayMenuSound(SoundKey.characterSelect_ruleSetSelect, 0f);
		this.profileManager.Select(this.profile);
	}

	// Token: 0x06003AA8 RID: 15016 RVA: 0x001127E8 File Offset: 0x00110BE8
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

	// Token: 0x06003AA9 RID: 15017 RVA: 0x00112888 File Offset: 0x00110C88
	private void confirmedDelete()
	{
		this.profileManager.DeleteProfile(this.profile, delegate(DeleteProfileResult result)
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
		});
	}

	// Token: 0x06003AAA RID: 15018 RVA: 0x001128A8 File Offset: 0x00110CA8
	private void mustKeepOneDialog()
	{
		this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.cannotDeleteLastProfile.title"), this.localization.GetText("dialog.cannotDeleteLastProfile.body"), this.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x001128FC File Offset: 0x00110CFC
	public void Removed()
	{
		this.MainButton.Removed();
		this.DeleteButton.Removed();
	}

	// Token: 0x0400286B RID: 10347
	public OptionsProfile profile;

	// Token: 0x0400286C RID: 10348
	public TextMeshProUGUI NameText;

	// Token: 0x0400286D RID: 10349
	public GameObject CheckMark;

	// Token: 0x0400286E RID: 10350
	public GameObject DeleteButtonObject;

	// Token: 0x0400286F RID: 10351
	public CursorTargetButton MainButton;

	// Token: 0x04002870 RID: 10352
	public CursorTargetButton DeleteButton;
}
