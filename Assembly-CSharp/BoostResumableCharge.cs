using System;

// Token: 0x020004CA RID: 1226
public class BoostResumableCharge : MoveComponent, IMoveStartComponent
{
	// Token: 0x06001B19 RID: 6937 RVA: 0x0008A3BC File Offset: 0x000887BC
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		this.chargeComponent = playerDelegate.GetCharacterComponent<ResumableChargeComponent>();
		if (this.chargeComponent == null)
		{
			throw new Exception("Character requires ResumableChargeComponent for ResumableCharge move components to work.");
		}
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x0008A3EF File Offset: 0x000887EF
	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		this.chargeComponent.AddChargeFrames(this.chargeFramesAdded);
	}

	// Token: 0x04001468 RID: 5224
	public int chargeFramesAdded;

	// Token: 0x04001469 RID: 5225
	private ResumableChargeComponent chargeComponent;
}
