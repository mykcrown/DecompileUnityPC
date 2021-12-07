// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Net;

namespace network
{
	public class NetAddress
	{
		public enum EType
		{
			Invalid,
			IPV4,
			TypeCount
		}

		private NetAddress.EType type = NetAddress.EType.TypeCount;

		private IPAddress address;

		private ushort port;

		public NetAddress.EType Type
		{
			get
			{
				return this.type;
			}
		}

		public IPAddress Address
		{
			get
			{
				return this.address;
			}
		}

		public string IpString
		{
			get
			{
				return this.address.ToString();
			}
		}

		public ushort Port
		{
			get
			{
				return this.port;
			}
		}

		public bool IsValid()
		{
			return this.type != NetAddress.EType.TypeCount;
		}

		public void Set(string ip, ushort _port)
		{
			this.type = NetAddress.EType.IPV4;
			this.address = IPAddress.Parse(ip);
			this.port = _port;
		}

		public override string ToString()
		{
			return this.address.ToString() + ":" + this.port;
		}

		public void Pack(NetMsgBase msg)
		{
			msg.Pack((byte)this.type);
			NetAddress.EType eType = this.type;
			if (eType == NetAddress.EType.Invalid)
			{
				return;
			}
			if (eType != NetAddress.EType.IPV4)
			{
				return;
			}
			uint val = BitConverter.ToUInt32(this.address.GetAddressBytes(), 0);
			msg.Pack(val);
			msg.Pack(this.port);
		}

		public void Unpack(NetMsgBase msg)
		{
			byte b = 0;
			msg.Unpack(ref b);
			this.type = (NetAddress.EType)b;
			NetAddress.EType eType = this.type;
			if (eType == NetAddress.EType.Invalid)
			{
				return;
			}
			if (eType != NetAddress.EType.IPV4)
			{
				return;
			}
			uint value = 0u;
			msg.Unpack(ref value);
			this.address = new IPAddress(BitConverter.GetBytes(value));
			msg.Unpack(ref this.port);
		}
	}
}
