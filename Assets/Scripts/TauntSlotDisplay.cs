// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TauntSlotDisplay : MonoBehaviour
{
	public Image Up;

	public Image Down;

	public Image Right;

	public Image Left;

	public Sprite UpHighlight;

	public Sprite DownHighlight;

	public Sprite RightHighlight;

	public Sprite LeftHighlight;

	public TauntSlotAssignedGroup AssignedGroup1;

	public TauntSlotAssignedGroup AssignedGroup2;

	public GameObject UnassignedGroup;

	private TauntSlotAssignedGroup currentAssignedGroup;

	private TauntSlotAssignedGroup targetGroup;

	public float SpacingX = 17f;

	private Dictionary<TauntSlot, Image> slotImageMap = new Dictionary<TauntSlot, Image>();

	private Dictionary<TauntSlot, Sprite> slotHighlightMap = new Dictionary<TauntSlot, Sprite>();

	private List<TauntSlotAssignedGroup> assignedGroups = new List<TauntSlotAssignedGroup>();

	private EquippableItem assignedTo;

	private bool initialized;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public EquipTauntDialogAPI api
	{
		get;
		set;
	}

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
		foreach (TauntSlotAssignedGroup current in this.assignedGroups)
		{
			current.TypeFlagText.text = string.Empty;
			current.ItemName.text = string.Empty;
			current.OnCleaned = new Action<TauntSlotAssignedGroup>(this.onGroupCleaned);
			current.gameObject.SetActive(false);
		}
		this.UnassignedGroup.SetActive(true);
		this.currentAssignedGroup = this.AssignedGroup1;
	}

	public void SetSlot(TauntSlot slot)
	{
		this.slotImageMap[slot].overrideSprite = this.slotHighlightMap[slot];
	}

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

	public void SetUnassigned()
	{
		if (this.assignedTo != null || !this.initialized)
		{
			this.initialized = true;
			this.assignedTo = null;
			this.UnassignedGroup.SetActive(true);
			foreach (TauntSlotAssignedGroup current in this.assignedGroups)
			{
				current.gameObject.SetActive(false);
			}
			this.currentAssignedGroup = null;
		}
	}

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

	private TauntSlotAssignedGroup getOtherGroup(TauntSlotAssignedGroup group)
	{
		if (group == this.AssignedGroup1)
		{
			return this.AssignedGroup2;
		}
		return this.AssignedGroup1;
	}
}
