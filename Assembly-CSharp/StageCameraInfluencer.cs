using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000640 RID: 1600
public class StageCameraInfluencer : StageProp, ICameraInfluencer
{
	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x06002729 RID: 10025 RVA: 0x000BF61A File Offset: 0x000BDA1A
	public override bool IsSimulation
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700099C RID: 2460
	// (get) Token: 0x0600272A RID: 10026 RVA: 0x000BF61D File Offset: 0x000BDA1D
	public bool IsActive
	{
		get
		{
			return this.model.IsActive;
		}
	}

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x0600272B RID: 10027 RVA: 0x000BF62A File Offset: 0x000BDA2A
	bool ICameraInfluencer.IsFlourishMode
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000999 RID: 2457
	// (get) Token: 0x0600272C RID: 10028 RVA: 0x000BF62D File Offset: 0x000BDA2D
	int ICameraInfluencer.IsDeadForFrames
	{
		get
		{
			return -1;
		}
	}

	// Token: 0x1700099A RID: 2458
	// (get) Token: 0x0600272D RID: 10029 RVA: 0x000BF630 File Offset: 0x000BDA30
	bool ICameraInfluencer.IsZoomMode
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700099D RID: 2461
	// (get) Token: 0x0600272E RID: 10030 RVA: 0x000BF634 File Offset: 0x000BDA34
	public Rect CameraInfluenceBox
	{
		get
		{
			return new Rect(base.transform.position.x + this.PositionOffset.x - this.Width * 0.5f, base.transform.position.y + this.PositionOffset.y - this.Height * 0.5f, this.Width, this.Height);
		}
	}

	// Token: 0x1700099E RID: 2462
	// (get) Token: 0x0600272F RID: 10031 RVA: 0x000BF6AC File Offset: 0x000BDAAC
	public Vector2 Position
	{
		get
		{
			Rect cameraInfluenceBox = this.CameraInfluenceBox;
			return new Vector2(cameraInfluenceBox.x + cameraInfluenceBox.width / 2f, cameraInfluenceBox.y - cameraInfluenceBox.height / 2f);
		}
	}

	// Token: 0x1700099F RID: 2463
	// (get) Token: 0x06002730 RID: 10032 RVA: 0x000BF6EF File Offset: 0x000BDAEF
	public HorizontalDirection Facing
	{
		get
		{
			return HorizontalDirection.None;
		}
	}

	// Token: 0x170009A0 RID: 2464
	// (get) Token: 0x06002731 RID: 10033 RVA: 0x000BF6F2 File Offset: 0x000BDAF2
	public bool InfluencesCamera
	{
		get
		{
			return this.IsActive;
		}
	}

	// Token: 0x170009A1 RID: 2465
	// (get) Token: 0x06002732 RID: 10034 RVA: 0x000BF6FA File Offset: 0x000BDAFA
	public Vector3 Velocity
	{
		get
		{
			return Vector3.zero;
		}
	}

	// Token: 0x170009A2 RID: 2466
	// (get) Token: 0x06002733 RID: 10035 RVA: 0x000BF701 File Offset: 0x000BDB01
	// (set) Token: 0x06002734 RID: 10036 RVA: 0x000BF709 File Offset: 0x000BDB09
	public Fixed FacingInterpolation { get; set; }

	// Token: 0x170009A3 RID: 2467
	// (get) Token: 0x06002735 RID: 10037 RVA: 0x000BF712 File Offset: 0x000BDB12
	// (set) Token: 0x06002736 RID: 10038 RVA: 0x000BF71A File Offset: 0x000BDB1A
	public int FacingTurnaroundWait { get; set; }

	// Token: 0x170009A4 RID: 2468
	// (get) Token: 0x06002737 RID: 10039 RVA: 0x000BF723 File Offset: 0x000BDB23
	// (set) Token: 0x06002738 RID: 10040 RVA: 0x000BF72B File Offset: 0x000BDB2B
	public HorizontalDirection WaitingForFacingTurnaround { get; set; }

	// Token: 0x06002739 RID: 10041 RVA: 0x000BF734 File Offset: 0x000BDB34
	public override void Init()
	{
		base.Init();
		if (base.gameManager != null)
		{
			base.gameManager.CameraInfluencers.Add(this);
		}
		this.model.shouldValidate = this.IsSimulation;
	}

	// Token: 0x0600273A RID: 10042 RVA: 0x000BF770 File Offset: 0x000BDB70
	private void OnDrawGizmos()
	{
		GizmoUtil.GizmosDrawRectangle(new Rect(base.transform.position.x + this.PositionOffset.x - this.Width * 0.5f, base.transform.position.y + this.PositionOffset.y - this.Height * 0.5f, this.Width, this.Height), Color.white, false);
	}

	// Token: 0x0600273B RID: 10043 RVA: 0x000BF7F4 File Offset: 0x000BDBF4
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

	// Token: 0x0600273C RID: 10044 RVA: 0x000BF86F File Offset: 0x000BDC6F
	public override void TickFrame()
	{
		if (base.gameManager.Frame == this.model.ToggleFrame)
		{
			this.SetIsActive(!this.IsActive, -1);
		}
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x000BF89C File Offset: 0x000BDC9C
	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<StageCameraInfluencerModel>(this.model));
		return true;
	}

	// Token: 0x0600273E RID: 10046 RVA: 0x000BF8B8 File Offset: 0x000BDCB8
	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StageCameraInfluencerModel>(ref this.model);
		return true;
	}

	// Token: 0x04001CB3 RID: 7347
	public float Width;

	// Token: 0x04001CB4 RID: 7348
	public float Height;

	// Token: 0x04001CB5 RID: 7349
	public Vector2 PositionOffset;

	// Token: 0x04001CB9 RID: 7353
	private StageCameraInfluencerModel model = new StageCameraInfluencerModel();
}
