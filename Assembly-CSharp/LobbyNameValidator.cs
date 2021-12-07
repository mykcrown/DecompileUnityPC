using System;

// Token: 0x02000923 RID: 2339
public class LobbyNameValidator : ITextValidator
{
	// Token: 0x17000E9B RID: 3739
	// (get) Token: 0x06003D0C RID: 15628 RVA: 0x0011A6DA File Offset: 0x00118ADA
	// (set) Token: 0x06003D0D RID: 15629 RVA: 0x0011A6E2 File Offset: 0x00118AE2
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x06003D0E RID: 15630 RVA: 0x0011A6EC File Offset: 0x00118AEC
	public TextValidationResult Validate(string name)
	{
		TextValidationResult result = default(TextValidationResult);
		if (name.Length < this.gameDataManager.ConfigData.lobbySettings.minNameLength)
		{
			result.IsOk = false;
			result.DisplayTextKey = "ui.customLobby.validationError.NAME_TOO_SHORT";
		}
		else
		{
			result.IsOk = true;
			result.DisplayTextKey = string.Empty;
		}
		return result;
	}
}
