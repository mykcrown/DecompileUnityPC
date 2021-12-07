// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IDependencyInjection
{
	void Inject(object obj);

	T GetInstance<T>(object name);

	T GetInstance<T>();

	object GetInstance(Type type);

	T CreateComponentWithGameObject<T>(string gameObjectName = null) where T : MonoBehaviour;

	GameObject CreateGameObjectWithComponent<T>(string gameObjectName = null) where T : MonoBehaviour;

	void BindToValue<T>(object value);
}
