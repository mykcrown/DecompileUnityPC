// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionGUI : ClientBehavior
{
	public CursorTargetButton LeftButton;

	public CursorTargetButton RightButton;

	public TextMeshProUGUI ValueText;

	public TextMeshProUGUI TitleText;

	public Image Background;

	public GameObject LeftArrow;

	public GameObject RightArrow;

	public GameObject OffsetContainer;

	private OptionDescription desc;

	private GameLoadPayload gamePayload;

	private float baseWidth;

	private float baseTitleX;

	private float baseLeftArrowX;

	private float baseRightArrowX;

	private Vector3 leftArrowPosition;

	private Vector3 rightArrowPosition;

	private Vector3 titlePosition;

	private bool isWidthInitialized;

	private bool textIsDirty = true;

	private float lastTextWidth;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	public bool StaticSize
	{
		get;
		set;
	}

	public float StaticWidth
	{
		get;
		set;
	}

	public OptionDescription Desc
	{
		get
		{
			return this.desc;
		}
	}

	public override void Awake()
	{
		base.Awake();
		this.baseWidth = this.Background.rectTransform.sizeDelta.x;
		this.baseTitleX = this.TitleText.transform.localPosition.x;
		this.baseLeftArrowX = this.LeftArrow.transform.localPosition.x;
		this.baseRightArrowX = this.RightArrow.transform.localPosition.x;
		this.LeftButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onLeftArrow);
		this.RightButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onRightArrow);
		this.LeftButton.RepeatOnHold = true;
		this.RightButton.RepeatOnHold = true;
	}

	private void onLeftArrow(CursorTargetButton button, PointerEventData eventData)
	{
		int nextValue = this.getNextValue(-1);
		base.events.Broadcast(new SetBattleSettingRequest(this.desc.type, nextValue));
		base.audioManager.PlayMenuSound(SoundKey.generic_arrowClickLeft, 0f);
	}

	private void onRightArrow(CursorTargetButton button, PointerEventData eventData)
	{
		int nextValue = this.getNextValue(1);
		base.events.Broadcast(new SetBattleSettingRequest(this.desc.type, nextValue));
		base.audioManager.PlayMenuSound(SoundKey.generic_arrowClickRight, 0f);
	}

	private int getNextValue(int direction)
	{
		int num = this.desc.valueList.IndexOf(this.getValue());
		num += direction;
		if (this.desc.allowLooping)
		{
			if (num < 0)
			{
				num = this.desc.valueList.Count - 1;
			}
			else if (num > this.desc.valueList.Count - 1)
			{
				num = 0;
			}
		}
		else
		{
			num = Mathf.Clamp(num, 0, this.desc.valueList.Count - 1);
		}
		return this.desc.valueList[num];
	}

	private void Update()
	{
		if (this.textIsDirty && this.TitleText.renderedWidth > 1f && this.TitleText.renderedWidth != this.lastTextWidth)
		{
			this.textIsDirty = false;
			this.updateSize();
			this.lastTextWidth = this.TitleText.renderedWidth;
		}
	}

	private int getValue()
	{
		return this.gamePayload.battleConfig.settings[this.desc.type];
	}

	private void updateValueText()
	{
		int value = this.getValue();
		if (this.desc.valueDisplayFunction != null)
		{
			this.ValueText.text = this.desc.valueDisplayFunction(value);
		}
		else
		{
			this.ValueText.text = value.ToString();
		}
	}

	public void UpdatePayload(GameLoadPayload gamePayload)
	{
		this.gamePayload = gamePayload;
		this.updateValueText();
	}

	public void LoadFromDesc(OptionDescription desc)
	{
		this.desc = desc;
		if (this.StaticWidth == 0f)
		{
			this.setWidth(desc.width);
		}
		else
		{
			this.setWidth(this.StaticWidth);
		}
		this.SetTitle(this.localization.GetText("ui.characterSelect.mainOption." + desc.type.ToString()));
	}

	private void setWidth(float width)
	{
		this.Background.rectTransform.sizeDelta = new Vector2(width, this.Background.rectTransform.sizeDelta.y);
		float num = (width - this.baseWidth) / 2f;
		this.titlePosition = this.TitleText.transform.localPosition;
		this.titlePosition.x = this.baseTitleX - num;
		this.leftArrowPosition = this.LeftArrow.transform.localPosition;
		this.leftArrowPosition.x = this.baseLeftArrowX - num;
		this.rightArrowPosition = this.RightArrow.transform.localPosition;
		this.rightArrowPosition.x = this.baseRightArrowX + num;
		this.ValueText.rectTransform.sizeDelta = new Vector2(width, this.ValueText.rectTransform.sizeDelta.y);
		this.isWidthInitialized = true;
		this.updateSize();
	}

	public void SetTitle(string title)
	{
		this.TitleText.text = title;
		this.updateSize();
	}

	private void syncWidth()
	{
		if (this.isWidthInitialized)
		{
			this.LeftArrow.transform.localPosition = this.leftArrowPosition;
			this.RightArrow.transform.localPosition = this.rightArrowPosition;
			this.TitleText.transform.localPosition = this.titlePosition;
		}
	}

	private void updateSize()
	{
		this.syncWidth();
		if (!this.StaticSize && this.TitleText.renderedWidth > 0f)
		{
			float num = this.TitleText.transform.localPosition.x - this.TitleText.renderedWidth;
			float num2 = this.Background.transform.localPosition.x + this.Background.rectTransform.sizeDelta.x / 2f;
			float x = num2 - num;
			base.GetComponent<RectTransform>().sizeDelta = new Vector2(x, 0f);
			float num3 = this.Background.transform.localPosition.x - this.Background.rectTransform.sizeDelta.x / 2f - num;
			Vector3 localPosition = this.OffsetContainer.transform.localPosition;
			localPosition.x = num3 / 2f;
			this.OffsetContainer.transform.localPosition = localPosition;
			LayoutGroup component = base.transform.parent.GetComponent<LayoutGroup>();
			if (component != null)
			{
				component.Redraw();
			}
		}
	}

	public void Removed()
	{
		this.LeftButton.Removed();
		this.RightButton.Removed();
	}
}
