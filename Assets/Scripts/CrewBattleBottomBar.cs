// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewBattleBottomBar : ClientBehavior
{
	public GameObject PlayerContainerLeft;

	public GameObject PlayerContainerRight;

	public GameObject AssistContainerLeft;

	public GameObject AssistContainerRight;

	public GameObject InactiveContainer;

	public GameObject AssistBeginLeft;

	public GameObject AssistBeginRight;

	public PlayerGUI PlayerGUIPrefab;

	private bool _isActive;

	private Dictionary<PlayerNum, PlayerGUI> playerGuis = new Dictionary<PlayerNum, PlayerGUI>();

	private Dictionary<PlayerNum, PlayerEngagementState> stateCache = new Dictionary<PlayerNum, PlayerEngagementState>();

	public bool IsActive
	{
		get
		{
			return this._isActive;
		}
		set
		{
			if (this._isActive != value)
			{
				this._isActive = value;
				base.gameObject.SetActive(this._isActive);
			}
		}
	}

	public override void Awake()
	{
		base.Awake();
		if (base.events != null)
		{
			base.events.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (base.events != null)
		{
			base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		}
	}

	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		if (this.IsActive)
		{
			this.updatePlayerState(playerEngagementStateChangedEvent.playerNum, playerEngagementStateChangedEvent.team, playerEngagementStateChangedEvent.engagement);
		}
	}

	private void updatePlayerState(PlayerNum player, TeamNum team, PlayerEngagementState state)
	{
		if (this.stateCache.ContainsKey(player) && this.stateCache[player] == state)
		{
			return;
		}
		PlayerEngagementState playerEngagementState = PlayerEngagementState.None;
		if (this.stateCache.ContainsKey(player))
		{
			playerEngagementState = this.stateCache[player];
		}
		this.stateCache[player] = state;
		PlayerGUI playerGUI = this.playerGuis[player];
		Transform transform = null;
		Transform transform2 = null;
		if (state == PlayerEngagementState.Primary)
		{
			if (team == TeamNum.Team1)
			{
				transform = this.PlayerContainerLeft.transform;
			}
			else
			{
				transform = this.PlayerContainerRight.transform;
			}
		}
		else if (state == PlayerEngagementState.Temporary)
		{
			if (team == TeamNum.Team1)
			{
				transform = this.AssistContainerLeft.transform;
				transform2 = this.AssistBeginLeft.transform;
			}
			else
			{
				transform = this.AssistContainerRight.transform;
				transform2 = this.AssistBeginRight.transform;
			}
		}
		else if (playerEngagementState != PlayerEngagementState.Temporary)
		{
			transform = this.InactiveContainer.transform;
		}
		if (transform != null)
		{
			playerGUI.transform.SetParent(transform, false);
		}
		if (transform2 != null)
		{
			playerGUI.transform.position = transform2.position;
			playerGUI.Alpha = 1f;
			playerGUI.SlideToPosition(Vector3.zero, 0.35f, 0.5f);
		}
	}

	public void TickFrame()
	{
		if (!this._isActive)
		{
			return;
		}
		foreach (PlayerGUI current in this.playerGuis.Values)
		{
			current.TickFrame();
		}
	}

	public void Initialize(BattleSettings battleConfig, PlayerSelectionList players)
	{
		IEnumerator enumerator = ((IEnumerable)players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
				if (playerSelectionInfo.type != PlayerType.None && !playerSelectionInfo.isSpectator)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PlayerGUIPrefab.gameObject);
					PlayerGUI component = gameObject.GetComponent<PlayerGUI>();
					component.transform.SetParent(this.InactiveContainer.transform, false);
					component.Initialize(battleConfig, playerSelectionInfo);
					this.playerGuis[playerSelectionInfo.playerNum] = component;
					if (base.gameController.currentGame != null)
					{
						PlayerReference playerReference = base.gameController.currentGame.GetPlayerReference(playerSelectionInfo.playerNum);
						this.updatePlayerState(playerSelectionInfo.playerNum, playerSelectionInfo.team, playerReference.EngagementState);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}
}
