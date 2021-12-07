using System;
using System.Collections.Generic;
using TMPro;

// Token: 0x020008B3 RID: 2227
public class CrewsHUDTeamGUI : GameBehavior
{
	// Token: 0x17000D92 RID: 3474
	// (get) Token: 0x060037F1 RID: 14321 RVA: 0x00106599 File Offset: 0x00104999
	// (set) Token: 0x060037F2 RID: 14322 RVA: 0x001065A1 File Offset: 0x001049A1
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x060037F3 RID: 14323 RVA: 0x001065AC File Offset: 0x001049AC
	public void Initialize(TeamNum team)
	{
		this.team = team;
		this.didSubscribe = true;
		base.events.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		this.playerSpawnerHandler(base.gameController.currentGame.PlayerSpawner);
		this.players = base.gameController.currentGame.PlayerReferences;
		this.setTextFields();
		this.updateStats();
	}

	// Token: 0x060037F4 RID: 14324 RVA: 0x00106644 File Offset: 0x00104A44
	private void setTextFields()
	{
		if (this.teamTitle != null)
		{
			UIColor uicolorFromTeam = PlayerUtil.GetUIColorFromTeam(this.team);
			if (uicolorFromTeam == UIColor.Red)
			{
				this.teamTitle.text = this.localization.GetText("ui.crewsHud.redTeam");
			}
			else
			{
				this.teamTitle.text = this.localization.GetText("ui.crewsHud.blueTeam");
			}
		}
	}

	// Token: 0x060037F5 RID: 14325 RVA: 0x001066B0 File Offset: 0x00104AB0
	private void playerSpawnerHandler(PlayerSpawner playerSpawner)
	{
		this.spawner = (playerSpawner as IBenchedPlayerSpawner);
	}

	// Token: 0x060037F6 RID: 14326 RVA: 0x001066BE File Offset: 0x00104ABE
	private void onEngagementStateChanged(GameEvent message)
	{
		this.updateStats();
	}

	// Token: 0x060037F7 RID: 14327 RVA: 0x001066C6 File Offset: 0x00104AC6
	private void onCharacterDeath(GameEvent message)
	{
		this.updateStats();
	}

	// Token: 0x060037F8 RID: 14328 RVA: 0x001066D0 File Offset: 0x00104AD0
	private void updateStats()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.players.Count; i++)
		{
			PlayerReference playerReference = this.players[i];
			if (playerReference.Team == this.team)
			{
				num += playerReference.Lives;
				num2 += this.spawner.GetAssistsRemaining(playerReference.PlayerNum);
			}
		}
		this.stockNumber.text = num.ToString();
		this.assistNumber.text = num2.ToString();
	}

	// Token: 0x060037F9 RID: 14329 RVA: 0x00106768 File Offset: 0x00104B68
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.didSubscribe)
		{
			base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
			base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		}
	}

	// Token: 0x0400262B RID: 9771
	public TextMeshProUGUI stockNumber;

	// Token: 0x0400262C RID: 9772
	public TextMeshProUGUI teamTitle;

	// Token: 0x0400262D RID: 9773
	public TextMeshProUGUI assistNumber;

	// Token: 0x0400262E RID: 9774
	private List<PlayerReference> players = new List<PlayerReference>();

	// Token: 0x0400262F RID: 9775
	private IBenchedPlayerSpawner spawner;

	// Token: 0x04002630 RID: 9776
	private TeamNum team;

	// Token: 0x04002631 RID: 9777
	private bool didSubscribe;
}
