// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMoveDelegate
{
	MoveData Data
	{
		get;
	}

	MoveModel Model
	{
		get;
	}

	int TotalFrames
	{
		get;
	}
}
