// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class TrainingWidget : ClientSelectable, ISubmitHandler, ICancelHandler, IEventSystemHandler
{
	private Button leftArrow;

	private Button rightArrow;

	public Button LeftArrowPrefab;

	public Button RightArrowPrefab;

	private bool _pointerEntered;

	public abstract void OnLeft();

	public abstract void OnRight();

	protected override void Awake()
	{
		base.Awake();
		if (this.leftArrow == null)
		{
			this.leftArrow = UnityEngine.Object.Instantiate<Button>(this.LeftArrowPrefab);
			this.leftArrow.transform.SetParent(base.transform, false);
		}
		this.leftArrow.onClick.AddListener(new UnityAction(this._Awake_m__0));
		this.leftArrow.gameObject.SetActive(false);
		if (this.rightArrow == null)
		{
			this.rightArrow = UnityEngine.Object.Instantiate<Button>(this.RightArrowPrefab);
			this.rightArrow.transform.SetParent(base.transform, false);
		}
		this.rightArrow.onClick.AddListener(new UnityAction(this._Awake_m__1));
		this.rightArrow.gameObject.SetActive(false);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		this.leftArrow.gameObject.SetActive(true);
		this.rightArrow.gameObject.SetActive(true);
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		if (!this._pointerEntered)
		{
			base.OnDeselect(eventData);
			this.leftArrow.gameObject.SetActive(false);
			this.rightArrow.gameObject.SetActive(false);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		this._pointerEntered = true;
		this.OnSelect(eventData);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		this._pointerEntered = false;
		this.OnDeselect(eventData);
	}

	public void OnSubmit(BaseEventData eventData)
	{
		this.OnRight();
	}

	public void OnCancel(BaseEventData eventData)
	{
		this.OnLeft();
	}

	private void _Awake_m__0()
	{
		this.OnLeft();
	}

	private void _Awake_m__1()
	{
		this.OnRight();
	}
}
