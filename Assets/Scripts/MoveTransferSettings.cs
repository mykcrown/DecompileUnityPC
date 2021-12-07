// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class MoveTransferSettings
{
	public bool transitioningToContinuingMove;

	public bool transferHitDisableTargets;

	public bool transferChargeData;

	public bool transferStale;

	public Fixed transferStaleMulti = 1;

	public Fixed chargeFractionOverride = -1;

	public static readonly MoveTransferSettings Default = new MoveTransferSettings();
}
