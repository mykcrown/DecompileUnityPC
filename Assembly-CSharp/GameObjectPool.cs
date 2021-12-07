using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B46 RID: 2886
public class GameObjectPool : ITickable, ICountOwner
{
	// Token: 0x17001367 RID: 4967
	// (get) Token: 0x060053CD RID: 21453 RVA: 0x001B0315 File Offset: 0x001AE715
	// (set) Token: 0x060053CE RID: 21454 RVA: 0x001B031D File Offset: 0x001AE71D
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17001368 RID: 4968
	// (get) Token: 0x060053CF RID: 21455 RVA: 0x001B0326 File Offset: 0x001AE726
	// (set) Token: 0x060053D0 RID: 21456 RVA: 0x001B032E File Offset: 0x001AE72E
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17001369 RID: 4969
	// (get) Token: 0x060053D1 RID: 21457 RVA: 0x001B0337 File Offset: 0x001AE737
	// (set) Token: 0x060053D2 RID: 21458 RVA: 0x001B033F File Offset: 0x001AE73F
	public int Capacity { get; private set; }

	// Token: 0x1700136A RID: 4970
	// (get) Token: 0x060053D3 RID: 21459 RVA: 0x001B0348 File Offset: 0x001AE748
	public int Count
	{
		get
		{
			return this.pooledObjects.Count;
		}
	}

	// Token: 0x1700136B RID: 4971
	// (get) Token: 0x060053D4 RID: 21460 RVA: 0x001B0358 File Offset: 0x001AE758
	private int CooloffFrames
	{
		get
		{
			if (this.gameController.currentGame != null && this.gameController.currentGame.FrameController != null && this.gameController.currentGame.FrameController.IsRollback)
			{
				return RollbackStatePoolContainer.ROLLBACK_FRAMES;
			}
			return 0;
		}
	}

	// Token: 0x060053D5 RID: 21461 RVA: 0x001B03B7 File Offset: 0x001AE7B7
	public void Init(GameObject prefab, Type componentType, int capacity, Transform parentTransform, Transform inactiveTransform, bool sanityCheck = true)
	{
		this.cachedPrefab = prefab;
		this.cachedComponentType = componentType;
		this.parentTransform = parentTransform;
		this.inactiveTransform = inactiveTransform;
		this.sanityCheck = sanityCheck;
		this.Capacity = 0;
		this.ExpandPoolToSize(capacity);
	}

