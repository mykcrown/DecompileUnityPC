using System;

// Token: 0x02000914 RID: 2324
public class CreditsScreenController : UIScreenController
{
	// Token: 0x17000E63 RID: 3683
	// (get) Token: 0x06003C46 RID: 15430 RVA: 0x001181AB File Offset: 0x001165AB
	// (set) Token: 0x06003C47 RID: 15431 RVA: 0x001181B3 File Offset: 0x001165B3
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000E64 RID: 3684
	// (get) Token: 0x06003C48 RID: 15432 RVA: 0x001181BC File Offset: 0x001165BC
	// (set) Token: 0x06003C49 RID: 15433 RVA: 0x001181C4 File Offset: 0x001165C4
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x06003C4A RID: 15434 RVA: 0x001181CD File Offset: 0x001165CD
	protected override void setupListeners()
	{
		base.subscribe(typeof(PreviousScreenRequest), new Action<GameEvent>(this.onPreviousScreenRequest));
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x001181EB File Offset: 0x001165EB
	private void onPreviousScreenRequest(GameEvent message)
	{
		this.richPresence.SetPresence(null, null, null, null);
		this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Previous));
	}
}
