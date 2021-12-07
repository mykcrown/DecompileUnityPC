using System;
using FixedPoint;

// Token: 0x02000358 RID: 856
public interface IArticleDelegate
{
	// Token: 0x17000349 RID: 841
	// (get) Token: 0x0600126C RID: 4716
	ArticleData Data { get; }

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x0600126D RID: 4717
	ArticleModel Model { get; }

	// Token: 0x0600126E RID: 4718
	IEvents getEvents();

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x0600126F RID: 4719
	Vector3F Position { get; }

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06001270 RID: 4720
	HorizontalDirection Facing { get; }

	// Token: 0x06001271 RID: 4721
	bool PerformBoundCast(AbsoluteDirection boundPoint, Fixed castDist, Vector2F castDirection, int castMask, out RaycastHitData hit);
}