	// Token: 0x060053D6 RID: 21462 RVA: 0x001B03F0 File Offset: 0x001AE7F0
	public void ExpandPoolToSize(int newCapacity)
	{
		if (newCapacity <= this.Capacity)
		{
			Debug.LogWarning("Attempt to reduce size of pool which is not supported.");
			return;
		}
		if (this.Capacity >= newCapacity)
		{
			Debug.LogWarning("Trying to expand pool that is already large enough");
			return;
		}
		for (int i = this.Capacity; i < newCapacity; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.cachedPrefab);
			object obj = null;
			PooledGameObject pooledGameObject = gameObject.AddComponent<PooledGameObject>();
			pooledGameObject.Init(this, this.sanityCheck, i);
			if (this.cachedComponentType != null)
			{
				obj = gameObject.GetComponent(this.cachedComponentType);
				if (obj == null)
				{
					obj = gameObject.AddComponent(this.cachedComponentType);
				}
				this.injector.Inject(obj);
				if (obj is IEffectOwner)
				{
					Effect effect = gameObject.GetComponent<Effect>() ?? gameObject.AddComponent<Effect>();
					this.injector.Inject(effect);
					IEffectOwner effectOwner = obj as IEffectOwner;
					effectOwner.SetEffect(effect);
					pooledGameObject.RegisterListener(effect);
				}
			}
			gameObject.transform.SetParent(this.inactiveTransform, false);
			gameObject.SetActive(false);
			if (obj != null && obj is IPooledGameObjectListener)
			{
				pooledGameObject.RegisterListener(obj as IPooledGameObjectListener);
			}
			this.pooledObjects.Add(pooledGameObject);
			this.freeIndexQueue.Enqueue(i);
		}
		this.Capacity = newCapacity;
	}

	// Token: 0x060053D7 RID: 21463 RVA: 0x001B0540 File Offset: 0x001AE940
	private void acquirePooledGameObject(PooledGameObject pooled)
	{
		pooled.gameObject.SetActive(true);
		pooled.gameObject.transform.SetParent(this.parentTransform);
		((IPoolListener)pooled).OnAcquired();
	}

	// Token: 0x060053D8 RID: 21464 RVA: 0x001B0578 File Offset: 0x001AE978
	public GameObject AcquireGameObject()
	{
		if (this.freeIndexQueue.Count == 0)
		{
			int num = this.Capacity * 2;
			if (SystemBoot.mode != SystemBoot.Mode.StagePreview)
			{
				Debug.LogError(string.Format("Ran out of pooled objects of prefab {0}, increasing pool size from {1} to {2}", this.cachedPrefab.name, this.Capacity, num));
			}
			this.ExpandPoolToSize(num);
		}
		int index = this.freeIndexQueue.Dequeue();
		PooledGameObject pooledGameObject = this.pooledObjects[index];
		this.acquirePooledGameObject(pooledGameObject);
		return pooledGameObject.gameObject;
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x001B0604 File Offset: 0x001AEA04
	public bool AcquirePooledGameObject(PooledGameObject pooled)
	{
		if (pooled.Pool != this || !pooled.IsInPool)
		{
			return false;
		}
		this.cooloffQueue.RemoveLast((GameObjectPool.IndexCooloff x) => x.poolIndex == pooled.PoolIndex);
		this.acquirePooledGameObject(pooled);
		return true;
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x001B0668 File Offset: 0x001AEA68
	public void ReleaseGameObject(GameObject obj)
	{
		PooledGameObject component = obj.GetComponent<PooledGameObject>();
		if (component == null)
		{
			Debug.LogError("Attempting to release a GameObject that does not contain a PooledGameObject component.");
			return;
		}
		this.ReleaseGameObject(component);
	}

	// Token: 0x060053DB RID: 21467 RVA: 0x001B069C File Offset: 0x001AEA9C
	public void ReleaseGameObject(PooledGameObject pooled)
	{
		((IPoolListener)pooled).OnReleased();
		if (this.CooloffFrames > 0)
		{
			this.cooloffQueue.Add(new GameObjectPool.IndexCooloff
			{
				poolIndex = pooled.PoolIndex,
				framesRemaining = this.CooloffFrames
			});
		}
		else
		{
			((IPoolListener)pooled).OnCooledOff();
			this.freeIndexQueue.Enqueue(pooled.PoolIndex);
		}
		pooled.gameObject.SetActive(false);
		pooled.gameObject.transform.SetParent(this.inactiveTransform, false);
	}

	// Token: 0x060053DC RID: 21468 RVA: 0x001B0728 File Offset: 0x001AEB28
	public void Clear()
	{
		for (int i = this.pooledObjects.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = this.pooledObjects[i].gameObject;
			UnityEngine.Object.Destroy(gameObject);
		}
		this.pooledObjects.Clear();
		this.freeIndexQueue.Clear();
	}

	// Token: 0x060053DD RID: 21469 RVA: 0x001B0784 File Offset: 0x001AEB84
	public void TickFrame()
	{
		int num = 0;
		for (int i = 0; i < this.cooloffQueue.Count; i++)
		{
			if (this.cooloffQueue[i].framesRemaining > 0)
			{
				this.cooloffQueue[i].framesRemaining--;
			}
			else
			{
				num++;
				this.freeIndexQueue.Enqueue(this.cooloffQueue[i].poolIndex);
				((IPoolListener)this.pooledObjects[this.cooloffQueue[i].poolIndex]).OnCooledOff();
			}
		}
		this.cooloffQueue.RemoveRange(0, num);
	}

	// Token: 0x04003537 RID: 13623
	private List<PooledGameObject> pooledObjects = new List<PooledGameObject>();

	// Token: 0x04003538 RID: 13624
	private Queue<int> freeIndexQueue = new Queue<int>();

	// Token: 0x04003539 RID: 13625
	private List<GameObjectPool.IndexCooloff> cooloffQueue = new List<GameObjectPool.IndexCooloff>();

	// Token: 0x0400353A RID: 13626
	private Transform parentTransform;

	// Token: 0x0400353B RID: 13627
	private Transform inactiveTransform;

	// Token: 0x0400353C RID: 13628
	private GameObject cachedPrefab;

	// Token: 0x0400353D RID: 13629
	private Type cachedComponentType;

	// Token: 0x0400353E RID: 13630
	private bool sanityCheck;

	// Token: 0x02000B47 RID: 2887
	private class IndexCooloff
	{
		// Token: 0x04003540 RID: 13632
		public int poolIndex;

		// Token: 0x04003541 RID: 13633
		public int framesRemaining;
	}
}
