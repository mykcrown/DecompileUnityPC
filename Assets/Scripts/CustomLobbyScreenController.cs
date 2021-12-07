// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CustomLobbyScreenController : UIScreenController
{
	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	protected override void setupListeners()
	{
		base.subscribe(typeof(PreviousScreenRequest), new Action<GameEvent>(this.onPreviousScreenRequest));
	}

	private void onPreviousScreenRequest(GameEvent message)
	{
		this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Previous));
	}
}
