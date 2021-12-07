using System;

// Token: 0x02000924 RID: 2340
public class LobbyPasswordValidator : ITextValidator
{
	// Token: 0x17000E9C RID: 3740
	// (get) Token: 0x06003D10 RID: 15632 RVA: 0x0011A757 File Offset: 0x00118B57
	// (set) Token: 0x06003D11 RID: 15633 RVA: 0x0011A75F File Offset: 0x00118B5F
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x06003D12 RID: 15634 RVA: 0x0011A768 File Offset: 0x00118B68
	public TextValidationResult Validate(string password)
	{
		TextValidationResult result = default(TextValidationResult);
		if (string.IsNullOrEmpty(password))
		{
			result.IsOk = true;
			result.DisplayTextKey = string.Empty;
		}
		else if (password.Length < this.gameDataManager.ConfigData.lobbySettings.minPWLength)
		{
			result.IsOk = false;
			result.DisplayTextKey = "ui.customLobby.validationError.PASSWORD_TOO_SHORT";
		}
		else
		{
			result.IsOk = true;
			result.DisplayTextKey = string.Empty;
		}
		return result;
	}
}
