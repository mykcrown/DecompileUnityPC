// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePlayerCursor : MonoBehaviour, IPlayerCursor
{
	private Vector3 lastPosition = Vector3.zero;

	private Transform theTransform;

	public PlayerCursorActions Actions
	{
		get
		{
			return null;
		}
	}

	public global::CursorMode CurrentMode
	{
		get
		{
			return global::CursorMode.Mouse;
		}
	}

	public Vector2 Position
	{
		get
		{
			return this.theTransform.position;
		}
	}

	public Vector3 PositionDelta
	{
		get
		{
			return this.theTransform.position - this.lastPosition;
		}
	}

	public int PointerId
	{
		get
		{
			return -1;
		}
	}

	public bool SubmitPressed
	{
		get
		{
			return Input.GetMouseButtonDown(0);
		}
	}

	public bool SubmitHeld
	{
		get
		{
			return Input.GetMouseButton(0);
		}
	}

	public bool SubmitReleased
	{
		get
		{
			return Input.GetMouseButtonUp(0);
		}
	}

	public bool CancelPressed
	{
		get
		{
			return false;
		}
	}

	public bool AltSubmitPressed
	{
		get
		{
			return Input.GetMouseButtonDown(1);
		}
	}

	public bool AltCancelPressed
	{
		get
		{
			return Input.GetMouseButtonDown(2);
		}
	}

	public bool Advance1Pressed
	{
		get
		{
			return false;
		}
	}

	public bool Previous1Pressed
	{
		get
		{
			return false;
		}
	}

	public bool Advance2Pressed
	{
		get
		{
			return false;
		}
	}

	public bool Previous2Pressed
	{
		get
		{
			return false;
		}
	}

	public bool FaceButton3Pressed
	{
		get
		{
			return false;
		}
	}

	public bool RightStickUpPressed
	{
		get
		{
			return false;
		}
	}

	public bool RightStickDownPressed
	{
		get
		{
			return false;
		}
	}

	public bool AdvanceSelectedPressed
	{
		get
		{
			return Input.GetAxis("Mouse ScrollWheel") > 0f;
		}
	}

	public bool PreviousSelectedPressed
	{
		get
		{
			return Input.GetAxis("Mouse ScrollWheel") < 0f;
		}
	}

	public bool AnythingPressed
	{
		get
		{
			return this.PositionDelta != Vector3.zero || this.SubmitPressed || this.CancelPressed || this.AltSubmitPressed || this.AltCancelPressed || this.Advance1Pressed || this.Previous1Pressed || this.Advance2Pressed || this.Previous2Pressed || this.FaceButton3Pressed || this.RightStickUpPressed || this.RightStickDownPressed || this.AdvanceSelectedPressed || this.PreviousSelectedPressed;
		}
	}

	public bool IsHidden
	{
		get;
		set;
	}

	public bool IsPaused
	{
		get;
		set;
	}

	public bool StartPressed
	{
		get
		{
			return false;
		}
	}

	public RaycastResult[] RaycastCache
	{
		get;
		set;
	}

	public GameObject LastSelectedObject
	{
		get;
		set;
	}

	private void Awake()
	{
		this.theTransform = base.transform;
	}

	public void Update()
	{
		this.lastPosition = this.theTransform.position;
		this.updatePosition();
	}

	public void ResetPosition(Vector2 vect)
	{
		this.updatePosition();
	}

	public void SuppressKeyboard(bool suppress)
	{
	}

	private void updatePosition()
	{
		Vector2 v = Input.mousePosition;
		if (v.x > 0f && v.x < (float)Screen.width && v.y > 0f && v.y < (float)Screen.height)
		{
			this.theTransform.position = v;
		}
	}
}
