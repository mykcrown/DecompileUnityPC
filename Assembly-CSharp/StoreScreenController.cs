using System;

// Token: 0x02000A30 RID: 2608
public class StoreScreenController : UIScreenController
{
	// Token: 0x17001218 RID: 4632
	// (get) Token: 0x06004C3D RID: 19517 RVA: 0x00144733 File Offset: 0x00142B33
	// (set) Token: 0x06004C3E RID: 19518 RVA: 0x0014473B File Offset: 0x00142B3B
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17001219 RID: 4633
	// (get) Token: 0x06004C3F RID: 19519 RVA: 0x00144744 File Offset: 0x00142B44
	// (set) Token: 0x06004C40 RID: 19520 RVA: 0x0014474C File Offset: 0x00142B4C
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x06004C41 RID: 19521 RVA: 0x00144755 File Offset: 0x00142B55
	protected override void setupListeners()
	{
		base.subscribe(typeof(PreviousScreenRequest), new Action<GameEvent>(this.onPreviousScreenRequest));
	}

	// Token: 0x06004C42 RID: 19522 RVA: 0x00144773 File Offset: 0x00142B73
	private void onPreviousScreenRequest(GameEvent message)
	{
		this.richPresence.SetPresence(null, null, null, null);
		this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Previous));
	}
}
