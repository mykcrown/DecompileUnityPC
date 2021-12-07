// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CreditsScreenController : UIScreenController
{
	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IRichPresence richPresence
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
		this.richPresence.SetPresence(null, null, null, null);
		this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Previous));
	}
}
