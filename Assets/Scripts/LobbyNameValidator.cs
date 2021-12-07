// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LobbyNameValidator : ITextValidator
{
	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

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
