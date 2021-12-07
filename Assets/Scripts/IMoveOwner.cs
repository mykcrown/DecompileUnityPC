// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMoveOwner
{
	MoveData MoveData
	{
		get;
	}

	bool MoveIsValid
	{
		get;
	}

	int InternalFrame
	{
		get;
	}
}
