// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterRendererState : RollbackStateTyped<CharacterRendererState>
{
	public ColorMode colorModeFlags;

	public bool IsVisible;

	public int overrideColorFrameCount;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static)]
	[NonSerialized]
	public AnimatingColor overrideColor;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Todo)]
	[NonSerialized]
	public Dictionary<GameObject, List<GeneratedEffect>> materialEmitters = new Dictionary<GameObject, List<GeneratedEffect>>();

	public override void CopyTo(CharacterRendererState target)
	{
		target.colorModeFlags = this.colorModeFlags;
		target.IsVisible = this.IsVisible;
		target.overrideColorFrameCount = this.overrideColorFrameCount;
		target.overrideColor = this.overrideColor;
		target.materialEmitters = this.materialEmitters;
	}
}
