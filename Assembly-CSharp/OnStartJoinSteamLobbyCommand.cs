using System;
using UnityEngine;

// Token: 0x02000AC6 RID: 2758
public class OnStartJoinSteamLobbyCommand : GameEvent
{
	// Token: 0x0600509E RID: 20638 RVA: 0x001504F5 File Offset: 0x0014E8F5
	public OnStartJoinSteamLobbyCommand(ulong SteamLobbyID)
	{
		Debug.LogFormat("Steam Lobby Join Request: [{0}]", new object[]
		{
			SteamLobbyID
		});
		this.SteamLobbyID = SteamLobbyID;
	}

	// Token: 0x040033E6 RID: 13286
	public ulong SteamLobbyID;
}
