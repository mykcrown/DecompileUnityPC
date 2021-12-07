// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class PlayerGUIComponent : GameBehavior, ITickable
{
	protected Transform anchor;

	protected PlayerNum player;

	public virtual void Initialize(PlayerNum player, Transform anchor)
	{
		this.anchor = anchor;
		this.player = player;
	}

	public virtual void TickFrame()
	{
	}
}
