using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A37 RID: 2615
public class TauntSlotAssignedGroup : MonoBehaviour
{
	// Token: 0x1700122D RID: 4653
	// (get) Token: 0x06004C98 RID: 19608 RVA: 0x00144A16 File Offset: 0x00142E16
	// (set) Token: 0x06004C99 RID: 19609 RVA: 0x00144A1E File Offset: 0x00142E1E
	public Action<TauntSlotAssignedGroup> OnCleaned { get; set; }

	// Token: 0x06004C9A RID: 19610 RVA: 0x00144A27 File Offset: 0x00142E27
	private void Awake()
	{
		this.typeFlagBgRect = this.TypeFlagBackground.GetComponent<RectTransform>();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x1700122E RID: 4654
	// (get) Token: 0x06004C9B RID: 19611 RVA: 0x00144A46 File Offset: 0x00142E46
	// (set) Token: 0x06004C9C RID: 19612 RVA: 0x00144A53 File Offset: 0x00142E53
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

	// Token: 0x06004C9D RID: 19613 RVA: 0x00144A64 File Offset: 0x00142E64
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

	// Token: 0x06004C9E RID: 19614 RVA: 0x00144ABC File Offset: 0x00142EBC
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

	// Token: 0x0400322C RID: 12844
	public TextMeshProUGUI ItemName;

	// Token: 0x0400322D RID: 12845
	public TextMeshProUGUI TypeFlagText;

	// Token: 0x0400322E RID: 12846
	public Image TypeFlagBackground;

	// Token: 0x0400322F RID: 12847
	public GameObject TypeFlag;

	// Token: 0x04003230 RID: 12848
	public float SpacingX = 17f;

	// Token: 0x04003231 RID: 12849
	private RectTransform typeFlagBgRect;

	// Token: 0x04003232 RID: 12850
	private CanvasGroup canvasGroup;

	// Token: 0x04003233 RID: 12851
	private float prevTypeFlagWidth;

	// Token: 0x04003234 RID: 12852
	private string currentTypeText;

	// Token: 0x04003235 RID: 12853
	private bool isDirty;
}
