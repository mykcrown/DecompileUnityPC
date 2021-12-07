// Decompile from assembly: Assembly-CSharp.dll

using System;

public class TransparencyProximityComponent : MoveComponent, IMoveStartComponent, IMoveEndComponent
{
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
	}

	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		this.playerDelegate.Renderer.ToggleVisibility(false);
	}

	public void OnEnd()
	{
		this.playerDelegate.Renderer.ToggleVisibility(true);
	}
}
