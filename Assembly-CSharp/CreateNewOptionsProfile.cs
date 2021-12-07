using System;
using UnityEngine;

// Token: 0x020008E3 RID: 2275
public class CreateNewOptionsProfile : MonoBehaviour
{
	// Token: 0x17000E03 RID: 3587
	// (get) Token: 0x06003A40 RID: 14912 RVA: 0x00110CE8 File Offset: 0x0010F0E8
	// (set) Token: 0x06003A41 RID: 14913 RVA: 0x00110CF0 File Offset: 0x0010F0F0
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000E04 RID: 3588
	// (get) Token: 0x06003A42 RID: 14914 RVA: 0x00110CF9 File Offset: 0x0010F0F9
	// (set) Token: 0x06003A43 RID: 14915 RVA: 0x00110D01 File Offset: 0x0010F101
	[Inject]
	public IOptionsProfileManager profileManager { get; set; }

	// Token: 0x17000E05 RID: 3589
	// (get) Token: 0x06003A44 RID: 14916 RVA: 0x00110D0A File Offset: 0x0010F10A
	// (set) Token: 0x06003A45 RID: 14917 RVA: 0x00110D12 File Offset: 0x0010F112
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000E06 RID: 3590
	// (get) Token: 0x06003A46 RID: 14918 RVA: 0x00110D1B File Offset: 0x0010F11B
	// (set) Token: 0x06003A47 RID: 14919 RVA: 0x00110D23 File Offset: 0x0010F123
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x06003A48 RID: 14920 RVA: 0x00110D2C File Offset: 0x0010F12C
	public void Awake()
	{
		this.InputField.EnterCallback = new Action(this.CreateProfile);
		this.InputField.ValueChangedCallback = new Action(this.onValueChanged);
	}

	// Token: 0x06003A49 RID: 14921 RVA: 0x00110D5C File Offset: 0x0010F15C
	[PostConstruct]
	public void Init()
	{
		this.InputField.characterLimit = this.profileManager.GetMaxNameLength();
		this.setErrorDisplay(false);
	}

	// Token: 0x06003A4A RID: 14922 RVA: 0x00110D7B File Offset: 0x0010F17B
	private void setErrorDisplay(bool value)
	{
		this.TextEntry.IsErrorState = value;
		this.TextEntry.UpdateHighlightState();
	}

	// Token: 0x06003A4B RID: 14923 RVA: 0x00110D94 File Offset: 0x0010F194
	private void onValueChanged()
	{
		this.setErrorDisplay(false);
	}

	// Token: 0x06003A4C RID: 14924 RVA: 0x00110D9D File Offset: 0x0010F19D
	public void Removed()
	{
		this.InputField.Removed();
		this.AddButton.Removed();
	}

	// Token: 0x06003A4D RID: 14925 RVA: 0x00110DB8 File Offset: 0x0010F1B8
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

	// Token: 0x06003A4E RID: 14926 RVA: 0x00110E1C File Offset: 0x0010F21C
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

	// Token: 0x0400280C RID: 10252
	public WavedashTMProInput InputField;

	// Token: 0x0400280D RID: 10253
	public CreateOptionsProfileTextEntry TextEntry;

	// Token: 0x0400280E RID: 10254
	public CursorTargetButton AddButton;
}
