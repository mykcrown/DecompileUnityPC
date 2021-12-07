// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class MoveUseTracker : IMoveUseTracker
{
	private delegate bool ResetCheck(MoveData moveData);

	private IPlayerDelegate player;

	private List<MoveLabel> moveLabelBuffer = new List<MoveLabel>();

	private MoveUseTracker.ResetCheck checkLedgeReset;

	private MoveUseTracker.ResetCheck checkGroundedReset;

	private MoveUseTracker.ResetCheck checkDeathReset;

	private MoveUseTracker.ResetCheck checkGrabbedReset;

	private MoveUseTracker.ResetCheck checkDealHitReset;

	private MoveUseTracker.ResetCheck checkReceiveHitReset;

	private static MoveUseTracker.ResetCheck __f__am_cache0;

	private static MoveUseTracker.ResetCheck __f__am_cache1;

	private static MoveUseTracker.ResetCheck __f__am_cache2;

	private static MoveUseTracker.ResetCheck __f__am_cache3;

	private static MoveUseTracker.ResetCheck __f__am_cache4;

	private static MoveUseTracker.ResetCheck __f__am_cache5;

	public MoveUseTracker(IPlayerDelegate player)
	{
		this.player = player;
		if (MoveUseTracker.__f__am_cache0 == null)
		{
			MoveUseTracker.__f__am_cache0 = new MoveUseTracker.ResetCheck(MoveUseTracker._MoveUseTracker_m__0);
		}
		this.checkGroundedReset = MoveUseTracker.__f__am_cache0;
		if (MoveUseTracker.__f__am_cache1 == null)
		{
			MoveUseTracker.__f__am_cache1 = new MoveUseTracker.ResetCheck(MoveUseTracker._MoveUseTracker_m__1);
		}
		this.checkDeathReset = MoveUseTracker.__f__am_cache1;
		if (MoveUseTracker.__f__am_cache2 == null)
		{
			MoveUseTracker.__f__am_cache2 = new MoveUseTracker.ResetCheck(MoveUseTracker._MoveUseTracker_m__2);
		}
		this.checkGrabbedReset = MoveUseTracker.__f__am_cache2;
		if (MoveUseTracker.__f__am_cache3 == null)
		{
			MoveUseTracker.__f__am_cache3 = new MoveUseTracker.ResetCheck(MoveUseTracker._MoveUseTracker_m__3);
		}
		this.checkDealHitReset = MoveUseTracker.__f__am_cache3;
		if (MoveUseTracker.__f__am_cache4 == null)
		{
			MoveUseTracker.__f__am_cache4 = new MoveUseTracker.ResetCheck(MoveUseTracker._MoveUseTracker_m__4);
		}
		this.checkReceiveHitReset = MoveUseTracker.__f__am_cache4;
		if (MoveUseTracker.__f__am_cache5 == null)
		{
			MoveUseTracker.__f__am_cache5 = new MoveUseTracker.ResetCheck(MoveUseTracker._MoveUseTracker_m__5);
		}
		this.checkLedgeReset = MoveUseTracker.__f__am_cache5;
	}

	public bool HasMoveUsesLeft(MoveData moveData)
	{
		return !moveData.moveUseOptions.trackUses || moveData.moveUseOptions.numberOfUses <= 0 || !this.player.Model.moveUses.ContainsKey(moveData.label) || this.player.Model.moveUses[moveData.label] < moveData.moveUseOptions.numberOfUses;
	}

	public void Grounded()
	{
		this.resetMovesWithCheck(this.checkGroundedReset);
	}

	public void OnRemovedFromGame()
	{
		this.resetMovesWithCheck(this.checkDeathReset);
	}

	public void OnGrabbed()
	{
		this.resetMovesWithCheck(this.checkGrabbedReset);
	}

	public void OnGiveHit()
	{
		this.resetMovesWithCheck(this.checkDealHitReset);
	}

	public void OnReceiveHit()
	{
		this.resetMovesWithCheck(this.checkReceiveHitReset);
	}

	public void OnGrabLedge()
	{
		this.resetMovesWithCheck(this.checkLedgeReset);
	}

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

	private void resetMovesWithCheck(MoveUseTracker.ResetCheck check)
	{
		this.moveLabelBuffer.Clear();
		foreach (KeyValuePair<MoveLabel, int> current in this.player.Model.moveUses)
		{
			this.moveLabelBuffer.Add(current.Key);
		}
		foreach (MoveLabel current2 in this.moveLabelBuffer)
		{
			MoveData[] moves = this.player.MoveSet.GetMoves(current2);
			MoveData[] array = moves;
			for (int i = 0; i < array.Length; i++)
			{
				MoveData moveData = array[i];
				if (moveData != null && check(moveData))
				{
					this.player.Model.moveUses[current2] = 0;
					break;
				}
			}
		}
	}

	public int GetMoveUsedCount(MoveLabel label)
	{
		return (!this.player.Model.moveUses.ContainsKey(label)) ? 0 : this.player.Model.moveUses[label];
	}

	private static bool _MoveUseTracker_m__0(MoveData moveData)
	{
		return moveData.moveUseOptions.resetIfGrounded;
	}

	private static bool _MoveUseTracker_m__1(MoveData moveData)
	{
		return moveData.moveUseOptions.resetOnDeath;
	}

	private static bool _MoveUseTracker_m__2(MoveData moveData)
	{
		return moveData.moveUseOptions.resetOnGrabbed;
	}

	private static bool _MoveUseTracker_m__3(MoveData moveData)
	{
		return moveData.moveUseOptions.resetOnDealHit;
	}

	private static bool _MoveUseTracker_m__4(MoveData moveData)
	{
		return moveData.moveUseOptions.resetOnReceiveHit;
	}

	private static bool _MoveUseTracker_m__5(MoveData moveData)
	{
		return moveData.moveUseOptions.resetIfGrabLedge;
	}
}
