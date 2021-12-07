// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using network;
using System;

public interface IDebugLatencyManager
{
	int AddInboundLatency
	{
		get;
		set;
	}

	int AddOutboundLatency
	{
		get;
		set;
	}

	void ProcessBroadcast(ServerEvent message, Action<ServerEvent> callback);

	void ProcessSend(NetMsgBase message, Action<NetMsgBase> callback);
}
