// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using network;
using System;
using System.Collections.Generic;

namespace MatchMaking
{
	public class SCustomLobbyParams
	{
		public ECustomMatchType type = ECustomMatchType.CustomMatchTypeCount;

		public ulong setCount = 1uL;

		public ulong numberOfLives = 3uL;

		public ulong maxPlayers = 2uL;

		public ulong steamLobbyId;

		public List<EIconStages> stages;

		public LobbyGameMode gameMode;

		public string lobbyName;

		public string lobbyPass;

		public bool Pack(NetMsgBase msg)
		{
			msg.Pack((uint)this.type);
			msg.Pack(this.setCount);
			msg.Pack(this.numberOfLives);
			msg.Pack(this.maxPlayers);
			msg.Pack(this.steamLobbyId);
			ulong num = 0uL;
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

		public bool Unpack(NetMsgBase msg)
		{
			return false;
		}
	}
}
