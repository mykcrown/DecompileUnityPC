// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShieldModel : RollbackStateTyped<ShieldModel>
{
	[IgnoreCopyValidation, IsClonedManually]
	public Hit gustHit_ground;

	[IgnoreCopyValidation, IsClonedManually]
	public Hit gustHit_air;

	[IgnoreCopyValidation, IsClonedManually]
	public Hit normalHit;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static), IsClonedManually]
	private Hit gustHit_ground_pool = new Hit();

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static), IsClonedManually]
	private Hit gustHit_air_pool = new Hit();

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static), IsClonedManually]
	private Hit normalHit_pool = new Hit();

	[IgnoreCopyValidation, IsClonedManually]
	public List<ActiveShieldHitType> activeShieldHits = new List<ActiveShieldHitType>(8);

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Visual)]
	[NonSerialized]
	public GameObject shield;

	public Fixed shieldHealth;

	public Fixed rotationAngle = 0;

	public Vector3F position;

	public Vector3F tiltOffset = Vector3F.zero;

	public bool shieldActive;

	public int shieldBeginFrame;

	public bool wasRunningBeforeShield;

	public int gustBeginFrame;

	public bool isGustSuccessful;

	public bool isGusting;

	public override object Clone()
	{
		ShieldModel shieldModel = new ShieldModel();
		this.CopyTo(shieldModel);
		return shieldModel;
	}

	public override void CopyTo(ShieldModel targetIn)
	{
		targetIn.shieldHealth = this.shieldHealth;
		targetIn.rotationAngle = this.rotationAngle;
		targetIn.position = this.position;
		targetIn.tiltOffset = this.tiltOffset;
		targetIn.shieldActive = this.shieldActive;
		targetIn.shieldBeginFrame = this.shieldBeginFrame;
		targetIn.wasRunningBeforeShield = this.wasRunningBeforeShield;
		targetIn.gustBeginFrame = this.gustBeginFrame;
		targetIn.isGustSuccessful = this.isGustSuccessful;
		targetIn.isGusting = this.isGusting;
		targetIn.shield = this.shield;
		this.hitCopy(ref this.gustHit_ground, ref targetIn.gustHit_ground, ref targetIn.gustHit_ground_pool);
		this.hitCopy(ref this.gustHit_air, ref targetIn.gustHit_air, ref targetIn.gustHit_air_pool);
		this.hitCopy(ref this.normalHit, ref targetIn.normalHit, ref targetIn.normalHit_pool);
		base.copyList<ActiveShieldHitType>(this.activeShieldHits, targetIn.activeShieldHits);
	}

	private void hitCopy(ref Hit source, ref Hit target, ref Hit targetPoolObj)
	{
		if (source == null)
		{
			target = null;
		}
		else
		{
			target = targetPoolObj;
			source.CopyTo(target);
		}
	}
}
