// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class MoreOptionsWasClosed : UIEvent, IUIRequest
{
	public bool revertChanges;

	public MoreOptionsWasClosed(bool revertChanges)
	{
		this.revertChanges = revertChanges;
	}
}
