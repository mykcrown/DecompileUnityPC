using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x0200035F RID: 863
[Serializable]
public class ArticleModel : RollbackStateTyped<ArticleModel>
{
	// Token: 0x0600127B RID: 4731 RVA: 0x0006A594 File Offset: 0x00068994
	public ArticleModel()
	{
		for (int i = 0; i < this.hits.Capacity; i++)
		{
			this.hitsPool.Add(new Hit());
		}
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x0006A640 File Offset: 0x00068A40
	public override void CopyTo(ArticleModel targetIn)
	{
		targetIn.rotationAngle = this.rotationAngle;
		targetIn.currentFacing = this.currentFacing;
		targetIn.currentFrame = this.currentFrame;
		targetIn.playerOwner = this.playerOwner;
		targetIn.team = this.team;
		targetIn.movementType = this.movementType;
		targetIn.isExpired = this.isExpired;
		targetIn.isCanceled = this.isCanceled;
		targetIn.staleDamageMultiplier = this.staleDamageMultiplier;
		targetIn.destroyOnHitLag = this.destroyOnHitLag;
		targetIn.hitLagFrames = this.hitLagFrames;
		targetIn.chargeFraction = this.chargeFraction;
		targetIn.didCollisionSpawn = this.didCollisionSpawn;
		targetIn.moveLabel = this.moveLabel;
		targetIn.moveName = this.moveName;
		targetIn.moveUID = this.moveUID;
		targetIn.hitOwnerID = this.hitOwnerID;
		targetIn.effect = this.effect;
		targetIn.chargeData = this.chargeData;
		base.copyDictionary<HitBoxState, CapsuleShape>(this.hitBoxCapsules, targetIn.hitBoxCapsules);
		this.disabledHits.CopyTo(targetIn.disabledHits);
		this.physicsModel.CopyTo(targetIn.physicsModel);
		targetIn.hits.Clear();
		int count = this.hits.Count;
		for (int i = 0; i < count; i++)
		{
			targetIn.hits.Add(targetIn.hitsPool[i]);
			this.hits[i].CopyTo(targetIn.hits[i]);
		}
		base.copyList<AudioHandle>(this.activeSoundEffects, targetIn.activeSoundEffects);
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x0006A7D4 File Offset: 0x00068BD4
	public override object Clone()
	{
		ArticleModel articleModel = new ArticleModel();
		this.CopyTo(articleModel);
		return articleModel;
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x0006A7F0 File Offset: 0x00068BF0
	public void Reset()
	{
		this.rotationAngle = 0;
		this.physicsModel.Reset();
		this.playerOwner = PlayerNum.Player1;
		this.team = TeamNum.Team1;
		this.movementType = MovementType.None;
		this.destroyOnHitLag = false;
		this.hitLagFrames = 0;
		this.moveLabel = MoveLabel.None;
		this.moveName = null;
		this.moveUID = 0;
		this.hitOwnerID = 0;
		this.effect = null;
		this.staleDamageMultiplier = (Fixed)1.0;
		this.currentFrame = 0;
		this.isExpired = false;
		this.isCanceled = false;
		this.chargeFraction = 0;
		this.chargeData = null;
		this.didCollisionSpawn = false;
		this.currentFacing = HorizontalDirection.Right;
		this.Clear();
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x0006A8AA File Offset: 0x00068CAA
	public bool IsActiveFor(IHitOwner other, int currentFrame)
	{
		return this.disabledHits.IsActiveFor(other, currentFrame);
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x0006A8BC File Offset: 0x00068CBC
	public override void Clear()
	{
		base.Clear();
		this.disabledHits.Clear();
		foreach (CapsuleShape capsuleShape in this.hitBoxCapsules.Values)
		{
			capsuleShape.Clear();
		}
		this.hitBoxCapsules.Clear();
		this.hits.Clear();
		this.activeSoundEffects.Clear();
	}

	// Token: 0x04000BFD RID: 3069
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public Effect effect;

	// Token: 0x04000BFE RID: 3070
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<Hit> hits = new List<Hit>(32);

	// Token: 0x04000BFF RID: 3071
	[IsClonedManually]
	[IgnoreCopyValidation]
	[IgnoreRollback(IgnoreRollbackType.Static)]
	private List<Hit> hitsPool = new List<Hit>();

	// Token: 0x04000C00 RID: 3072
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public ChargeConfig chargeData;

	// Token: 0x04000C01 RID: 3073
	[IgnoreRollback(IgnoreRollbackType.Debug)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public Dictionary<HitBoxState, CapsuleShape> hitBoxCapsules = new Dictionary<HitBoxState, CapsuleShape>(32, default(HitBoxStateComparer));

	// Token: 0x04000C02 RID: 3074
	[IsClonedManually]
	[IgnoreCopyValidation]
	[IgnoreRollback(IgnoreRollbackType.Todo)]
	[NonSerialized]
	public HitDisableDataMap disabledHits = new HitDisableDataMap();

	// Token: 0x04000C03 RID: 3075
	[IsClonedManually]
	[IgnoreCopyValidation]
	public PhysicsModel physicsModel = new PhysicsModel();

	// Token: 0x04000C04 RID: 3076
	[IsClonedManually]
	[IgnoreCopyValidation]
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public List<AudioHandle> activeSoundEffects = new List<AudioHandle>(16);

	// Token: 0x04000C05 RID: 3077
	public Fixed rotationAngle;

	// Token: 0x04000C06 RID: 3078
	public HorizontalDirection currentFacing;

	// Token: 0x04000C07 RID: 3079
	public int currentFrame;

	// Token: 0x04000C08 RID: 3080
	public PlayerNum playerOwner;

	// Token: 0x04000C09 RID: 3081
	public TeamNum team;

	// Token: 0x04000C0A RID: 3082
	public MovementType movementType;

	// Token: 0x04000C0B RID: 3083
	public bool isExpired;

	// Token: 0x04000C0C RID: 3084
	public bool isCanceled;

	// Token: 0x04000C0D RID: 3085
	public Fixed staleDamageMultiplier = (Fixed)1.0;

	// Token: 0x04000C0E RID: 3086
	public bool destroyOnHitLag;

	// Token: 0x04000C0F RID: 3087
	public int hitLagFrames;

	// Token: 0x04000C10 RID: 3088
	public Fixed chargeFraction;

	// Token: 0x04000C11 RID: 3089
	public bool didCollisionSpawn;

	// Token: 0x04000C12 RID: 3090
	public MoveLabel moveLabel;

	// Token: 0x04000C13 RID: 3091
	public string moveName;

	// Token: 0x04000C14 RID: 3092
	public int moveUID;

	// Token: 0x04000C15 RID: 3093
	public int hitOwnerID;
}
