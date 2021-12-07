using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000992 RID: 2450
public class LoginPanel : LoginScreenPanel
{
	// Token: 0x060042A2 RID: 17058 RVA: 0x00128780 File Offset: 0x00126B80
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

	// Token: 0x060042A3 RID: 17059 RVA: 0x001288AA File Offset: 0x00126CAA
	public override void OnHide()
	{
		this.LoginButton.Removed();
		this.CreateAccountButton.Removed();
		this.RememberMeButton.Removed();
		this.CantLoginButton.Removed();
	}

	// Token: 0x060042A4 RID: 17060 RVA: 0x001288D8 File Offset: 0x00126CD8
	private void onUpdate()
	{
		this.updateRememberMe();
	}

	// Token: 0x060042A5 RID: 17061 RVA: 0x001288E0 File Offset: 0x00126CE0
	private void updateRememberMe()
	{
		this.RememberMeCheckmark.SetActive(base.api.RememberMe);
	}

	// Token: 0x060042A6 RID: 17062 RVA: 0x001288F8 File Offset: 0x00126CF8
	private void onTabUserNameField()
	{
		base.selectTextField(this.passwordInputField.TargetInput);
	}

	// Token: 0x060042A7 RID: 17063 RVA: 0x0012890B File Offset: 0x00126D0B
	private void onTabPasswordField()
	{
		base.selectTextField(null);
	}

	// Token: 0x060042A8 RID: 17064 RVA: 0x00128914 File Offset: 0x00126D14
	private void onLoginClick(CursorTargetButton button, PointerEventData eventData)
	{
		this.onLogin();
	}

	// Token: 0x060042A9 RID: 17065 RVA: 0x0012891C File Offset: 0x00126D1C
	private void onLogin()
	{
		Debug.Log("Login");
	}

	// Token: 0x060042AA RID: 17066 RVA: 0x00128928 File Offset: 0x00126D28
	private void onCreateAccountClick(CursorTargetButton button, PointerEventData eventData)
	{
		base.windowManager.Add<NewAccountWindow>(this.NewAccountWindowPrefab, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
	}

	// Token: 0x060042AB RID: 17067 RVA: 0x00128954 File Offset: 0x00126D54
	private void onCantLoginClick(CursorTargetButton button, PointerEventData eventData)
	{
		Debug.Log("Can't login");
	}

	// Token: 0x060042AC RID: 17068 RVA: 0x00128960 File Offset: 0x00126D60
	private void onRememberMeClick(CursorTargetButton button, PointerEventData eventData)
	{
		base.api.RememberMe = !base.api.RememberMe;
	}

	// Token: 0x060042AD RID: 17069 RVA: 0x0012897B File Offset: 0x00126D7B
	public override void InitSelection()
	{
		if (base.IsCurrentPanel)
		{
			base.selectTextField(this.usernameInputField.TargetInput);
		}
	}

	// Token: 0x04002C80 RID: 11392
	public GameObject NewAccountWindowPrefab;

	// Token: 0x04002C81 RID: 11393
	public GameObject UsernameInputStub;

	// Token: 0x04002C82 RID: 11394
	public GameObject PasswordInputStub;

	// Token: 0x04002C83 RID: 11395
	public CursorTargetButton LoginButton;

	// Token: 0x04002C84 RID: 11396
	public CursorTargetButton CreateAccountButton;

	// Token: 0x04002C85 RID: 11397
	public CursorTargetButton RememberMeButton;

	// Token: 0x04002C86 RID: 11398
	public CursorTargetButton CantLoginButton;

	// Token: 0x04002C87 RID: 11399
	public GameObject RememberMeCheckmark;

	// Token: 0x04002C88 RID: 11400
	private LoginTextInputField usernameInputField;

	// Token: 0x04002C89 RID: 11401
	private LoginTextInputField passwordInputField;
}
