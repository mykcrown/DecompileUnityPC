// Decompile from assembly: Assembly-CSharp.dll

using MatchMaking;
using System;

public class RequestMatchCommand : ConnectionEvent
{
	public EQueueTypes queueType;

	public RequestMatchCommand(EQueueTypes queueType)
	{
		this.queueType = queueType;
	}
}
