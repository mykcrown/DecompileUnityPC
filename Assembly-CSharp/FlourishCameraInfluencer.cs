using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000384 RID: 900
public class FlourishCameraInfluencer : ICameraInfluencer
{
	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06001331 RID: 4913 RVA: 0x0006F5B4 File Offset: 0x0006D9B4
	public Rect CameraInfluenceBox
	{
		get
		{
			return new Rect(this.position.x - this.width / 2f, this.position.y + this.height / 2f, this.width, this.height);
		}
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x0006F602 File Offset: 0x0006DA02
	public void SetHighlightPosition(Vector2 position, float width, float height)
	{
		this.position = position;
		this.width = width;
		this.height = height;
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06001333 RID: 4915 RVA: 0x0006F619 File Offset: 0x0006DA19
	public HorizontalDirection Facing
	{
		get
		{
			return HorizontalDirection.Left;
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06001334 RID: 4916 RVA: 0x0006F61C File Offset: 0x0006DA1C
	// (set) Token: 0x06001335 RID: 4917 RVA: 0x0006F624 File Offset: 0x0006DA24
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

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06001336 RID: 4918 RVA: 0x0006F626 File Offset: 0x0006DA26
	// (set) Token: 0x06001337 RID: 4919 RVA: 0x0006F629 File Offset: 0x0006DA29
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

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06001338 RID: 4920 RVA: 0x0006F62B File Offset: 0x0006DA2B
	public bool InfluencesCamera
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06001339 RID: 4921 RVA: 0x0006F62E File Offset: 0x0006DA2E
	public Vector2 Position
	{
		get
		{
			return this.position;
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x0600133A RID: 4922 RVA: 0x0006F636 File Offset: 0x0006DA36
	bool ICameraInfluencer.IsFlourishMode
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x0600133B RID: 4923 RVA: 0x0006F639 File Offset: 0x0006DA39
	int ICameraInfluencer.IsDeadForFrames
	{
		get
		{
			return -1;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x0600133C RID: 4924 RVA: 0x0006F63C File Offset: 0x0006DA3C
	bool ICameraInfluencer.IsZoomMode
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x0600133D RID: 4925 RVA: 0x0006F63F File Offset: 0x0006DA3F
	// (set) Token: 0x0600133E RID: 4926 RVA: 0x0006F642 File Offset: 0x0006DA42
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

	// Token: 0x04000CBC RID: 3260
	private Vector2 position = default(Vector2);

	// Token: 0x04000CBD RID: 3261
	private float width = 3f;

	// Token: 0x04000CBE RID: 3262
	private float height = 3f;
}
