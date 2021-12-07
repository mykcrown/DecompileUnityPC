// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using MatchMaking;
using network;
using System;
using System.Collections.Generic;

namespace P2P
{
	public class P2PMatchConnectBattleMsg : NetMsgBase, IP2PMessage
	{
		public Guid MatchID = MatchMaking.Constants.skInvalidMatchId;

		public ulong hostSteamID;

		public uint matchLengthSeconds;

		public uint numberOfLives;

		public uint assistCount;

		public List<EIconStages> stages = new List<EIconStages>();

		public byte gameMode;

		public ETeamAttack teamAttack = ETeamAttack.Enabled;

		public List<SP2PMatchBasicPlayerDesc> players = new List<SP2PMatchBasicPlayerDesc>();

		public uint characterSelectSeconds;

		public P2PMatchConnectBattleMsg()
		{
		}

		public P2PMatchConnectBattleMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 19u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.MatchID);
			base.Pack(this.hostSteamID);
			base.Pack(this.gameMode);
			base.Pack(this.characterSelectSeconds);
			base.Pack(this.matchLengthSeconds);
			base.Pack(this.numberOfLives);
			base.Pack(this.assistCount);
			ulong num = 0uL;
			foreach (EIconStages current in this.stages)
			{
				num |= 1uL << (int)(current & (EIconStages)31);
			}
			base.Pack(num);
			base.Pack((ulong)((long)this.players.Count));
			foreach (SP2PMatchBasicPlayerDesc current2 in this.players)
			{
				current2.Pack(this);
			}
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.MatchID);
			base.Unpack(ref this.hostSteamID);
			base.Unpack(ref this.gameMode);
			base.Unpack(ref this.characterSelectSeconds);
			base.Unpack(ref this.matchLengthSeconds);
			base.Unpack(ref this.numberOfLives);
			base.Unpack(ref this.assistCount);
			ulong num = 0uL;
			base.Unpack(ref num);
			this.stages.Clear();
			for (uint num2 = 0u; num2 < 9u; num2 += 1u)
			{
				if (num == 0uL)
				{
					break;
				}
				if ((num & 1uL) > 0uL)
				{
					this.stages.Add((EIconStages)num2);
				}
				num >>= 1;
			}
			ulong num3 = 0uL;
			base.Unpack(ref num3);
			uint num4 = 0u;
			while ((ulong)num4 < num3)
			{
				SP2PMatchBasicPlayerDesc sP2PMatchBasicPlayerDesc = new SP2PMatchBasicPlayerDesc();
				sP2PMatchBasicPlayerDesc.Unpack(this);
				this.players.Add(sP2PMatchBasicPlayerDesc);
				num4 += 1u;
			}
		}
	}
}
