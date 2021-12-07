// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;

public class ServerUtilityConsoleCommands
{
	[Inject]
	public IDevConsole devConsole
	{
		get;
		set;
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	public void Init()
	{
		this.devConsole.AddCommand(new Action(this.networkTest), "server", "test", "Endpoint for network path testing.");
		this.networkListeners();
	}

	private void networkListeners()
	{
	}

	private void onOpenLootboxes(ServerEvent evt)
	{
		this.devConsole.PrintLn("Ack event received");
	}

	private void networkTest()
	{
		this.devConsole.PrintLn("EXECUTE NETWORK TEST");
	}
}
