// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : ITickable
{
	public const int DEFAULT_POOL_SIZE = 4;

	private Dictionary<string, GameObjectPool> pools = new Dictionary<string, GameObjectPool>();

	private Transform parentTransform;

	private Transform inactiveTransform;

	[Inject]
	public IDependencyInjection dependencyInjector
	{
		get;
		set;
	}

	public Transform ParentTransform
	{
		get
		{
			return this.parentTransform;
		}
	}

	public Transform InactiveTransform
	{
		get
		{
			return this.inactiveTransform;
		}
	}

	public void Init(Transform parentTransform, Transform inactiveTransform)
	{
		this.parentTransform = parentTransform;
		this.inactiveTransform = inactiveTransform;
	}

	public GameObject GetObject(GameObject prefab, Type componentType, string keyid, int poolSize = 4, bool sanityCheck = true)
	{
		string key = prefab.name + keyid;
		if (!this.pools.ContainsKey(key))
		{
			GameObjectPool instance = this.dependencyInjector.GetInstance<GameObjectPool>();
			instance.Init(prefab, componentType, poolSize, this.parentTransform, this.inactiveTransform, sanityCheck);
			this.pools.Add(key, instance);
		}
		return this.pools[key].AcquireGameObject();
	}

	public void TickFrame()
	{
		foreach (KeyValuePair<string, GameObjectPool> current in this.pools)
		{
			current.Value.TickFrame();
		}
	}

	public string GenerateDebugString(bool verbose = false)
	{
		string text = string.Empty;
		int num = 0;
		foreach (string current in this.pools.Keys)
		{
			if (verbose)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n",
					current,
					this.pools[current].Count
				});
			}
			num += this.pools[current].Count;
		}
		text = string.Concat(new object[]
		{
			"Total: ",
			num,
			" ",
			text
		});
		return text;
	}
}
