using System;
using IconsServer;

// Token: 0x020007C4 RID: 1988
public class ConnectionController
{
	// Token: 0x17000BFD RID: 3069
	// (get) Token: 0x06003159 RID: 12633 RVA: 0x000F0FD9 File Offset: 0x000EF3D9
	// (set) Token: 0x0600315A RID: 12634 RVA: 0x000F0FE1 File Offset: 0x000EF3E1
	[Inject]
	public IAccountAPI accountAPI { get; set; }

	// Token: 0x17000BFE RID: 3070
	// (get) Token: 0x0600315B RID: 12635 RVA: 0x000F0FEA File Offset: 0x000EF3EA
	// (set) Token: 0x0600315C RID: 12636 RVA: 0x000F0FF2 File Offset: 0x000EF3F2
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000BFF RID: 3071
	// (get) Token: 0x0600315D RID: 12637 RVA: 0x000F0FFB File Offset: 0x000EF3FB
	// (set) Token: 0x0600315E RID: 12638 RVA: 0x000F1003 File Offset: 0x000EF403
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000C00 RID: 3072
	// (get) Token: 0x0600315F RID: 12639 RVA: 0x000F100C File Offset: 0x000EF40C
	// (set) Token: 0x06003160 RID: 12640 RVA: 0x000F1014 File Offset: 0x000EF414
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x06003161 RID: 12641 RVA: 0x000F101D File Offset: 0x000EF41D
	public void Initialize()
	{
		this.accountAPI.Initialize();
		this.battleServerAPI.Initialize();
		this.customLobby.Initialize();
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x000F1040 File Offset: 0x000EF440
	public void OnDestroy()
	{
		this.battleServerAPI.OnDestroy();
		this.iconsServerAPI.Shutdown();
	}
}
