// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabHeader : MonoBehaviour
{
	public TextMeshProUGUI Title;

	public MenuItemButton Button;

	public float BoundaryX = 84f;

	public float NotificationX = 15f;

	public float SelectedScale = 1f;

	public Color SelectedTextColor;

	public Image SelectedImage;

	public Transform Notification;

	private bool titlePositionDirty;

	private float prevTitleRenderedWidth;

	private Color normalTextColor;

	private bool selected;

	public TabDefinition def
	{
		get;
		private set;
	}

	public bool IsDirty
	{
		get
		{
			return this.titlePositionDirty;
		}
	}

	public void Init(TabDefinition def)
	{
		this.def = def;
		this.normalTextColor = this.Button.TextField.color;
		this.SelectedImage.gameObject.SetActive(false);
	}

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

	public void SetTitle(string text)
	{
		if (this.Title.text != text)
		{
			this.Title.text = text;
			this.titlePositionDirty = true;
		}
	}

	public void ShowNotification(bool show)
	{
		this.Notification.gameObject.SetActive(show);
	}

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
}
