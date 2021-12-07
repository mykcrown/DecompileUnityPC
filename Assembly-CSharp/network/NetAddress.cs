using System;
using System.Net;

namespace network
{
	// Token: 0x020007BF RID: 1983
	public class NetAddress
	{
		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06003106 RID: 12550 RVA: 0x000F0E98 File Offset: 0x000EF298
		public NetAddress.EType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x000F0EA0 File Offset: 0x000EF2A0
		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06003108 RID: 12552 RVA: 0x000F0EA8 File Offset: 0x000EF2A8
		public string IpString
		{
			get
			{
				return this.address.ToString();
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x000F0EB5 File Offset: 0x000EF2B5
		public ushort Port
		{
			get
			{
				return this.port;
			}
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x000F0EBD File Offset: 0x000EF2BD
		public bool IsValid()
		{
			return this.type != NetAddress.EType.TypeCount;
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x000F0ECB File Offset: 0x000EF2CB
		public void Set(string ip, ushort _port)
		{
			this.type = NetAddress.EType.IPV4;
			this.address = IPAddress.Parse(ip);
			this.port = _port;
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x000F0EE7 File Offset: 0x000EF2E7
		public override string ToString()
		{
			return this.address.ToString() + ":" + this.port;
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x000F0F0C File Offset: 0x000EF30C
		public void Pack(NetMsgBase msg)
		{
			msg.Pack((byte)this.type);
			NetAddress.EType etype = this.type;
			if (etype == NetAddress.EType.Invalid)
			{
				return;
			}
			if (etype != NetAddress.EType.IPV4)
			{
				return;
			}
			uint val = BitConverter.ToUInt32(this.address.GetAddressBytes(), 0);
			msg.Pack(val);
			msg.Pack(this.port);
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x000F0F6C File Offset: 0x000EF36C
		public void Unpack(NetMsgBase msg)
		{
			byte b = 0;
			msg.Unpack(ref b);
			this.type = (NetAddress.EType)b;
			NetAddress.EType etype = this.type;
			if (etype == NetAddress.EType.Invalid)
			{
				return;
			}
			if (etype != NetAddress.EType.IPV4)
			{
				return;
			}
			uint value = 0U;
			msg.Unpack(ref value);
			this.address = new IPAddress(BitConverter.GetBytes(value));
			msg.Unpack(ref this.port);
		}

		// Token: 0x04002284 RID: 8836
		private NetAddress.EType type = NetAddress.EType.TypeCount;

		// Token: 0x04002285 RID: 8837
		private IPAddress address;

		// Token: 0x04002286 RID: 8838
		private ushort port;

		// Token: 0x020007C0 RID: 1984
		public enum EType
		{
			// Token: 0x04002288 RID: 8840
			Invalid,
			// Token: 0x04002289 RID: 8841
			IPV4,
			// Token: 0x0400228A RID: 8842
			TypeCount
		}
	}
}
