// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class StageCameraInfluencer : StageProp, ICameraInfluencer
{
	public float Width;

	public float Height;

	public Vector2 PositionOffset;

	private StageCameraInfluencerModel model = new StageCameraInfluencerModel();

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

	public override bool IsSimulation
	{
		get
		{
			return false;
		}
	}

	public bool IsActive
	{
		get
		{
			return this.model.IsActive;
		}
	}

	public Rect CameraInfluenceBox
	{
		get
		{
			return new Rect(base.transform.position.x + this.PositionOffset.x - this.Width * 0.5f, base.transform.position.y + this.PositionOffset.y - this.Height * 0.5f, this.Width, this.Height);
		}
	}

	public Vector2 Position
	{
		get
		{
			Rect cameraInfluenceBox = this.CameraInfluenceBox;
			return new Vector2(cameraInfluenceBox.x + cameraInfluenceBox.width / 2f, cameraInfluenceBox.y - cameraInfluenceBox.height / 2f);
		}
	}

	public HorizontalDirection Facing
	{
		get
		{
			return HorizontalDirection.None;
		}
	}

	public bool InfluencesCamera
	{
		get
		{
			return this.IsActive;
		}
	}

	public Vector3 Velocity
	{
		get
		{
			return Vector3.zero;
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

	public override void Init()
	{
		base.Init();
		if (base.gameManager != null)
		{
			base.gameManager.CameraInfluencers.Add(this);
		}
		this.model.shouldValidate = this.IsSimulation;
	}

	private void OnDrawGizmos()
	{
		GizmoUtil.GizmosDrawRectangle(new Rect(base.transform.position.x + this.PositionOffset.x - this.Width * 0.5f, base.transform.position.y + this.PositionOffset.y - this.Height * 0.5f, this.Width, this.Height), Color.white, false);
	}

	public void SetIsActive(bool active, int duration)
	{
		if (duration > 0)
		{
			this.model.IsActive = active;
			this.model.ToggleFrame = base.gameManager.Frame + duration;
		}
		else if (duration == 0)
		{
			this.model.IsActive = !active;
			this.model.ToggleFrame = -1;
		}
		else
		{
			this.model.IsActive = active;
			this.model.ToggleFrame = -1;
		}
	}

	public override void TickFrame()
	{
		if (base.gameManager.Frame == this.model.ToggleFrame)
		{
			this.SetIsActive(!this.IsActive, -1);
		}
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<StageCameraInfluencerModel>(this.model));
		return true;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StageCameraInfluencerModel>(ref this.model);
		return true;
	}
}
