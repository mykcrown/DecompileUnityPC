// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class DisconnectUpdate : GameEvent, IUIUpdate
{
	public NetErrorCode errorCode;

	public DisconnectUpdate(NetErrorCode code)
	{
		this.errorCode = code;
	}
}
