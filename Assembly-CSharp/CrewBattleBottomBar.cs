using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008AD RID: 2221
public class CrewBattleBottomBar : ClientBehavior
{
	// Token: 0x060037C4 RID: 14276 RVA: 0x00105883 File Offset: 0x00103C83
	public override void Awake()
	{
		base.Awake();
		if (base.events != null)
		{
			base.events.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		}
	}

	// Token: 0x060037C5 RID: 14277 RVA: 0x001058B7 File Offset: 0x00103CB7
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (base.events != null)
		{
			base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		}
	}

	// Token: 0x17000D8C RID: 3468
	// (get) Token: 0x060037C6 RID: 14278 RVA: 0x001058EB File Offset: 0x00103CEB
	// (set) Token: 0x060037C7 RID: 14279 RVA: 0x001058F3 File Offset: 0x00103CF3
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

	// Token: 0x060037C8 RID: 14280 RVA: 0x0010591C File Offset: 0x00103D1C
	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		if (this.IsActive)
		{
			this.updatePlayerState(playerEngagementStateChangedEvent.playerNum, playerEngagementStateChangedEvent.team, playerEngagementStateChangedEvent.engagement);
		}
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x00105954 File Offset: 0x00103D54
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

	// Token: 0x060037CA RID: 14282 RVA: 0x00105AA4 File Offset: 0x00103EA4
	public void TickFrame()
	{
		if (!this._isActive)
		{
			return;
		}
		foreach (PlayerGUI playerGUI in this.playerGuis.Values)
		{
			playerGUI.TickFrame();
		}
	}

	// Token: 0x060037CB RID: 14283 RVA: 0x00105B10 File Offset: 0x00103F10
	public void Initialize(BattleSettings battleConfig, PlayerSelectionList players)
	{
		IEnumerator enumerator = ((IEnumerable)players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
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

	// Token: 0x040025F8 RID: 9720
	public GameObject PlayerContainerLeft;

	// Token: 0x040025F9 RID: 9721
	public GameObject PlayerContainerRight;

	// Token: 0x040025FA RID: 9722
	public GameObject AssistContainerLeft;

	// Token: 0x040025FB RID: 9723
	public GameObject AssistContainerRight;

	// Token: 0x040025FC RID: 9724
	public GameObject InactiveContainer;

	// Token: 0x040025FD RID: 9725
	public GameObject AssistBeginLeft;

	// Token: 0x040025FE RID: 9726
	public GameObject AssistBeginRight;

	// Token: 0x040025FF RID: 9727
	public PlayerGUI PlayerGUIPrefab;

	// Token: 0x04002600 RID: 9728
	private bool _isActive;

	// Token: 0x04002601 RID: 9729
	private Dictionary<PlayerNum, PlayerGUI> playerGuis = new Dictionary<PlayerNum, PlayerGUI>();

	// Token: 0x04002602 RID: 9730
	private Dictionary<PlayerNum, PlayerEngagementState> stateCache = new Dictionary<PlayerNum, PlayerEngagementState>();
}
