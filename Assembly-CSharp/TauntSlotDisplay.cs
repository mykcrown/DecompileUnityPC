using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A38 RID: 2616
public class TauntSlotDisplay : MonoBehaviour
{
	// Token: 0x1700122F RID: 4655
	// (get) Token: 0x06004CA0 RID: 19616 RVA: 0x00144BF5 File Offset: 0x00142FF5
	// (set) Token: 0x06004CA1 RID: 19617 RVA: 0x00144BFD File Offset: 0x00142FFD
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17001230 RID: 4656
	// (get) Token: 0x06004CA2 RID: 19618 RVA: 0x00144C06 File Offset: 0x00143006
	// (set) Token: 0x06004CA3 RID: 19619 RVA: 0x00144C0E File Offset: 0x0014300E
	[Inject]
	public EquipTauntDialogAPI api { get; set; }

	// Token: 0x06004CA4 RID: 19620 RVA: 0x00144C18 File Offset: 0x00143018
	[PostConstruct]
	public void Init()
	{
		this.assignedGroups.Add(this.AssignedGroup1);
		this.assignedGroups.Add(this.AssignedGroup2);
		this.slotImageMap[TauntSlot.UP] = this.Up;
		this.slotImageMap[TauntSlot.DOWN] = this.Down;
		this.slotImageMap[TauntSlot.RIGHT] = this.Right;
		this.slotImageMap[TauntSlot.LEFT] = this.Left;
		this.slotHighlightMap[TauntSlot.UP] = this.UpHighlight;
		this.slotHighlightMap[TauntSlot.DOWN] = this.DownHighlight;
		this.slotHighlightMap[TauntSlot.RIGHT] = this.RightHighlight;
		this.slotHighlightMap[TauntSlot.LEFT] = this.LeftHighlight;
		foreach (TauntSlotAssignedGroup tauntSlotAssignedGroup in this.assignedGroups)
		{
			tauntSlotAssignedGroup.TypeFlagText.text = string.Empty;
			tauntSlotAssignedGroup.ItemName.text = string.Empty;
			tauntSlotAssignedGroup.OnCleaned = new Action<TauntSlotAssignedGroup>(this.onGroupCleaned);
			tauntSlotAssignedGroup.gameObject.SetActive(false);
		}
		this.UnassignedGroup.SetActive(true);
		this.currentAssignedGroup = this.AssignedGroup1;
	}

	// Token: 0x06004CA5 RID: 19621 RVA: 0x00144D78 File Offset: 0x00143178
	public void SetSlot(TauntSlot slot)
	{
		this.slotImageMap[slot].overrideSprite = this.slotHighlightMap[slot];
	}

	// Token: 0x06004CA6 RID: 19622 RVA: 0x00144D98 File Offset: 0x00143198
	public void SetAssigned(EquippableItem item)
	{
		if (this.assignedTo != item || !this.initialized)
		{
			this.initialized = true;
			this.assignedTo = item;
			this.targetGroup = this.getOtherGroup(this.currentAssignedGroup);
			this.targetGroup.Alpha = 0f;
			this.targetGroup.gameObject.SetActive(true);
			this.targetGroup.ItemName.text = this.api.GetLocalizedEquipmentName(item);
			this.targetGroup.UpdateTypeText(this.localization.GetText("equipType.singular." + item.type));
		}
	}

	// Token: 0x06004CA7 RID: 19623 RVA: 0x00144E44 File Offset: 0x00143244
	public void SetUnassigned()
	{
		if (this.assignedTo != null || !this.initialized)
		{
			this.initialized = true;
			this.assignedTo = null;
			this.UnassignedGroup.SetActive(true);
			foreach (TauntSlotAssignedGroup tauntSlotAssignedGroup in this.assignedGroups)
			{
				tauntSlotAssignedGroup.gameObject.SetActive(false);
			}
			this.currentAssignedGroup = null;
		}
	}

	// Token: 0x06004CA8 RID: 19624 RVA: 0x00144EDC File Offset: 0x001432DC
	private void onGroupCleaned(TauntSlotAssignedGroup theGroup)
	{
		if (theGroup == this.targetGroup)
		{
			TauntSlotAssignedGroup otherGroup = this.getOtherGroup(this.targetGroup);
			this.targetGroup.Alpha = 1f;
			this.targetGroup.gameObject.SetActive(true);
			otherGroup.gameObject.SetActive(false);
			this.UnassignedGroup.SetActive(false);
			this.currentAssignedGroup = this.targetGroup;
			this.targetGroup = null;
		}
	}

	// Token: 0x06004CA9 RID: 19625 RVA: 0x00144F53 File Offset: 0x00143353
	private TauntSlotAssignedGroup getOtherGroup(TauntSlotAssignedGroup group)
	{
		if (group == this.AssignedGroup1)
		{
			return this.AssignedGroup2;
		}
		return this.AssignedGroup1;
	}

	// Token: 0x04003239 RID: 12857
	public Image Up;

	// Token: 0x0400323A RID: 12858
	public Image Down;

	// Token: 0x0400323B RID: 12859
	public Image Right;

	// Token: 0x0400323C RID: 12860
	public Image Left;

	// Token: 0x0400323D RID: 12861
	public Sprite UpHighlight;

	// Token: 0x0400323E RID: 12862
	public Sprite DownHighlight;

	// Token: 0x0400323F RID: 12863
	public Sprite RightHighlight;

	// Token: 0x04003240 RID: 12864
	public Sprite LeftHighlight;

	// Token: 0x04003241 RID: 12865
	public TauntSlotAssignedGroup AssignedGroup1;

	// Token: 0x04003242 RID: 12866
	public TauntSlotAssignedGroup AssignedGroup2;

	// Token: 0x04003243 RID: 12867
	public GameObject UnassignedGroup;

	// Token: 0x04003244 RID: 12868
	private TauntSlotAssignedGroup currentAssignedGroup;

	// Token: 0x04003245 RID: 12869
	private TauntSlotAssignedGroup targetGroup;

	// Token: 0x04003246 RID: 12870
	public float SpacingX = 17f;

	// Token: 0x04003247 RID: 12871
	private Dictionary<TauntSlot, Image> slotImageMap = new Dictionary<TauntSlot, Image>();

	// Token: 0x04003248 RID: 12872
	private Dictionary<TauntSlot, Sprite> slotHighlightMap = new Dictionary<TauntSlot, Sprite>();

	// Token: 0x04003249 RID: 12873
	private List<TauntSlotAssignedGroup> assignedGroups = new List<TauntSlotAssignedGroup>();

	// Token: 0x0400324A RID: 12874
	private EquippableItem assignedTo;

	// Token: 0x0400324B RID: 12875
	private bool initialized;
}
