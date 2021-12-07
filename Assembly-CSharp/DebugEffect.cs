using System;
using UnityEngine;

// Token: 0x02000431 RID: 1073
public class DebugEffect : Effect
{
	// Token: 0x06001626 RID: 5670 RVA: 0x0007900A File Offset: 0x0007740A
	public new void Init()
	{
		base.gameController.currentGame.DynamicObjects.AddDynamicObject(base.gameObject);
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x00079027 File Offset: 0x00077427
	public override void Destroy()
	{
		this.model.isDead = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
