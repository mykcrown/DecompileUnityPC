// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewAccountWindow : BaseWindow
{
	public LoginTextInputField LoginTextInputFieldPrefab;

	public GameObject UsernameInputStub;

	public GameObject EmailInputStub;

	public GameObject PasswordInputStub;

	public CursorTargetButton CreateButton;

	public CursorTargetButton CloseButton;

	public CursorTargetButton TermsButton;

	public GameObject TermsCheckmark;

	public GameObject TermsErrorState;

	private LoginTextInputField usernameInputField;

	private LoginTextInputField emailInputField;

	private LoginTextInputField passwordInputField;

	[Inject]
	public INewAccountWindowAPI api
	{
		get;
		set;
	}

	[Inject]
	public ILoginValidator validator
	{
		get;
		set;
	}

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

	public override void ReadyForSelections()
	{
		base.ReadyForSelections();
		base.selectTextField(this.usernameInputField.TargetInput);
	}

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

	private void onUpdate()
	{
		this.updateTermsAccepted();
	}

	private void updateTermsAccepted()
	{
		this.TermsCheckmark.SetActive(this.api.TermsAccepted);
		if (this.api.TermsAccepted)
		{
			this.TermsErrorState.SetActive(false);
		}
	}

	private void onTabUserNameField()
	{
		base.selectTextField(this.emailInputField.TargetInput);
	}

	private void onTabEmailField()
	{
		base.selectTextField(this.passwordInputField.TargetInput);
	}

	private void onTabPasswordField()
	{
		(base.uiManager.CurrentInputModule as CursorInputModule).SetSelectedInputField(null);
	}

	private void onCreateClick(CursorTargetButton button, PointerEventData eventData)
	{
		this.createAccount();
	}

	private void createAccount()
	{
	}

	private void resetErrorStates()
	{
		this.usernameInputField.ShowNeutral();
		this.emailInputField.ShowNeutral();
		this.passwordInputField.ShowNeutral();
		this.TermsErrorState.SetActive(false);
	}

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

	private void onTermsClick(CursorTargetButton button, PointerEventData eventData)
	{
		this.api.TermsAccepted = !this.api.TermsAccepted;
	}

	private void onClose(CursorTargetButton button, PointerEventData eventData)
	{
		this.Close();
	}
}
