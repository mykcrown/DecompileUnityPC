using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020008EE RID: 2286
public class OptionGUI : ClientBehavior
{
	// Token: 0x17000E0F RID: 3599
	// (get) Token: 0x06003A88 RID: 14984 RVA: 0x001120BA File Offset: 0x001104BA
	// (set) Token: 0x06003A89 RID: 14985 RVA: 0x001120C2 File Offset: 0x001104C2
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000E10 RID: 3600
	// (get) Token: 0x06003A8A RID: 14986 RVA: 0x001120CB File Offset: 0x001104CB
	// (set) Token: 0x06003A8B RID: 14987 RVA: 0x001120D3 File Offset: 0x001104D3
	public bool StaticSize { get; set; }

	// Token: 0x17000E11 RID: 3601
	// (get) Token: 0x06003A8C RID: 14988 RVA: 0x001120DC File Offset: 0x001104DC
	// (set) Token: 0x06003A8D RID: 14989 RVA: 0x001120E4 File Offset: 0x001104E4
	public float StaticWidth { get; set; }

	// Token: 0x06003A8E RID: 14990 RVA: 0x001120F0 File Offset: 0x001104F0
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

	// Token: 0x17000E12 RID: 3602
	// (get) Token: 0x06003A8F RID: 14991 RVA: 0x001121C1 File Offset: 0x001105C1
	public OptionDescription Desc
	{
		get
		{
			return this.desc;
		}
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x001121CC File Offset: 0x001105CC
	private void onLeftArrow(CursorTargetButton button, PointerEventData eventData)
	{
		int nextValue = this.getNextValue(-1);
		base.events.Broadcast(new SetBattleSettingRequest(this.desc.type, nextValue));
		base.audioManager.PlayMenuSound(SoundKey.generic_arrowClickLeft, 0f);
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x00112210 File Offset: 0x00110610
	private void onRightArrow(CursorTargetButton button, PointerEventData eventData)
	{
		int nextValue = this.getNextValue(1);
		base.events.Broadcast(new SetBattleSettingRequest(this.desc.type, nextValue));
		base.audioManager.PlayMenuSound(SoundKey.generic_arrowClickRight, 0f);
	}

	// Token: 0x06003A92 RID: 14994 RVA: 0x00112254 File Offset: 0x00110654
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

	// Token: 0x06003A93 RID: 14995 RVA: 0x001122F8 File Offset: 0x001106F8
	private void Update()
	{
		if (this.textIsDirty && this.TitleText.renderedWidth > 1f && this.TitleText.renderedWidth != this.lastTextWidth)
		{
			this.textIsDirty = false;
			this.updateSize();
			this.lastTextWidth = this.TitleText.renderedWidth;
		}
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x00112359 File Offset: 0x00110759
	private int getValue()
	{
		return this.gamePayload.battleConfig.settings[this.desc.type];
	}

	// Token: 0x06003A95 RID: 14997 RVA: 0x0011237C File Offset: 0x0011077C
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

	// Token: 0x06003A96 RID: 14998 RVA: 0x001123D9 File Offset: 0x001107D9
	public void UpdatePayload(GameLoadPayload gamePayload)
	{
		this.gamePayload = gamePayload;
		this.updateValueText();
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x001123E8 File Offset: 0x001107E8
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

	// Token: 0x06003A98 RID: 15000 RVA: 0x00112458 File Offset: 0x00110858
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

	// Token: 0x06003A99 RID: 15001 RVA: 0x00112558 File Offset: 0x00110958
	public void SetTitle(string title)
	{
		this.TitleText.text = title;
		this.updateSize();
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x0011256C File Offset: 0x0011096C
	private void syncWidth()
	{
		if (this.isWidthInitialized)
		{
			this.LeftArrow.transform.localPosition = this.leftArrowPosition;
			this.RightArrow.transform.localPosition = this.rightArrowPosition;
			this.TitleText.transform.localPosition = this.titlePosition;
		}
	}

	// Token: 0x06003A9B RID: 15003 RVA: 0x001125C8 File Offset: 0x001109C8
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

	// Token: 0x06003A9C RID: 15004 RVA: 0x0011270B File Offset: 0x00110B0B
	public void Removed()
	{
		this.LeftButton.Removed();
		this.RightButton.Removed();
	}

	// Token: 0x04002851 RID: 10321
	public CursorTargetButton LeftButton;

	// Token: 0x04002852 RID: 10322
	public CursorTargetButton RightButton;

	// Token: 0x04002853 RID: 10323
	public TextMeshProUGUI ValueText;

	// Token: 0x04002854 RID: 10324
	public TextMeshProUGUI TitleText;

	// Token: 0x04002855 RID: 10325
	public Image Background;

	// Token: 0x04002856 RID: 10326
	public GameObject LeftArrow;

	// Token: 0x04002857 RID: 10327
	public GameObject RightArrow;

	// Token: 0x04002858 RID: 10328
	public GameObject OffsetContainer;

	// Token: 0x04002859 RID: 10329
	private OptionDescription desc;

	// Token: 0x0400285A RID: 10330
	private GameLoadPayload gamePayload;

	// Token: 0x0400285D RID: 10333
	private float baseWidth;

	// Token: 0x0400285E RID: 10334
	private float baseTitleX;

	// Token: 0x0400285F RID: 10335
	private float baseLeftArrowX;

	// Token: 0x04002860 RID: 10336
	private float baseRightArrowX;

	// Token: 0x04002861 RID: 10337
	private Vector3 leftArrowPosition;

	// Token: 0x04002862 RID: 10338
	private Vector3 rightArrowPosition;

	// Token: 0x04002863 RID: 10339
	private Vector3 titlePosition;

	// Token: 0x04002864 RID: 10340
	private bool isWidthInitialized;

	// Token: 0x04002865 RID: 10341
	private bool textIsDirty = true;

	// Token: 0x04002866 RID: 10342
	private float lastTextWidth;
}
