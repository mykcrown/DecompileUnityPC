using System;
using strange.extensions.injector.api;
using strange.extensions.injector.impl;
using UnityEngine;

// Token: 0x0200020A RID: 522
public class DependencyInjection : IDependencyInjection
{
	// Token: 0x060009C3 RID: 2499 RVA: 0x00050C86 File Offset: 0x0004F086
	public DependencyInjection(IInjectionBinder injectionBinder)
	{
		this.injectionBinder = injectionBinder;
		StaticInject.staticInjector = this;
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00050C9B File Offset: 0x0004F09B
	public void BindToValue<T>(object value)
	{
		this.injectionBinder.Bind<T>().ToValue(value);
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00050CAF File Offset: 0x0004F0AF
	public void Inject(object obj)
	{
		this.injectionBinder.injector.Inject(obj);
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00050CC4 File Offset: 0x0004F0C4
	public T GetInstance<T>(object name)
	{
		T result;
		try
		{
			result = this.injectionBinder.GetInstance<T>(name);
		}
		catch (InjectionException ex)
		{
			if (ex.Message.IndexOf(DependencyInjection.IGNORE_MESSAGE) != 0)
			{
				throw ex;
			}
			result = default(T);
		}
		return result;
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00050D1C File Offset: 0x0004F11C
	public T GetInstance<T>()
	{
		return this.injectionBinder.GetInstance<T>();
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00050D2C File Offset: 0x0004F12C
	public object GetInstance(Type theType)
	{
		object result;
		try
		{
			result = this.injectionBinder.GetInstance(theType);
		}
		catch (InjectionException ex)
		{
			if (ex.Message.IndexOf(DependencyInjection.IGNORE_MESSAGE) != 0)
			{
				throw ex;
			}
			result = null;
		}
		return result;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00050D7C File Offset: 0x0004F17C
	public T CreateComponentWithGameObject<T>(string gameObjectName = null) where T : MonoBehaviour
	{
		T t = new GameObject(gameObjectName).AddComponent<T>();
		this.Inject(t);
		return t;
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00050DA4 File Offset: 0x0004F1A4
	public GameObject CreateGameObjectWithComponent<T>(string gameObjectName = null) where T : MonoBehaviour
	{
		GameObject gameObject = new GameObject(gameObjectName);
		T t = gameObject.AddComponent<T>();
		this.Inject(t);
		return gameObject;
	}

	// Token: 0x040006EC RID: 1772
	private static string IGNORE_MESSAGE = "InjectionBinder has no binding";

	// Token: 0x040006ED RID: 1773
	private IInjectionBinder injectionBinder;
}
