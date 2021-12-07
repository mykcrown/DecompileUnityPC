using System;
using TMPro;
using UnityEngine;

// Token: 0x02000997 RID: 2455
public class LoginTextInputField : WavedashTextEntry
{
	// Token: 0x17000FDA RID: 4058
	// (get) Token: 0x060042FD RID: 17149 RVA: 0x0012955A File Offset: 0x0012795A
	// (set) Token: 0x060042FE RID: 17150 RVA: 0x00129562 File Offset: 0x00127962
	[Inject]
	public ILoginValidator loginValidator { get; set; }

	// Token: 0x17000FDB RID: 4059
	// (get) Token: 0x060042FF RID: 17151 RVA: 0x0012956B File Offset: 0x0012796B
	// (set) Token: 0x06004300 RID: 17152 RVA: 0x00129573 File Offset: 0x00127973
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000FDC RID: 4060
	// (get) Token: 0x06004301 RID: 17153 RVA: 0x0012957C File Offset: 0x0012797C
	// (set) Token: 0x06004302 RID: 17154 RVA: 0x00129584 File Offset: 0x00127984
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x06004303 RID: 17155 RVA: 0x00129590 File Offset: 0x00127990
	[PostConstruct]
	public void Init()
	{
		this.TargetInput.ValueChangedCallback = new Action(this.onValueChanged);
		this.TargetInput.CursorOverCallback = new Action(this.onCursorOver);
		this.TargetInput.CursorOutCallback = new Action(this.onCursorOut);
	}

	// Token: 0x06004304 RID: 17156 RVA: 0x001295E2 File Offset: 0x001279E2
	private void onCursorOver()
	{
		this.isCursorOver = true;
		this.UpdateHighlightState();
	}

	// Token: 0x06004305 RID: 17157 RVA: 0x001295F1 File Offset: 0x001279F1
	private void onCursorOut()
	{
		this.isCursorOver = false;
		this.UpdateHighlightState();
	}

	// Token: 0x06004306 RID: 17158 RVA: 0x00129600 File Offset: 0x00127A00
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

	// Token: 0x17000FDD RID: 4061
	// (get) Token: 0x06004307 RID: 17159 RVA: 0x0012965D File Offset: 0x00127A5D
	// (set) Token: 0x06004308 RID: 17160 RVA: 0x00129668 File Offset: 0x00127A68
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

	// Token: 0x17000FDE RID: 4062
	// (get) Token: 0x06004309 RID: 17161 RVA: 0x001296BC File Offset: 0x00127ABC
	// (set) Token: 0x0600430A RID: 17162 RVA: 0x001296C4 File Offset: 0x00127AC4
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

	// Token: 0x0600430B RID: 17163 RVA: 0x001296D4 File Offset: 0x00127AD4
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

	// Token: 0x0600430C RID: 17164 RVA: 0x00129788 File Offset: 0x00127B88
	public void ShowError(LoginValidationResult error)
	{
		string text = this.localization.GetText("ui.login.validationError." + error.state, error.value + string.Empty);
		this.ErrorText.text = text;
		this.isErrorState = true;
		this.isSuccessState = false;
		this.ErrorTextFlash.Flash();
		this.UpdateHighlightState();
	}

	// Token: 0x0600430D RID: 17165 RVA: 0x001297F6 File Offset: 0x00127BF6
	public void ShowValid()
	{
		this.isSuccessState = true;
		this.isErrorState = false;
		this.UpdateHighlightState();
	}

	// Token: 0x0600430E RID: 17166 RVA: 0x0012980C File Offset: 0x00127C0C
	public void ShowNeutral()
	{
		this.isSuccessState = false;
		this.isErrorState = false;
		this.UpdateHighlightState();
	}

	// Token: 0x0600430F RID: 17167 RVA: 0x00129822 File Offset: 0x00127C22
	private int getMaxNameLength()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.maxNameLength;
	}

	// Token: 0x04002CAC RID: 11436
	public TextMeshProUGUI Title;

	// Token: 0x04002CAD RID: 11437
	public GameObject RedBorder;

	// Token: 0x04002CAE RID: 11438
	public GameObject GreenBorder;

	// Token: 0x04002CAF RID: 11439
	public TextMeshProUGUI ErrorText;

	// Token: 0x04002CB0 RID: 11440
	public FlashableText ErrorTextFlash;

	// Token: 0x04002CB1 RID: 11441
	private LoginEntryType type;

	// Token: 0x04002CB2 RID: 11442
	private bool useValidation = true;

	// Token: 0x04002CB3 RID: 11443
	private bool isErrorState;

	// Token: 0x04002CB4 RID: 11444
	private bool isSuccessState;

	// Token: 0x04002CB5 RID: 11445
	private bool isCursorOver;
}
