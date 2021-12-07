// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class SNetwork : NetEvent
	{
		public enum ENetEvent
		{
			Invalid,
			Connect,
			Disconnect,
			Error
		}

		public SNetwork.ENetEvent networkEvent;

		public bool success;

		public string msg;

		public override EEvents Type
		{
			get
			{
				return EEvents.Network;
			}
		}
	}
}
