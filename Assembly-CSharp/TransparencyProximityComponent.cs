using System;

// Token: 0x020004EB RID: 1259
public class TransparencyProximityComponent : MoveComponent, IMoveStartComponent, IMoveEndComponent
{
	// Token: 0x06001B85 RID: 7045 RVA: 0x0008BA0E File Offset: 0x00089E0E
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x0008BA19 File Offset: 0x00089E19
	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		this.playerDelegate.Renderer.ToggleVisibility(false);
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x0008BA2C File Offset: 0x00089E2C
	public void OnEnd()
	{
		this.playerDelegate.Renderer.ToggleVisibility(true);
	}
}
