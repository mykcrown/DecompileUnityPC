// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class RespawnConfig : IPreloadedGameAsset
{
	public Fixed platformOffscreenHeight = (Fixed)8.0;

	public int platformDurationFrames = 330;

	public int dismountInvincibilityFrames = 100;

	public Fixed platformSpeed = (Fixed)8.0;

	public GameObject respawnPlatformPrefab;

	public GameObject deathEffectPrefab;

	public AudioData deathEffectSound;

	public AudioData respawnSound;

	public int respawnDelayFrames = 30;

	public void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.respawnPlatformPrefab, PreloadType.EFFECT), 4);
		context.Register(new PreloadDef(this.deathEffectPrefab, PreloadType.EFFECT), 4);
	}
}
