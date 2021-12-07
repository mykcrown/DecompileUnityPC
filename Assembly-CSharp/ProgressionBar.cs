using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200094B RID: 2379
public class ProgressionBar : MonoBehaviour
{
	// Token: 0x17000EF8 RID: 3832
	// (get) Token: 0x06003F28 RID: 16168 RVA: 0x0011F31C File Offset: 0x0011D71C
	// (set) Token: 0x06003F29 RID: 16169 RVA: 0x0011F324 File Offset: 0x0011D724
	public ulong CurrentValue { get; private set; }

	// Token: 0x17000EF9 RID: 3833
	// (get) Token: 0x06003F2A RID: 16170 RVA: 0x0011F32D File Offset: 0x0011D72D
	// (set) Token: 0x06003F2B RID: 16171 RVA: 0x0011F335 File Offset: 0x0011D735
	public ulong MostRecentStartValue { get; private set; }

	// Token: 0x17000EFA RID: 3834
	// (get) Token: 0x06003F2C RID: 16172 RVA: 0x0011F33E File Offset: 0x0011D73E
	public float Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x17000EFB RID: 3835
	// (get) Token: 0x06003F2D RID: 16173 RVA: 0x0011F348 File Offset: 0x0011D748
	private float fullBarWidth
	{
		get
		{
			return ((RectTransform)base.transform).rect.width;
		}
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x0011F36D File Offset: 0x0011D76D
	private void OnEnable()
	{
		this.initializeToCurrent();
		this.extraProgress.fillAmount = 0f;
	}

	// Token: 0x06003F2F RID: 16175 RVA: 0x0011F385 File Offset: 0x0011D785
	public void Initialize(List<ulong> thresholds, ulong current, float target)
	{
		this.speedMultiplier = 1f;
		this.thresholds = thresholds;
		this.current = current;
		this.target = target;
		this.initializeToCurrent();
		this.UpdateMainProgressBarVisual();
	}

	// Token: 0x06003F30 RID: 16176 RVA: 0x0011F3B3 File Offset: 0x0011D7B3
	private void UpdateMainProgressBarVisual()
	{
		this.UpdateMainProgressBarVisual(this.CurrentThreshold);
	}

	// Token: 0x06003F31 RID: 16177 RVA: 0x0011F3C4 File Offset: 0x0011D7C4
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

	// Token: 0x06003F32 RID: 16178 RVA: 0x0011F4C4 File Offset: 0x0011D8C4
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

	// Token: 0x06003F33 RID: 16179 RVA: 0x0011F542 File Offset: 0x0011D942
	private void invokeUpdateMethod()
	{
		if (this.CurrentValue != (ulong)((uint)this.animatedTarget))
		{
			this.CurrentValue = (ulong)((uint)this.animatedTarget);
			this.onValueUpdate.Invoke();
		}
	}

	// Token: 0x06003F34 RID: 16180 RVA: 0x0011F570 File Offset: 0x0011D970
	private void initializeToCurrent()
	{
		this.MostRecentStartValue = this.current;
		this.CurrentValue = this.current;
		this.animatedTarget = this.current;
		this.onValueUpdate.Invoke();
	}

	// Token: 0x06003F35 RID: 16181 RVA: 0x0011F5A4 File Offset: 0x0011D9A4
	private IEnumerator animateToTarget()
	{
		this.initializeToCurrent();
		ulong startValue = this.current;
		float range = this.target - this.current;
		int threshold = this.CurrentThreshold;
		float currentThresholdValue = this.ValueForThreshold(threshold);
		float previousThresholdValue = this.ValueForThreshold(threshold - 1);
		float totalTime = (this.animationSpeed > 0f) ? Mathf.Min((this.target - this.current) / this.animationSpeed, this.maxAnimationTime) : this.maxAnimationTime;
		float currentTime = 0f;
		for (;;)
		{
			currentTime = Mathf.Min(currentTime + Time.smoothDeltaTime * this.speedMultiplier, totalTime);
			float timePercent = currentTime / totalTime;
			this.animatedTarget = Mathf.Min(this.progressCurve.Evaluate(timePercent) * range + startValue, currentThresholdValue);
			float noFillValue = Mathf.Max(startValue, previousThresholdValue);
			float maxFillValue = Mathf.Min(this.target, currentThresholdValue);
			float fillAmount = (maxFillValue == noFillValue) ? 0f : ((this.animatedTarget - noFillValue) / (maxFillValue - noFillValue));
			this.extraProgress.fillAmount = fillAmount;
			this.invokeUpdateMethod();
			if (this.animatedTarget == currentThresholdValue)
			{
				if (this.animatedTarget < this.target)
				{
					threshold++;
					if (this.ClampAtMaxTier && threshold >= this.thresholds.Count)
					{
						break;
					}
					this.onWrap.Invoke();
					currentThresholdValue = this.ValueForThreshold(threshold);
					previousThresholdValue = this.ValueForThreshold(threshold - 1);
					yield return new WaitForSeconds(this.wrapPauseTime);
					this.onWrapAfterWait.Invoke();
					this.UpdateMainProgressBarVisual(threshold);
				}
				else
				{
					this.onCap.Invoke();
				}
			}
			yield return null;
			if (currentTime >= totalTime)
			{
				goto IL_366;
			}
		}
		this.onCap.Invoke();
		IL_366:
		if (!this.ClampAtMaxTier)
		{
			this.animatedTarget = (float)((int)this.target);
		}
		this.invokeUpdateMethod();
		this.animateToTargetCoroutine = null;
		yield break;
	}

	// Token: 0x17000EFC RID: 3836
	// (get) Token: 0x06003F36 RID: 16182 RVA: 0x0011F5BF File Offset: 0x0011D9BF
	public int CurrentThreshold
	{
		get
		{
			return this.ThresholdForValue(this.current);
		}
	}

	// Token: 0x17000EFD RID: 3837
	// (get) Token: 0x06003F37 RID: 16183 RVA: 0x0011F5CF File Offset: 0x0011D9CF
	public int MaxThreshold
	{
		get
		{
			return this.thresholds.Count - 1;
		}
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x0011F5E0 File Offset: 0x0011D9E0
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

	// Token: 0x17000EFE RID: 3838
	// (get) Token: 0x06003F39 RID: 16185 RVA: 0x0011F65C File Offset: 0x0011DA5C
	public int LastThresholdIncrement
	{
		get
		{
			return (int)((this.thresholds.Count <= 1) ? this.thresholds[this.thresholds.Count - 1] : (this.thresholds[this.thresholds.Count - 1] - this.thresholds[this.thresholds.Count - 2]));
		}
	}

	// Token: 0x06003F3A RID: 16186 RVA: 0x0011F6CC File Offset: 0x0011DACC
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

	// Token: 0x06003F3B RID: 16187 RVA: 0x0011F751 File Offset: 0x0011DB51
	public bool ValueLessThanMaxThreshold(ulong value)
	{
		return value < this.thresholds[this.thresholds.Count - 1];
	}

	// Token: 0x04002ACB RID: 10955
	[SerializeField]
	private Slider mainProgress;

	// Token: 0x04002ACC RID: 10956
	[SerializeField]
	private Image extraProgress;

	// Token: 0x04002ACD RID: 10957
	[SerializeField]
	private List<ulong> thresholds = new List<ulong>
	{
		0UL,
		100UL,
		200UL,
		400UL,
		800UL,
		1200UL
	};

	// Token: 0x04002ACE RID: 10958
	[SerializeField]
	private ulong current;

	// Token: 0x04002ACF RID: 10959
	[SerializeField]
	private float target;

	// Token: 0x04002AD0 RID: 10960
	[SerializeField]
	private float wrapPauseTime;

	// Token: 0x04002AD1 RID: 10961
	[SerializeField]
	private float animationSpeed = 50f;

	// Token: 0x04002AD2 RID: 10962
	public float speedMultiplier = 1f;

	// Token: 0x04002AD3 RID: 10963
	[SerializeField]
	private float maxAnimationTime = 3.5f;

	// Token: 0x04002AD4 RID: 10964
	[SerializeField]
	private AnimationCurve progressCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04002AD5 RID: 10965
	public UnityEvent onCap;

	// Token: 0x04002AD6 RID: 10966
	public UnityEvent onWrap;

	// Token: 0x04002AD7 RID: 10967
	public UnityEvent onWrapAfterWait;

	// Token: 0x04002AD8 RID: 10968
	public UnityEvent onValueUpdate;

	// Token: 0x04002AD9 RID: 10969
	public bool ClampAtMaxTier = true;

	// Token: 0x04002ADC RID: 10972
	private Coroutine animateToTargetCoroutine;

	// Token: 0x04002ADD RID: 10973
	private float animatedTarget;
}
