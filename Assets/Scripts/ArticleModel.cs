// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

[Serializable]
public class ArticleModel : RollbackStateTyped<ArticleModel>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public Effect effect;

	[IgnoreCopyValidation, IsClonedManually]
	public List<Hit> hits = new List<Hit>(32);

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static), IsClonedManually]
	private List<Hit> hitsPool = new List<Hit>();

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static)]
	[NonSerialized]
	public ChargeConfig chargeData;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Debug)]
	[NonSerialized]
	public Dictionary<HitBoxState, CapsuleShape> hitBoxCapsules = new Dictionary<HitBoxState, CapsuleShape>(32, default(HitBoxStateComparer));

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Todo), IsClonedManually]
	[NonSerialized]
	public HitDisableDataMap disabledHits = new HitDisableDataMap();

	[IgnoreCopyValidation, IsClonedManually]
	public PhysicsModel physicsModel = new PhysicsModel();

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy), IsClonedManually]
	[NonSerialized]
	public List<AudioHandle> activeSoundEffects = new List<AudioHandle>(16);

	public Fixed rotationAngle;

	public HorizontalDirection currentFacing;

	public int currentFrame;

	public PlayerNum playerOwner;

	public TeamNum team;

	public MovementType movementType;

	public bool isExpired;

	public bool isCanceled;

	public Fixed staleDamageMultiplier = (Fixed)1.0;

	public bool destroyOnHitLag;

	public int hitLagFrames;

	public Fixed chargeFraction;

	public bool didCollisionSpawn;

	public MoveLabel moveLabel;

	public string moveName;

	public int moveUID;

	public int hitOwnerID;

	public ArticleModel()
	{
		for (int i = 0; i < this.hits.Capacity; i++)
		{
			this.hitsPool.Add(new Hit());
		}
	}

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

	public override object Clone()
	{
		ArticleModel articleModel = new ArticleModel();
		this.CopyTo(articleModel);
		return articleModel;
	}

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

	public bool IsActiveFor(IHitOwner other, int currentFrame)
	{
		return this.disabledHits.IsActiveFor(other, currentFrame);
	}

	public override void Clear()
	{
		base.Clear();
		this.disabledHits.Clear();
		foreach (CapsuleShape current in this.hitBoxCapsules.Values)
		{
			current.Clear();
		}
		this.hitBoxCapsules.Clear();
		this.hits.Clear();
		this.activeSoundEffects.Clear();
	}
}
