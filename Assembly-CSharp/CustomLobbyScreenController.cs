using System;

// Token: 0x02000920 RID: 2336
public class CustomLobbyScreenController : UIScreenController
{
	// Token: 0x17000E96 RID: 3734
	// (get) Token: 0x06003CF9 RID: 15609 RVA: 0x0011A550 File Offset: 0x00118950
	// (set) Token: 0x06003CFA RID: 15610 RVA: 0x0011A558 File Offset: 0x00118958
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x06003CFB RID: 15611 RVA: 0x0011A561 File Offset: 0x00118961
	protected override void setupListeners()
	{
		base.subscribe(typeof(PreviousScreenRequest), new Action<GameEvent>(this.onPreviousScreenRequest));
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x0011A57F File Offset: 0x0011897F
	private void onPreviousScreenRequest(GameEvent message)
	{
		this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Previous));
	}
}
