using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004EA RID: 1258
public class TotemDuoSwapComponent : MoveComponent, IMoveTickMoveFrameComponent
{
	// Token: 0x06001B81 RID: 7041 RVA: 0x0008B8F8 File Offset: 0x00089CF8
	private TotemDuoComponent getTotemDuoComponent(IPlayerDelegate player)
	{
		TotemDuoComponent totemDuoComponent = null;
		foreach (ICharacterComponent characterComponent in player.Components)
		{
			if (characterComponent is TotemDuoComponent)
			{
				totemDuoComponent = (characterComponent as TotemDuoComponent);
				break;
			}
		}
		if (totemDuoComponent == null)
		{
			Debug.LogWarning("TotemDuoLaunchComponent failed to find totem duo component");
		}
		return totemDuoComponent;
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x0008B980 File Offset: 0x00089D80
	public void TickMoveFrame(InputButtonsData input)
	{
		if (this.performsSwap && this.moveDelegate.Model.internalFrame == this.swapFrame)
		{
			this.getTotemDuoComponent(this.playerDelegate).PerformSwap();
		}
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x0008B9BC File Offset: 0x00089DBC
	public override bool ValidateRequirements(MoveData data, IPlayerDelegate player, InputButtonsData input)
	{
		if (!base.ValidateRequirements(data, this.playerDelegate, input))
		{
			return false;
		}
		MoveData data2 = this.getTotemDuoComponent(player).MyState.partner.ActiveMove.Data;
		return this.partnerRequiredSwapMoves.Contains(data2);
	}

	// Token: 0x040014C6 RID: 5318
	public bool performsSwap = true;

	// Token: 0x040014C7 RID: 5319
	public int swapFrame;

	// Token: 0x040014C8 RID: 5320
	public List<MoveData> partnerRequiredSwapMoves = new List<MoveData>();
}
