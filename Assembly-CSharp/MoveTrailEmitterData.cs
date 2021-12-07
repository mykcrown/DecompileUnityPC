using System;

// Token: 0x02000505 RID: 1285
[Serializable]
public class MoveTrailEmitterData : IPreloadedGameAsset
{
	// Token: 0x06001BE3 RID: 7139 RVA: 0x0008D237 File Offset: 0x0008B637
	public void RegisterPreload(PreloadContext context)
	{
		this.trailData.RegisterPreload(context);
	}

	// Token: 0x04001649 RID: 5705
	public TrailEmitterData trailData;

	// Token: 0x0400164A RID: 5706
	public int startFrame;

	// Token: 0x0400164B RID: 5707
	public int endFrame;
}
