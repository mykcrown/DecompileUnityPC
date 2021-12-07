using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000605 RID: 1541
[Serializable]
public class ShieldModel : RollbackStateTyped<ShieldModel>
{
	// Token: 0x060025E2 RID: 9698 RVA: 0x000BB724 File Offset: 0x000B9B24
	public override object Clone()
	{
		ShieldModel shieldModel = new ShieldModel();
		this.CopyTo(shieldModel);
		return shieldModel;
	}

	// Token: 0x060025E3 RID: 9699 RVA: 0x000BB740 File Offset: 0x000B9B40
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

	// Token: 0x060025E4 RID: 9700 RVA: 0x000BB82B File Offset: 0x000B9C2B
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

	// Token: 0x04001BBF RID: 7103
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Hit gustHit_ground;

	// Token: 0x04001BC0 RID: 7104
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Hit gustHit_air;

	// Token: 0x04001BC1 RID: 7105
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Hit normalHit;

	// Token: 0x04001BC2 RID: 7106
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[IsClonedManually]
	[IgnoreCopyValidation]
	private Hit gustHit_ground_pool = new Hit();

	// Token: 0x04001BC3 RID: 7107
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[IsClonedManually]
	[IgnoreCopyValidation]
	private Hit gustHit_air_pool = new Hit();

	// Token: 0x04001BC4 RID: 7108
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[IsClonedManually]
	[IgnoreCopyValidation]
	private Hit normalHit_pool = new Hit();

	// Token: 0x04001BC5 RID: 7109
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<ActiveShieldHitType> activeShieldHits = new List<ActiveShieldHitType>(8);

	// Token: 0x04001BC6 RID: 7110
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public GameObject shield;

	// Token: 0x04001BC7 RID: 7111
	public Fixed shieldHealth;

	// Token: 0x04001BC8 RID: 7112
	public Fixed rotationAngle = 0;

	// Token: 0x04001BC9 RID: 7113
	public Vector3F position;

	// Token: 0x04001BCA RID: 7114
	public Vector3F tiltOffset = Vector3F.zero;

	// Token: 0x04001BCB RID: 7115
	public bool shieldActive;

	// Token: 0x04001BCC RID: 7116
	public int shieldBeginFrame;

	// Token: 0x04001BCD RID: 7117
	public bool wasRunningBeforeShield;

	// Token: 0x04001BCE RID: 7118
	public int gustBeginFrame;

	// Token: 0x04001BCF RID: 7119
	public bool isGustSuccessful;

	// Token: 0x04001BD0 RID: 7120
	public bool isGusting;
}
