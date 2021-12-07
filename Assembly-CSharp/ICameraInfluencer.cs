using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000385 RID: 901
public interface ICameraInfluencer
{
	// Token: 0x1700037C RID: 892
	// (get) Token: 0x0600133F RID: 4927
	bool InfluencesCamera { get; }

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06001340 RID: 4928
	int IsDeadForFrames { get; }

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06001341 RID: 4929
	bool IsFlourishMode { get; }

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06001342 RID: 4930
	bool IsZoomMode { get; }

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06001343 RID: 4931
	Rect CameraInfluenceBox { get; }

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06001344 RID: 4932
	Vector2 Position { get; }

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06001345 RID: 4933
	HorizontalDirection Facing { get; }

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06001346 RID: 4934
	// (set) Token: 0x06001347 RID: 4935
	Fixed FacingInterpolation { get; set; }

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06001348 RID: 4936
	// (set) Token: 0x06001349 RID: 4937
	int FacingTurnaroundWait { get; set; }

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x0600134A RID: 4938
	// (set) Token: 0x0600134B RID: 4939
	HorizontalDirection WaitingForFacingTurnaround { get; set; }
}
