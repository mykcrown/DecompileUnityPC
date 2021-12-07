// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TauntSlotAssignedGroup : MonoBehaviour
{
	public TextMeshProUGUI ItemName;

	public TextMeshProUGUI TypeFlagText;

	public Image TypeFlagBackground;

	public GameObject TypeFlag;

	public float SpacingX = 17f;

	private RectTransform typeFlagBgRect;

	private CanvasGroup canvasGroup;

	private float prevTypeFlagWidth;

	private string currentTypeText;

	private bool isDirty;

	public Action<TauntSlotAssignedGroup> OnCleaned
	{
		get;
		set;
	}

	public float Alpha
	{
		get
		{
			return this.canvasGroup.alpha;
		}
		set
		{
			this.canvasGroup.alpha = value;
		}
	}

	private void Awake()
	{
		this.typeFlagBgRect = this.TypeFlagBackground.GetComponent<RectTransform>();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
	}

	public void UpdateTypeText(string text)
	{
		string b = this.currentTypeText;
		this.currentTypeText = text;
		this.TypeFlagText.text = this.currentTypeText;
		if (this.currentTypeText != b)
		{
			this.isDirty = true;
		}
		else
		{
			this.OnCleaned(this);
		}
	}

	private void Update()
	{
		if (this.isDirty && this.TypeFlagText.renderedWidth != this.prevTypeFlagWidth && this.TypeFlagText.renderedWidth > 1f)
		{
			this.isDirty = false;
			float x = this.TypeFlagText.transform.localPosition.x;
			float num = this.TypeFlagText.renderedWidth + x * 2f;
			Vector2 sizeDelta = this.typeFlagBgRect.sizeDelta;
			sizeDelta.x = num;
			this.typeFlagBgRect.sizeDelta = sizeDelta;
			float x2 = this.TypeFlag.transform.localPosition.x + num + this.SpacingX;
			Vector3 localPosition = this.ItemName.transform.localPosition;
			localPosition.x = x2;
			this.ItemName.transform.localPosition = localPosition;
			this.prevTypeFlagWidth = this.TypeFlagText.renderedWidth;
			this.OnCleaned(this);
		}
	}
}
