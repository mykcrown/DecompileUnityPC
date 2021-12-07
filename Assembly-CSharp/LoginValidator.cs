using System;
using System.Text.RegularExpressions;

// Token: 0x02000998 RID: 2456
public class LoginValidator : ILoginValidator
{
	// Token: 0x17000FDF RID: 4063
	// (get) Token: 0x06004311 RID: 17169 RVA: 0x00129841 File Offset: 0x00127C41
	// (set) Token: 0x06004312 RID: 17170 RVA: 0x00129849 File Offset: 0x00127C49
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x06004313 RID: 17171 RVA: 0x00129852 File Offset: 0x00127C52
	public LoginValidationResult Validate(LoginEntryType type, string text)
	{
		if (type == LoginEntryType.USERNAME)
		{
			return this.validateUsername(text);
		}
		if (type == LoginEntryType.EMAIL)
		{
			return this.validateEmail(text);
		}
		if (type != LoginEntryType.PASSWORD)
		{
			return null;
		}
		return this.validatePassword(text);
	}

	// Token: 0x06004314 RID: 17172 RVA: 0x00129886 File Offset: 0x00127C86
	private LoginValidationResult validateTerms(bool accepted)
	{
		if (!accepted)
		{
			return new LoginValidationResult(LoginEntryType.TERMS, LoginValidationState.MUST_ACCEPT_TERMS, 0);
		}
		return new LoginValidationResult(LoginEntryType.TERMS, LoginValidationState.OK, 0);
	}

	// Token: 0x06004315 RID: 17173 RVA: 0x001298A0 File Offset: 0x00127CA0
	private LoginValidationResult validateUsername(string name)
	{
		if (name.Length < this.getMinNameLength())
		{
			return new LoginValidationResult(LoginEntryType.USERNAME, LoginValidationState.TOO_SHORT, this.getMinNameLength());
		}
		if (!Regex.IsMatch(name, "^[A-Za-z0-9]*$"))
		{
			return new LoginValidationResult(LoginEntryType.USERNAME, LoginValidationState.INVALID_CHARACTERS, 0);
		}
		return new LoginValidationResult(LoginEntryType.USERNAME, LoginValidationState.OK, 0);
	}

	// Token: 0x06004316 RID: 17174 RVA: 0x001298ED File Offset: 0x00127CED
	private LoginValidationResult validateEmail(string name)
	{
		if (!Regex.IsMatch(name, "[@]"))
		{
			return new LoginValidationResult(LoginEntryType.EMAIL, LoginValidationState.INVALID_EMAIL, 0);
		}
		return new LoginValidationResult(LoginEntryType.EMAIL, LoginValidationState.OK, 0);
	}

	// Token: 0x06004317 RID: 17175 RVA: 0x00129910 File Offset: 0x00127D10
	private LoginValidationResult validatePassword(string text)
	{
		if (Regex.IsMatch(text, "[ ]"))
		{
			return new LoginValidationResult(LoginEntryType.PASSWORD, LoginValidationState.PW_NO_SPACES, 0);
		}
		if (!Regex.IsMatch(text, "[A-Z]"))
		{
			return new LoginValidationResult(LoginEntryType.PASSWORD, LoginValidationState.PW_NEED_CAPITAL, 0);
		}
		if (!Regex.IsMatch(text, "[0-9]"))
		{
			return new LoginValidationResult(LoginEntryType.PASSWORD, LoginValidationState.PW_NEED_NUMBER, 0);
		}
		if (!Regex.IsMatch(text, "[^A-Za-z0-9]"))
		{
			return new LoginValidationResult(LoginEntryType.PASSWORD, LoginValidationState.PW_NEED_SYMBOL, 0);
		}
		if (text.Length < this.getMinPasswordLength())
		{
			return new LoginValidationResult(LoginEntryType.PASSWORD, LoginValidationState.PW_LENGTH, this.getMinPasswordLength());
		}
		return new LoginValidationResult(LoginEntryType.PASSWORD, LoginValidationState.OK, 0);
	}

	// Token: 0x06004318 RID: 17176 RVA: 0x001299A8 File Offset: 0x00127DA8
	private int getMinNameLength()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.minNameLength;
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x001299BF File Offset: 0x00127DBF
	private int getMinPasswordLength()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.minPWLength;
	}
}
