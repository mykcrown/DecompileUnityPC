using System;
using System.Collections.Generic;
using IconsServer;
using network;

namespace P2P
{
	// Token: 0x0200081D RID: 2077
	public class P2PMatchDetailsMsg : NetMsgFast, IP2PMessage
	{
		// Token: 0x06003372 RID: 13170 RVA: 0x000F504C File Offset: 0x000F344C
		public P2PMatchDetailsMsg()
		{
			this.m_msgbuffer = new byte[1000];
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x000F506F File Offset: 0x000F346F
		public P2PMatchDetailsMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x000F50A7 File Offset: 0x000F34A7
		public override uint MsgID()
		{
			return 21U;
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x000F50AB File Offset: 0x000F34AB
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x000F50B0 File Offset: 0x000F34B0
		public override void SerializeMsg()
		{
			base.Pack(this.players.Count, 4U);
			foreach (P2PMatchDetailsMsg.SPlayerDesc splayerDesc in this.players)
			{
				splayerDesc.Pack(this);
			}
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x000F5120 File Offset: 0x000F3520
		public override void DeserializeMsg()
		{
			ushort num = 0;
			base.Unpack(ref num, 4U);
			this.players.Clear();
			for (uint num2 = 0U; num2 < (uint)num; num2 += 1U)
			{
				P2PMatchDetailsMsg.SPlayerDesc splayerDesc = new P2PMatchDetailsMsg.SPlayerDesc();
				splayerDesc.Unpack(this);
				this.players.Add(splayerDesc);
			}
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06003378 RID: 13176 RVA: 0x000F516E File Offset: 0x000F356E
		public List<P2PMatchDetailsMsg.SPlayerDesc> Players
		{
			get
			{
				return this.players;
			}
		}

		// Token: 0x040023F3 RID: 9203
		private List<P2PMatchDetailsMsg.SPlayerDesc> players = new List<P2PMatchDetailsMsg.SPlayerDesc>();

		// Token: 0x0200081E RID: 2078
		public class SPlayerDesc
		{
			// Token: 0x0600337A RID: 13178 RVA: 0x000F519C File Offset: 0x000F359C
			public void Pack(NetMsgFast msg)
			{
				msg.PackString64(this.name);
				msg.Pack(this.userID, 64U);
				msg.Pack(this.isSpectator);
				msg.Pack((uint)this.team, 3U);
				msg.Pack((uint)this.characterSelection, 4U);
				msg.Pack(this.characterIndex, 2U);
				msg.Pack(this.skinId, 16U);
				msg.Pack((ushort)this.equippedPlayerItemIds.Count, 4U);
				for (int num = 0; num != this.equippedPlayerItemIds.Count; num++)
				{
					msg.Pack(this.equippedPlayerItemIds[num], 16U);
				}
				msg.Pack((ulong)((long)this.equippedCharacterItemIds.Count), 4U);
				for (int num2 = 0; num2 != this.equippedCharacterItemIds.Count; num2++)
				{
					msg.Pack(this.equippedCharacterItemIds[num2], 16U);
				}
			}

			// Token: 0x0600337B RID: 13179 RVA: 0x000F528C File Offset: 0x000F368C
			public void Unpack(NetMsgFast msg)
			{
				msg.UnpackString64(ref this.name);
				msg.Unpack(ref this.userID, 64U);
				msg.Unpack(ref this.isSpectator);
				byte b = 0;
				msg.Unpack(ref b, 3U);
				this.team = b;
				uint num = 8U;
				msg.Unpack(ref num, 4U);
				this.characterSelection = (ECharacterType)num;
				msg.Unpack(ref this.characterIndex, 2U);
				msg.Unpack(ref this.skinId, 16U);
				ushort num2 = 0;
				msg.Unpack(ref num2, 4U);
				for (ulong num3 = 0UL; num3 != (ulong)num2; num3 += 1UL)
				{
					ushort item = 0;
					msg.Unpack(ref item, 16U);
					this.equippedPlayerItemIds.Add(item);
				}
				num2 = 0;
				msg.Unpack(ref num2, 4U);
				for (ulong num4 = 0UL; num4 != (ulong)num2; num4 += 1UL)
				{
					ushort item2 = 0;
					msg.Unpack(ref item2, 16U);
					this.equippedCharacterItemIds.Add(item2);
				}
			}

			// Token: 0x040023F4 RID: 9204
			public string name;

			// Token: 0x040023F5 RID: 9205
			public ulong userID;

			// Token: 0x040023F6 RID: 9206
			public bool isSpectator;

			// Token: 0x040023F7 RID: 9207
			public byte team;

			// Token: 0x040023F8 RID: 9208
			public ECharacterType characterSelection = ECharacterType.CharacterTypeCount;

			// Token: 0x040023F9 RID: 9209
			public ushort characterIndex;

			// Token: 0x040023FA RID: 9210
			public ushort skinId;

			// Token: 0x040023FB RID: 9211
			public List<ushort> equippedPlayerItemIds = new List<ushort>();

			// Token: 0x040023FC RID: 9212
			public List<ushort> equippedCharacterItemIds = new List<ushort>();
		}
	}
}
