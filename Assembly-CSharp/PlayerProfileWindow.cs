using System;

// Token: 0x020008F4 RID: 2292
public class PlayerProfileWindow : ClientBehavior
{
	// Token: 0x17000E1F RID: 3615
	// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x00112E18 File Offset: 0x00111218
	// (set) Token: 0x06003AD7 RID: 15063 RVA: 0x00112E20 File Offset: 0x00111220
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x17000E20 RID: 3616
	// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x00112E29 File Offset: 0x00111229
	// (set) Token: 0x06003AD9 RID: 15065 RVA: 0x00112E31 File Offset: 0x00111231
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x06003ADA RID: 15066 RVA: 0x00112E3A File Offset: 0x0011123A
	public void Initialize(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
	}

	// Token: 0x06003ADB RID: 15067 RVA: 0x00112E43 File Offset: 0x00111243
	public override void Awake()
	{
		base.Awake();
		this.InputField.EnterCallback = new Action(this.enterName);
	}

	// Token: 0x06003ADC RID: 15068 RVA: 0x00112E62 File Offset: 0x00111262
	public void OnOpened()
	{
		this.timer.SetTimeout(1, delegate
		{
			(this.uiManager.CurrentInputModule as CursorInputModule).SetSelectedInputField(this.InputField);
		});
	}

	// Token: 0x06003ADD RID: 15069 RVA: 0x00112E7C File Offset: 0x0011127C
	public void enterName()
	{
		base.signalBus.GetSignal<SetPlayerProfileNameSignal>().Dispatch(this.playerNum, this.InputField.text);
		this.OnCloseRequest();
	}

	// Token: 0x0400287C RID: 10364
	public WavedashTMProInput InputField;

	// Token: 0x0400287D RID: 10365
	public WavedashTextEntry TextEntry;

	// Token: 0x0400287E RID: 10366
	public Action OnCloseRequest;

	// Token: 0x0400287F RID: 10367
	private PlayerNum playerNum;
}
