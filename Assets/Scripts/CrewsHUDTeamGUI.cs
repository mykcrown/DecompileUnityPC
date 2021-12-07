// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;

public class CrewsHUDTeamGUI : GameBehavior
{
	public TextMeshProUGUI stockNumber;

	public TextMeshProUGUI teamTitle;

	public TextMeshProUGUI assistNumber;

	private List<PlayerReference> players = new List<PlayerReference>();

	private IBenchedPlayerSpawner spawner;

	private TeamNum team;

	private bool didSubscribe;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

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

	private void setTextFields()
	{
		if (this.teamTitle != null)
		{
			UIColor uIColorFromTeam = PlayerUtil.GetUIColorFromTeam(this.team);
			if (uIColorFromTeam == UIColor.Red)
			{
				this.teamTitle.text = this.localization.GetText("ui.crewsHud.redTeam");
			}
			else
			{
				this.teamTitle.text = this.localization.GetText("ui.crewsHud.blueTeam");
			}
		}
	}

	private void playerSpawnerHandler(PlayerSpawner playerSpawner)
	{
		this.spawner = (playerSpawner as IBenchedPlayerSpawner);
	}

	private void onEngagementStateChanged(GameEvent message)
	{
		this.updateStats();
	}

	private void onCharacterDeath(GameEvent message)
	{
		this.updateStats();
	}

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
