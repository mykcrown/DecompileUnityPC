using System;
using System.Collections.Generic;

// Token: 0x02000A86 RID: 2694
public class VictoryScreenAPI : IVictoryScreenAPI
{
	// Token: 0x170012B2 RID: 4786
	// (get) Token: 0x06004EF2 RID: 20210 RVA: 0x0014AFDA File Offset: 0x001493DA
	// (set) Token: 0x06004EF3 RID: 20211 RVA: 0x0014AFE2 File Offset: 0x001493E2
	[Inject]
	public IUserGlobalEquippedModel userGlobalEquippedModel { get; set; }

	// Token: 0x170012B3 RID: 4787
	// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x0014AFEB File Offset: 0x001493EB
	// (set) Token: 0x06004EF5 RID: 20213 RVA: 0x0014AFF3 File Offset: 0x001493F3
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170012B4 RID: 4788
	// (get) Token: 0x06004EF6 RID: 20214 RVA: 0x0014AFFC File Offset: 0x001493FC
	// (set) Token: 0x06004EF7 RID: 20215 RVA: 0x0014B004 File Offset: 0x00149404
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x06004EF8 RID: 20216 RVA: 0x0014B010 File Offset: 0x00149410
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
