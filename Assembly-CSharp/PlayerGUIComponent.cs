using System;
using UnityEngine;

// Token: 0x020008D3 RID: 2259
public class PlayerGUIComponent : GameBehavior, ITickable
{
	// Token: 0x06003912 RID: 14610 RVA: 0x00109B44 File Offset: 0x00107F44
	public virtual void Initialize(PlayerNum player, Transform anchor)
	{
		this.anchor = anchor;
		this.player = player;
	}

	// Token: 0x06003913 RID: 14611 RVA: 0x00109B54 File Offset: 0x00107F54
	public virtual void TickFrame()
	{
	}

	// Token: 0x04002759 RID: 10073
	protected Transform anchor;

	// Token: 0x0400275A RID: 10074
	protected PlayerNum player;
}
