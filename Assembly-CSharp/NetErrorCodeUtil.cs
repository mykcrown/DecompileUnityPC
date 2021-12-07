using System;
using System.Collections.Generic;

// Token: 0x020007D9 RID: 2009
public class NetErrorCodeUtil
{
	// Token: 0x060031E3 RID: 12771 RVA: 0x000F23F3 File Offset: 0x000F07F3
	public static string GetErrorCodeString(NetErrorCode code)
	{
		if (NetErrorCodeUtil.errorCodeText.ContainsKey(code))
		{
			return NetErrorCodeUtil.errorCodeText[code];
		}
		return "Unrecognized error";
	}

	// Token: 0x060031E4 RID: 12772 RVA: 0x000F2416 File Offset: 0x000F0816
	public static bool ShouldReplaceError(NetErrorCode currentCode, NetErrorCode newCode)
	{
		return currentCode != newCode && (currentCode == NetErrorCode.None || currentCode == NetErrorCode.ClientServerMatchFailure);
	}

	// Token: 0x04002301 RID: 8961
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
}
