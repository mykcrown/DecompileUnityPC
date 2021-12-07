// Decompile from assembly: Assembly-CSharp.dll

using System;

internal class SignalListenerRecord
{
	public string name;

	public Action theFunction;

	public SignalListenerRecord(string name, Action theFunction)
	{
		this.name = name;
		this.theFunction = theFunction;
	}
}
