// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class ProxyMono : MonoBehaviour
{
	public Action OnUpdate;

	public Action OnLateUpdate;

	public Action OnDestroyFn;

	public Action<bool> OnApplicationFocusFn;

	protected void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	private void LateUpdate()
	{
		if (this.OnLateUpdate != null)
		{
			this.OnLateUpdate();
		}
	}

	public void OnDestroy()
	{
		if (this.OnDestroyFn != null)
		{
			this.OnDestroyFn();
		}
	}

	public void OnApplicationFocus(bool hasFocus)
	{
		if (this.OnApplicationFocusFn != null)
		{
			this.OnApplicationFocusFn(hasFocus);
		}
	}

	public static ProxyMono CreateNew(string name)
	{
		ProxyMono proxyMono = new GameObject(name).AddComponent<ProxyMono>();
		ProxyMono.Attach(proxyMono.gameObject);
		return proxyMono;
	}

	public static void Attach(GameObject obj)
	{
		obj.transform.SetParent(GameObject.Find("ProxyMonoObjects").transform);
	}
}
