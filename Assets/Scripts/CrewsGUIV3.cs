// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrewsGUIV3 : GameBehavior, ICrewsGUI
{
	public TextMeshProUGUI CenterText;

	public GameObject Hud2v2;

	public GameObject Hud3v3;

	public CrewsHUDTeamGUI LeftTeamHud;

	public CrewsHUDTeamGUI RightTeamHud;

	public CrewsHUDPlayerList LeftPlayerList;

	public CrewsHUDPlayerList RightPlayerList;

	public Image WinningTeamBg;

	public Sprite NeutralTeamWinningSprite;

	public Sprite RedTeamWinningSprite;

	public Sprite BlueTeamWinningSprite;

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
			this.LeftTeamHud.Initialize(TeamNum.Team1);
			this.RightTeamHud.Initialize(TeamNum.Team2);
			this.LeftPlayerList.Initialize(config, dictionary[TeamNum.Team1], TeamNum.Team1, CrewsGUISide.LEFT);
			this.RightPlayerList.Initialize(config, dictionary[TeamNum.Team2], TeamNum.Team2, CrewsGUISide.RIGHT);
		}
		else
		{
			this.LeftTeamHud.Initialize(TeamNum.Team2);
			this.RightTeamHud.Initialize(TeamNum.Team1);
			this.LeftPlayerList.Initialize(config, dictionary[TeamNum.Team2], TeamNum.Team2, CrewsGUISide.LEFT);
			this.RightPlayerList.Initialize(config, dictionary[TeamNum.Team1], TeamNum.Team1, CrewsGUISide.RIGHT);
		}
		this.setTextFields();
		this.didSubscribe = true;
		base.events.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		this.players = base.gameController.currentGame.PlayerReferences;
		this.updateWinningBg();
		this.updateHud();
	}

	private void setTextFields()
	{
		this.CenterText.text = this.localization.GetText("ui.crewsHud.vs");
	}

	private void onEngagementStateChanged(GameEvent message)
	{
		this.updateWinningBg();
	}

	private void onCharacterDeath(GameEvent message)
	{
		this.updateWinningBg();
	}

	private void updateHud()
	{
		this.Hud2v2.SetActive(false);
		this.Hud3v3.SetActive(false);
		if (this.players.Count > 4)
		{
			this.Hud3v3.SetActive(true);
		}
		else
		{
			this.Hud2v2.SetActive(true);
		}
	}

	private void updateWinningBg()
	{
		int score = this.getScore((this.team1Color != UIColor.Red) ? TeamNum.Team2 : TeamNum.Team1);
		int score2 = this.getScore((this.team1Color != UIColor.Red) ? TeamNum.Team1 : TeamNum.Team2);
		this.WinningTeamBg.gameObject.SetActive(true);
		if (score == score2)
		{
			this.WinningTeamBg.GetComponent<Image>().sprite = this.NeutralTeamWinningSprite;
		}
		else if (score > score2)
		{
			this.WinningTeamBg.GetComponent<Image>().sprite = this.RedTeamWinningSprite;
		}
		else
		{
			this.WinningTeamBg.GetComponent<Image>().sprite = this.BlueTeamWinningSprite;
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

	public void TickFrame()
	{
		this.LeftPlayerList.TickFrame();
		this.RightPlayerList.TickFrame();
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
