// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IPlayerCursor
{
	PlayerCursorActions Actions
	{
		get;
	}

	RaycastResult[] RaycastCache
	{
		get;
		set;
	}

	GameObject LastSelectedObject
	{
		get;
		set;
	}

	Vector2 Position
	{
		get;
	}

	Vector3 PositionDelta
	{
		get;
	}

	int PointerId
	{
		get;
	}

	bool SubmitPressed
	{
		get;
	}

	bool SubmitHeld
	{
		get;
	}

	bool SubmitReleased
	{
		get;
	}

	bool CancelPressed
	{
		get;
	}

	bool AltSubmitPressed
	{
		get;
	}

	bool FaceButton3Pressed
	{
		get;
	}

	bool AltCancelPressed
	{
		get;
	}

	bool StartPressed
	{
		get;
	}

	bool Advance1Pressed
	{
		get;
	}

	bool Previous1Pressed
	{
		get;
	}

	bool Advance2Pressed
	{
		get;
	}

	bool Previous2Pressed
	{
		get;
	}

	bool RightStickUpPressed
	{
		get;
	}

	bool RightStickDownPressed
	{
		get;
	}

	bool AdvanceSelectedPressed
	{
		get;
	}

	bool PreviousSelectedPressed
	{
		get;
	}

	bool AnythingPressed
	{
		get;
	}

	bool IsHidden
	{
		get;
		set;
	}

	bool IsPaused
	{
		get;
		set;
	}

	global::CursorMode CurrentMode
	{
		get;
	}

	void ResetPosition(Vector2 vect);

	void SuppressKeyboard(bool suppress);
}
