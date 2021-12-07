using System;
using System.Collections;
using System.Text;
using UnityEngine;

// Token: 0x02000B03 RID: 2819
public static class WDGameObjectExtensions
{
	// Token: 0x060050FC RID: 20732 RVA: 0x00150AA8 File Offset: 0x0014EEA8
	public static void DestroySafe(this GameObject obj)
	{
		if (obj == null)
		{
			Debug.LogError("Somehow tried to safely destroy a null object!");
			return;
		}
		PooledGameObject component = obj.GetComponent<PooledGameObject>();
		if (component != null)
		{
			component.Release();
		}
		else
		{
			UnityEngine.Object.Destroy(obj);
		}
	}

	// Token: 0x060050FD RID: 20733 RVA: 0x00150AF0 File Offset: 0x0014EEF0
	public static string GetFullName(this GameObject gameObject)
	{
		return WDGameObjectExtensions.GetFullNameFromTransform(gameObject.transform);
	}

	// Token: 0x060050FE RID: 20734 RVA: 0x00150B00 File Offset: 0x0014EF00
	public static string GetFullNameFromTransform(Transform transform)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("{0}", transform.name);
		while (transform.parent != null)
		{
			transform = transform.parent;
			stringBuilder.Insert(0, string.Format("{0}/", transform.name));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060050FF RID: 20735 RVA: 0x00150B64 File Offset: 0x0014EF64
	public static void SetLayer(this GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		IEnumerator enumerator = gameObject.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.gameObject.SetLayer(layer);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06005100 RID: 20736 RVA: 0x00150BD8 File Offset: 0x0014EFD8
	public static T GetOrAddComponent<T>(this GameObject child) where T : Component
	{
		T t = child.GetComponent<T>();
		if (t == null)
		{
			t = child.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x06005101 RID: 20737 RVA: 0x00150C08 File Offset: 0x0014F008
	public static Transform FindDeep(this Transform aParent, string aName)
	{
		Transform transform = aParent.Find(aName);
		if (transform != null)
		{
			return transform;
		}
		IEnumerator enumerator = aParent.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform aParent2 = (Transform)obj;
				transform = aParent2.FindDeep(aName);
				if (transform != null)
				{
					return transform;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}
}
