// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace InControl
{
	public abstract class SingletonMonoBehavior<T, P> : MonoBehaviour where T : MonoBehaviour where P : MonoBehaviour
	{
		private static T instance;

		private static bool hasInstance;

		private static object lockObject = new object();

		public static T Instance
		{
			get
			{
				return SingletonMonoBehavior<T, P>.GetInstance();
			}
		}

		private static void CreateInstance()
		{
			GameObject gameObject;
			if (typeof(P) == typeof(MonoBehaviour))
			{
				gameObject = new GameObject();
				gameObject.name = typeof(T).Name;
			}
			else
			{
				P p = UnityEngine.Object.FindObjectOfType<P>();
				if (!p)
				{
					UnityEngine.Debug.LogError("Could not find object with required component " + typeof(P).Name);
					return;
				}
				gameObject = p.gameObject;
			}
			UnityEngine.Debug.Log("Creating instance of singleton component " + typeof(T).Name);
			SingletonMonoBehavior<T, P>.instance = gameObject.AddComponent<T>();
			SingletonMonoBehavior<T, P>.hasInstance = true;
		}

		private static T GetInstance()
		{
			object obj = SingletonMonoBehavior<T, P>.lockObject;
			T result;
			lock (obj)
			{
				if (SingletonMonoBehavior<T, P>.hasInstance)
				{
					result = SingletonMonoBehavior<T, P>.instance;
				}
				else
				{
					Type typeFromHandle = typeof(T);
					T[] array = UnityEngine.Object.FindObjectsOfType<T>();
					if (array.Length > 0)
					{
						SingletonMonoBehavior<T, P>.instance = array[0];
						SingletonMonoBehavior<T, P>.hasInstance = true;
						if (array.Length > 1)
						{
							UnityEngine.Debug.LogWarning("Multiple instances of singleton " + typeFromHandle + " found; destroying all but the first.");
							for (int i = 1; i < array.Length; i++)
							{
								UnityEngine.Object.DestroyImmediate(array[i].gameObject);
							}
						}
						result = SingletonMonoBehavior<T, P>.instance;
					}
					else
					{
						SingletonPrefabAttribute singletonPrefabAttribute = Attribute.GetCustomAttribute(typeFromHandle, typeof(SingletonPrefabAttribute)) as SingletonPrefabAttribute;
						if (singletonPrefabAttribute == null)
						{
							SingletonMonoBehavior<T, P>.CreateInstance();
						}
						else
						{
							string name = singletonPrefabAttribute.Name;
							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(name));
							if (gameObject == null)
							{
								UnityEngine.Debug.LogError(string.Concat(new object[]
								{
									"Could not find prefab ",
									name,
									" for singleton of type ",
									typeFromHandle,
									"."
								}));
								SingletonMonoBehavior<T, P>.CreateInstance();
							}
							else
							{
								gameObject.name = name;
								SingletonMonoBehavior<T, P>.instance = gameObject.GetComponent<T>();
								if (SingletonMonoBehavior<T, P>.instance == null)
								{
									UnityEngine.Debug.LogWarning(string.Concat(new object[]
									{
										"There wasn't a component of type \"",
										typeFromHandle,
										"\" inside prefab \"",
										name,
										"\"; creating one now."
									}));
									SingletonMonoBehavior<T, P>.instance = gameObject.AddComponent<T>();
									SingletonMonoBehavior<T, P>.hasInstance = true;
								}
							}
						}
						result = SingletonMonoBehavior<T, P>.instance;
					}
				}
			}
			return result;
		}

		protected bool EnforceSingleton()
		{
			object obj = SingletonMonoBehavior<T, P>.lockObject;
			lock (obj)
			{
				if (SingletonMonoBehavior<T, P>.hasInstance)
				{
					T[] array = UnityEngine.Object.FindObjectsOfType<T>();
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].GetInstanceID() != SingletonMonoBehavior<T, P>.instance.GetInstanceID())
						{
							UnityEngine.Object.DestroyImmediate(array[i].gameObject);
						}
					}
				}
			}
			int arg_9F_0 = base.GetInstanceID();
			T t = SingletonMonoBehavior<T, P>.Instance;
			return arg_9F_0 == t.GetInstanceID();
		}

		protected bool EnforceSingletonComponent()
		{
			object obj = SingletonMonoBehavior<T, P>.lockObject;
			lock (obj)
			{
				if (SingletonMonoBehavior<T, P>.hasInstance && base.GetInstanceID() != SingletonMonoBehavior<T, P>.instance.GetInstanceID())
				{
					UnityEngine.Object.DestroyImmediate(this);
					return false;
				}
			}
			return true;
		}

		private void OnDestroy()
		{
			SingletonMonoBehavior<T, P>.hasInstance = false;
		}
	}
}
