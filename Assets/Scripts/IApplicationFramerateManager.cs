// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IApplicationFramerateManager
{
	FrameSyncMode frameSyncMode
	{
		get;
		set;
	}

	int overrideTargetFramerate
	{
		get;
		set;
	}

	bool UseRenderWait
	{
		get;
	}
}
