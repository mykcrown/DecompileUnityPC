using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008B0 RID: 2224
public class CrewsGUIV3 : GameBehavior, ICrewsGUI
{
	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x060037DE RID: 14302 RVA: 0x001060BB File Offset: 0x001044BB
	// (set) Token: 0x060037DF RID: 14303 RVA: 0x001060C3 File Offset: 0x001044C3
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x060037E0 RID: 14304 RVA: 0x001060CC File Offset: 0x001044CC
	public void Initialize(BattleSettings config, PlayerSelectionList players)
	{
		Dictionary<TeamNum, List<PlayerSelectionInfo>> dictionary = new Dictionary<TeamNum, List<PlayerSelectionInfo>>();
		IEnumerator enumerator = ((IEnumerable)players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
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

	// Token: 0x060037E1 RID: 14305 RVA: 0x00106274 File Offset: 0x00104674
	private void setTextFields()
	{
		this.CenterText.text = this.localization.GetText("ui.crewsHud.vs");
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x00106291 File Offset: 0x00104691
	private void onEngagementStateChanged(GameEvent message)
	{
		this.updateWinningBg();
	}

	// Token: 0x060037E3 RID: 14307 RVA: 0x00106299 File Offset: 0x00104699
	private void onCharacterDeath(GameEvent message)
	{
		this.updateWinningBg();
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x001062A4 File Offset: 0x001046A4
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

	// Token: 0x060037E5 RID: 14309 RVA: 0x001062F8 File Offset: 0x001046F8
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

	// Token: 0x060037E6 RID: 14310 RVA: 0x001063A4 File Offset: 0x001047A4
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

	// Token: 0x060037E7 RID: 14311 RVA: 0x001063F2 File Offset: 0x001047F2
	public void TickFrame()
	{
		this.LeftPlayerList.TickFrame();
		this.RightPlayerList.TickFrame();
	}

	// Token: 0x17000D90 RID: 3472
	// (get) Token: 0x060037E8 RID: 14312 RVA: 0x0010640A File Offset: 0x0010480A
	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x060037E9 RID: 14313 RVA: 0x00106414 File Offset: 0x00104814
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.didSubscribe)
		{
			base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
			base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		}
	}

	// Token: 0x04002617 RID: 9751
	public TextMeshProUGUI CenterText;

	// Token: 0x04002618 RID: 9752
	public GameObject Hud2v2;

	// Token: 0x04002619 RID: 9753
	public GameObject Hud3v3;

	// Token: 0x0400261A RID: 9754
	public CrewsHUDTeamGUI LeftTeamHud;

	// Token: 0x0400261B RID: 9755
	public CrewsHUDTeamGUI RightTeamHud;

	// Token: 0x0400261C RID: 9756
	public CrewsHUDPlayerList LeftPlayerList;

	// Token: 0x0400261D RID: 9757
	public CrewsHUDPlayerList RightPlayerList;

	// Token: 0x0400261E RID: 9758
	public Image WinningTeamBg;

	// Token: 0x0400261F RID: 9759
	public Sprite NeutralTeamWinningSprite;

	// Token: 0x04002620 RID: 9760
	public Sprite RedTeamWinningSprite;

	// Token: 0x04002621 RID: 9761
	public Sprite BlueTeamWinningSprite;

	// Token: 0x04002622 RID: 9762
	private UIColor team1Color;

	// Token: 0x04002623 RID: 9763
	private bool didSubscribe;

	// Token: 0x04002624 RID: 9764
	private List<PlayerReference> players = new List<PlayerReference>();
}
