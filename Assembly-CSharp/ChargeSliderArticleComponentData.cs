using System;
using FixedPoint;

// Token: 0x020004CE RID: 1230
[Serializable]
public class ChargeSliderArticleComponentData
{
	// Token: 0x04001473 RID: 5235
	public ArticleData slideAimArticle;

	// Token: 0x04001474 RID: 5236
	public ArticleData hitArticle;

	// Token: 0x04001475 RID: 5237
	public Fixed fullChargeMaxDistance;

	// Token: 0x04001476 RID: 5238
	public Fixed spawnDistanceOffset;

	// Token: 0x04001477 RID: 5239
	public bool enableLedgeProtection;

	// Token: 0x04001478 RID: 5240
	public bool fireOnStart;

	// Token: 0x04001479 RID: 5241
	public int fireDelay;
}
