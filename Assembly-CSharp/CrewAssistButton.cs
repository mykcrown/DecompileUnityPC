using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008AC RID: 2220
public class CrewAssistButton : MonoBehaviour
{
	// Token: 0x060037BD RID: 14269 RVA: 0x00105314 File Offset: 0x00103714
	private void Start()
	{
		this.ourTransform = base.GetComponent<RectTransform>();
		this.visiblePosition = this.slideOutContainer.localPosition;
		this.visibleHeight = this.ourTransform.sizeDelta.y;
		this.invisiblePosition = this.visiblePosition + new Vector3(((!this.slidesLeft) ? 1f : -1f) * this.slideDistance, 0f, 0f);
		this.IsVisible = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x001053AB File Offset: 0x001037AB
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

	// Token: 0x060037BF RID: 14271 RVA: 0x001053D6 File Offset: 0x001037D6
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

	// Token: 0x060037C0 RID: 14272 RVA: 0x001053F0 File Offset: 0x001037F0
	private IEnumerator SlideOutAction()
	{
		while (this.slideInRoutine != null)
		{
			yield return null;
		}
		float accumulatedTime = 0f;
		while (accumulatedTime < this.slideDuration)
		{
			accumulatedTime = Mathf.Min(this.slideDuration, accumulatedTime + Time.smoothDeltaTime);
			float slidePercent = MathUtil.MixOut(0f, 1f, accumulatedTime / this.slideDuration, this.slideStrength);
			this.slideOutContainer.localPosition = Vector3.Lerp(this.visiblePosition, this.invisiblePosition, slidePercent);
			yield return null;
		}
		accumulatedTime = 0f;
		while (accumulatedTime < this.collapseDuration)
		{
			accumulatedTime = Mathf.Min(this.collapseDuration, accumulatedTime + Time.smoothDeltaTime);
			this.ourTransform.sizeDelta = new Vector2(this.ourTransform.sizeDelta.x, MathUtil.Mix(this.visibleHeight, 0f, accumulatedTime / this.collapseDuration, 1f));
			yield return null;
		}
		this.IsVisible = false;
		this.slideOutRoutine = null;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x0010540C File Offset: 0x0010380C
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

	// Token: 0x060037C2 RID: 14274 RVA: 0x00105484 File Offset: 0x00103884
	private IEnumerator SlideInAction()
	{
		while (this.slideOutRoutine != null)
		{
			yield return null;
		}
		float accumulatedTime = 0f;
		while (accumulatedTime < this.slideDuration)
		{
			accumulatedTime = Mathf.Min(this.slideDuration, accumulatedTime + Time.smoothDeltaTime);
			float slidePercent = MathUtil.MixOut(0f, 1f, accumulatedTime / this.slideDuration, this.slideStrength);
			this.slideOutContainer.localPosition = Vector3.Lerp(this.invisiblePosition, this.visiblePosition, slidePercent);
			yield return null;
		}
		this.IsVisible = true;
		this.slideInRoutine = null;
		yield break;
	}

	// Token: 0x040025EB RID: 9707
	private RectTransform ourTransform;

	// Token: 0x040025EC RID: 9708
	public RectTransform slideOutContainer;

	// Token: 0x040025ED RID: 9709
	public bool slidesLeft;

	// Token: 0x040025EE RID: 9710
	public float slideDuration = 0.2f;

	// Token: 0x040025EF RID: 9711
	public float collapseDuration = 0.15f;

	// Token: 0x040025F0 RID: 9712
	public float slideDistance = 200f;

	// Token: 0x040025F1 RID: 9713
	public float slideStrength = 2f;

	// Token: 0x040025F2 RID: 9714
	private Coroutine slideOutRoutine;

	// Token: 0x040025F3 RID: 9715
	private Coroutine slideInRoutine;

	// Token: 0x040025F4 RID: 9716
	private Vector3 visiblePosition;

	// Token: 0x040025F5 RID: 9717
	private Vector3 invisiblePosition;

	// Token: 0x040025F6 RID: 9718
	private float visibleHeight;

	// Token: 0x040025F7 RID: 9719
	public bool IsVisible;
}
