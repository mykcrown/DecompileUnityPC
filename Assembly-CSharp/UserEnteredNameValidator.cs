using System;
using System.Text.RegularExpressions;

// Token: 0x0200066E RID: 1646
public class UserEnteredNameValidator : IUserEnteredNameValidator
{
	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x060028B0 RID: 10416 RVA: 0x000C54BC File Offset: 0x000C38BC
	// (set) Token: 0x060028B1 RID: 10417 RVA: 0x000C54C4 File Offset: 0x000C38C4
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x060028B2 RID: 10418 RVA: 0x000C54D0 File Offset: 0x000C38D0
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

	// Token: 0x060028B3 RID: 10419 RVA: 0x000C5501 File Offset: 0x000C3901
	public int GetMinNameLength()
	{
		return 1;
	}

	// Token: 0x060028B4 RID: 10420 RVA: 0x000C5504 File Offset: 0x000C3904
	public int GetMaxOptionsProfileNameLength()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.maxOptionProfileNameLength;
	}

	// Token: 0x060028B5 RID: 10421 RVA: 0x000C551B File Offset: 0x000C391B
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

	// Token: 0x060028B6 RID: 10422 RVA: 0x000C5544 File Offset: 0x000C3944
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
