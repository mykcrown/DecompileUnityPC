// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;

public class UserEnteredNameValidator : IUserEnteredNameValidator
{
	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public NameValidationResult CheckOptionsProfile(string text)
	{
		NameValidationResult nameValidationResult = this.standardCheck(text);
		if (nameValidationResult != NameValidationResult.OK)
		{
			return nameValidationResult;
		}
		if (text.Length > this.GetMaxOptionsProfileNameLength())
		{
			return NameValidationResult.TOO_LONG;
		}
		return NameValidationResult.OK;
	}

	public int GetMinNameLength()
	{
		return 1;
	}

	public int GetMaxOptionsProfileNameLength()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.maxOptionProfileNameLength;
	}

	private NameValidationResult standardCheck(string text)
	{
		if (text.Length < this.GetMinNameLength())
		{
			return NameValidationResult.TOO_SHORT;
		}
		if (!Regex.IsMatch(text, "^[a-zA-Z0-9 ]*$"))
		{
			return NameValidationResult.INVALID_CHAR;
		}
		return NameValidationResult.OK;
	}

	public string FixSpaces(string text)
	{
		char c = '\0';
		for (int i = text.Length - 1; i >= 0; i--)
		{
			if (i < text.Length - 1 && c == ' ' && text[i] == ' ')
			{
				text = text.Remove(i, 1);
			}
			c = text[i];
		}
		text = text.Trim();
		return text;
	}
}
