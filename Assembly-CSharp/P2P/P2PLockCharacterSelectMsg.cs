using System;
using IconsServer;
using network;

namespace P2P
{
	// Token: 0x0200081A RID: 2074
	public class P2PLockCharacterSelectMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x06003360 RID: 13152 RVA: 0x000F4DF4 File Offset: 0x000F31F4
		public P2PLockCharacterSelectMsg(ulong steamID, ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom, ulong[] equipped)
		{
			this.steamID = steamID;
			this.characterID = characterID;
			this.characterIndex = characterIndex;
			this.skinID = skinID;
			this.isRandom = isRandom;
			this.equipped = equipped;
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x000F4E41 File Offset: 0x000F3241
		public P2PLockCharacterSelectMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x000F4E79 File Offset: 0x000F3279
		public override uint MsgID()
		{
			return 13U;
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x000F4E7D File Offset: 0x000F327D
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x000F4E80 File Offset: 0x000F3280
		public override void SerializeMsg()
		{
			base.Pack(this.steamID);
			base.Pack((uint)this.characterID);
			base.Pack(this.characterIndex);
			base.Pack(this.skinID);
			base.Pack(this.isRandom);
			base.Pack(this.equipped.Length);
			for (int i = 0; i < this.equipped.Length; i++)
			{
				base.Pack(this.equipped[i]);
			}
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x000F4F00 File Offset: 0x000F3300
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.steamID);
			uint num = 8U;
			base.Unpack(ref num);
			this.characterID = (ECharacterType)num;
			base.Unpack(ref this.characterIndex);
			base.Unpack(ref this.skinID);
			base.Unpack(ref this.isRandom);
			int num2 = 0;
			base.Unpack(ref num2);
			this.equipped = new ulong[num2];
			for (int i = 0; i < num2; i++)
			{
				base.Unpack(ref this.equipped[i]);
			}
		}

		// Token: 0x040023EA RID: 9194
		public ulong steamID;

		// Token: 0x040023EB RID: 9195
		public ECharacterType characterID;

		// Token: 0x040023EC RID: 9196
		public uint characterIndex;

		// Token: 0x040023ED RID: 9197
		public ulong skinID;

		// Token: 0x040023EE RID: 9198
		public bool isRandom;

		// Token: 0x040023EF RID: 9199
		public ulong[] equipped = new ulong[10];
	}
}
