using System;

// Token: 0x02000201 RID: 513
public interface IRichPresence
{
	// Token: 0x06000976 RID: 2422
	void SetLobbyParameters(PresenceLobbyParameters presenceParams);

	// Token: 0x06000977 RID: 2423
	void ClearPresence();

	// Token: 0x06000978 RID: 2424
	void SetPresence(string statusString, string loc1 = null, string portraitKey = null, string portraitCaption = null);
}
