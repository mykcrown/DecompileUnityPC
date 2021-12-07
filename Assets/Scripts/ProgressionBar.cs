// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour
{
	private sealed class _animateToTarget_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ulong _startValue___0;

		internal float _range___0;

		internal int _threshold___0;

		internal float _currentThresholdValue___0;

		internal float _previousThresholdValue___0;

		internal float _totalTime___0;

		internal float _currentTime___0;

		internal float _timePercent___1;

		internal float _noFillValue___1;

		internal float _maxFillValue___1;

		internal float _fillAmount___1;

		internal ProgressionBar _this;

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

		public _animateToTarget_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.initializeToCurrent();
				this._startValue___0 = this._this.current;
				this._range___0 = this._this.target - this._this.current;
				this._threshold___0 = this._this.CurrentThreshold;
				this._currentThresholdValue___0 = this._this.ValueForThreshold(this._threshold___0);
				this._previousThresholdValue___0 = this._this.ValueForThreshold(this._threshold___0 - 1);
				this._totalTime___0 = ((this._this.animationSpeed > 0f) ? Mathf.Min((this._this.target - this._this.current) / this._this.animationSpeed, this._this.maxAnimationTime) : this._this.maxAnimationTime);
				this._currentTime___0 = 0f;
				break;
			case 1u:
				this._this.onWrapAfterWait.Invoke();
				this._this.UpdateMainProgressBarVisual(this._threshold___0);
				goto IL_33A;
			case 2u:
				if (this._currentTime___0 >= this._totalTime___0)
				{
					goto IL_366;
				}
				break;
			default:
				return false;
			}
			this._currentTime___0 = Mathf.Min(this._currentTime___0 + Time.smoothDeltaTime * this._this.speedMultiplier, this._totalTime___0);
			this._timePercent___1 = this._currentTime___0 / this._totalTime___0;
			this._this.animatedTarget = Mathf.Min(this._this.progressCurve.Evaluate(this._timePercent___1) * this._range___0 + this._startValue___0, this._currentThresholdValue___0);
			this._noFillValue___1 = Mathf.Max(this._startValue___0, this._previousThresholdValue___0);
			this._maxFillValue___1 = Mathf.Min(this._this.target, this._currentThresholdValue___0);
			this._fillAmount___1 = ((this._maxFillValue___1 == this._noFillValue___1) ? 0f : ((this._this.animatedTarget - this._noFillValue___1) / (this._maxFillValue___1 - this._noFillValue___1)));
			this._this.extraProgress.fillAmount = this._fillAmount___1;
			this._this.invokeUpdateMethod();
			if (this._this.animatedTarget == this._currentThresholdValue___0)
			{
				if (this._this.animatedTarget < this._this.target)
				{
					this._threshold___0++;
					if (this._this.ClampAtMaxTier && this._threshold___0 >= this._this.thresholds.Count)
					{
						this._this.onCap.Invoke();
						goto IL_366;
					}
					this._this.onWrap.Invoke();
					this._currentThresholdValue___0 = this._this.ValueForThreshold(this._threshold___0);
					this._previousThresholdValue___0 = this._this.ValueForThreshold(this._threshold___0 - 1);
					this._current = new WaitForSeconds(this._this.wrapPauseTime);
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				else
				{
					this._this.onCap.Invoke();
				}
			}
			IL_33A:
			this._current = null;
			if (!this._disposing)
			{
				this._PC = 2;
			}
			return true;
			IL_366:
			if (!this._this.ClampAtMaxTier)
			{
				this._this.animatedTarget = (float)((int)this._this.target);
			}
			this._this.invokeUpdateMethod();
			this._this.animateToTargetCoroutine = null;
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

	[SerializeField]
	private Slider mainProgress;

	[SerializeField]
	private Image extraProgress;

	[SerializeField]
	private List<ulong> thresholds = new List<ulong>
	{
		0uL,
		100uL,
		200uL,
		400uL,
		800uL,
		1200uL
	};

	[SerializeField]
	private ulong current;

	[SerializeField]
	private float target;

	[SerializeField]
	private float wrapPauseTime;

	[SerializeField]
	private float animationSpeed = 50f;

	public float speedMultiplier = 1f;

	[SerializeField]
	private float maxAnimationTime = 3.5f;

	[SerializeField]
	private AnimationCurve progressCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	public UnityEvent onCap;

	public UnityEvent onWrap;

	public UnityEvent onWrapAfterWait;

	public UnityEvent onValueUpdate;

	public bool ClampAtMaxTier = true;

	private Coroutine animateToTargetCoroutine;

	private float animatedTarget;

	public ulong CurrentValue
	{
		get;
		private set;
	}

	public ulong MostRecentStartValue
	{
		get;
		private set;
	}

	public float Target
	{
		get
		{
			return this.target;
		}
	}

	private float fullBarWidth
	{
		get
		{
			return ((RectTransform)base.transform).rect.width;
		}
	}

	public int CurrentThreshold
	{
		get
		{
			return this.ThresholdForValue(this.current);
		}
	}

	public int MaxThreshold
	{
		get
		{
			return this.thresholds.Count - 1;
		}
	}

	public int LastThresholdIncrement
	{
		get
		{
			return (int)((this.thresholds.Count <= 1) ? this.thresholds[this.thresholds.Count - 1] : (this.thresholds[this.thresholds.Count - 1] - this.thresholds[this.thresholds.Count - 2]));
		}
	}

	private void OnEnable()
	{
		this.initializeToCurrent();
		this.extraProgress.fillAmount = 0f;
	}

	public void Initialize(List<ulong> thresholds, ulong current, float target)
	{
		this.speedMultiplier = 1f;
		this.thresholds = thresholds;
		this.current = current;
		this.target = target;
		this.initializeToCurrent();
		this.UpdateMainProgressBarVisual();
	}

	private void UpdateMainProgressBarVisual()
	{
		this.UpdateMainProgressBarVisual(this.CurrentThreshold);
	}

	private void UpdateMainProgressBarVisual(int index)
	{
		if (index > 0)
		{
			float num = this.ValueForThreshold(index);
			float num2 = this.ValueForThreshold(index - 1);
			float num3 = num - num2;
			float value = (this.current - num2) / num3;
			this.mainProgress.value = value;
			float num4 = (Mathf.Max(num2, this.current) - num2) / num3;
			float num5 = (num - Math.Min(num, this.target)) / num3;
			this.extraProgress.rectTransform.offsetMin = new Vector2(num4 * this.fullBarWidth, this.extraProgress.rectTransform.offsetMin.y);
			this.extraProgress.rectTransform.offsetMax = new Vector2(-num5 * this.fullBarWidth, this.extraProgress.rectTransform.offsetMax.y);
		}
		else
		{
			this.mainProgress.value = 1f;
		}
		this.extraProgress.fillAmount = 0f;
	}

	public Coroutine KickoffVisualUpdate()
	{
		if (this.animateToTargetCoroutine == null)
		{
			this.UpdateMainProgressBarVisual();
			if (this.target > this.current && (!this.ClampAtMaxTier || this.ValueLessThanMaxThreshold(this.current)))
			{
				this.animateToTargetCoroutine = base.StartCoroutine(this.animateToTarget());
			}
			else
			{
				this.target = this.current;
				this.initializeToCurrent();
			}
		}
		return this.animateToTargetCoroutine;
	}

	private void invokeUpdateMethod()
	{
		if (this.CurrentValue != (ulong)((uint)this.animatedTarget))
		{
			this.CurrentValue = (ulong)((uint)this.animatedTarget);
			this.onValueUpdate.Invoke();
		}
	}

	private void initializeToCurrent()
	{
		this.MostRecentStartValue = this.current;
		this.CurrentValue = this.current;
		this.animatedTarget = this.current;
		this.onValueUpdate.Invoke();
	}

	private IEnumerator animateToTarget()
	{
		ProgressionBar._animateToTarget_c__Iterator0 _animateToTarget_c__Iterator = new ProgressionBar._animateToTarget_c__Iterator0();
		_animateToTarget_c__Iterator._this = this;
		return _animateToTarget_c__Iterator;
	}

	public int ThresholdForValue(float value)
	{
		for (int i = 0; i < this.thresholds.Count; i++)
		{
			if (this.thresholds[i] > value)
			{
				return i;
			}
		}
		if (this.ClampAtMaxTier)
		{
			return -1;
		}
		return (int)(value - this.thresholds[this.thresholds.Count - 1]) / this.LastThresholdIncrement + this.thresholds.Count;
	}

	public float ValueForThreshold(int index)
	{
		if (index < 0)
		{
			return 0f;
		}
		if (index >= this.thresholds.Count)
		{
			return (!this.ClampAtMaxTier) ? ((uint)((float)(index - this.thresholds.Count + 1) * (float)this.LastThresholdIncrement + this.thresholds[this.thresholds.Count - 1])) : -1f;
		}
		return this.thresholds[index];
	}

	public bool ValueLessThanMaxThreshold(ulong value)
	{
		return value < this.thresholds[this.thresholds.Count - 1];
	}
}
