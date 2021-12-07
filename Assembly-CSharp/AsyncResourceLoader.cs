using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B10 RID: 2832
public class AsyncResourceLoader : IResourceLoader
{
	// Token: 0x0600514D RID: 20813 RVA: 0x00151F0C File Offset: 0x0015030C
	public void Load<T>(string path, Action<T> callback) where T : UnityEngine.Object
	{
		ResourceRequest request;
		if (this.requests.TryGetValue(path, out request))
		{
			request.completed += delegate(AsyncOperation op)
			{
				callback(request.asset as T);
			};
		}
		else
		{
			request = Resources.LoadAsync<T>(path);
			this.requests.Add(path, request);
			request.completed += delegate(AsyncOperation op)
			{
				this.requests.Remove(path);
				callback(request.asset as T);
			};
		}
	}

	// Token: 0x04003463 RID: 13411
	private Dictionary<string, ResourceRequest> requests = new Dictionary<string, ResourceRequest>(32);
}
