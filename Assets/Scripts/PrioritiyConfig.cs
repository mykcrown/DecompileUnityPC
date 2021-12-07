// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class PrioritiyConfig : IPreloadedGameAsset
{
	public Fixed priorityThreshold = (Fixed)10.0;

	public GameObject clankParticle;

	public AudioData clankSound;

	public void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.clankParticle, PreloadType.EFFECT), 4);
	}
}
