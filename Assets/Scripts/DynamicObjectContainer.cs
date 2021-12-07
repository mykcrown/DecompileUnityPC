// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DynamicObjectContainer : MonoBehaviour, ITickable, IRollbackStateOwner
{
	private DynamicObjectContainerState state = new DynamicObjectContainerState();

	private DynamicObjectContainerState cachedState = new DynamicObjectContainerState();

	private Dictionary<ITickable, GameObject> map = new Dictionary<ITickable, GameObject>();

	private Dictionary<Type, object> componentPoolMap = new Dictionary<Type, object>();

	private bool poolingEnabled = true;

	private Comparison<ITickable> sortEffectFirst;

	private static Comparison<ITickable> __f__am_cache0;

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
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

	public DynamicObjectContainer()
	{
		if (DynamicObjectContainer.__f__am_cache0 == null)
		{
			DynamicObjectContainer.__f__am_cache0 = new Comparison<ITickable>(DynamicObjectContainer._DynamicObjectContainer_m__0);
		}
		this.sortEffectFirst = DynamicObjectContainer.__f__am_cache0;
	}

	[PostConstruct]
	public void Init()
	{
		this.poolingEnabled = !this.devConfig.disableAllPooling;
	}

	public void TickFrame()
	{
		for (int i = this.state.tickables.Count - 1; i >= 0; i--)
		{
			ITickable tickable = this.state.tickables[i];
			bool flag = this.checkDead(tickable);
			if (!flag)
			{
				tickable.TickFrame();
				flag = this.checkDead(tickable);
			}
			if (flag)
			{
				this.map.Remove(tickable);
				this.state.tickables.RemoveAt(i);
			}
		}
	}

	private bool checkDead(ITickable tickable)
	{
		return tickable is IExpirable && (tickable as IExpirable).IsExpired;
	}

	public T InstantiateDynamicObject<T>(GameObject prefab, int poolSize = 4, bool sanityCheck = true) where T : MonoBehaviour
	{
		GameObject gameObject = this.instantiateDynamicObject<T>(prefab, poolSize, sanityCheck);
		this.AddDynamicObject(gameObject);
		return gameObject.GetComponent<T>();
	}

	private GameObject instantiateDynamicObject<T>(GameObject prefab, int poolSize = 4, bool sanityCheck = true) where T : MonoBehaviour
	{
		if (prefab == null)
		{
			return null;
		}
		GameObject result;
		if (this.poolingEnabled && poolSize > 0)
		{
			result = this.gameController.currentGame.ObjectPools.GetObject(prefab, typeof(T), null, poolSize, sanityCheck);
		}
		else
		{
			result = UnityEngine.Object.Instantiate<GameObject>(prefab);
		}
		return result;
	}

	public GameObject PreloadDynamicObject<T>(GameObject prefab, int poolSize = 4) where T : Component, IPooledComponent, new()
	{
		if (prefab == null)
		{
			return null;
		}
		T t;
		return this.createDynamicObject<T>(prefab, out t, poolSize);
	}

	public T InstantiateDynamicArticle<T>(GameObject prefab, out GameObject hostObject, int poolSize = 4) where T : Component, IPooledComponent, new()
	{
		if (prefab == null)
		{
			hostObject = null;
			return (T)((object)null);
		}
		T result;
		GameObject gameObject = this.createDynamicObject<T>(prefab, out result, poolSize);
		this.AddDynamicObject(gameObject);
		hostObject = gameObject;
		return result;
	}

	private GameObject createDynamicObject<T>(GameObject prefab, out T outComponent, int poolSize = 4) where T : Component, IPooledComponent, new()
	{
		GameObject gameObject;
		if (this.poolingEnabled && poolSize > 0)
		{
			gameObject = this.gameController.currentGame.ObjectPools.GetObject(prefab, typeof(T), typeof(T).ToString(), poolSize, true);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		}
		T t = gameObject.GetComponentInChildren<T>();
		bool flag = false;
		if (t == null)
		{
			if (this.poolingEnabled)
			{
				Type typeFromHandle = typeof(T);
				GenericResetObjectPool<T> genericResetObjectPool;
				if (this.componentPoolMap.ContainsKey(typeFromHandle))
				{
					genericResetObjectPool = (GenericResetObjectPool<T>)this.componentPoolMap[typeFromHandle];
				}
				else
				{
					genericResetObjectPool = new GenericResetObjectPool<T>(0, new GenericResetObjectPool<T>.NewCallback(this._createDynamicObject`1_m__1<T>), null);
					this.componentPoolMap[typeFromHandle] = genericResetObjectPool;
				}
				t = genericResetObjectPool.New();
			}
			else
			{
				GameObject gameObject2 = new GameObject("ComponentContainer");
				gameObject2.transform.SetParent(base.transform);
				t = gameObject2.AddComponent<T>();
			}
			flag = true;
		}
		else if (t is IResetable)
		{
			t.Reset();
			flag = true;
		}
		if (flag)
		{
			t.transform.SetParent(gameObject.transform);
			t.SetHost(gameObject);
			if (t is IEffectOwner)
			{
				Effect component = gameObject.GetComponent<Effect>();
				IEffectOwner effectOwner = t as IEffectOwner;
				effectOwner.SetEffect(component);
			}
		}
		outComponent = t;
		return gameObject;
	}

	public void AddDynamicObject(GameObject child)
	{
		if (child == null)
		{
			UnityEngine.Debug.LogWarning("Attempted to add a null child object to the DynamicObjectContainer");
			return;
		}
		ITickable[] componentsInChildren = child.GetComponentsInChildren<ITickable>();
		Array.Sort<ITickable>(componentsInChildren, this.sortEffectFirst);
		ITickable[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			ITickable tickable = array[i];
			this.state.tickables.Add(tickable);
			this.map[tickable] = child;
		}
		child.transform.SetParent(base.transform);
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<DynamicObjectContainerState>(this.state));
		foreach (ITickable current in this.state.tickables)
		{
			if (current is IRollbackStateOwner)
			{
				((IRollbackStateOwner)current).ExportState(ref container);
			}
		}
		return true;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		this.state.CopyTo(this.cachedState);
		container.ReadState<DynamicObjectContainerState>(ref this.state);
		foreach (ITickable current in this.state.tickables)
		{
			if (current is IRollbackStateOwner)
			{
				((IRollbackStateOwner)current).LoadState(container);
			}
		}
		foreach (ITickable current2 in this.cachedState.tickables)
		{
			if (!this.state.tickables.Contains(current2) && this.map.ContainsKey(current2))
			{
				this.map[current2].DestroySafe();
				this.map.Remove(current2);
			}
		}
		foreach (ITickable current3 in this.state.tickables)
		{
			if (!this.cachedState.tickables.Contains(current3) && current3 is IRollbackStateOwner)
			{
				MonoBehaviour monoBehaviour = current3 as MonoBehaviour;
				this.map[current3] = monoBehaviour.gameObject;
				PooledGameObject component = monoBehaviour.gameObject.GetComponent<PooledGameObject>();
				if (component != null)
				{
					component.Acquire();
				}
				this.map[current3].SetActive(true);
			}
		}
		return true;
	}

	public string GenerateDebugString(bool verbose = false)
	{
		string text = string.Empty;
		int num = 0;
		text = text + "Tickable map: " + this.map.Count;
		text = text + "\nPools: " + this.componentPoolMap.Count;
		foreach (Type current in this.componentPoolMap.Keys)
		{
			int count = ((ICountOwner)this.componentPoolMap[current]).Count;
			if (verbose)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n",
					current.ToString(),
					": ",
					count
				});
			}
			num += count;
		}
		text = text + "\nTotal: " + num;
		return text;
	}

	private static int _DynamicObjectContainer_m__0(ITickable t1, ITickable t2)
	{
		if (t1 is Effect && !(t2 is Effect))
		{
			return -1;
		}
		if (t2 is Effect && !(t1 is Effect))
		{
			return 1;
		}
		return 0;
	}

	private T _createDynamicObject<T>() where T : Component, IPooledComponent, new()
	{
		GameObject gameObject = new GameObject("ComponentContainer");
		gameObject.transform.SetParent(base.transform);
		return gameObject.AddComponent<T>();
	}
}
