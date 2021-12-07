// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FlashSprite : MonoBehaviour
{
	private sealed class _tickFlash_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float amount;

		internal float _startAmount___0;

		internal float duration;

		internal float _delta___0;

		internal FlashSprite _this;

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

		public _tickFlash_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._startAmount___0 = this.amount;
				this._delta___0 = this.amount / this.duration;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this.amount > 0f)
			{
				this.amount -= this._delta___0 * Time.deltaTime;
				this._this.Image.material.SetFloat("_FlashAmount", this.amount);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.Image.material.SetFloat("_FlashAmount", 0f);
			this._this.isFlashing = false;
			if (this._this.loopFlash)
			{
				this._this.flashColor(this._startAmount___0, this.duration);
			}
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

	private bool isFlashing;

	public Material FlashMaterial;

	public float FlashAmount = 0.7f;

	public float FlashDuration = 0.3f;

	public Image Image;

	protected bool loopFlash;

	protected virtual void Awake()
	{
		if (this.Image != null && this.FlashMaterial != null)
		{
			this.Image.material = UnityEngine.Object.Instantiate<Material>(this.FlashMaterial);
		}
	}

	public void Flash()
	{
		this.flashColor(this.FlashAmount, this.FlashDuration);
	}

	private void flashColor(float amount, float duration)
	{
		if (this.isFlashing || this.FlashMaterial == null || this.Image == null)
		{
			return;
		}
		this.isFlashing = true;
		base.StartCoroutine(this.tickFlash(amount, duration));
	}

	private IEnumerator tickFlash(float amount, float duration)
	{
		FlashSprite._tickFlash_c__Iterator0 _tickFlash_c__Iterator = new FlashSprite._tickFlash_c__Iterator0();
		_tickFlash_c__Iterator.amount = amount;
		_tickFlash_c__Iterator.duration = duration;
		_tickFlash_c__Iterator._this = this;
		return _tickFlash_c__Iterator;
	}

	private void OnDisable()
	{
		this.isFlashing = false;
		this.Image.material.SetFloat("_FlashAmount", 0f);
	}

	private void OnDestroy()
	{
		if (this.Image.material != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Image.material);
		}
	}
}
