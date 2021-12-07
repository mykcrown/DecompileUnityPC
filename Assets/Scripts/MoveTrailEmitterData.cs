// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class MoveTrailEmitterData : IPreloadedGameAsset
{
	public TrailEmitterData trailData;

	public int startFrame;

	public int endFrame;

	public void RegisterPreload(PreloadContext context)
	{
		this.trailData.RegisterPreload(context);
	}
}
