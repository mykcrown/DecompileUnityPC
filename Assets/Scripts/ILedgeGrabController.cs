// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface ILedgeGrabController : ITickable
{
	bool IsLedgeGrabbing
	{
		get;
	}

	void OnLedgeGrabComplete();

	void ReleaseGrabbedLedge(bool unlockLedge, bool reposition);

	FixedRect GetLedgeGrabBox(HorizontalDirection facing, EnvironmentBounds bounds);
}
