using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200099E RID: 2462
public class NewAccountWindow : BaseWindow
{
	// Token: 0x17000FEC RID: 4076
	// (get) Token: 0x06004333 RID: 17203 RVA: 0x00129A67 File Offset: 0x00127E67
	// (set) Token: 0x06004334 RID: 17204 RVA: 0x00129A6F File Offset: 0x00127E6F
	[Inject]
	public INewAccountWindowAPI api { get; set; }

	// Token: 0x17000FED RID: 4077
	// (get) Token: 0x06004335 RID: 17205 RVA: 0x00129A78 File Offset: 0x00127E78
	// (set) Token: 0x06004336 RID: 17206 RVA: 0x00129A80 File Offset: 0x00127E80
	[Inject]
	public ILoginValidator validator { get; set; }

	// Token: 0x06004337 RID: 17207 RVA: 0x00129A8C File Offset: 0x00127E8C
	[PostConstruct]
	public void Init()
	{
		this.usernameInputField = this.createTextEntry(this.UsernameInputStub, "ui.login.username", LoginEntryType.USERNAME);
		this.usernameInputField.TabCallback = new Action(this.onTabUserNameField);
		this.usernameInputField.EnterCallback = new Action(this.onTabUserNameField);
		this.emailInputField = this.createTextEntry(this.EmailInputStub, "ui.login.email", LoginEntryType.EMAIL);
		this.emailInputField.TabCallback = new Action(this.onTabEmailField);
		this.emailInputField.EnterCallback = new Action(this.onTabEmailField);
		this.passwordInputField = this.createTextEntry(this.PasswordInputStub, "ui.login.password", LoginEntryType.PASSWORD);
		this.passwordInputField.TabCallback = new Action(this.onTabPasswordField);
		this.passwordInputField.EnterCallback = new Action(this.createAccount);
		this.CreateButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onCreateClick);
		this.TermsButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onTermsClick);
		this.CloseButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClose);
		base.listen(NewAccountWindowAPI.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	// Token: 0x06004338 RID: 17208 RVA: 0x00129BCD File Offset: 0x00127FCD
	public override void ReadyForSelections()
	{
		base.ReadyForSelections();
		base.selectTextField(this.usernameInputField.TargetInput);
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x00129BE8 File Offset: 0x00127FE8
	private LoginTextInputField createTextEntry(GameObject stub, string titleKey, LoginEntryType type)
	{
		while (stub.transform.childCount > 0)
		{
			Transform child = stub.transform.GetChild(0);
			child.SetParent(null);
			UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
		LoginTextInputField loginTextInputField = UnityEngine.Object.Instantiate<LoginTextInputField>(this.LoginTextInputFieldPrefab);
		base.injector.Inject(loginTextInputField);
		loginTextInputField.Type = type;
		loginTextInputField.transform.SetParent(stub.transform, false);
		loginTextInputField.Title.text = base.localization.GetText(titleKey);
		return loginTextInputField;
	}

	// Token: 0x0600433A RID: 17210 RVA: 0x00129C73 File Offset: 0x00128073
	private void onUpdate()
	{
		this.updateTermsAccepted();
	}

	// Token: 0x0600433B RID: 17211 RVA: 0x00129C7B File Offset: 0x0012807B
	private void updateTermsAccepted()
	{
		this.TermsCheckmark.SetActive(this.api.TermsAccepted);
		if (this.api.TermsAccepted)
		{
			this.TermsErrorState.SetActive(false);
		}
	}

	// Token: 0x0600433C RID: 17212 RVA: 0x00129CAF File Offset: 0x001280AF
	private void onTabUserNameField()
	{
		base.selectTextField(this.emailInputField.TargetInput);
	}

	// Token: 0x0600433D RID: 17213 RVA: 0x00129CC2 File Offset: 0x001280C2
	private void onTabEmailField()
	{
		base.selectTextField(this.passwordInputField.TargetInput);
	}

	// Token: 0x0600433E RID: 17214 RVA: 0x00129CD5 File Offset: 0x001280D5
	private void onTabPasswordField()
	{
		(base.uiManager.CurrentInputModule as CursorInputModule).SetSelectedInputField(null);
	}

	// Token: 0x0600433F RID: 17215 RVA: 0x00129CED File Offset: 0x001280ED
	private void onCreateClick(CursorTargetButton button, PointerEventData eventData)
	{
		this.createAccount();
	}

	// Token: 0x06004340 RID: 17216 RVA: 0x00129CF5 File Offset: 0x001280F5
	private void createAccount()
	{
	}

	// Token: 0x06004341 RID: 17217 RVA: 0x00129CF7 File Offset: 0x001280F7
	private void resetErrorStates()
	{
		this.usernameInputField.ShowNeutral();
		this.emailInputField.ShowNeutral();
		this.passwordInputField.ShowNeutral();
		this.TermsErrorState.SetActive(false);
	}

	// Token: 0x06004342 RID: 17218 RVA: 0x00129D28 File Offset: 0x00128128
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
		}
	}

	// Token: 0x06004343 RID: 17219 RVA: 0x00129DA3 File Offset: 0x001281A3
	private void onTermsClick(CursorTargetButton button, PointerEventData eventData)
	{
		this.api.TermsAccepted = !this.api.TermsAccepted;
	}

	// Token: 0x06004344 RID: 17220 RVA: 0x00129DBE File Offset: 0x001281BE
	private void onClose(CursorTargetButton button, PointerEventData eventData)
	{
		this.Close();
	}

	// Token: 0x04002CCE RID: 11470
	public LoginTextInputField LoginTextInputFieldPrefab;

	// Token: 0x04002CCF RID: 11471
	public GameObject UsernameInputStub;

	// Token: 0x04002CD0 RID: 11472
	public GameObject EmailInputStub;

	// Token: 0x04002CD1 RID: 11473
	public GameObject PasswordInputStub;

	// Token: 0x04002CD2 RID: 11474
	public CursorTargetButton CreateButton;

	// Token: 0x04002CD3 RID: 11475
	public CursorTargetButton CloseButton;

	// Token: 0x04002CD4 RID: 11476
	public CursorTargetButton TermsButton;

	// Token: 0x04002CD5 RID: 11477
	public GameObject TermsCheckmark;

	// Token: 0x04002CD6 RID: 11478
	public GameObject TermsErrorState;

	// Token: 0x04002CD7 RID: 11479
	private LoginTextInputField usernameInputField;

	// Token: 0x04002CD8 RID: 11480
	private LoginTextInputField emailInputField;

	// Token: 0x04002CD9 RID: 11481
	private LoginTextInputField passwordInputField;
}
