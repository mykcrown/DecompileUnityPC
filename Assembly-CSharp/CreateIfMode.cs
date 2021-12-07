using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000497 RID: 1175
public class CreateIfMode : GameBehavior
{
	// Token: 0x060019B3 RID: 6579 RVA: 0x00085271 File Offset: 0x00083671
	public override void Awake()
	{
		base.Awake();
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x0008529A File Offset: 0x0008369A
	public override void OnDestroy()
	{
		base.OnDestroy();
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x000852C4 File Offset: 0x000836C4
	private void onGameInit(GameEvent message)
	{
		if (this.Container == null)
		{
			this.debugLog("Error: Attempt to spawn object without a contianer");
			return;
		}
		if (this.Prefab == null)
		{
			this.debugLog("Error: Attempt to spawn object without a prefab");
			return;
		}
		if (this.Modes.Contains(base.gameManager.BattleSettings.mode))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefab);
			if (gameObject != null)
			{
				gameObject.transform.SetParent(this.Container.transform, false);
			}
			else
			{
				this.debugLog("Failed to instantiate object");
			}
		}
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x0008536A File Offset: 0x0008376A
	private void debugLog(string message)
	{
		Debug.Log("SpawnTag: " + this.SpawnTag + " " + message);
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x00085387 File Offset: 0x00083787
	private void OnDrawGizmos()
	{
		if (this.Modes.Count <= 0)
		{
			return;
		}
		Gizmos.color = GameModeUtil.GetColorForGameMode(this.Modes[0]);
		Gizmos.DrawSphere(base.transform.position, 0.1f);
	}

	// Token: 0x04001344 RID: 4932
	public string SpawnTag = string.Empty;

	// Token: 0x04001345 RID: 4933
	public List<GameMode> Modes;

	// Token: 0x04001346 RID: 4934
	public GameObject Prefab;

	// Token: 0x04001347 RID: 4935
	public GameObject Container;
}
