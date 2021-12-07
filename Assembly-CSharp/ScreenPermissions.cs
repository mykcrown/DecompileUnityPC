using System;

// Token: 0x020009C8 RID: 2504
public class ScreenPermissions : IScreenPermissions
{
	// Token: 0x170010D9 RID: 4313
	// (get) Token: 0x06004633 RID: 17971 RVA: 0x00132706 File Offset: 0x00130B06
	// (set) Token: 0x06004634 RID: 17972 RVA: 0x0013270E File Offset: 0x00130B0E
	[Inject]
	public IOfflineModeDetector offlineMode { get; set; }

	// Token: 0x170010DA RID: 4314
	// (get) Token: 0x06004635 RID: 17973 RVA: 0x00132717 File Offset: 0x00130B17
	// (set) Token: 0x06004636 RID: 17974 RVA: 0x0013271F File Offset: 0x00130B1F
	[Inject]
	public IServerConnectionManager serverManager { get; set; }

	// Token: 0x06004637 RID: 17975 RVA: 0x00132728 File Offset: 0x00130B28
	public ScreenType GetRedirect(ScreenType screen)
	{
		if ((screen == ScreenType.MainMenu || screen == ScreenType.CharacterSelect) && !this.isCoreGameAllowed())
		{
			return ScreenType.LoginScreen;
		}
		return screen;
	}

	// Token: 0x06004638 RID: 17976 RVA: 0x00132746 File Offset: 0x00130B46
	private bool isCoreGameAllowed()
	{
		return this.offlineMode.IsOfflineMode() || (this.serverManager.HasAccount && this.serverManager.IsCoreConnectionReady);
	}
}
