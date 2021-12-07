using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000A3F RID: 2623
public abstract class TrainingWidget : ClientSelectable, ISubmitHandler, ICancelHandler, IEventSystemHandler
{
	// Token: 0x06004CC7 RID: 19655
	public abstract void OnLeft();

	// Token: 0x06004CC8 RID: 19656
	public abstract void OnRight();

	// Token: 0x06004CC9 RID: 19657 RVA: 0x0014504C File Offset: 0x0014344C
	protected override void Awake()
	{
		base.Awake();
		if (this.leftArrow == null)
		{
			this.leftArrow = UnityEngine.Object.Instantiate<Button>(this.LeftArrowPrefab);
			this.leftArrow.transform.SetParent(base.transform, false);
		}
		this.leftArrow.onClick.AddListener(delegate()
		{
			this.OnLeft();
		});
		this.leftArrow.gameObject.SetActive(false);
		if (this.rightArrow == null)
		{
			this.rightArrow = UnityEngine.Object.Instantiate<Button>(this.RightArrowPrefab);
			this.rightArrow.transform.SetParent(base.transform, false);
		}
		this.rightArrow.onClick.AddListener(delegate()
		{
			this.OnRight();
		});
		this.rightArrow.gameObject.SetActive(false);
	}

	// Token: 0x06004CCA RID: 19658 RVA: 0x0014512B File Offset: 0x0014352B
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		this.leftArrow.gameObject.SetActive(true);
		this.rightArrow.gameObject.SetActive(true);
	}

	// Token: 0x06004CCB RID: 19659 RVA: 0x00145156 File Offset: 0x00143556
	public override void OnDeselect(BaseEventData eventData)
	{
		if (!this._pointerEntered)
		{
			base.OnDeselect(eventData);
			this.leftArrow.gameObject.SetActive(false);
			this.rightArrow.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004CCC RID: 19660 RVA: 0x0014518C File Offset: 0x0014358C
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this._pointerEntered = true;
		this.OnSelect(eventData);
	}

	// Token: 0x06004CCD RID: 19661 RVA: 0x0014519C File Offset: 0x0014359C
	public override void OnPointerExit(PointerEventData eventData)
	{
		this._pointerEntered = false;
		this.OnDeselect(eventData);
	}

	// Token: 0x06004CCE RID: 19662 RVA: 0x001451AC File Offset: 0x001435AC
	public void OnSubmit(BaseEventData eventData)
	{
		this.OnRight();
	}

	// Token: 0x06004CCF RID: 19663 RVA: 0x001451B4 File Offset: 0x001435B4
	public void OnCancel(BaseEventData eventData)
	{
		this.OnLeft();
	}

	// Token: 0x04003259 RID: 12889
	private Button leftArrow;

	// Token: 0x0400325A RID: 12890
	private Button rightArrow;

	// Token: 0x0400325B RID: 12891
	public Button LeftArrowPrefab;

	// Token: 0x0400325C RID: 12892
	public Button RightArrowPrefab;

	// Token: 0x0400325D RID: 12893
	private bool _pointerEntered;
}
