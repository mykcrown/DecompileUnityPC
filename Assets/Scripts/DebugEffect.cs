// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class DebugEffect : Effect
{
	public new void Init()
	{
		base.gameController.currentGame.DynamicObjects.AddDynamicObject(base.gameObject);
	}

	public override void Destroy()
	{
		this.model.isDead = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
