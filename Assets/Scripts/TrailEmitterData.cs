// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class TrailEmitterData : MonoBehaviour, IPreloadedGameAsset
{
	public GameObject particlePrefab;

	public bool distanceMode;

	public float emitDistance;

	public int emitFrequencyFrames;

	public int particleLifespanFrames;

	public int preloadCount = 64;

	public void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.particlePrefab, PreloadType.EFFECT), this.preloadCount);
	}
}
