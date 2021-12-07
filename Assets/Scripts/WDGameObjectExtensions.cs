// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Text;
using UnityEngine;

public static class WDGameObjectExtensions
{
	public static void DestroySafe(this GameObject obj)
	{
		if (obj == null)
		{
			UnityEngine.Debug.LogError("Somehow tried to safely destroy a null object!");
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

	public static string GetFullName(this GameObject gameObject)
	{
		return WDGameObjectExtensions.GetFullNameFromTransform(gameObject.transform);
	}

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

	public static void SetLayer(this GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		IEnumerator enumerator = gameObject.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
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

	public static T GetOrAddComponent<T>(this GameObject child) where T : Component
	{
		T t = child.GetComponent<T>();
		if (t == null)
		{
			t = child.AddComponent<T>();
		}
		return t;
	}

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
				Transform aParent2 = (Transform)enumerator.Current;
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
