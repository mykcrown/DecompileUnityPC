// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HitCollisionManager : ITickable, IRollbackStateOwner
{
	private HitsManagerModel model = new HitsManagerModel();

	private HitCollisionCalculator collisions = new HitCollisionCalculator();

	private List<HitCollisionCalculator.ISortedHitCollisionInvoke> hitResults = new List<HitCollisionCalculator.ISortedHitCollisionInvoke>();

	private Comparison<HitCollisionCalculator.ISortedHitCollisionInvoke> hitResultSortFn;

	private static Comparison<HitCollisionCalculator.ISortedHitCollisionInvoke> __f__am_cache0;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	private List<IHitOwner> hitOwners
	{
		get
		{
			return this.model.hitOwners;
		}
	}

	public HitCollisionManager()
	{
		if (HitCollisionManager.__f__am_cache0 == null)
		{
			HitCollisionManager.__f__am_cache0 = new Comparison<HitCollisionCalculator.ISortedHitCollisionInvoke>(HitCollisionManager._HitCollisionManager_m__0);
		}
		this.hitResultSortFn = HitCollisionManager.__f__am_cache0;
	}

	[PostConstruct]
	public void Init()
	{
		this.injector.Inject(this.collisions);
		this.events.Subscribe(typeof(CharacterInitEvent), new Events.EventHandler(this.onCharacterInit));
	}

	public void OnDestroy()
	{
		this.events.Unsubscribe(typeof(CharacterInitEvent), new Events.EventHandler(this.onCharacterInit));
	}

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
		foreach (HitCollisionCalculator.ISortedHitCollisionInvoke current in this.hitResults)
		{
			if (current.IsHitSuccess())
			{
				num++;
			}
		}
		foreach (HitCollisionCalculator.ISortedHitCollisionInvoke current2 in this.hitResults)
		{
			current2.hitContext.totalHitSuccess = num;
			current2.Invoke();
		}
		this.model.hitOwnersActiveThisFrame.Clear();
	}

	public void QueueCollisionCheck(IHitOwner owner)
	{
		this.model.hitOwnersActiveThisFrame.Add(owner);
	}

	private void checkCollision(IHitOwner source)
	{
		List<IHitOwner> relevantHitOwners = this.GetRelevantHitOwners(source);
		for (int i = relevantHitOwners.Count - 1; i >= 0; i--)
		{
			IHitOwner owner = relevantHitOwners[i];
			this.collisions.CheckCollision(source, owner, this.hitResults);
		}
	}

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
					return (!(boundsOwner.Position.x < source.Position.x)) ? (-1) : 1;
				}
			}
		}
		return 0;
	}

	public List<IHitOwner> GetRelevantHitOwners(IHitOwner source)
	{
		return this.hitOwners;
	}

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
					UnityEngine.Debug.LogWarning("Attempted to add duplicate bounds owner");
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("Attempted to add a duplicate hit owner");
		}
	}

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
					UnityEngine.Debug.LogWarning("Attempted to remove unknown bounds owner");
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("Attempted to remove an unknown hitowner");
		}
	}

	private void onCharacterInit(GameEvent message)
	{
		CharacterInitEvent characterInitEvent = message as CharacterInitEvent;
		this.Register(characterInitEvent.character);
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<HitsManagerModel>(ref this.model);
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<HitsManagerModel>(this.model));
	}

	private static int _HitCollisionManager_m__0(HitCollisionCalculator.ISortedHitCollisionInvoke a, HitCollisionCalculator.ISortedHitCollisionInvoke b)
	{
		if (a.impactType == ImpactType.Grab && b.impactType == ImpactType.Grab && (a.hitter != b.hitter || a.receiver != b.receiver) && (a.hitter != b.receiver || a.receiver != b.hitter))
		{
			return (int)((a.hitter.Position - a.receiver.Position).sqrMagnitude - (b.hitter.Position - b.receiver.Position).sqrMagnitude);
		}
		return a.Priority() - b.Priority();
	}
}
