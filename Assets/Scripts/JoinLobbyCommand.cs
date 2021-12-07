// Decompile from assembly: Assembly-CSharp.dll

using System;

public class JoinLobbyCommand : ConnectionEvent
{
	public class JoinResponse
	{
		public bool success;

		public JoinResponse(bool success)
		{
			this.success = success;
		}
	}

	public Action<JoinLobbyCommand.JoinResponse> handler;

	public JoinLobbyCommand(Action<JoinLobbyCommand.JoinResponse> handler)
	{
		this.handler = handler;
	}
}
