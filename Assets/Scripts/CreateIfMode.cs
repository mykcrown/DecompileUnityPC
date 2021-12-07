// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateIfMode : GameBehavior
{
	public string SpawnTag = string.Empty;

	public List<GameMode> Modes;

	public GameObject Prefab;

	public GameObject Container;

	public override void Awake()
	{
		base.Awake();
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

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

	private void debugLog(string message)
	{
		UnityEngine.Debug.Log("SpawnTag: " + this.SpawnTag + " " + message);
	}

	private void OnDrawGizmos()
	{
		if (this.Modes.Count <= 0)
		{
			return;
		}
		Gizmos.color = GameModeUtil.GetColorForGameMode(this.Modes[0]);
		Gizmos.DrawSphere(base.transform.position, 0.1f);
	}
}
