// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using network;
using System;
using System.Collections.Generic;

namespace P2P
{
	public class P2PMatchDetailsMsg : NetMsgFast, IP2PMessage
	{
		public class SPlayerDesc
		{
			public string name;

			public ulong userID;

			public bool isSpectator;

			public byte team;

			public ECharacterType characterSelection = ECharacterType.CharacterTypeCount;

			public ushort characterIndex;

			public ushort skinId;

			public List<ushort> equippedPlayerItemIds = new List<ushort>();

			public List<ushort> equippedCharacterItemIds = new List<ushort>();

			public void Pack(NetMsgFast msg)
			{
				msg.PackString64(this.name);
				msg.Pack(this.userID, 64u);
				msg.Pack(this.isSpectator);
				msg.Pack((uint)this.team, 3u);
				msg.Pack((uint)this.characterSelection, 4u);
				msg.Pack(this.characterIndex, 2u);
				msg.Pack(this.skinId, 16u);
				msg.Pack((ushort)this.equippedPlayerItemIds.Count, 4u);
				for (int num = 0; num != this.equippedPlayerItemIds.Count; num++)
				{
					msg.Pack(this.equippedPlayerItemIds[num], 16u);
				}
				msg.Pack((ulong)((long)this.equippedCharacterItemIds.Count), 4u);
				for (int num2 = 0; num2 != this.equippedCharacterItemIds.Count; num2++)
				{
					msg.Pack(this.equippedCharacterItemIds[num2], 16u);
				}
			}

			public void Unpack(NetMsgFast msg)
			{
				msg.UnpackString64(ref this.name);
				msg.Unpack(ref this.userID, 64u);
				msg.Unpack(ref this.isSpectator);
				byte b = 0;
				msg.Unpack(ref b, 3u);
				this.team = b;
				uint num = 8u;
				msg.Unpack(ref num, 4u);
				this.characterSelection = (ECharacterType)num;
				msg.Unpack(ref this.characterIndex, 2u);
				msg.Unpack(ref this.skinId, 16u);
				ushort num2 = 0;
				msg.Unpack(ref num2, 4u);
				for (ulong num3 = 0uL; num3 != (ulong)num2; num3 += 1uL)
				{
					ushort item = 0;
					msg.Unpack(ref item, 16u);
					this.equippedPlayerItemIds.Add(item);
				}
				num2 = 0;
				msg.Unpack(ref num2, 4u);
				for (ulong num4 = 0uL; num4 != (ulong)num2; num4 += 1uL)
				{
					ushort item2 = 0;
					msg.Unpack(ref item2, 16u);
					this.equippedCharacterItemIds.Add(item2);
				}
			}
		}

		private List<P2PMatchDetailsMsg.SPlayerDesc> players = new List<P2PMatchDetailsMsg.SPlayerDesc>();

		public List<P2PMatchDetailsMsg.SPlayerDesc> Players
		{
			get
			{
				return this.players;
			}
		}

		public P2PMatchDetailsMsg()
		{
			this.m_msgbuffer = new byte[1000];
		}

		public P2PMatchDetailsMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8u;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 21u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.players.Count, 4u);
			foreach (P2PMatchDetailsMsg.SPlayerDesc current in this.players)
			{
				current.Pack(this);
			}
		}

		public override void DeserializeMsg()
		{
			ushort num = 0;
			base.Unpack(ref num, 4u);
			this.players.Clear();
			for (uint num2 = 0u; num2 < (uint)num; num2 += 1u)
			{
				P2PMatchDetailsMsg.SPlayerDesc sPlayerDesc = new P2PMatchDetailsMsg.SPlayerDesc();
				sPlayerDesc.Unpack(this);
				this.players.Add(sPlayerDesc);
			}
		}
	}
}
