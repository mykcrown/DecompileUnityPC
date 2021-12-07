using System;
using System.Collections.Generic;

// Token: 0x020005D8 RID: 1496
public class MoveUseTracker : IMoveUseTracker
{
	// Token: 0x060021D6 RID: 8662 RVA: 0x000A7B18 File Offset: 0x000A5F18
	public MoveUseTracker(IPlayerDelegate player)
	{
		this.player = player;
		this.checkGroundedReset = ((MoveData moveData) => moveData.moveUseOptions.resetIfGrounded);
		this.checkDeathReset = ((MoveData moveData) => moveData.moveUseOptions.resetOnDeath);
		this.checkGrabbedReset = ((MoveData moveData) => moveData.moveUseOptions.resetOnGrabbed);
		this.checkDealHitReset = ((MoveData moveData) => moveData.moveUseOptions.resetOnDealHit);
		this.checkReceiveHitReset = ((MoveData moveData) => moveData.moveUseOptions.resetOnReceiveHit);
		this.checkLedgeReset = ((MoveData moveData) => moveData.moveUseOptions.resetIfGrabLedge);
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000A7C10 File Offset: 0x000A6010
	public bool HasMoveUsesLeft(MoveData moveData)
	{
		return !moveData.moveUseOptions.trackUses || moveData.moveUseOptions.numberOfUses <= 0 || !this.player.Model.moveUses.ContainsKey(moveData.label) || this.player.Model.moveUses[moveData.label] < moveData.moveUseOptions.numberOfUses;
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000A7C88 File Offset: 0x000A6088
	public void Grounded()
	{
		this.resetMovesWithCheck(this.checkGroundedReset);
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000A7C96 File Offset: 0x000A6096
	public void OnRemovedFromGame()
	{
		this.resetMovesWithCheck(this.checkDeathReset);
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000A7CA4 File Offset: 0x000A60A4
	public void OnGrabbed()
	{
		this.resetMovesWithCheck(this.checkGrabbedReset);
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000A7CB2 File Offset: 0x000A60B2
	public void OnGiveHit()
	{
		this.resetMovesWithCheck(this.checkDealHitReset);
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x000A7CC0 File Offset: 0x000A60C0
	public void OnReceiveHit()
	{
		this.resetMovesWithCheck(this.checkReceiveHitReset);
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x000A7CCE File Offset: 0x000A60CE
	public void OnGrabLedge()
	{
		this.resetMovesWithCheck(this.checkLedgeReset);
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000A7CDC File Offset: 0x000A60DC
	public void OnMoveStart(MoveData move)
	{
		if (move.moveUseOptions.trackUses)
		{
			if (this.player.Model.moveUses.ContainsKey(move.label))
			{
				SerializableDictionary<MoveLabel, int> moveUses;
				MoveLabel label;
				(moveUses = this.player.Model.moveUses)[label = move.label] = moveUses[label] + 1;
			}
			else
			{
				this.player.Model.moveUses.Add(move.label, 1);
			}
		}
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x000A7D64 File Offset: 0x000A6164
	private void resetMovesWithCheck(MoveUseTracker.ResetCheck check)
	{
		this.moveLabelBuffer.Clear();
		foreach (KeyValuePair<MoveLabel, int> keyValuePair in this.player.Model.moveUses)
		{
			this.moveLabelBuffer.Add(keyValuePair.Key);
		}
		foreach (MoveLabel moveLabel in this.moveLabelBuffer)
		{
			MoveData[] moves = this.player.MoveSet.GetMoves(moveLabel);
			foreach (MoveData moveData in moves)
			{
				if (moveData != null && check(moveData))
				{
					this.player.Model.moveUses[moveLabel] = 0;
					break;
				}
			}
		}
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x000A7E94 File Offset: 0x000A6294
	public int GetMoveUsedCount(MoveLabel label)
	{
		return (!this.player.Model.moveUses.ContainsKey(label)) ? 0 : this.player.Model.moveUses[label];
	}

	// Token: 0x04001A63 RID: 6755
	private IPlayerDelegate player;

	// Token: 0x04001A64 RID: 6756
	private List<MoveLabel> moveLabelBuffer = new List<MoveLabel>();

	// Token: 0x04001A65 RID: 6757
	private MoveUseTracker.ResetCheck checkLedgeReset;

	// Token: 0x04001A66 RID: 6758
	private MoveUseTracker.ResetCheck checkGroundedReset;

	// Token: 0x04001A67 RID: 6759
	private MoveUseTracker.ResetCheck checkDeathReset;

	// Token: 0x04001A68 RID: 6760
	private MoveUseTracker.ResetCheck checkGrabbedReset;

	// Token: 0x04001A69 RID: 6761
	private MoveUseTracker.ResetCheck checkDealHitReset;

	// Token: 0x04001A6A RID: 6762
	private MoveUseTracker.ResetCheck checkReceiveHitReset;

	// Token: 0x020005D9 RID: 1497
	// (Invoke) Token: 0x060021E8 RID: 8680
	private delegate bool ResetCheck(MoveData moveData);
}
