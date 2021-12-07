using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B4F RID: 2895
public class ObjectPoolManager : ITickable
{
	// Token: 0x1700136E RID: 4974
	// (get) Token: 0x060053F2 RID: 21490 RVA: 0x001B099D File Offset: 0x001AED9D
	// (set) Token: 0x060053F3 RID: 21491 RVA: 0x001B09A5 File Offset: 0x001AEDA5
	[Inject]
	public IDependencyInjection dependencyInjector { get; set; }

	// Token: 0x1700136F RID: 4975
	// (get) Token: 0x060053F4 RID: 21492 RVA: 0x001B09AE File Offset: 0x001AEDAE
	public Transform ParentTransform
	{
		get
		{
			return this.parentTransform;
		}
	}

	// Token: 0x17001370 RID: 4976
	// (get) Token: 0x060053F5 RID: 21493 RVA: 0x001B09B6 File Offset: 0x001AEDB6
	public Transform InactiveTransform
	{
		get
		{
			return this.inactiveTransform;
		}
	}

	// Token: 0x060053F6 RID: 21494 RVA: 0x001B09BE File Offset: 0x001AEDBE
	public void Init(Transform parentTransform, Transform inactiveTransform)
	{
		this.parentTransform = parentTransform;
		this.inactiveTransform = inactiveTransform;
	}

	// Token: 0x060053F7 RID: 21495 RVA: 0x001B09D0 File Offset: 0x001AEDD0
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

	// Token: 0x060053F8 RID: 21496 RVA: 0x001B0A40 File Offset: 0x001AEE40
	public void TickFrame()
	{
		foreach (KeyValuePair<string, GameObjectPool> keyValuePair in this.pools)
		{
			keyValuePair.Value.TickFrame();
		}
	}

	// Token: 0x060053F9 RID: 21497 RVA: 0x001B0AA4 File Offset: 0x001AEEA4
	public string GenerateDebugString(bool verbose = false)
	{
		string text = string.Empty;
		int num = 0;
		foreach (string text2 in this.pools.Keys)
		{
			if (verbose)
			{
				string text3 = text;
				text = string.Concat(new object[]
				{
					text3,
					"\n",
					text2,
					this.pools[text2].Count
				});
			}
			num += this.pools[text2].Count;
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

	// Token: 0x0400354A RID: 13642
	public const int DEFAULT_POOL_SIZE = 4;

	// Token: 0x0400354B RID: 13643
	private Dictionary<string, GameObjectPool> pools = new Dictionary<string, GameObjectPool>();

	// Token: 0x0400354C RID: 13644
	private Transform parentTransform;

	// Token: 0x0400354D RID: 13645
	private Transform inactiveTransform;
}
