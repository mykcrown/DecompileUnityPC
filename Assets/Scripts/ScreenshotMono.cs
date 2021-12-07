// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScreenshotMono : MonoBehaviour
{
	private sealed class _CopyTexture_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ScreenshotMono _this;

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

		public _CopyTexture_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForEndOfFrame();
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._this.captureScreenshotCallback != null)
				{
					Action captureScreenshotCallback = this._this.captureScreenshotCallback;
					this._this.captureScreenshotCallback = null;
					this._this.screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
					this._this.screenshot.filterMode = FilterMode.Point;
					this._this.screenshot.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);
					this._this.screenshot.Apply();
					captureScreenshotCallback();
				}
				this._PC = -1;
				break;
			}
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

	private Texture2D screenshot;

	private Action captureScreenshotCallback;

	public bool InProgress
	{
		get
		{
			return this.captureScreenshotCallback != null;
		}
	}

	public void SaveScreenshot(Action callback)
	{
		this.captureScreenshotCallback = callback;
		base.StartCoroutine("CopyTexture");
	}

	private IEnumerator CopyTexture()
	{
		ScreenshotMono._CopyTexture_c__Iterator0 _CopyTexture_c__Iterator = new ScreenshotMono._CopyTexture_c__Iterator0();
		_CopyTexture_c__Iterator._this = this;
		return _CopyTexture_c__Iterator;
	}

	public Texture2D GetScreenshot()
	{
		return this.screenshot;
	}
}
