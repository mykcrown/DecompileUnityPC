using System;
using UnityEngine;

// Token: 0x02000B11 RID: 2833
public interface IResourceLoader
{
	// Token: 0x0600514E RID: 20814
	void Load<T>(string path, Action<T> callback) where T : UnityEngine.Object;
}
