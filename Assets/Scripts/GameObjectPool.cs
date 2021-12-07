// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameObjectPool : ITickable, ICountOwner
{
	private class IndexCooloff
	{
		public int poolIndex;

		public int framesRemaining;
	}

	private sealed class _AcquirePooledGameObject_c__AnonStorey0
	{
		internal PooledGameObject pooled;

		internal bool __m__0(GameObjectPool.IndexCooloff x)
		{
			return x.poolIndex == this.pooled.PoolIndex;
		}
	}

	private List<PooledGameObject> pooledObjects = new List<PooledGameObject>();

	private Queue<int> freeIndexQueue = new Queue<int>();

	private List<GameObjectPool.IndexCooloff> cooloffQueue = new List<GameObjectPool.IndexCooloff>();

	private Transform parentTransform;

	private Transform inactiveTransform;

	private GameObject cachedPrefab;

	private Type cachedComponentType;

	private bool sanityCheck;

	[Inject]
	public GameController gameController
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

	public int Capacity
	{
		get;
		private set;
	}

	public int Count
	{
		get
		{
			return this.pooledObjects.Count;
		}
	}

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

	public void ExpandPoolToSize(int newCapacity)
	{
		if (newCapacity <= this.Capacity)
		{
			UnityEngine.Debug.LogWarning("Attempt to reduce size of pool which is not supported.");
			return;
		}
		if (this.Capacity >= newCapacity)
		{
			UnityEngine.Debug.LogWarning("Trying to expand pool that is already large enough");
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

	private void acquirePooledGameObject(PooledGameObject pooled)
	{
		pooled.gameObject.SetActive(true);
		pooled.gameObject.transform.SetParent(this.parentTransform);
		((IPoolListener)pooled).OnAcquired();
	}

	public GameObject AcquireGameObject()
	{
		if (this.freeIndexQueue.Count == 0)
		{
			int num = this.Capacity * 2;
			if (SystemBoot.mode != SystemBoot.Mode.StagePreview)
			{
				UnityEngine.Debug.LogError(string.Format("Ran out of pooled objects of prefab {0}, increasing pool size from {1} to {2}", this.cachedPrefab.name, this.Capacity, num));
			}
			this.ExpandPoolToSize(num);
		}
		int index = this.freeIndexQueue.Dequeue();
		PooledGameObject pooledGameObject = this.pooledObjects[index];
		this.acquirePooledGameObject(pooledGameObject);
		return pooledGameObject.gameObject;
	}

	public bool AcquirePooledGameObject(PooledGameObject pooled)
	{
		GameObjectPool._AcquirePooledGameObject_c__AnonStorey0 _AcquirePooledGameObject_c__AnonStorey = new GameObjectPool._AcquirePooledGameObject_c__AnonStorey0();
		_AcquirePooledGameObject_c__AnonStorey.pooled = pooled;
		if (_AcquirePooledGameObject_c__AnonStorey.pooled.Pool != this || !_AcquirePooledGameObject_c__AnonStorey.pooled.IsInPool)
		{
			return false;
		}
		this.cooloffQueue.RemoveLast(new Predicate<GameObjectPool.IndexCooloff>(_AcquirePooledGameObject_c__AnonStorey.__m__0));
		this.acquirePooledGameObject(_AcquirePooledGameObject_c__AnonStorey.pooled);
		return true;
	}

	public void ReleaseGameObject(GameObject obj)
	{
		PooledGameObject component = obj.GetComponent<PooledGameObject>();
		if (component == null)
		{
			UnityEngine.Debug.LogError("Attempting to release a GameObject that does not contain a PooledGameObject component.");
			return;
		}
		this.ReleaseGameObject(component);
	}

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
}
