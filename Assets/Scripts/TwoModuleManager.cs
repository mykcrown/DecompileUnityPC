// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class TwoModuleManager : IEquipmentSelectorModule
{
	private List<IEquipmentSelectorModule> allModules = new List<IEquipmentSelectorModule>();

	private EquipmentSelectorModule currentlyActive;

	private List<IEquipmentSelectorModule> sendEventsTo = new List<IEquipmentSelectorModule>();

	public void AddModule(IEquipmentSelectorModule module)
	{
		this.allModules.Add(module);
	}

	public void SetActive(EquipmentSelectorModule module)
	{
		this.currentlyActive = module;
		this.sendEventsTo.Clear();
		this.sendEventsTo.Add(module);
	}

	public EquipmentSelectorModule GetCurrent()
	{
		return this.currentlyActive;
	}

	public void LoadItems(List<EquippableItem> items)
	{
		foreach (IEquipmentSelectorModule current in this.allModules)
		{
			current.LoadItems(items);
		}
	}

	public void Activate()
	{
		foreach (IEquipmentSelectorModule current in this.allModules)
		{
			current.Activate();
		}
	}

	public void OnDrawComplete()
	{
		foreach (IEquipmentSelectorModule current in this.allModules)
		{
			current.OnDrawComplete();
		}
	}

	public bool OnCancelPressed()
	{
		bool flag = false;
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			flag |= current.OnCancelPressed();
		}
		return flag;
	}

	public bool OnLeft()
	{
		bool flag = false;
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			flag |= current.OnLeft();
		}
		return flag;
	}

	public bool OnRight()
	{
		bool flag = false;
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			flag |= current.OnRight();
		}
		return flag;
	}

	public void ForceRedraws()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.ForceRedraws();
		}
	}

	public void BeginMenuFocus()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.BeginMenuFocus();
		}
	}

	public void OnMouseModeUpdate()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.OnMouseModeUpdate();
		}
	}

	public void SyncButtonModeSelection()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.SyncButtonModeSelection();
		}
	}

	public void RebuildList()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.RebuildList();
		}
	}

	public void ReleaseSelections()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.ReleaseSelections();
		}
	}

	public void DeselectEquipment()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.DeselectEquipment();
		}
	}

	public void EnterFromRight()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.EnterFromRight();
		}
	}

	public void EnterFromBottom()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.EnterFromBottom();
		}
	}

	public void OnYButton()
	{
		foreach (IEquipmentSelectorModule current in this.sendEventsTo)
		{
			current.OnYButton();
		}
	}
}
