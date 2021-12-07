// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IRichPresence
{
	void SetLobbyParameters(PresenceLobbyParameters presenceParams);

	void ClearPresence();

	void SetPresence(string statusString, string loc1 = null, string portraitKey = null, string portraitCaption = null);
}
