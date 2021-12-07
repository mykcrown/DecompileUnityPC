// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AsyncResourceLoader : IResourceLoader
{
	private sealed class _Load_c__AnonStorey0<T> where T : UnityEngine.Object
	{
		internal Action<T> callback;

		internal ResourceRequest request;

		internal string path;

		internal AsyncResourceLoader _this;

		internal void __m__0(AsyncOperation op)
		{
			this.callback(this.request.asset as T);
		}

		internal void __m__1(AsyncOperation op)
		{
			this._this.requests.Remove(this.path);
			this.callback(this.request.asset as T);
		}
	}

	private Dictionary<string, ResourceRequest> requests = new Dictionary<string, ResourceRequest>(32);

	public void Load<T>(string path, Action<T> callback) where T : UnityEngine.Object
	{
		AsyncResourceLoader._Load_c__AnonStorey0<T> _Load_c__AnonStorey = new AsyncResourceLoader._Load_c__AnonStorey0<T>();
		_Load_c__AnonStorey.callback = callback;
		_Load_c__AnonStorey.path = path;
		_Load_c__AnonStorey._this = this;
		if (this.requests.TryGetValue(_Load_c__AnonStorey.path, out _Load_c__AnonStorey.request))
		{
			_Load_c__AnonStorey.request.completed += new Action<AsyncOperation>(_Load_c__AnonStorey.__m__0);
		}
		else
		{
			_Load_c__AnonStorey.request = Resources.LoadAsync<T>(_Load_c__AnonStorey.path);
			this.requests.Add(_Load_c__AnonStorey.path, _Load_c__AnonStorey.request);
			_Load_c__AnonStorey.request.completed += new Action<AsyncOperation>(_Load_c__AnonStorey.__m__1);
		}
	}
}
