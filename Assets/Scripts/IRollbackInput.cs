// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IRollbackInput : IResetable
{
	InputValuesSnapshot values
	{
		get;
		set;
	}

	int frame
	{
		get;
		set;
	}

	int playerID
	{
		get;
		set;
	}

	bool Equals(IRollbackInput other);
}
