// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class PooledGameObject : MonoBehaviour, IPoolListener
{
	private GameObjectPool pool;

	[ReadOnly, SerializeField]
	private bool sanityCheck;

	[ReadOnly, SerializeField]
	private int poolIndex = -1;

	[ReadOnly, SerializeField]
	private bool isInPool = true;

	private HashSet<IPooledGameObjectListener> listeners = new HashSet<IPooledGameObjectListener>();

	public bool IsInPool
	{
		get
		{
			return this.isInPool;
		}
	}

	public GameObjectPool Pool
	{
		get
		{
			return this.pool;
		}
	}

	public int PoolIndex
	{
		get
		{
			return this.poolIndex;
		}
	}

	public void Init(GameObjectPool pool, bool sanityCheck, int poolIndex)
	{
		this.pool = pool;
		this.sanityCheck = sanityCheck;
		this.poolIndex = poolIndex;
	}

	private void Start()
	{
		if (this.sanityCheck && !base.gameObject.GetComponentInChildren(typeof(ITickable)))
		{
			UnityEngine.Debug.LogWarning("Attempting to pool an object that won't get ticked. Are you sure you know what you're doing?");
		}
	}

	private void Update()
	{
		if (this.isInPool && base.gameObject.activeInHierarchy)
		{
			UnityEngine.Debug.LogWarning(string.Format("GameObject {0} is marked as in pool but is currently active.", base.gameObject.name));
		}
	}

	public void Acquire()
	{
		if (!this.isInPool)
		{
			return;
		}
		if (!this.pool.AcquirePooledGameObject(this))
		{
			UnityEngine.Debug.LogError(string.Format("Error when attempting to acquire PooledGameObject {0}", base.gameObject.name));
		}
	}

	public void Release()
	{
		if (this.isInPool)
		{
			return;
		}
		this.pool.ReleaseGameObject(base.gameObject);
	}

	void IPoolListener.OnAcquired()
	{
		this.isInPool = false;
		foreach (IPooledGameObjectListener current in this.listeners)
		{
			current.OnAcquired();
		}
	}

	void IPoolListener.OnCooledOff()
	{
		foreach (IPooledGameObjectListener current in this.listeners)
		{
			current.OnCooledOff();
		}
	}

	void IPoolListener.OnReleased()
	{
		this.isInPool = true;
		foreach (IPooledGameObjectListener current in this.listeners)
		{
			current.OnReleased();
		}
	}

	public void RegisterListener(IPooledGameObjectListener listener)
	{
		this.listeners.Add(listener);
		if (!this.IsInPool)
		{
			listener.OnAcquired();
		}
	}

	public void UnregisterListener(IPooledGameObjectListener listener)
	{
		this.listeners.Remove(listener);
	}
}
