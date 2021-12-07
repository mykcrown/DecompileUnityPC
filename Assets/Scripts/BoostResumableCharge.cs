// Decompile from assembly: Assembly-CSharp.dll

using System;

public class BoostResumableCharge : MoveComponent, IMoveStartComponent
{
	public int chargeFramesAdded;

	private ResumableChargeComponent chargeComponent;

	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		this.chargeComponent = playerDelegate.GetCharacterComponent<ResumableChargeComponent>();
		if (this.chargeComponent == null)
		{
			throw new Exception("Character requires ResumableChargeComponent for ResumableCharge move components to work.");
		}
	}

	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		this.chargeComponent.AddChargeFrames(this.chargeFramesAdded);
	}
}
