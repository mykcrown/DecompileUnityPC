// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;

public class LoginValidator : ILoginValidator
{
	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

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

	private LoginValidationResult validateTerms(bool accepted)
	{
		if (!accepted)
		{
			return new LoginValidationResult(LoginEntryType.TERMS, LoginValidationState.MUST_ACCEPT_TERMS, 0);
		}
		return new LoginValidationResult(LoginEntryType.TERMS, LoginValidationState.OK, 0);
	}

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

	private LoginValidationResult validateEmail(string name)
	{
		if (!Regex.IsMatch(name, "[@]"))
		{
			return new LoginValidationResult(LoginEntryType.EMAIL, LoginValidationState.INVALID_EMAIL, 0);
		}
		return new LoginValidationResult(LoginEntryType.EMAIL, LoginValidationState.OK, 0);
	}

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

	private int getMinNameLength()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.minNameLength;
	}

	private int getMinPasswordLength()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.minPWLength;
	}
}
