// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FlickerSprite : MonoBehaviour
{
	private sealed class _flicker_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timeElapsed___0;

		internal bool _visible___0;

		internal float flickerDuration;

		internal float flickerInterval;

		internal FlickerSprite _this;

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

		public _flicker_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timeElapsed___0 = 0f;
				this._visible___0 = true;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timeElapsed___0 < this.flickerDuration)
			{
				this._visible___0 = !this._visible___0;
				this._this.Image.enabled = this._visible___0;
				this._timeElapsed___0 += WTime.deltaTime;
				this._current = new WaitForSeconds(this.flickerInterval);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.Image.enabled = true;
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

	public float FlickerDuration = 0.3f;

	public float FlickerInterval = 0.1f;

	private Coroutine flickerRoutine;

	public Image Image;

	public void Flicker(float flickerDuration, float flickerInterval)
	{
		this.beginFlicker(flickerDuration, flickerInterval);
	}

	public void Flicker()
	{
		this.beginFlicker(this.FlickerDuration, this.FlickerInterval);
	}

	public void StopFlicker()
	{
		if (this.flickerRoutine != null)
		{
			base.StopCoroutine(this.flickerRoutine);
			this.Image.enabled = true;
			this.flickerRoutine = null;
		}
	}

	private void beginFlicker(float flickerDuration, float flickerInterval)
	{
		this.flickerRoutine = base.StartCoroutine(this.flicker(flickerDuration, flickerInterval));
	}

	private IEnumerator flicker(float flickerDuration, float flickerInterval)
	{
		FlickerSprite._flicker_c__Iterator0 _flicker_c__Iterator = new FlickerSprite._flicker_c__Iterator0();
		_flicker_c__Iterator.flickerDuration = flickerDuration;
		_flicker_c__Iterator.flickerInterval = flickerInterval;
		_flicker_c__Iterator._this = this;
		return _flicker_c__Iterator;
	}
}
