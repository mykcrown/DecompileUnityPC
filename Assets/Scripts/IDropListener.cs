// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IDropListener
{
	bool AllowFastFall
	{
		get;
	}

	void OnDrop();
}
