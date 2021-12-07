// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMoveSkipAheadComponent
{
	bool ShouldSkipToFrame
	{
		get;
	}

	int SkipToFrame
	{
		get;
	}

	bool ShouldSkipToMove
	{
		get;
	}

	MoveData SkipToMove
	{
		get;
	}
}
