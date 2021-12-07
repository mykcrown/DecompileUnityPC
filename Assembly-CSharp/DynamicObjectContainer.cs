using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class DynamicObjectContainer : MonoBehaviour, ITickable, IRollbackStateOwner
{
	// Token: 0x0600160F RID: 5647 RVA: 0x00077D74 File Offset: 0x00076174
	public DynamicObjectContainer()
	{
		this.sortEffectFirst = delegate(ITickable t1, ITickable t2)
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
		};
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06001610 RID: 5648 RVA: 0x00077DDD File Offset: 0x000761DD
	// (set) Token: 0x06001611 RID: 5649 RVA: 0x00077DE5 File Offset: 0x000761E5
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06001612 RID: 5650 RVA: 0x00077DEE File Offset: 0x000761EE
	// (set) Token: 0x06001613 RID: 5651 RVA: 0x00077DF6 File Offset: 0x000761F6
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06001614 RID: 5652 RVA: 0x00077DFF File Offset: 0x000761FF
	// (set) Token: 0x06001615 RID: 5653 RVA: 0x00077E07 File Offset: 0x00076207
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x06001616 RID: 5654 RVA: 0x00077E10 File Offset: 0x00076210
	[PostConstruct]
	public void Init()
	{
		this.poolingEnabled = !this.devConfig.disableAllPooling;
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x00077E28 File Offset: 0x00076228
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

	// Token: 0x06001618 RID: 5656 RVA: 0x00077EAA File Offset: 0x000762AA
	private bool checkDead(ITickable tickable)
	{
		return tickable is IExpirable && (tickable as IExpirable).IsExpired;
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x00077EC4 File Offset: 0x000762C4
	public T InstantiateDynamicObject<T>(GameObject prefab, int poolSize = 4, bool sanityCheck = true) where T : MonoBehaviour
	{
		GameObject gameObject = this.instantiateDynamicObject<T>(prefab, poolSize, sanityCheck);
		this.AddDynamicObject(gameObject);
		return gameObject.GetComponent<T>();
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x00077EE8 File Offset: 0x000762E8
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

	// Token: 0x0600161B RID: 5659 RVA: 0x00077F48 File Offset: 0x00076348
	public GameObject PreloadDynamicObject<T>(GameObject prefab, int poolSize = 4) where T : Component, IPooledComponent, new()
	{
		if (prefab == null)
		{
			return null;
		}
		T t;
		return this.createDynamicObject<T>(prefab, out t, poolSize);
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x00077F70 File Offset: 0x00076370
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

	// Token: 0x0600161D RID: 5661 RVA: 0x00077FAC File Offset: 0x000763AC
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
					genericResetObjectPool = new GenericResetObjectPool<T>(0, delegate()
					{
						GameObject gameObject3 = new GameObject("ComponentContainer");
						gameObject3.transform.SetParent(base.transform);
						return gameObject3.AddComponent<T>();
					}, null);
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

	// Token: 0x0600161E RID: 5662 RVA: 0x00078140 File Offset: 0x00076540
	public void AddDynamicObject(GameObject child)
	{
		if (child == null)
		{
			Debug.LogWarning("Attempted to add a null child object to the DynamicObjectContainer");
			return;
		}
		ITickable[] componentsInChildren = child.GetComponentsInChildren<ITickable>();
		Array.Sort<ITickable>(componentsInChildren, this.sortEffectFirst);
		foreach (ITickable tickable in componentsInChildren)
		{
			this.state.tickables.Add(tickable);
			this.map[tickable] = child;
		}
		child.transform.SetParent(base.transform);
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x000781C0 File Offset: 0x000765C0
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<DynamicObjectContainerState>(this.state));
		foreach (ITickable tickable in this.state.tickables)
		{
			if (tickable is IRollbackStateOwner)
			{
				((IRollbackStateOwner)tickable).ExportState(ref container);
			}
		}
		return true;
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x0007824C File Offset: 0x0007664C
	public bool LoadState(RollbackStateContainer container)
	{
		this.state.CopyTo(this.cachedState);
		container.ReadState<DynamicObjectContainerState>(ref this.state);
		foreach (ITickable tickable in this.state.tickables)
		{
			if (tickable is IRollbackStateOwner)
			{
				((IRollbackStateOwner)tickable).LoadState(container);
			}
		}
		foreach (ITickable tickable2 in this.cachedState.tickables)
		{
			if (!this.state.tickables.Contains(tickable2) && this.map.ContainsKey(tickable2))
			{
				this.map[tickable2].DestroySafe();
				this.map.Remove(tickable2);
			}
		}
		foreach (ITickable tickable3 in this.state.tickables)
		{
			if (!this.cachedState.tickables.Contains(tickable3) && tickable3 is IRollbackStateOwner)
			{
				MonoBehaviour monoBehaviour = tickable3 as MonoBehaviour;
				this.map[tickable3] = monoBehaviour.gameObject;
				PooledGameObject component = monoBehaviour.gameObject.GetComponent<PooledGameObject>();
				if (component != null)
				{
					component.Acquire();
				}
				this.map[tickable3].SetActive(true);
			}
		}
		return true;
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x0007842C File Offset: 0x0007682C
	public string GenerateDebugString(bool verbose = false)
	{
		string text = string.Empty;
		int num = 0;
		text = text + "Tickable map: " + this.map.Count;
		text = text + "\nPools: " + this.componentPoolMap.Count;
		foreach (Type type in this.componentPoolMap.Keys)
		{
			int count = ((ICountOwner)this.componentPoolMap[type]).Count;
			if (verbose)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n",
					type.ToString(),
					": ",
					count
				});
			}
			num += count;
		}
		text = text + "\nTotal: " + num;
		return text;
	}

	// Token: 0x040010FB RID: 4347
	private DynamicObjectContainerState state = new DynamicObjectContainerState();

	// Token: 0x040010FC RID: 4348
	private DynamicObjectContainerState cachedState = new DynamicObjectContainerState();

	// Token: 0x040010FD RID: 4349
	private Dictionary<ITickable, GameObject> map = new Dictionary<ITickable, GameObject>();

	// Token: 0x040010FE RID: 4350
	private Dictionary<Type, object> componentPoolMap = new Dictionary<Type, object>();

	// Token: 0x040010FF RID: 4351
	private bool poolingEnabled = true;

	// Token: 0x04001100 RID: 4352
	private Comparison<ITickable> sortEffectFirst;
}
