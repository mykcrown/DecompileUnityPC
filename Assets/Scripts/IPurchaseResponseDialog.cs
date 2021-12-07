// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPurchaseResponseDialog
{
	Action CloseCallback
	{
		set;
	}

	void Close();

	void ShowError(string title, string body);
}
