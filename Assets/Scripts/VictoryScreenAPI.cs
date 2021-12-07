// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class VictoryScreenAPI : IVictoryScreenAPI
{
	[Inject]
	public IUserGlobalEquippedModel userGlobalEquippedModel
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	public Dictionary<int, Netsuke> GetLocalEquipmentNetsuke(PlayerNum playerNum)
	{
		Dictionary<int, Netsuke> dictionary = new Dictionary<int, Netsuke>();
		for (int i = 0; i < UserGlobalEquippedModel.NETSUKE_SLOTS; i++)
		{
			EquipmentID equippedNetsuke = this.userGlobalEquippedModel.GetEquippedNetsuke(i, this.userInputManager.GetBestPortId(playerNum));
			if (!equippedNetsuke.IsNull())
			{
				Netsuke netsukeFromItem = this.equipmentModel.GetNetsukeFromItem(equippedNetsuke);
				if (netsukeFromItem != null)
				{
					dictionary[i] = netsukeFromItem;
				}
			}
		}
		return dictionary;
	}
}
