using System;
using System.Collections.Generic;

// Token: 0x020003BE RID: 958
[Serializable]
public class HostedHit : Hit, ITickable, ICopyable<HostedHit>, ICopyable
{
	// Token: 0x060014A4 RID: 5284 RVA: 0x0007349B File Offset: 0x0007189B
	public HostedHit()
	{
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x000734B0 File Offset: 0x000718B0
	public HostedHit(HitData data, IHitOwner host, IBodyOwner hostBody, IHitOwner owner, Dictionary<HitBoxState, CapsuleShape> capsules, bool hostIsImmune) : base(data)
	{
		this.HostBody = hostBody;
		this.Host = host;
		this.Owner = owner;
		this.capsules = capsules;
		this.AbortOnHostLand = false;
		data.dataType = HitDataType.Hosted;
		if (hostIsImmune)
		{
			this.nextEnabledFrames[host.HitOwnerID] = int.MaxValue;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x060014A6 RID: 5286 RVA: 0x00073519 File Offset: 0x00071919
	// (set) Token: 0x060014A7 RID: 5287 RVA: 0x00073521 File Offset: 0x00071921
	public bool AbortOnHostLand { get; set; }

	// Token: 0x060014A8 RID: 5288 RVA: 0x0007352C File Offset: 0x0007192C
	public void CopyTo(HostedHit targetIn)
	{
		base.CopyTo(targetIn);
		targetIn.HostBody = this.HostBody;
		targetIn.Host = this.Host;
		targetIn.Owner = this.Owner;
		targetIn.isAborted = this.isAborted;
		targetIn.capsules = this.capsules;
		targetIn.AbortOnHostLand = this.AbortOnHostLand;
		targetIn.nextEnabledFrames.Clear();
		foreach (KeyValuePair<int, int> keyValuePair in this.nextEnabledFrames)
		{
			targetIn.nextEnabledFrames[keyValuePair.Key] = this.nextEnabledFrames[keyValuePair.Key];
		}
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x00073600 File Offset: 0x00071A00
	public override object Clone()
	{
		HostedHit hostedHit = new HostedHit();
		this.CopyTo(hostedHit);
		return hostedHit;
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x0007361B File Offset: 0x00071A1B
	public virtual void TickFrame()
	{
		MoveData.UpdateHitboxPosition(this, this.capsules, this.Owner, this.HostBody);
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x00073635 File Offset: 0x00071A35
	public void Abort()
	{
		this.isAborted = true;
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x0007363E File Offset: 0x00071A3E
	public void OnHostHit()
	{
		this.Abort();
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x00073646 File Offset: 0x00071A46
	public void OnHostLand()
	{
		if (this.AbortOnHostLand)
		{
			this.Abort();
		}
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x0007365C File Offset: 0x00071A5C
	public virtual bool IsActiveFor(IHitOwner other, int currentFrame)
	{
		return !this.IsDead && (other == null || !this.nextEnabledFrames.ContainsKey(other.HitOwnerID) || this.nextEnabledFrames[other.HitOwnerID] <= currentFrame);
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x000736AE File Offset: 0x00071AAE
	public virtual void DisableFor(IHitOwner other, int currentFrame)
	{
		this.nextEnabledFrames[other.HitOwnerID] = int.MaxValue;
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x060014B0 RID: 5296 RVA: 0x000736C6 File Offset: 0x00071AC6
	public virtual bool IsDead
	{
		get
		{
			return this.isAborted;
		}
	}

	// Token: 0x04000DC4 RID: 3524
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	protected IBodyOwner HostBody;

	// Token: 0x04000DC5 RID: 3525
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	protected IHitOwner Host;

	// Token: 0x04000DC6 RID: 3526
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	protected IHitOwner Owner;

	// Token: 0x04000DC7 RID: 3527
	[IsClonedManually]
	[IgnoreRollback(IgnoreRollbackType.Todo)]
	[NonSerialized]
	private Dictionary<HitBoxState, CapsuleShape> capsules;

	// Token: 0x04000DC8 RID: 3528
	[IsClonedManually]
	[IgnoreCopyValidation]
	public SerializableDictionary<int, int> nextEnabledFrames = new SerializableDictionary<int, int>(8);

	// Token: 0x04000DC9 RID: 3529
	public bool isAborted;
}
