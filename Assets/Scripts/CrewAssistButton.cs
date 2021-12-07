// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CrewAssistButton : MonoBehaviour
{
	private sealed class _SlideOutAction_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _accumulatedTime___0;

		internal float _slidePercent___1;

		internal CrewAssistButton _this;

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

		public _SlideOutAction_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				break;
			case 1u:
				break;
			case 2u:
				goto IL_109;
			case 3u:
				goto IL_1C5;
			default:
				return false;
			}
			if (this._this.slideInRoutine != null)
			{
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._accumulatedTime___0 = 0f;
			IL_109:
			if (this._accumulatedTime___0 < this._this.slideDuration)
			{
				this._accumulatedTime___0 = Mathf.Min(this._this.slideDuration, this._accumulatedTime___0 + Time.smoothDeltaTime);
				this._slidePercent___1 = MathUtil.MixOut(0f, 1f, this._accumulatedTime___0 / this._this.slideDuration, this._this.slideStrength);
				this._this.slideOutContainer.localPosition = Vector3.Lerp(this._this.visiblePosition, this._this.invisiblePosition, this._slidePercent___1);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._accumulatedTime___0 = 0f;
			IL_1C5:
			if (this._accumulatedTime___0 < this._this.collapseDuration)
			{
				this._accumulatedTime___0 = Mathf.Min(this._this.collapseDuration, this._accumulatedTime___0 + Time.smoothDeltaTime);
				this._this.ourTransform.sizeDelta = new Vector2(this._this.ourTransform.sizeDelta.x, MathUtil.Mix(this._this.visibleHeight, 0f, this._accumulatedTime___0 / this._this.collapseDuration, 1f));
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 3;
				}
				return true;
			}
			this._this.IsVisible = false;
			this._this.slideOutRoutine = null;
			this._this.gameObject.SetActive(false);
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

	private sealed class _SlideInAction_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _accumulatedTime___0;

		internal float _slidePercent___1;

		internal CrewAssistButton _this;

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

		public _SlideInAction_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				break;
			case 1u:
				break;
			case 2u:
				goto IL_105;
			default:
				return false;
			}
			if (this._this.slideOutRoutine != null)
			{
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._accumulatedTime___0 = 0f;
			IL_105:
			if (this._accumulatedTime___0 < this._this.slideDuration)
			{
				this._accumulatedTime___0 = Mathf.Min(this._this.slideDuration, this._accumulatedTime___0 + Time.smoothDeltaTime);
				this._slidePercent___1 = MathUtil.MixOut(0f, 1f, this._accumulatedTime___0 / this._this.slideDuration, this._this.slideStrength);
				this._this.slideOutContainer.localPosition = Vector3.Lerp(this._this.invisiblePosition, this._this.visiblePosition, this._slidePercent___1);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._this.IsVisible = true;
			this._this.slideInRoutine = null;
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

	private RectTransform ourTransform;

	public RectTransform slideOutContainer;

	public bool slidesLeft;

	public float slideDuration = 0.2f;

	public float collapseDuration = 0.15f;

	public float slideDistance = 200f;

	public float slideStrength = 2f;

	private Coroutine slideOutRoutine;

	private Coroutine slideInRoutine;

	private Vector3 visiblePosition;

	private Vector3 invisiblePosition;

	private float visibleHeight;

	public bool IsVisible;

	private void Start()
	{
		this.ourTransform = base.GetComponent<RectTransform>();
		this.visiblePosition = this.slideOutContainer.localPosition;
		this.visibleHeight = this.ourTransform.sizeDelta.y;
		this.invisiblePosition = this.visiblePosition + new Vector3(((!this.slidesLeft) ? 1f : -1f) * this.slideDistance, 0f, 0f);
		this.IsVisible = false;
		base.gameObject.SetActive(false);
	}

	public void SlideOut()
	{
		if (!this.IsVisible)
		{
			return;
		}
		if (this.slideOutRoutine == null)
		{
			this.slideOutRoutine = base.StartCoroutine(this.SlideOutAction());
		}
	}

	public void SetState(bool visibility)
	{
		if (visibility)
		{
			this.SlideIn();
		}
		else
		{
			this.SlideOut();
		}
	}

	private IEnumerator SlideOutAction()
	{
		CrewAssistButton._SlideOutAction_c__Iterator0 _SlideOutAction_c__Iterator = new CrewAssistButton._SlideOutAction_c__Iterator0();
		_SlideOutAction_c__Iterator._this = this;
		return _SlideOutAction_c__Iterator;
	}

	public void SlideIn()
	{
		if (this.IsVisible)
		{
			return;
		}
		if (this.slideInRoutine == null)
		{
			this.ourTransform.SetAsLastSibling();
			this.ourTransform.sizeDelta = new Vector2(this.ourTransform.sizeDelta.x, this.visibleHeight);
			base.gameObject.SetActive(true);
			this.slideInRoutine = base.StartCoroutine(this.SlideInAction());
		}
	}

	private IEnumerator SlideInAction()
	{
		CrewAssistButton._SlideInAction_c__Iterator1 _SlideInAction_c__Iterator = new CrewAssistButton._SlideInAction_c__Iterator1();
		_SlideInAction_c__Iterator._this = this;
		return _SlideInAction_c__Iterator;
	}
}
