using System;

// Token: 0x02000982 RID: 2434
public class SettingsScreenController : UIScreenController
{
	// Token: 0x17000F88 RID: 3976
	// (get) Token: 0x060041DA RID: 16858 RVA: 0x0012702E File Offset: 0x0012542E
	// (set) Token: 0x060041DB RID: 16859 RVA: 0x00127036 File Offset: 0x00125436
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x060041DC RID: 16860 RVA: 0x0012703F File Offset: 0x0012543F
	protected override void setupListeners()
	{
		base.subscribe(typeof(PreviousScreenRequest), new Action<GameEvent>(this.onPreviousScreenRequest));
	}

	// Token: 0x060041DD RID: 16861 RVA: 0x0012705D File Offset: 0x0012545D
	private void onPreviousScreenRequest(GameEvent message)
	{
		this.events.Broadcast(new LoadScreenCommand(this.adapter.PreviousScreen, null, ScreenUpdateType.Previous));
	}
}
