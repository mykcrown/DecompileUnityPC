using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000951 RID: 2385
public class TabHeader : MonoBehaviour
{
	// Token: 0x17000F02 RID: 3842
	// (get) Token: 0x06003F55 RID: 16213 RVA: 0x001200F8 File Offset: 0x0011E4F8
	// (set) Token: 0x06003F56 RID: 16214 RVA: 0x00120100 File Offset: 0x0011E500
	public TabDefinition def { get; private set; }

	// Token: 0x06003F57 RID: 16215 RVA: 0x00120109 File Offset: 0x0011E509
	public void Init(TabDefinition def)
	{
		this.def = def;
		this.normalTextColor = this.Button.TextField.color;
		this.SelectedImage.gameObject.SetActive(false);
	}

	// Token: 0x06003F58 RID: 16216 RVA: 0x0012013C File Offset: 0x0011E53C
	public void SetCurrentlySelected(bool selected)
	{
		if (this.selected != selected)
		{
			this.selected = selected;
			if (selected)
			{
				this.Button.InteractableButton.interactable = false;
				this.Button.Freeze();
				this.Button.ScaleTarget.transform.localScale = new Vector3(this.SelectedScale, this.SelectedScale, this.SelectedScale);
				this.Button.TextField.color = this.SelectedTextColor;
				this.SelectedImage.gameObject.SetActive(true);
			}
			else
			{
				this.Button.InteractableButton.interactable = true;
				this.Button.ScaleTarget.transform.localScale = new Vector3(1f, 1f, 1f);
				this.Button.TextField.color = this.normalTextColor;
				this.Button.Unfreeze();
				this.SelectedImage.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06003F59 RID: 16217 RVA: 0x00120242 File Offset: 0x0011E642
	public void SetTitle(string text)
	{
		if (this.Title.text != text)
		{
			this.Title.text = text;
			this.titlePositionDirty = true;
		}
	}

	// Token: 0x06003F5A RID: 16218 RVA: 0x0012026D File Offset: 0x0011E66D
	public void ShowNotification(bool show)
	{
		this.Notification.gameObject.SetActive(show);
	}

	// Token: 0x06003F5B RID: 16219 RVA: 0x00120280 File Offset: 0x0011E680
	private void Update()
	{
		if (this.titlePositionDirty && this.Title.renderedWidth > 1f && this.Title.renderedWidth != this.prevTitleRenderedWidth)
		{
			float num = this.Title.renderedWidth + this.BoundaryX;
			RectTransform component = base.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(num, component.sizeDelta.y);
			RectTransform component2 = this.Button.ScaleTarget.GetComponent<RectTransform>();
			float num2 = this.Title.renderedWidth / num;
			Vector2 pivot = component2.pivot;
			pivot.x = 0.5f * num2;
			component2.pivot = pivot;
			RectTransform component3 = this.Button.InteractableButton.GetComponent<RectTransform>();
			Vector2 offsetMax = component3.offsetMax;
			offsetMax.x = -this.BoundaryX - component3.offsetMin.x;
			component3.offsetMax = offsetMax;
			this.titlePositionDirty = false;
			this.prevTitleRenderedWidth = this.Title.renderedWidth;
			this.SelectedImage.rectTransform.sizeDelta = new Vector2(num, this.SelectedImage.rectTransform.sizeDelta.y);
			Vector3 localPosition = this.Notification.transform.localPosition;
			localPosition.x = this.Title.renderedWidth / 2f + this.NotificationX;
			this.Notification.transform.localPosition = localPosition;
		}
	}

	// Token: 0x17000F03 RID: 3843
	// (get) Token: 0x06003F5C RID: 16220 RVA: 0x00120403 File Offset: 0x0011E803
	public bool IsDirty
	{
		get
		{
			return this.titlePositionDirty;
		}
	}

	// Token: 0x04002AF0 RID: 10992
	public TextMeshProUGUI Title;

	// Token: 0x04002AF1 RID: 10993
	public MenuItemButton Button;

	// Token: 0x04002AF2 RID: 10994
	public float BoundaryX = 84f;

	// Token: 0x04002AF3 RID: 10995
	public float NotificationX = 15f;

	// Token: 0x04002AF4 RID: 10996
	public float SelectedScale = 1f;

	// Token: 0x04002AF5 RID: 10997
	public Color SelectedTextColor;

	// Token: 0x04002AF6 RID: 10998
	public Image SelectedImage;

	// Token: 0x04002AF7 RID: 10999
	public Transform Notification;

	// Token: 0x04002AF8 RID: 11000
	private bool titlePositionDirty;

	// Token: 0x04002AF9 RID: 11001
	private float prevTitleRenderedWidth;

	// Token: 0x04002AFB RID: 11003
	private Color normalTextColor;

	// Token: 0x04002AFC RID: 11004
	private bool selected;
}
