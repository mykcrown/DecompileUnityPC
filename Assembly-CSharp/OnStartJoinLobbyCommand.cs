using System;
using UnityEngine;

// Token: 0x02000AC5 RID: 2757
public class OnStartJoinLobbyCommand : GameEvent
{
	// Token: 0x0600509D RID: 20637 RVA: 0x001504C7 File Offset: 0x0014E8C7
	public OnStartJoinLobbyCommand(string LobbyName, string LobbyPassword)
	{
		Debug.LogFormat("Steam Lobby Join Request: [{0}] [{1}]", new object[]
		{
			LobbyName,
			LobbyPassword
		});
		this.LobbyName = LobbyName;
		this.LobbyPassword = LobbyPassword;
	}

	// Token: 0x040033E4 RID: 13284
	public string LobbyName;

	// Token: 0x040033E5 RID: 13285
	public string LobbyPassword;
}
