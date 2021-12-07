// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace IconsServer
{
	public class SCharacterDetails
	{
		public ulong id;

		public ECharacterType type = ECharacterType.CharacterTypeCount;

		public ulong statusMask;

		public uint wins;

		public uint losses;

		public uint draws;

		public ulong TotalGamesPlayed
		{
			get
			{
				return (ulong)(this.wins + this.losses + this.draws);
			}
		}

		public void Pack(NetMsgBase msg)
		{
			msg.Pack(this.id);
			msg.Pack((uint)this.type);
			msg.Pack(this.statusMask);
			msg.Pack(this.wins);
			msg.Pack(this.losses);
			msg.Pack(this.draws);
		}

		public void Unpack(NetMsgBase msg)
		{
			msg.Unpack(ref this.id);
			uint num = 8u;
			msg.Unpack(ref num);
			this.type = (ECharacterType)num;
			msg.Unpack(ref this.statusMask);
			msg.Unpack(ref this.wins);
			msg.Unpack(ref this.losses);
			msg.Unpack(ref this.draws);
		}

		public override string ToString()
		{
			string text = "[" + this.type + " ";
			if (this.id == 0uL)
			{
				text += "C:Inv ";
			}
			else
			{
				text = text + "C:" + this.id;
			}
			return text + "]";
		}
	}
}
