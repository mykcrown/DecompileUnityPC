// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class HostedHit : Hit, ITickable, ICopyable<HostedHit>, ICopyable
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	protected IBodyOwner HostBody;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	protected IHitOwner Host;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	protected IHitOwner Owner;

	[IgnoreRollback(IgnoreRollbackType.Todo), IsClonedManually]
	[NonSerialized]
	private Dictionary<HitBoxState, CapsuleShape> capsules;

	[IgnoreCopyValidation, IsClonedManually]
	public SerializableDictionary<int, int> nextEnabledFrames = new SerializableDictionary<int, int>(8);

	public bool isAborted;

	public bool AbortOnHostLand
	{
		get;
		set;
	}

	public virtual bool IsDead
	{
		get
		{
			return this.isAborted;
		}
	}

	public HostedHit()
	{
	}

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
			this.nextEnabledFrames[host.HitOwnerID] = 2147483647;
		}
	}

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
		foreach (KeyValuePair<int, int> current in this.nextEnabledFrames)
		{
			targetIn.nextEnabledFrames[current.Key] = this.nextEnabledFrames[current.Key];
		}
	}

	public override object Clone()
	{
		HostedHit hostedHit = new HostedHit();
		this.CopyTo(hostedHit);
		return hostedHit;
	}

	public virtual void TickFrame()
	{
		MoveData.UpdateHitboxPosition(this, this.capsules, this.Owner, this.HostBody);
	}

	public void Abort()
	{
		this.isAborted = true;
	}

	public void OnHostHit()
	{
		this.Abort();
	}

	public void OnHostLand()
	{
		if (this.AbortOnHostLand)
		{
			this.Abort();
		}
	}

	public virtual bool IsActiveFor(IHitOwner other, int currentFrame)
	{
		return !this.IsDead && (other == null || !this.nextEnabledFrames.ContainsKey(other.HitOwnerID) || this.nextEnabledFrames[other.HitOwnerID] <= currentFrame);
	}

	public virtual void DisableFor(IHitOwner other, int currentFrame)
	{
		this.nextEnabledFrames[other.HitOwnerID] = 2147483647;
	}
}
