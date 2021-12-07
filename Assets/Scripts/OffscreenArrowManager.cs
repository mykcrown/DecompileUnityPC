// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenArrowManager : ClientBehavior
{
	private List<OffscreenArrow> arrows = new List<OffscreenArrow>();

	private GameManager gameManager;

	public void Init(GameManager gameManager, GameObject arrowPrefab, Camera playerTextureCameraPrefab)
	{
		this.gameManager = gameManager;
		foreach (PlayerController current in gameManager.GetPlayers())
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(arrowPrefab);
			gameObject.transform.SetParent(base.transform, false);
			OffscreenArrow component = gameObject.GetComponent<OffscreenArrow>();
			component.Init(current, playerTextureCameraPrefab);
			this.arrows.Add(component);
		}
		gameManager.events.Subscribe(typeof(GameEndEvent), new Events.EventHandler(this.OnGameEnded));
	}

	public override void OnDestroy()
	{
		if (this.gameManager == null)
		{
			return;
		}
		base.OnDestroy();
		this.gameManager.events.Unsubscribe(typeof(GameEndEvent), new Events.EventHandler(this.OnGameEnded));
	}

	private void OnGameEnded(GameEvent message)
	{
		foreach (OffscreenArrow current in this.arrows)
		{
			if (current != null)
			{
				UnityEngine.Object.Destroy(current.gameObject);
			}
		}
	}
}
