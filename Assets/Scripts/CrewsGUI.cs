// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrewsGUI : GameBehavior, ICrewsGUI
{
	public CrewsHUDTeamGUI leftTeamHud;

	public CrewsHUDTeamGUI rightTeamHud;

	public CrewsHUDPlayerList leftPlayerList;

	public CrewsHUDPlayerList rightPlayerList;

	public GameObject WinningTeamBg;

	public Sprite blueTeamWinningSprite;

	public Sprite redTeamWinningSprite;

	public Sprite neutralTeamWinningSprite;

	public TextMeshProUGUI centerText;

	private UIColor team1Color;

	private bool didSubscribe;

	private List<PlayerReference> players = new List<PlayerReference>();

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	public void Initialize(BattleSettings config, PlayerSelectionList players)
	{
		Dictionary<TeamNum, List<PlayerSelectionInfo>> dictionary = new Dictionary<TeamNum, List<PlayerSelectionInfo>>();
		IEnumerator enumerator = ((IEnumerable)players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
				if (!dictionary.ContainsKey(playerSelectionInfo.team))
				{
					dictionary[playerSelectionInfo.team] = new List<PlayerSelectionInfo>();
				}
				dictionary[playerSelectionInfo.team].Add(playerSelectionInfo);
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
		this.team1Color = PlayerUtil.GetUIColorFromTeam(TeamNum.Team1);
		if (this.team1Color == UIColor.Red)
		{
			this.leftTeamHud.Initialize(TeamNum.Team1);
			this.rightTeamHud.Initialize(TeamNum.Team2);
			this.leftPlayerList.Initialize(config, dictionary[TeamNum.Team1], TeamNum.Team1, CrewsGUISide.LEFT);
			this.rightPlayerList.Initialize(config, dictionary[TeamNum.Team2], TeamNum.Team2, CrewsGUISide.RIGHT);
		}
		else
		{
			this.leftTeamHud.Initialize(TeamNum.Team2);
			this.rightTeamHud.Initialize(TeamNum.Team1);
			this.leftPlayerList.Initialize(config, dictionary[TeamNum.Team2], TeamNum.Team2, CrewsGUISide.LEFT);
			this.rightPlayerList.Initialize(config, dictionary[TeamNum.Team1], TeamNum.Team1, CrewsGUISide.RIGHT);
		}
		this.setTextFields();
		this.didSubscribe = true;
		base.events.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		this.players = base.gameController.currentGame.PlayerReferences;
		this.updateWinningBg();
	}

	private void setTextFields()
	{
		this.centerText.text = this.localization.GetText("ui.crewsHud.vs");
	}

	public void TickFrame()
	{
		this.leftPlayerList.TickFrame();
		this.rightPlayerList.TickFrame();
	}

	private void onEngagementStateChanged(GameEvent message)
	{
		this.updateWinningBg();
	}

	private void onCharacterDeath(GameEvent message)
	{
		this.updateWinningBg();
	}

	private void updateWinningBg()
	{
		int score = this.getScore((this.team1Color != UIColor.Red) ? TeamNum.Team2 : TeamNum.Team1);
		int score2 = this.getScore((this.team1Color != UIColor.Red) ? TeamNum.Team1 : TeamNum.Team2);
		this.WinningTeamBg.SetActive(true);
		if (score == score2)
		{
			this.WinningTeamBg.GetComponent<Image>().sprite = this.neutralTeamWinningSprite;
		}
		else if (score > score2)
		{
			this.WinningTeamBg.GetComponent<Image>().sprite = this.redTeamWinningSprite;
		}
		else
		{
			this.WinningTeamBg.GetComponent<Image>().sprite = this.blueTeamWinningSprite;
		}
	}

	private int getScore(TeamNum team)
	{
		int num = 0;
		for (int i = 0; i < this.players.Count; i++)
		{
			PlayerReference playerReference = this.players[i];
			if (playerReference.Team == team)
			{
				num += playerReference.Lives;
			}
		}
		return num;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.didSubscribe)
		{
			base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
			base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		}
	}
}
