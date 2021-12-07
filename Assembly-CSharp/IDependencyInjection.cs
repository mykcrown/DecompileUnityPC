using System;
using UnityEngine;

// Token: 0x0200020B RID: 523
public interface IDependencyInjection
{
	// Token: 0x060009CC RID: 2508
	void Inject(object obj);

	// Token: 0x060009CD RID: 2509
	T GetInstance<T>(object name);

	// Token: 0x060009CE RID: 2510
	T GetInstance<T>();

	// Token: 0x060009CF RID: 2511
	object GetInstance(Type type);

	// Token: 0x060009D0 RID: 2512
	T CreateComponentWithGameObject<T>(string gameObjectName = null) where T : MonoBehaviour;

	// Token: 0x060009D1 RID: 2513
	GameObject CreateGameObjectWithComponent<T>(string gameObjectName = null) where T : MonoBehaviour;

	// Token: 0x060009D2 RID: 2514
	void BindToValue<T>(object value);
}
