// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class ClientJoinedRoomEvent : GameEvent
{
	public List<PlayerNum> localPlayers = new List<PlayerNum>();
}
