// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using strange.extensions.injector.impl;
using System;
using UnityEngine;

public class DependencyInjection : IDependencyInjection
{
	private static string IGNORE_MESSAGE = "InjectionBinder has no binding";

	private IInjectionBinder injectionBinder;

	public DependencyInjection(IInjectionBinder injectionBinder)
	{
		this.injectionBinder = injectionBinder;
		StaticInject.staticInjector = this;
	}

	public void BindToValue<T>(object value)
	{
		this.injectionBinder.Bind<T>().ToValue(value);
	}

	public void Inject(object obj)
	{
		this.injectionBinder.injector.Inject(obj);
	}

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

	public T GetInstance<T>()
	{
		return this.injectionBinder.GetInstance<T>();
	}

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

	public T CreateComponentWithGameObject<T>(string gameObjectName = null) where T : MonoBehaviour
	{
		T t = new GameObject(gameObjectName).AddComponent<T>();
		this.Inject(t);
		return t;
	}

	public GameObject CreateGameObjectWithComponent<T>(string gameObjectName = null) where T : MonoBehaviour
	{
		GameObject gameObject = new GameObject(gameObjectName);
		T t = gameObject.AddComponent<T>();
		this.Inject(t);
		return gameObject;
	}
}
