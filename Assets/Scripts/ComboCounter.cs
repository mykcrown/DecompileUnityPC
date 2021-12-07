// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine.UI;

public class ComboCounter : GameBehavior, ITickable
{
	public PlayerNum PlayerNumber;

	public TeamNum Team;

	public Text CountText;

	public Text DamageText;

	private ComboTracker tracker;

	public void Start()
	{
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	private void onGameInit(GameEvent message)
	{
		this.tracker = (message as GameInitEvent).gameManager.ComboManager.GetTracker(this.PlayerNumber);
	}

	public void TickFrame()
	{
		ComboState comboState = this.tracker.GetComboState();
		this.CountText.text = comboState.Count.ToString();
		this.DamageText.text = comboState.Damage.ToString();
	}
}
