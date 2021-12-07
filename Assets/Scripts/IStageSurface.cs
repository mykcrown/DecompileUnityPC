// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IStageSurface : ITickable, IMovingObject
{
	bool IsPlatform
	{
		get;
	}

	void UpdateCollisionData();
}
