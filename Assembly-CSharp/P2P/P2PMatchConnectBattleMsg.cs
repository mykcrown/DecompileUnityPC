using System;
using System.Collections.Generic;
using IconsServer;
using MatchMaking;
using network;

namespace P2P
{
	// Token: 0x02000816 RID: 2070
	public class P2PMatchConnectBattleMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x06003355 RID: 13141 RVA: 0x000F4AFC File Offset: 0x000F2EFC
		public P2PMatchConnectBattleMsg()
		{
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x000F4B2C File Offset: 0x000F2F2C
		public P2PMatchConnectBattleMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x000F4B8A File Offset: 0x000F2F8A
		public override uint MsgID()
		{
			return 19U;
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000F4B8E File Offset: 0x000F2F8E
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000F4B94 File Offset: 0x000F2F94
		public override void SerializeMsg()
		{
			base.Pack(this.MatchID);
			base.Pack(this.hostSteamID);
			base.Pack(this.gameMode);
			base.Pack(this.characterSelectSeconds);
			base.Pack(this.matchLengthSeconds);
			base.Pack(this.numberOfLives);
			base.Pack(this.assistCount);
			ulong num = 0UL;
			foreach (EIconStages eiconStages in this.stages)
			{
				num |= 1UL << (int)(eiconStages & (EIconStages)31);
			}
			base.Pack(num);
			base.Pack((ulong)((long)this.players.Count));
			foreach (SP2PMatchBasicPlayerDesc sp2PMatchBasicPlayerDesc in this.players)
			{
				sp2PMatchBasicPlayerDesc.Pack(this);
			}
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x000F4CB0 File Offset: 0x000F30B0
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.MatchID);
			base.Unpack(ref this.hostSteamID);
			base.Unpack(ref this.gameMode);
			base.Unpack(ref this.characterSelectSeconds);
			base.Unpack(ref this.matchLengthSeconds);
			base.Unpack(ref this.numberOfLives);
			base.Unpack(ref this.assistCount);
			ulong num = 0UL;
			base.Unpack(ref num);
			this.stages.Clear();
			for (uint num2 = 0U; num2 < 9U; num2 += 1U)
			{
				if (num == 0UL)
				{
					break;
				}
				if ((num & 1UL) > 0UL)
				{
					this.stages.Add((EIconStages)num2);
				}
				num >>= 1;
			}
			ulong num3 = 0UL;
			base.Unpack(ref num3);
			uint num4 = 0U;
			while ((ulong)num4 < num3)
			{
				SP2PMatchBasicPlayerDesc sp2PMatchBasicPlayerDesc = new SP2PMatchBasicPlayerDesc();
				sp2PMatchBasicPlayerDesc.Unpack(this);
				this.players.Add(sp2PMatchBasicPlayerDesc);
				num4 += 1U;
			}
		}

		// Token: 0x040023D5 RID: 9173
		public Guid MatchID = MatchMaking.Constants.skInvalidMatchId;

		// Token: 0x040023D6 RID: 9174
		public ulong hostSteamID;

		// Token: 0x040023D7 RID: 9175
		public uint matchLengthSeconds;

		// Token: 0x040023D8 RID: 9176
		public uint numberOfLives;

		// Token: 0x040023D9 RID: 9177
		public uint assistCount;

		// Token: 0x040023DA RID: 9178
		public List<EIconStages> stages = new List<EIconStages>();

		// Token: 0x040023DB RID: 9179
		public byte gameMode;

		// Token: 0x040023DC RID: 9180
		public ETeamAttack teamAttack = ETeamAttack.Enabled;

		// Token: 0x040023DD RID: 9181
		public List<SP2PMatchBasicPlayerDesc> players = new List<SP2PMatchBasicPlayerDesc>();

		// Token: 0x040023DE RID: 9182
		public uint characterSelectSeconds;
	}
}
