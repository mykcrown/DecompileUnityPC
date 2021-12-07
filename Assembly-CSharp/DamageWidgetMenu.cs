using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A3B RID: 2619
public class DamageWidgetMenu : GameBehavior
{
	// Token: 0x06004CB4 RID: 19636 RVA: 0x00145288 File Offset: 0x00143688
	public override void Awake()
	{
		base.Awake();
		List<PlayerReference> playerReferences = base.gameController.currentGame.PlayerReferences;
		this.damageWidgets = new List<DamageWidget>();
		foreach (PlayerReference playerReference in playerReferences)
		{
			DamageWidget damageWidget = UnityEngine.Object.Instantiate<DamageWidget>(this.DamageWidgetPrefab);
			damageWidget.PlayerNumber = playerReference.PlayerNum;
			damageWidget.transform.SetParent(base.transform, false);
			this.damageWidgets.Add(damageWidget);
		}
	}

	// Token: 0x04003254 RID: 12884
	private List<DamageWidget> damageWidgets;

	// Token: 0x04003255 RID: 12885
	public DamageWidget DamageWidgetPrefab;
}
