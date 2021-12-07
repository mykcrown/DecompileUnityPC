using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004F4 RID: 1268
[Serializable]
public class CreateArticleAction : IPreloadedGameAsset
{
	// Token: 0x06001BA9 RID: 7081 RVA: 0x0008BFD7 File Offset: 0x0008A3D7
	public void RegisterPreload(PreloadContext context)
	{
		if (this.data != null)
		{
			this.data.RegisterPreload(context);
		}
	}

	// Token: 0x0400155E RID: 5470
	public int fireFrame;

	// Token: 0x0400155F RID: 5471
	public BodyPart fireBodyPart;

	// Token: 0x04001560 RID: 5472
	public int fireAngle;

	// Token: 0x04001561 RID: 5473
	[FormerlySerializedAs("projectile")]
	public ArticleData data;

	// Token: 0x04001562 RID: 5474
	public float speed;

	// Token: 0x04001563 RID: 5475
	public MovementType movementType;

	// Token: 0x04001564 RID: 5476
	public Vector2 inheritSpeedAmount;

	// Token: 0x04001565 RID: 5477
	public Vector3 worldOffset;
}
