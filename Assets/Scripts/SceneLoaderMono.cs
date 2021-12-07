// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderMono : MonoBehaviour
{
	private sealed class _unloadScene_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal AsyncOperation _async___0;

		internal string sceneName;

		internal Action callback;

		internal SceneLoaderMono _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _unloadScene_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._async___0 = null;
				try
				{
					this._async___0 = SceneManager.UnloadSceneAsync(this.sceneName);
				}
				catch
				{
					UnityEngine.Debug.Log("Failed to unload " + this.sceneName);
				}
				if (this._async___0 == null)
				{
					UnityEngine.Debug.Log("Unload failed just proceed");
					this.callback();
					goto IL_D7;
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!this._async___0.isDone)
			{
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.unloadCount--;
			this._this.updateActiveState();
			this.callback();
			IL_D7:
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _loadScene_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal string sceneName;

		internal LoadSceneMode mode;

		internal AsyncOperation _async___0;

		internal Action callback;

		internal SceneLoaderMono _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _loadScene_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._async___0 = SceneManager.LoadSceneAsync(this.sceneName, this.mode);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!this._async___0.isDone)
			{
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.loadCount--;
			this._this.updateActiveState();
			this.callback();
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private int unloadCount;

	private int loadCount;

	public static SceneLoaderMono CreateNew(string name)
	{
		SceneLoaderMono sceneLoaderMono = new GameObject(name).AddComponent<SceneLoaderMono>();
		UnityEngine.Object.DontDestroyOnLoad(sceneLoaderMono.gameObject);
		sceneLoaderMono.gameObject.SetActive(false);
		return sceneLoaderMono;
	}

	private void updateActiveState()
	{
		if (this.unloadCount > 0 || this.loadCount > 0)
		{
			base.gameObject.SetActive(true);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	public void UnloadScene(string sceneName, Action callback)
	{
		this.unloadCount++;
		this.updateActiveState();
		base.StartCoroutine(this.unloadScene(sceneName, callback));
	}

	private IEnumerator unloadScene(string sceneName, Action callback)
	{
		SceneLoaderMono._unloadScene_c__Iterator0 _unloadScene_c__Iterator = new SceneLoaderMono._unloadScene_c__Iterator0();
		_unloadScene_c__Iterator.sceneName = sceneName;
		_unloadScene_c__Iterator.callback = callback;
		_unloadScene_c__Iterator._this = this;
		return _unloadScene_c__Iterator;
	}

	public void LoadSceneSync(string sceneName, LoadSceneMode mode)
	{
		SceneManager.LoadScene(sceneName, mode);
	}

	public void LoadScene(string sceneName, LoadSceneMode mode, Action callback)
	{
		this.loadCount++;
		this.updateActiveState();
		base.StartCoroutine(this.loadScene(sceneName, mode, callback));
	}

	private IEnumerator loadScene(string sceneName, LoadSceneMode mode, Action callback)
	{
		SceneLoaderMono._loadScene_c__Iterator1 _loadScene_c__Iterator = new SceneLoaderMono._loadScene_c__Iterator1();
		_loadScene_c__Iterator.sceneName = sceneName;
		_loadScene_c__Iterator.mode = mode;
		_loadScene_c__Iterator.callback = callback;
		_loadScene_c__Iterator._this = this;
		return _loadScene_c__Iterator;
	}
}
