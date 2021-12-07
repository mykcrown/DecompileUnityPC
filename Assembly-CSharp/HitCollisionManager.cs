using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003BA RID: 954
public class HitCollisionManager : ITickable, IRollbackStateOwner
{
	// Token: 0x06001487 RID: 5255 RVA: 0x00072BDC File Offset: 0x00070FDC
	public HitCollisionManager()
	{
		this.hitResultSortFn = delegate(HitCollisionCalculator.ISortedHitCollisionInvoke a, HitCollisionCalculator.ISortedHitCollisionInvoke b)
		{
			if (a.impactType == ImpactType.Grab && b.impactType == ImpactType.Grab && (a.hitter != b.hitter || a.receiver != b.receiver) && (a.hitter != b.receiver || a.receiver != b.hitter))
			{
				return (int)((a.hitter.Position - a.receiver.Position).sqrMagnitude - (b.hitter.Position - b.receiver.Position).sqrMagnitude);
			}
			return a.Priority() - b.Priority();
		};
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06001488 RID: 5256 RVA: 0x00072C33 File Offset: 0x00071033
	// (set) Token: 0x06001489 RID: 5257 RVA: 0x00072C3B File Offset: 0x0007103B
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x0600148A RID: 5258 RVA: 0x00072C44 File Offset: 0x00071044
	// (set) Token: 0x0600148B RID: 5259 RVA: 0x00072C4C File Offset: 0x0007104C
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x0600148C RID: 5260 RVA: 0x00072C55 File Offset: 0x00071055
	// (set) Token: 0x0600148D RID: 5261 RVA: 0x00072C5D File Offset: 0x0007105D
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x0600148E RID: 5262 RVA: 0x00072C66 File Offset: 0x00071066
	private List<IHitOwner> hitOwners
	{
		get
		{
			return this.model.hitOwners;
		}
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x00072C73 File Offset: 0x00071073
	[PostConstruct]
	public void Init()
	{
		this.injector.Inject(this.collisions);
		this.events.Subscribe(typeof(CharacterInitEvent), new Events.EventHandler(this.onCharacterInit));
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x00072CA7 File Offset: 0x000710A7
	public void OnDestroy()
	{
		this.events.Unsubscribe(typeof(CharacterInitEvent), new Events.EventHandler(this.onCharacterInit));
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x00072CCC File Offset: 0x000710CC
	private void cullOverlappingGrabs()
	{
		for (int i = 0; i < this.hitResults.Count; i++)
		{
			if (this.hitResults[i].impactType == ImpactType.Grab)
			{
				HitCollisionCalculator.ISortedHitCollisionInvoke sortedHitCollisionInvoke = this.hitResults[i];
				for (int j = this.hitResults.Count - 1; j >= i + 1; j--)
				{
					if (this.hitResults[i].impactType == ImpactType.Grab)
					{
						HitCollisionCalculator.ISortedHitCollisionInvoke sortedHitCollisionInvoke2 = this.hitResults[j];
						if (sortedHitCollisionInvoke.hitter != sortedHitCollisionInvoke2.hitter || sortedHitCollisionInvoke.receiver != sortedHitCollisionInvoke2.receiver)
						{
							if (sortedHitCollisionInvoke.hitter != sortedHitCollisionInvoke2.receiver || sortedHitCollisionInvoke.receiver != sortedHitCollisionInvoke2.hitter)
							{
								this.hitResults.RemoveAt(j);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x00072DB4 File Offset: 0x000711B4
	public void TickFrame()
	{
		this.hitResults.Clear();
		for (int i = 0; i < this.model.hitOwnersActiveThisFrame.Count; i++)
		{
			IHitOwner source = this.model.hitOwnersActiveThisFrame[i];
			this.checkCollision(source);
		}
		this.hitResults.Sort(this.hitResultSortFn);
		this.cullOverlappingGrabs();
		int num = 0;
		foreach (HitCollisionCalculator.ISortedHitCollisionInvoke sortedHitCollisionInvoke in this.hitResults)
		{
			if (sortedHitCollisionInvoke.IsHitSuccess())
			{
				num++;
			}
		}
		foreach (HitCollisionCalculator.ISortedHitCollisionInvoke sortedHitCollisionInvoke2 in this.hitResults)
		{
			sortedHitCollisionInvoke2.hitContext.totalHitSuccess = num;
			sortedHitCollisionInvoke2.Invoke();
		}
		this.model.hitOwnersActiveThisFrame.Clear();
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x00072EE4 File Offset: 0x000712E4
	public void QueueCollisionCheck(IHitOwner owner)
	{
		this.model.hitOwnersActiveThisFrame.Add(owner);
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x00072EF8 File Offset: 0x000712F8
	private void checkCollision(IHitOwner source)
	{
		List<IHitOwner> relevantHitOwners = this.GetRelevantHitOwners(source);
		for (int i = relevantHitOwners.Count - 1; i >= 0; i--)
		{
			IHitOwner owner = relevantHitOwners[i];
			this.collisions.CheckCollision(source, owner, this.hitResults);
		}
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x00072F44 File Offset: 0x00071344
	public int CheckBodyOverlap(IBoundsOwner source)
	{
		if (!source.AllowPushing)
		{
			return 0;
		}
		for (int i = this.model.boundsOwners.Count - 1; i >= 0; i--)
		{
			IBoundsOwner boundsOwner = this.model.boundsOwners[i];
			if (boundsOwner != source && boundsOwner.AllowPushing)
			{
				bool flag = source.Bounds.getRect(source.Position).Overlaps(boundsOwner.Bounds.getRect(boundsOwner.Position));
				if (flag)
				{
					return (!(boundsOwner.Position.x < source.Position.x)) ? -1 : 1;
				}
			}
		}
		return 0;
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x0007300C File Offset: 0x0007140C
	public List<IHitOwner> GetRelevantHitOwners(IHitOwner source)
	{
		return this.hitOwners;
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x00073014 File Offset: 0x00071414
	public void Register(IHitOwner owner)
	{
		if (!this.hitOwners.Contains(owner))
		{
			owner.HitOwnerID = this.model.nextUID;
			this.model.nextUID++;
			this.hitOwners.Add(owner);
			if (owner is IBoundsOwner)
			{
				IBoundsOwner item = owner as IBoundsOwner;
				if (!this.model.boundsOwners.Contains(item))
				{
					this.model.boundsOwners.Add(item);
				}
				else
				{
					Debug.LogWarning("Attempted to add duplicate bounds owner");
				}
			}
		}
		else
		{
			Debug.LogWarning("Attempted to add a duplicate hit owner");
		}
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x000730BC File Offset: 0x000714BC
	public void Unregister(IHitOwner owner)
	{
		if (this.hitOwners.Contains(owner))
		{
			this.hitOwners.Remove(owner);
			if (owner is IBoundsOwner)
			{
				IBoundsOwner item = owner as IBoundsOwner;
				if (this.model.boundsOwners.Contains(item))
				{
					this.model.boundsOwners.Remove(item);
				}
				else
				{
					Debug.LogWarning("Attempted to remove unknown bounds owner");
				}
			}
		}
		else
		{
			Debug.LogWarning("Attempted to remove an unknown hitowner");
		}
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x00073140 File Offset: 0x00071540
	private void onCharacterInit(GameEvent message)
	{
		CharacterInitEvent characterInitEvent = message as CharacterInitEvent;
		this.Register(characterInitEvent.character);
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x00073160 File Offset: 0x00071560
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<HitsManagerModel>(ref this.model);
		return true;
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x00073170 File Offset: 0x00071570
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<HitsManagerModel>(this.model));
	}

	// Token: 0x04000DB6 RID: 3510
	private HitsManagerModel model = new HitsManagerModel();

	// Token: 0x04000DB7 RID: 3511
	private HitCollisionCalculator collisions = new HitCollisionCalculator();

	// Token: 0x04000DB8 RID: 3512
	private List<HitCollisionCalculator.ISortedHitCollisionInvoke> hitResults = new List<HitCollisionCalculator.ISortedHitCollisionInvoke>();

	// Token: 0x04000DB9 RID: 3513
	private Comparison<HitCollisionCalculator.ISortedHitCollisionInvoke> hitResultSortFn;
}
