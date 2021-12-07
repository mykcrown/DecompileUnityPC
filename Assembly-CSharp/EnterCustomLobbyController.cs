using System;

// Token: 0x02000921 RID: 2337
public class EnterCustomLobbyController
{
	// Token: 0x17000E97 RID: 3735
	// (get) Token: 0x06003CFE RID: 15614 RVA: 0x0011A59C File Offset: 0x0011899C
	// (set) Token: 0x06003CFF RID: 15615 RVA: 0x0011A5A4 File Offset: 0x001189A4
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x17000E98 RID: 3736
	// (get) Token: 0x06003D00 RID: 15616 RVA: 0x0011A5AD File Offset: 0x001189AD
	// (set) Token: 0x06003D01 RID: 15617 RVA: 0x0011A5B5 File Offset: 0x001189B5
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000E99 RID: 3737
	// (get) Token: 0x06003D02 RID: 15618 RVA: 0x0011A5BE File Offset: 0x001189BE
	// (set) Token: 0x06003D03 RID: 15619 RVA: 0x0011A5C6 File Offset: 0x001189C6
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06003D04 RID: 15620 RVA: 0x0011A5CF File Offset: 0x001189CF
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(CustomLobbyController.UPDATED, new Action(this.checkLoadCustomLobby));
		this.wasInLobby = false;
	}

	// Token: 0x06003D05 RID: 15621 RVA: 0x0011A5F4 File Offset: 0x001189F4
	private void checkLoadCustomLobby()
	{
		if (this.customLobby.IsInLobby && !this.wasInLobby)
		{
			this.events.Broadcast(new LoadScreenCommand(ScreenType.CustomLobbyScreen, null, ScreenUpdateType.Next));
		}
		this.wasInLobby = this.customLobby.IsInLobby;
	}

	// Token: 0x040029A5 RID: 10661
	private bool wasInLobby;
}
