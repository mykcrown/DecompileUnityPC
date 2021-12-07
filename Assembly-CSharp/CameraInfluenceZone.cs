using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000383 RID: 899
public class CameraInfluenceZone : GameBehavior, ICameraInfluencer
{
	// Token: 0x1700036A RID: 874
	// (get) Token: 0x0600131E RID: 4894 RVA: 0x0006F3B5 File Offset: 0x0006D7B5
	public bool InfluencesCamera
	{
		get
		{
			return this.influenceEnabled;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x0600131F RID: 4895 RVA: 0x0006F3BD File Offset: 0x0006D7BD
	// (set) Token: 0x06001320 RID: 4896 RVA: 0x0006F3C5 File Offset: 0x0006D7C5
	public Fixed FacingInterpolation { get; set; }

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06001321 RID: 4897 RVA: 0x0006F3CE File Offset: 0x0006D7CE
	// (set) Token: 0x06001322 RID: 4898 RVA: 0x0006F3D6 File Offset: 0x0006D7D6
	public int FacingTurnaroundWait { get; set; }

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06001323 RID: 4899 RVA: 0x0006F3DF File Offset: 0x0006D7DF
	// (set) Token: 0x06001324 RID: 4900 RVA: 0x0006F3E7 File Offset: 0x0006D7E7
	public HorizontalDirection WaitingForFacingTurnaround { get; set; }

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06001325 RID: 4901 RVA: 0x0006F3F0 File Offset: 0x0006D7F0
	public HorizontalDirection Facing
	{
		get
		{
			return HorizontalDirection.None;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001326 RID: 4902 RVA: 0x0006F3F4 File Offset: 0x0006D7F4
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

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001327 RID: 4903 RVA: 0x0006F4D8 File Offset: 0x0006D8D8
	public Vector2 Position
	{
		get
		{
			Rect cameraInfluenceBox = this.CameraInfluenceBox;
			return new Vector3(cameraInfluenceBox.x + cameraInfluenceBox.width / 2f, cameraInfluenceBox.y - cameraInfluenceBox.height / 2f);
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06001328 RID: 4904 RVA: 0x0006F520 File Offset: 0x0006D920
	public Vector3 Velocity
	{
		get
		{
			return this._velocity;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x06001329 RID: 4905 RVA: 0x0006F528 File Offset: 0x0006D928
	bool ICameraInfluencer.IsFlourishMode
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x0600132A RID: 4906 RVA: 0x0006F52B File Offset: 0x0006D92B
	int ICameraInfluencer.IsDeadForFrames
	{
		get
		{
			return -1;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x0600132B RID: 4907 RVA: 0x0006F52E File Offset: 0x0006D92E
	bool ICameraInfluencer.IsZoomMode
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x0006F531 File Offset: 0x0006D931
	private void Start()
	{
		if (base.gameManager != null)
		{
			base.gameManager.CameraInfluencers.Add(this);
		}
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x0006F555 File Offset: 0x0006D955
	private void OnDrawGizmos()
	{
		GizmoUtil.GizmosDrawRectangle(this.CameraInfluenceBox, WColor.WDOrange, true);
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x0006F568 File Offset: 0x0006D968
	public void EnableCameraInfluence()
	{
		this.influenceEnabled = true;
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x0006F571 File Offset: 0x0006D971
	public void DisableCameraInfluence()
	{
		this.influenceEnabled = false;
	}

	// Token: 0x04000CB5 RID: 3253
	public Transform TopLeftCameraPoint;

	// Token: 0x04000CB6 RID: 3254
	public Transform BottomRightCameraPoint;

	// Token: 0x04000CB7 RID: 3255
	private bool influenceEnabled = true;

	// Token: 0x04000CB8 RID: 3256
	private Vector3 _velocity = default(Vector3);
}
