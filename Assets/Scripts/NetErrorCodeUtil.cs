// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class NetErrorCodeUtil
{
	private static Dictionary<NetErrorCode, string> errorCodeText = new Dictionary<NetErrorCode, string>
	{
		{
			NetErrorCode.Unknown,
			"Unknown error"
		},
		{
			NetErrorCode.ClientDesync,
			"dialog.OnlineGame.Error.Desync.body"
		},
		{
			NetErrorCode.ClientDisconnected,
			"dialog.OnlineGame.Error.Disconnect.body"
		},
		{
			NetErrorCode.ClientAbandoned,
			"dialog.OnlineGame.Error.Abandoned.body"
		},
		{
			NetErrorCode.ClientAFKSelection,
			"dialog.OnlineGame.Error.AFKSelection.body"
		},
		{
			NetErrorCode.VersionMismatch,
			"dialog.OnlineGame.Error.Mismatch.body"
		},
		{
			NetErrorCode.ClientInExistingMatch,
			"dialog.OnlineGame.Error.InExistingMatch.body"
		},
		{
			NetErrorCode.ClientQueueFailure,
			"dialog.OnlineGame.Error.QueueFailure.body"
		},
		{
			NetErrorCode.ClientServerMatchFailure,
			"dialog.OnlineGame.Error.ServerMatchFailure.body"
		},
		{
			NetErrorCode.ClientUDPFailure,
			"dialog.OnlineGame.Error.UDPFailure.body"
		}
	};

	public static string GetErrorCodeString(NetErrorCode code)
	{
		if (NetErrorCodeUtil.errorCodeText.ContainsKey(code))
		{
			return NetErrorCodeUtil.errorCodeText[code];
		}
		return "Unrecognized error";
	}

	public static bool ShouldReplaceError(NetErrorCode currentCode, NetErrorCode newCode)
	{
		return currentCode != newCode && (currentCode == NetErrorCode.None || currentCode == NetErrorCode.ClientServerMatchFailure);
	}
}
