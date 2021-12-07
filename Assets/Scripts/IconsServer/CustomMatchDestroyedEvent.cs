// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class CustomMatchDestroyedEvent : ServerEvent
	{
		public enum EReason
		{
			Reason_OwnerDestroyed,
			ReasonCount
		}

		public CustomMatchDestroyedEvent.EReason reason;

		public ulong id;

		public string lobbyName;

		public CustomMatchDestroyedEvent(CustomMatchDestroyedEvent.EReason reason, ulong id, string lobbyName)
		{
			this.reason = reason;
			this.id = id;
			this.lobbyName = lobbyName;
		}
	}
}
