using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008AF RID: 2223
public class CrewsGUI : GameBehavior, ICrewsGUI
{
	// Token: 0x17000D8D RID: 3469
	// (get) Token: 0x060037D2 RID: 14290 RVA: 0x00105D4E File Offset: 0x0010414E
	// (set) Token: 0x060037D3 RID: 14291 RVA: 0x00105D56 File Offset: 0x00104156
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x060037D4 RID: 14292 RVA: 0x00105D60 File Offset: 0x00104160
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

	// Token: 0x060037D5 RID: 14293 RVA: 0x00105F00 File Offset: 0x00104300
	private void setTextFields()
	{
		this.centerText.text = this.localization.GetText("ui.crewsHud.vs");
	}

	// Token: 0x060037D6 RID: 14294 RVA: 0x00105F1D File Offset: 0x0010431D
	public void TickFrame()
	{
		this.leftPlayerList.TickFrame();
		this.rightPlayerList.TickFrame();
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x00105F35 File Offset: 0x00104335
	private void onEngagementStateChanged(GameEvent message)
	{
		this.updateWinningBg();
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x00105F3D File Offset: 0x0010433D
	private void onCharacterDeath(GameEvent message)
	{
		this.updateWinningBg();
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x00105F48 File Offset: 0x00104348
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

	// Token: 0x060037DA RID: 14298 RVA: 0x00105FF0 File Offset: 0x001043F0
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

	// Token: 0x17000D8E RID: 3470
	// (get) Token: 0x060037DB RID: 14299 RVA: 0x0010603E File Offset: 0x0010443E
	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x00106048 File Offset: 0x00104448
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.didSubscribe)
		{
			base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
			base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		}
	}

	// Token: 0x0400260A RID: 9738
	public CrewsHUDTeamGUI leftTeamHud;

	// Token: 0x0400260B RID: 9739
	public CrewsHUDTeamGUI rightTeamHud;

	// Token: 0x0400260C RID: 9740
	public CrewsHUDPlayerList leftPlayerList;

	// Token: 0x0400260D RID: 9741
	public CrewsHUDPlayerList rightPlayerList;

	// Token: 0x0400260E RID: 9742
	public GameObject WinningTeamBg;

	// Token: 0x0400260F RID: 9743
	public Sprite blueTeamWinningSprite;

	// Token: 0x04002610 RID: 9744
	public Sprite redTeamWinningSprite;

	// Token: 0x04002611 RID: 9745
	public Sprite neutralTeamWinningSprite;

	// Token: 0x04002612 RID: 9746
	public TextMeshProUGUI centerText;

	// Token: 0x04002613 RID: 9747
	private UIColor team1Color;

	// Token: 0x04002614 RID: 9748
	private bool didSubscribe;

	// Token: 0x04002615 RID: 9749
	private List<PlayerReference> players = new List<PlayerReference>();
}
