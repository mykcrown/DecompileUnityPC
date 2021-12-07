// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public interface ICameraInfluencer
{
	bool InfluencesCamera
	{
		get;
	}

	int IsDeadForFrames
	{
		get;
	}

	bool IsFlourishMode
	{
		get;
	}

	bool IsZoomMode
	{
		get;
	}

	Rect CameraInfluenceBox
	{
		get;
	}

	Vector2 Position
	{
		get;
	}

	HorizontalDirection Facing
	{
		get;
	}

	Fixed FacingInterpolation
	{
		get;
		set;
	}

	int FacingTurnaroundWait
	{
		get;
		set;
	}

	HorizontalDirection WaitingForFacingTurnaround
	{
		get;
		set;
	}
}
