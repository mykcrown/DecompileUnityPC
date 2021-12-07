// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoginPanel : LoginScreenPanel
{
	public GameObject NewAccountWindowPrefab;

	public GameObject UsernameInputStub;

	public GameObject PasswordInputStub;

	public CursorTargetButton LoginButton;

	public CursorTargetButton CreateAccountButton;

	public CursorTargetButton RememberMeButton;

	public CursorTargetButton CantLoginButton;

	public GameObject RememberMeCheckmark;

	private LoginTextInputField usernameInputField;

	private LoginTextInputField passwordInputField;

	[PostConstruct]
	public void Init()
	{
		this.usernameInputField = base.createTextEntry(this.UsernameInputStub, "ui.login.username", LoginEntryType.USERNAME);
		this.usernameInputField.UseValidation = false;
		this.usernameInputField.TabCallback = new Action(this.onTabUserNameField);
		this.usernameInputField.EnterCallback = new Action(this.onTabUserNameField);
		this.passwordInputField = base.createTextEntry(this.PasswordInputStub, "ui.login.password", LoginEntryType.PASSWORD);
		this.passwordInputField.UseValidation = false;
		this.passwordInputField.TabCallback = new Action(this.onTabPasswordField);
		this.passwordInputField.EnterCallback = new Action(this.onLogin);
		this.LoginButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onLoginClick);
		this.CreateAccountButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onCreateAccountClick);
		this.RememberMeButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onRememberMeClick);
		this.CantLoginButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onCantLoginClick);
		base.listen(LoginScreenAPI.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	public override void OnHide()
	{
		this.LoginButton.Removed();
		this.CreateAccountButton.Removed();
		this.RememberMeButton.Removed();
		this.CantLoginButton.Removed();
	}

	private void onUpdate()
	{
		this.updateRememberMe();
	}

	private void updateRememberMe()
	{
		this.RememberMeCheckmark.SetActive(base.api.RememberMe);
	}

	private void onTabUserNameField()
	{
		base.selectTextField(this.passwordInputField.TargetInput);
	}

	private void onTabPasswordField()
	{
		base.selectTextField(null);
	}

	private void onLoginClick(CursorTargetButton button, PointerEventData eventData)
	{
		this.onLogin();
	}

	private void onLogin()
	{
		UnityEngine.Debug.Log("Login");
	}

	private void onCreateAccountClick(CursorTargetButton button, PointerEventData eventData)
	{
		base.windowManager.Add<NewAccountWindow>(this.NewAccountWindowPrefab, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
	}

	private void onCantLoginClick(CursorTargetButton button, PointerEventData eventData)
	{
		UnityEngine.Debug.Log("Can't login");
	}

	private void onRememberMeClick(CursorTargetButton button, PointerEventData eventData)
	{
		base.api.RememberMe = !base.api.RememberMe;
	}

	public override void InitSelection()
	{
		if (base.IsCurrentPanel)
		{
			base.selectTextField(this.usernameInputField.TargetInput);
		}
	}
}
