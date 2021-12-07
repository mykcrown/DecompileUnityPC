// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class CameraInfluenceZone : GameBehavior, ICameraInfluencer
{
	public Transform TopLeftCameraPoint;

	public Transform BottomRightCameraPoint;

	private bool influenceEnabled = true;

	private Vector3 _velocity = default(Vector3);

	bool ICameraInfluencer.IsFlourishMode
	{
		get
		{
			return false;
		}
	}

	int ICameraInfluencer.IsDeadForFrames
	{
		get
		{
			return -1;
		}
	}

	bool ICameraInfluencer.IsZoomMode
	{
		get
		{
			return false;
		}
	}

	public bool InfluencesCamera
	{
		get
		{
			return this.influenceEnabled;
		}
	}

	public Fixed FacingInterpolation
	{
		get;
		set;
	}

	public int FacingTurnaroundWait
	{
		get;
		set;
	}

	public HorizontalDirection WaitingForFacingTurnaround
	{
		get;
		set;
	}

	public HorizontalDirection Facing
	{
		get
		{
			return HorizontalDirection.None;
		}
	}

	public Rect CameraInfluenceBox
	{
		get
		{
			if (this.TopLeftCameraPoint != null && this.BottomRightCameraPoint != null)
			{
				return new Rect(this.TopLeftCameraPoint.position.x, this.TopLeftCameraPoint.position.y, this.BottomRightCameraPoint.position.x - this.TopLeftCameraPoint.position.x, this.TopLeftCameraPoint.position.y - this.BottomRightCameraPoint.position.y);
			}
			return new Rect(base.transform.position.x, base.transform.position.y, 2f, 2f);
		}
	}

	public Vector2 Position
	{
		get
		{
			Rect cameraInfluenceBox = this.CameraInfluenceBox;
			return new Vector3(cameraInfluenceBox.x + cameraInfluenceBox.width / 2f, cameraInfluenceBox.y - cameraInfluenceBox.height / 2f);
		}
	}

	public Vector3 Velocity
	{
		get
		{
			return this._velocity;
		}
	}

	private void Start()
	{
		if (base.gameManager != null)
		{
			base.gameManager.CameraInfluencers.Add(this);
		}
	}

	private void OnDrawGizmos()
	{
		GizmoUtil.GizmosDrawRectangle(this.CameraInfluenceBox, WColor.WDOrange, true);
	}

	public void EnableCameraInfluence()
	{
		this.influenceEnabled = true;
	}

	public void DisableCameraInfluence()
	{
		this.influenceEnabled = false;
	}
}
