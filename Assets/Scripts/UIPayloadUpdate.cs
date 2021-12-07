// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class UIPayloadUpdate : UIEvent, IUIUpdate
{
	public GameLoadPayload payload;

	public UIPayloadUpdate(GameLoadPayload payload)
	{
		this.payload = payload;
	}
}
