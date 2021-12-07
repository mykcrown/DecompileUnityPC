using System;

// Token: 0x02000684 RID: 1668
public class KeyBindingManager : IKeyBindingManager
{
	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x0600292D RID: 10541 RVA: 0x000DAFDF File Offset: 0x000D93DF
	// (set) Token: 0x0600292E RID: 10542 RVA: 0x000DAFE7 File Offset: 0x000D93E7
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x0600292F RID: 10543 RVA: 0x000DAFF0 File Offset: 0x000D93F0
	// (set) Token: 0x06002930 RID: 10544 RVA: 0x000DAFF8 File Offset: 0x000D93F8
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x06002931 RID: 10545 RVA: 0x000DB001 File Offset: 0x000D9401
	// (set) Token: 0x06002932 RID: 10546 RVA: 0x000DB009 File Offset: 0x000D9409
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x06002933 RID: 10547 RVA: 0x000DB012 File Offset: 0x000D9412
	public void Begin()
	{
		this.timer.CancelTimeout(new Action(this.end));
		if (!this.isBindingKey)
		{
			this.isBindingKey = true;
			this.signalBus.Dispatch(KeyBindingManager.UPDATED);
		}
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x000DB04D File Offset: 0x000D944D
	public void End()
	{
		this.timer.CancelTimeout(new Action(this.end));
		this.timer.SetTimeout(1, new Action(this.end));
	}

	// Token: 0x06002935 RID: 10549 RVA: 0x000DB07E File Offset: 0x000D947E
	private void end()
	{
		if (this.isBindingKey)
		{
			this.isBindingKey = false;
			this.signalBus.Dispatch(KeyBindingManager.UPDATED);
		}
	}

	// Token: 0x17000A17 RID: 2583
	// (get) Token: 0x06002936 RID: 10550 RVA: 0x000DB0A2 File Offset: 0x000D94A2
	public bool IsBindingKey
	{
		get
		{
			return this.isBindingKey;
		}
	}

	// Token: 0x04001DD5 RID: 7637
	public static string UPDATED = "KeyBindingManager.UPDATED";

	// Token: 0x04001DD9 RID: 7641
	private bool isBindingKey;
}
