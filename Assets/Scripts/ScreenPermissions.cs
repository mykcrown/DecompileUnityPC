// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ScreenPermissions : IScreenPermissions
{
	[Inject]
	public IOfflineModeDetector offlineMode
	{
		get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverManager
	{
		get;
		set;
	}

	public ScreenType GetRedirect(ScreenType screen)
	{
		if ((screen == ScreenType.MainMenu || screen == ScreenType.CharacterSelect) && !this.isCoreGameAllowed())
		{
			return ScreenType.LoginScreen;
		}
		return screen;
	}

	private bool isCoreGameAllowed()
	{
		return this.offlineMode.IsOfflineMode() || (this.serverManager.HasAccount && this.serverManager.IsCoreConnectionReady);
	}
}
