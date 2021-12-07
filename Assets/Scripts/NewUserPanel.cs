// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewUserPanel : LoginScreenPanel
{
	public GameObject UsernameInputStub;

	public GameObject EmailInputStub;

	public GameObject PasswordInputStub;

	public CursorTargetButton CreateAccountButton;

	public CursorTargetButton HaveAccountButton;

	public CursorTargetButton TermsButton;

	public CursorTargetButton TermsAndConditionsLinkButton;

	public GameObject TermsCheckmark;

	public GameObject TermsErrorState;

	public FlashableText TermsErrorFlash;

	public TMP_Text TermsText;

	private LoginTextInputField usernameInputField;

	private LoginTextInputField emailInputField;

	private LoginTextInputField passwordInputField;

	[Inject]
	public IHyperlinkHandler hyperlinkHandler
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.usernameInputField = base.createTextEntry(this.UsernameInputStub, "ui.login.username", LoginEntryType.USERNAME);
		this.usernameInputField.TabCallback = new Action(this.onTabUserNameField);
		this.usernameInputField.EnterCallback = new Action(this.onTabUserNameField);
		LoginTextInputField expr_4C = this.usernameInputField;
		expr_4C.EndEditCallback = (Action)Delegate.Combine(expr_4C.EndEditCallback, new Action(this.onEndEditUsername));
		this.usernameInputField.UseValidation = false;
		this.emailInputField = base.createTextEntry(this.EmailInputStub, "ui.login.email", LoginEntryType.EMAIL);
		this.emailInputField.TabCallback = new Action(this.onTabEmailField);
		this.emailInputField.EnterCallback = new Action(this.onTabEmailField);
		LoginTextInputField expr_C5 = this.emailInputField;
		expr_C5.EndEditCallback = (Action)Delegate.Combine(expr_C5.EndEditCallback, new Action(this.onEndEditEmail));
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

	public override void OnHide()
	{
		this.CreateAccountButton.Removed();
		this.HaveAccountButton.Removed();
	}

	private void onUpdate()
	{
		this.updateTermsAccepted();
	}

	private void updateTermsAccepted()
	{
		this.TermsCheckmark.SetActive(base.api.TermsAccepted);
		if (base.api.TermsAccepted)
		{
			this.TermsErrorState.SetActive(false);
		}
	}

	private void onEndEditUsername()
	{
		if (this.usernameInputField.Text.Length > 0)
		{
			this.usernameInputField.UseValidation = true;
		}
	}

	private void onEndEditEmail()
	{
		if (this.emailInputField.Text.Length > 0)
		{
			this.emailInputField.UseValidation = true;
		}
	}

	private void onTabUserNameField()
	{
		base.selectTextField(this.emailInputField.TargetInput);
	}

	private void onTabEmailField()
	{
		base.selectTextField(null);
	}

	private void onTabPasswordField()
	{
		base.selectTextField(null);
	}

	private void onCreateAccount()
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
			this.TermsErrorFlash.Flash();
		}
	}

	private void onCreateAccountClicked(CursorTargetButton button, PointerEventData eventData)
	{
		this.onCreateAccount();
	}

	private void onHaveAccountClicked(CursorTargetButton button, PointerEventData eventData)
	{
	}

	private void onTermsClicked(CursorTargetButton button, PointerEventData eventData)
	{
		base.api.TermsAccepted = !base.api.TermsAccepted;
	}

	private void onTermsTextClicked(CursorTargetButton button, PointerEventData eventData)
	{
		this.hyperlinkHandler.TryClickLink(this.TermsText, Input.mousePosition);
	}

	public override void InitSelection()
	{
		if (base.IsCurrentPanel)
		{
			base.selectTextField(this.usernameInputField.TargetInput);
		}
	}
}
