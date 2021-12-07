using System;
using UnityEngine.UI;

// Token: 0x02000A39 RID: 2617
public class ComboCounter : GameBehavior, ITickable
{
	// Token: 0x06004CAB RID: 19627 RVA: 0x00144F7B File Offset: 0x0014337B
	public void Start()
	{
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x06004CAC RID: 19628 RVA: 0x00144F9E File Offset: 0x0014339E
	public override void OnDestroy()
	{
		base.OnDestroy();
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x06004CAD RID: 19629 RVA: 0x00144FC7 File Offset: 0x001433C7
	private void onGameInit(GameEvent message)
	{
		this.tracker = (message as GameInitEvent).gameManager.ComboManager.GetTracker(this.PlayerNumber);
	}

	// Token: 0x06004CAE RID: 19630 RVA: 0x00144FEC File Offset: 0x001433EC
	public void TickFrame()
	{
		ComboState comboState = this.tracker.GetComboState();
		this.CountText.text = comboState.Count.ToString();
		this.DamageText.text = comboState.Damage.ToString();
	}

	// Token: 0x0400324C RID: 12876
	public PlayerNum PlayerNumber;

	// Token: 0x0400324D RID: 12877
	public TeamNum Team;

	// Token: 0x0400324E RID: 12878
	public Text CountText;

	// Token: 0x0400324F RID: 12879
	public Text DamageText;

	// Token: 0x04003250 RID: 12880
	private ComboTracker tracker;
}
