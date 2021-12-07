using System;
using System.Collections.Generic;
using IconsServer;
using network;

namespace MatchMaking
{
	// Token: 0x020007BE RID: 1982
	public class SCustomLobbyParams
	{
		// Token: 0x06003103 RID: 12547 RVA: 0x000F0DDC File Offset: 0x000EF1DC
		public bool Pack(NetMsgBase msg)
		{
			msg.Pack((uint)this.type);
			msg.Pack(this.setCount);
			msg.Pack(this.numberOfLives);
			msg.Pack(this.maxPlayers);
			msg.Pack(this.steamLobbyId);
			ulong num = 0UL;
			for (int i = 0; i < this.stages.Count; i++)
			{
				num |= (ulong)(1L << (int)(this.stages[i] & (EIconStages)31));
			}
			msg.Pack(num);
			msg.Pack(this.lobbyName);
			msg.Pack(this.lobbyPass);
			msg.Pack((byte)this.gameMode);
			return true;
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x000F0E86 File Offset: 0x000EF286
		public bool Unpack(NetMsgBase msg)
		{
			return false;
		}

		// Token: 0x0400227B RID: 8827
		public ECustomMatchType type = ECustomMatchType.CustomMatchTypeCount;

		// Token: 0x0400227C RID: 8828
		public ulong setCount = 1UL;

		// Token: 0x0400227D RID: 8829
		public ulong numberOfLives = 3UL;

		// Token: 0x0400227E RID: 8830
		public ulong maxPlayers = 2UL;

		// Token: 0x0400227F RID: 8831
		public ulong steamLobbyId;

		// Token: 0x04002280 RID: 8832
		public List<EIconStages> stages;

		// Token: 0x04002281 RID: 8833
		public LobbyGameMode gameMode;

		// Token: 0x04002282 RID: 8834
		public string lobbyName;

		// Token: 0x04002283 RID: 8835
		public string lobbyPass;
	}
}
