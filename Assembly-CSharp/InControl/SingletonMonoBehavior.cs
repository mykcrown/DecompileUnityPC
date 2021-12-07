using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020001D7 RID: 471
	public abstract class SingletonMonoBehavior<T, P> : MonoBehaviour where T : MonoBehaviour where P : MonoBehaviour
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0001801F File Offset: 0x0001641F
		public static T Instance
		{
			get
			{
				return SingletonMonoBehavior<T, P>.GetInstance();
			}
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00018028 File Offset: 0x00016428
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
					Debug.LogError("Could not find object with required component " + typeof(P).Name);
					return;
				}
				gameObject = p.gameObject;
			}
			Debug.Log("Creating instance of singleton component " + typeof(T).Name);
			SingletonMonoBehavior<T, P>.instance = gameObject.AddComponent<T>();
			SingletonMonoBehavior<T, P>.hasInstance = true;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000180EC File Offset: 0x000164EC
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
							Debug.LogWarning("Multiple instances of singleton " + typeFromHandle + " found; destroying all but the first.");
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
								Debug.LogError(string.Concat(new object[]
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
									Debug.LogWarning(string.Concat(new object[]
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

		// Token: 0x06000829 RID: 2089 RVA: 0x000182D4 File Offset: 0x000166D4
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
			int instanceID = base.GetInstanceID();
			T t = SingletonMonoBehavior<T, P>.Instance;
			return instanceID == t.GetInstanceID();
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00018394 File Offset: 0x00016794
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

		// Token: 0x0600082B RID: 2091 RVA: 0x00018408 File Offset: 0x00016808
		private void OnDestroy()
		{
			SingletonMonoBehavior<T, P>.hasInstance = false;
		}

		// Token: 0x040005E3 RID: 1507
		private static T instance;

		// Token: 0x040005E4 RID: 1508
		private static bool hasInstance;

		// Token: 0x040005E5 RID: 1509
		private static object lockObject = new object();
	}
}
