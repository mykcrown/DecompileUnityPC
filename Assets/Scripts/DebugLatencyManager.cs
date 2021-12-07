// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using network;
using System;
using System.Runtime.CompilerServices;

public class DebugLatencyManager : IDebugLatencyManager
{
	private sealed class _ProcessBroadcast_c__AnonStorey0
	{
		internal Action<ServerEvent> callback;

		internal ServerEvent message;

		internal void __m__0()
		{
			this.callback(this.message);
		}
	}

	private sealed class _ProcessSend_c__AnonStorey1
	{
		internal Action<NetMsgBase> callback;

		internal NetMsgBase message;

		internal void __m__0()
		{
			this.callback(this.message);
		}
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	public int AddOutboundLatency
	{
		get;
		set;
	}

	public int AddInboundLatency
	{
		get;
		set;
	}

	public void ProcessBroadcast(ServerEvent message, Action<ServerEvent> callback)
	{
		DebugLatencyManager._ProcessBroadcast_c__AnonStorey0 _ProcessBroadcast_c__AnonStorey = new DebugLatencyManager._ProcessBroadcast_c__AnonStorey0();
		_ProcessBroadcast_c__AnonStorey.callback = callback;
		_ProcessBroadcast_c__AnonStorey.message = message;
		int addInboundLatency = this.AddInboundLatency;
		if (addInboundLatency <= 0)
		{
			_ProcessBroadcast_c__AnonStorey.callback(_ProcessBroadcast_c__AnonStorey.message);
		}
		else
		{
			if (_ProcessBroadcast_c__AnonStorey.message is BatchEvent)
			{
				_ProcessBroadcast_c__AnonStorey.message = (ServerEvent)(_ProcessBroadcast_c__AnonStorey.message as BatchEvent).Clone();
			}
			this.timer.SetTimeout(addInboundLatency, new Action(_ProcessBroadcast_c__AnonStorey.__m__0));
		}
	}

	public void ProcessSend(NetMsgBase message, Action<NetMsgBase> callback)
	{
		DebugLatencyManager._ProcessSend_c__AnonStorey1 _ProcessSend_c__AnonStorey = new DebugLatencyManager._ProcessSend_c__AnonStorey1();
		_ProcessSend_c__AnonStorey.callback = callback;
		_ProcessSend_c__AnonStorey.message = message;
		int addOutboundLatency = this.AddOutboundLatency;
		if (addOutboundLatency <= 0)
		{
			_ProcessSend_c__AnonStorey.callback(_ProcessSend_c__AnonStorey.message);
		}
		else
		{
			this.timer.SetTimeout(addOutboundLatency, new Action(_ProcessSend_c__AnonStorey.__m__0));
		}
	}
}
