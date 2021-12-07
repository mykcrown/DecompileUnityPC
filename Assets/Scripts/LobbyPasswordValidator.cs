// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LobbyPasswordValidator : ITextValidator
{
	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

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
