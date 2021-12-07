using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008C4 RID: 2244
public class OffscreenArrowManager : ClientBehavior
{
	// Token: 0x06003896 RID: 14486 RVA: 0x001099DC File Offset: 0x00107DDC
	public void Init(GameManager gameManager, GameObject arrowPrefab, Camera playerTextureCameraPrefab)
	{
		this.gameManager = gameManager;
		foreach (PlayerController playerController in gameManager.GetPlayers())
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(arrowPrefab);
			gameObject.transform.SetParent(base.transform, false);
			OffscreenArrow component = gameObject.GetComponent<OffscreenArrow>();
			component.Init(playerController, playerTextureCameraPrefab);
			this.arrows.Add(component);
		}
		gameManager.events.Subscribe(typeof(GameEndEvent), new Events.EventHandler(this.OnGameEnded));
	}

	// Token: 0x06003897 RID: 14487 RVA: 0x00109A90 File Offset: 0x00107E90
	public override void OnDestroy()
	{
		if (this.gameManager == null)
		{
			return;
		}
		base.OnDestroy();
		this.gameManager.events.Unsubscribe(typeof(GameEndEvent), new Events.EventHandler(this.OnGameEnded));
	}

	// Token: 0x06003898 RID: 14488 RVA: 0x00109AD0 File Offset: 0x00107ED0
	private void OnGameEnded(GameEvent message)
	{
		foreach (OffscreenArrow offscreenArrow in this.arrows)
		{
			if (offscreenArrow != null)
			{
				UnityEngine.Object.Destroy(offscreenArrow.gameObject);
			}
		}
	}

	// Token: 0x040026F9 RID: 9977
	private List<OffscreenArrow> arrows = new List<OffscreenArrow>();

	// Token: 0x040026FA RID: 9978
	private GameManager gameManager;
}
