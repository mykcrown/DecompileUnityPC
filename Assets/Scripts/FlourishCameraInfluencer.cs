// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class FlourishCameraInfluencer : ICameraInfluencer
{
	private Vector2 position = default(Vector2);

	private float width = 3f;

	private float height = 3f;

	bool ICameraInfluencer.IsFlourishMode
	{
		get
		{
			return true;
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

	public Rect CameraInfluenceBox
	{
		get
		{
			return new Rect(this.position.x - this.width / 2f, this.position.y + this.height / 2f, this.width, this.height);
		}
	}

	public HorizontalDirection Facing
	{
		get
		{
			return HorizontalDirection.Left;
		}
	}

	public Fixed FacingInterpolation
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public int FacingTurnaroundWait
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public bool InfluencesCamera
	{
		get
		{
			return true;
		}
	}

	public Vector2 Position
	{
		get
		{
			return this.position;
		}
	}

	public HorizontalDirection WaitingForFacingTurnaround
	{
		get
		{
			return HorizontalDirection.Left;
		}
		set
		{
		}
	}

	public void SetHighlightPosition(Vector2 position, float width, float height)
	{
		this.position = position;
		this.width = width;
		this.height = height;
	}
}
