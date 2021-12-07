using System;
using network;

namespace IconsServer
{
	// Token: 0x020007B0 RID: 1968
	public class SCharacterDetails
	{
		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x060030F6 RID: 12534 RVA: 0x000F0B89 File Offset: 0x000EEF89
		public ulong TotalGamesPlayed
		{
			get
			{
				return (ulong)(this.wins + this.losses + this.draws);
			}
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x000F0BA0 File Offset: 0x000EEFA0
		public void Pack(NetMsgBase msg)
		{
			msg.Pack(this.id);
			msg.Pack((uint)this.type);
			msg.Pack(this.statusMask);
			msg.Pack(this.wins);
			msg.Pack(this.losses);
			msg.Pack(this.draws);
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x000F0BF8 File Offset: 0x000EEFF8
		public void Unpack(NetMsgBase msg)
		{
			msg.Unpack(ref this.id);
			uint num = 8U;
			msg.Unpack(ref num);
			this.type = (ECharacterType)num;
			msg.Unpack(ref this.statusMask);
			msg.Unpack(ref this.wins);
			msg.Unpack(ref this.losses);
			msg.Unpack(ref this.draws);
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x000F0C54 File Offset: 0x000EF054
		public override string ToString()
		{
			string text = "[" + this.type + " ";
			if (this.id == 0UL)
			{
				text += "C:Inv ";
			}
			else
			{
				text = text + "C:" + this.id;
			}
			return text + "]";
		}

		// Token: 0x04002233 RID: 8755
		public ulong id;

		// Token: 0x04002234 RID: 8756
		public ECharacterType type = ECharacterType.CharacterTypeCount;

		// Token: 0x04002235 RID: 8757
		public ulong statusMask;

		// Token: 0x04002236 RID: 8758
		public uint wins;

		// Token: 0x04002237 RID: 8759
		public uint losses;

		// Token: 0x04002238 RID: 8760
		public uint draws;
	}
}
