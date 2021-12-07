using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B50 RID: 2896
public class PooledGameObject : MonoBehaviour, IPoolListener
{
	// Token: 0x17001371 RID: 4977
	// (get) Token: 0x060053FB RID: 21499 RVA: 0x001B0BA5 File Offset: 0x001AEFA5
	public bool IsInPool
	{
		get
		{
			return this.isInPool;
		}
	}

	// Token: 0x17001372 RID: 4978
	// (get) Token: 0x060053FC RID: 21500 RVA: 0x001B0BAD File Offset: 0x001AEFAD
	public GameObjectPool Pool
	{
		get
		{
			return this.pool;
		}
	}

	// Token: 0x17001373 RID: 4979
	// (get) Token: 0x060053FD RID: 21501 RVA: 0x001B0BB5 File Offset: 0x001AEFB5
	public int PoolIndex
	{
		get
		{
			return this.poolIndex;
		}
	}

	// Token: 0x060053FE RID: 21502 RVA: 0x001B0BBD File Offset: 0x001AEFBD
	public void Init(GameObjectPool pool, bool sanityCheck, int poolIndex)
	{
		this.pool = pool;
		this.sanityCheck = sanityCheck;
		this.poolIndex = poolIndex;
	}

	// Token: 0x060053FF RID: 21503 RVA: 0x001B0BD4 File Offset: 0x001AEFD4
	private void Start()
	{
		if (this.sanityCheck && !base.gameObject.GetComponentInChildren(typeof(ITickable)))
		{
			Debug.LogWarning("Attempting to pool an object that won't get ticked. Are you sure you know what you're doing?");
		}
	}

	// Token: 0x06005400 RID: 21504 RVA: 0x001B0C0A File Offset: 0x001AF00A
	private void Update()
	{
		if (this.isInPool && base.gameObject.activeInHierarchy)
		{
			Debug.LogWarning(string.Format("GameObject {0} is marked as in pool but is currently active.", base.gameObject.name));
		}
	}

	// Token: 0x06005401 RID: 21505 RVA: 0x001B0C41 File Offset: 0x001AF041
	public void Acquire()
	{
		if (!this.isInPool)
		{
			return;
		}
		if (!this.pool.AcquirePooledGameObject(this))
		{
			Debug.LogError(string.Format("Error when attempting to acquire PooledGameObject {0}", base.gameObject.name));
		}
	}

	// Token: 0x06005402 RID: 21506 RVA: 0x001B0C7A File Offset: 0x001AF07A
	public void Release()
	{
		if (this.isInPool)
		{
			return;
		}
		this.pool.ReleaseGameObject(base.gameObject);
	}

	// Token: 0x06005403 RID: 21507 RVA: 0x001B0C9C File Offset: 0x001AF09C
	void IPoolListener.OnAcquired()
	{
		this.isInPool = false;
		foreach (IPooledGameObjectListener pooledGameObjectListener in this.listeners)
		{
			pooledGameObjectListener.OnAcquired();
		}
	}

	// Token: 0x06005404 RID: 21508 RVA: 0x001B0D00 File Offset: 0x001AF100
	void IPoolListener.OnCooledOff()
	{
		foreach (IPooledGameObjectListener pooledGameObjectListener in this.listeners)
		{
			pooledGameObjectListener.OnCooledOff();
		}
	}

	// Token: 0x06005405 RID: 21509 RVA: 0x001B0D5C File Offset: 0x001AF15C
	void IPoolListener.OnReleased()
	{
		this.isInPool = true;
		foreach (IPooledGameObjectListener pooledGameObjectListener in this.listeners)
		{
			pooledGameObjectListener.OnReleased();
		}
	}

	// Token: 0x06005406 RID: 21510 RVA: 0x001B0DC0 File Offset: 0x001AF1C0
	public void RegisterListener(IPooledGameObjectListener listener)
	{
		this.listeners.Add(listener);
		if (!this.IsInPool)
		{
			listener.OnAcquired();
		}
	}

	// Token: 0x06005407 RID: 21511 RVA: 0x001B0DE0 File Offset: 0x001AF1E0
	public void UnregisterListener(IPooledGameObjectListener listener)
	{
		this.listeners.Remove(listener);
	}

	// Token: 0x0400354E RID: 13646
	private GameObjectPool pool;

	// Token: 0x0400354F RID: 13647
	[SerializeField]
	[ReadOnly]
	private bool sanityCheck;

	// Token: 0x04003550 RID: 13648
	[SerializeField]
	[ReadOnly]
	private int poolIndex = -1;

	// Token: 0x04003551 RID: 13649
	[SerializeField]
	[ReadOnly]
	private bool isInPool = true;

	// Token: 0x04003552 RID: 13650
	private HashSet<IPooledGameObjectListener> listeners = new HashSet<IPooledGameObjectListener>();
}
