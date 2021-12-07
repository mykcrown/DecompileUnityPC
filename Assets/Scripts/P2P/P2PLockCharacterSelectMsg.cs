// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using network;
using System;

namespace P2P
{
	public class P2PLockCharacterSelectMsg : NetMsgBase, IP2PMessage
	{
		public ulong steamID;

		public ECharacterType characterID;

		public uint characterIndex;

		public ulong skinID;

		public bool isRandom;

		public ulong[] equipped = new ulong[10];

		public P2PLockCharacterSelectMsg(ulong steamID, ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom, ulong[] equipped)
		{
			this.steamID = steamID;
			this.characterID = characterID;
			this.characterIndex = characterIndex;
			this.skinID = skinID;
			this.isRandom = isRandom;
			this.equipped = equipped;
		}

		public P2PLockCharacterSelectMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 13u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

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

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.steamID);
			uint num = 8u;
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
	}
}
