using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020009A1 RID: 2465
public class NewUserPanel : LoginScreenPanel
{
	// Token: 0x17000FF1 RID: 4081
	// (get) Token: 0x0600434E RID: 17230 RVA: 0x00129E20 File Offset: 0x00128220
	// (set) Token: 0x0600434F RID: 17231 RVA: 0x00129E28 File Offset: 0x00128228
	[Inject]
	public IHyperlinkHandler hyperlinkHandler { get; set; }

	// Token: 0x06004350 RID: 17232 RVA: 0x00129E34 File Offset: 0x00128234
	[PostConstruct]
	public void Init()
	{
		this.usernameInputField = base.createTextEntry(this.UsernameInputStub, "ui.login.username", LoginEntryType.USERNAME);
		this.usernameInputField.TabCallback = new Action(this.onTabUserNameField);
		this.usernameInputField.EnterCallback = new Action(this.onTabUserNameField);
		LoginTextInputField loginTextInputField = this.usernameInputField;
		loginTextInputField.EndEditCallback = (Action)Delegate.Combine(loginTextInputField.EndEditCallback, new Action(this.onEndEditUsername));
		this.usernameInputField.UseValidation = false;
		this.emailInputField = base.createTextEntry(this.EmailInputStub, "ui.login.email", LoginEntryType.EMAIL);
		this.emailInputField.TabCallback = new Action(this.onTabEmailField);
		this.emailInputField.EnterCallback = new Action(this.onTabEmailField);
		LoginTextInputField loginTextInputField2 = this.emailInputField;
		loginTextInputField2.EndEditCallback = (Action)Delegate.Combine(loginTextInputField2.EndEditCallback, new Action(this.onEndEditEmail));
		this.emailInputField.UseValidation = false;
		this.passwordInputField = base.createTextEntry(this.PasswordInputStub, "ui.login.password", LoginEntryType.PASSWORD);
		this.passwordInputField.TabCallback = new Action(this.onTabPasswordField);
		this.passwordInputField.EnterCallback = new Action(this.onCreateAccount);
		this.TermsButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onTermsClicked);
		this.TermsAndConditionsLinkButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onTermsTextClicked);
		this.CreateAccountButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onCreateAccountClicked);
		this.HaveAccountButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onHaveAccountClicked);
		base.listen(LoginScreenAPI.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x00129FF2 File Offset: 0x001283F2
	public override void OnHide()
	{
		this.CreateAccountButton.Removed();
		this.HaveAccountButton.Removed();
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x0012A00A File Offset: 0x0012840A
	private void onUpdate()
	{
		this.updateTermsAccepted();
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x0012A012 File Offset: 0x00128412
	private void updateTermsAccepted()
	{
		this.TermsCheckmark.SetActive(base.api.TermsAccepted);
		if (base.api.TermsAccepted)
		{
			this.TermsErrorState.SetActive(false);
		}
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x0012A046 File Offset: 0x00128446
	private void onEndEditUsername()
	{
		if (this.usernameInputField.Text.Length > 0)
		{
			this.usernameInputField.UseValidation = true;
		}
	}

	// Token: 0x06004355 RID: 17237 RVA: 0x0012A06A File Offset: 0x0012846A
	private void onEndEditEmail()
	{
		if (this.emailInputField.Text.Length > 0)
		{
			this.emailInputField.UseValidation = true;
		}
	}

	// Token: 0x06004356 RID: 17238 RVA: 0x0012A08E File Offset: 0x0012848E
	private void onTabUserNameField()
	{
		base.selectTextField(this.emailInputField.TargetInput);
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x0012A0A1 File Offset: 0x001284A1
	private void onTabEmailField()
	{
		base.selectTextField(null);
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x0012A0AA File Offset: 0x001284AA
	private void onTabPasswordField()
	{
		base.selectTextField(null);
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x0012A0B3 File Offset: 0x001284B3
	private void onCreateAccount()
	{
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x0012A0B5 File Offset: 0x001284B5
	private void resetErrorStates()
	{
		this.usernameInputField.ShowNeutral();
		this.emailInputField.ShowNeutral();
		this.passwordInputField.ShowNeutral();
		this.TermsErrorState.SetActive(false);
	}

	// Token: 0x0600435B RID: 17243 RVA: 0x0012A0E4 File Offset: 0x001284E4
	private void handleError(LoginValidationResult error)
	{
		if (error.type == LoginEntryType.USERNAME)
		{
			this.usernameInputField.ShowError(error);
		}
		else if (error.type == LoginEntryType.EMAIL)
		{
			this.emailInputField.ShowError(error);
		}
		else if (error.type == LoginEntryType.PASSWORD)
		{
			this.passwordInputField.ShowError(error);
		}
		else if (error.type == LoginEntryType.TERMS)
		{
			this.TermsErrorState.SetActive(true);
			this.TermsErrorFlash.Flash();
		}
	}

	// Token: 0x0600435C RID: 17244 RVA: 0x0012A16A File Offset: 0x0012856A
	private void onCreateAccountClicked(CursorTargetButton button, PointerEventData eventData)
	{
		this.onCreateAccount();
	}

	// Token: 0x0600435D RID: 17245 RVA: 0x0012A172 File Offset: 0x00128572
	private void onHaveAccountClicked(CursorTargetButton button, PointerEventData eventData)
	{
	}

	// Token: 0x0600435E RID: 17246 RVA: 0x0012A174 File Offset: 0x00128574
	private void onTermsClicked(CursorTargetButton button, PointerEventData eventData)
	{
		base.api.TermsAccepted = !base.api.TermsAccepted;
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x0012A18F File Offset: 0x0012858F
	private void onTermsTextClicked(CursorTargetButton button, PointerEventData eventData)
	{
		this.hyperlinkHandler.TryClickLink(this.TermsText, Input.mousePosition);
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x0012A1AD File Offset: 0x001285AD
	public override void InitSelection()
	{
		if (base.IsCurrentPanel)
		{
			base.selectTextField(this.usernameInputField.TargetInput);
		}
	}

	// Token: 0x04002CDE RID: 11486
	public GameObject UsernameInputStub;

	// Token: 0x04002CDF RID: 11487
	public GameObject EmailInputStub;

	// Token: 0x04002CE0 RID: 11488
	public GameObject PasswordInputStub;

	// Token: 0x04002CE1 RID: 11489
	public CursorTargetButton CreateAccountButton;

	// Token: 0x04002CE2 RID: 11490
	public CursorTargetButton HaveAccountButton;

	// Token: 0x04002CE3 RID: 11491
	public CursorTargetButton TermsButton;

	// Token: 0x04002CE4 RID: 11492
	public CursorTargetButton TermsAndConditionsLinkButton;

	// Token: 0x04002CE5 RID: 11493
	public GameObject TermsCheckmark;

	// Token: 0x04002CE6 RID: 11494
	public GameObject TermsErrorState;

	// Token: 0x04002CE7 RID: 11495
	public FlashableText TermsErrorFlash;

	// Token: 0x04002CE8 RID: 11496
	public TMP_Text TermsText;

	// Token: 0x04002CE9 RID: 11497
	private LoginTextInputField usernameInputField;

	// Token: 0x04002CEA RID: 11498
	private LoginTextInputField emailInputField;

	// Token: 0x04002CEB RID: 11499
	private LoginTextInputField passwordInputField;
}
