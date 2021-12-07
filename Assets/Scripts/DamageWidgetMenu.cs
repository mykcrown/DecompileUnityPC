// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageWidgetMenu : GameBehavior
{
	private List<DamageWidget> damageWidgets;

	public DamageWidget DamageWidgetPrefab;

	public override void Awake()
	{
		base.Awake();
		List<PlayerReference> playerReferences = base.gameController.currentGame.PlayerReferences;
		this.damageWidgets = new List<DamageWidget>();
		foreach (PlayerReference current in playerReferences)
		{
			DamageWidget damageWidget = UnityEngine.Object.Instantiate<DamageWidget>(this.DamageWidgetPrefab);
			damageWidget.PlayerNumber = current.PlayerNum;
			damageWidget.transform.SetParent(base.transform, false);
			this.damageWidgets.Add(damageWidget);
		}
	}
}
