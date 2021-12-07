// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class HologramData : ScriptableObject
{
	public Texture2D texture;

	public bool hasOverrideVFX;

	public ParticleData overrideVFX;

	public void RegisterPreload(PreloadContext context)
	{
		if (this.hasOverrideVFX && this.overrideVFX != null)
		{
			context.Register(new PreloadDef(this.overrideVFX.prefab, PreloadType.EFFECT), 16);
		}
	}
}
