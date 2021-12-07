// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class TotemDuoSwapComponent : MoveComponent, IMoveTickMoveFrameComponent
{
	public bool performsSwap = true;

	public int swapFrame;

	public List<MoveData> partnerRequiredSwapMoves = new List<MoveData>();

	private TotemDuoComponent getTotemDuoComponent(IPlayerDelegate player)
	{
		TotemDuoComponent totemDuoComponent = null;
		foreach (ICharacterComponent current in player.Components)
		{
			if (current is TotemDuoComponent)
			{
				totemDuoComponent = (current as TotemDuoComponent);
				break;
			}
		}
		if (totemDuoComponent == null)
		{
			UnityEngine.Debug.LogWarning("TotemDuoLaunchComponent failed to find totem duo component");
		}
		return totemDuoComponent;
	}

	public void TickMoveFrame(InputButtonsData input)
	{
		if (this.performsSwap && this.moveDelegate.Model.internalFrame == this.swapFrame)
		{
			this.getTotemDuoComponent(this.playerDelegate).PerformSwap();
		}
	}

	public override bool ValidateRequirements(MoveData data, IPlayerDelegate player, InputButtonsData input)
	{
		if (!base.ValidateRequirements(data, this.playerDelegate, input))
		{
			return false;
		}
		MoveData data2 = this.getTotemDuoComponent(player).MyState.partner.ActiveMove.Data;
		return this.partnerRequiredSwapMoves.Contains(data2);
	}
}
