using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008B2 RID: 2226
public class CrewsHUDPlayerList : GameBehavior
{
	// Token: 0x060037EE RID: 14318 RVA: 0x00106488 File Offset: 0x00104888
	public void Initialize(BattleSettings config, List<PlayerSelectionInfo> teamPlayers, TeamNum team, CrewsGUISide side)
	{
		for (int i = 0; i < teamPlayers.Count; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = teamPlayers[i];
			if (playerSelectionInfo.type != PlayerType.None)
			{
				ICrewsPlayerUI crewsPlayerUI;
				if (this.CrewsPlayerPrefab != null)
				{
					crewsPlayerUI = UnityEngine.Object.Instantiate<CrewsPlayerGUI>(this.CrewsPlayerPrefab);
				}
				else
				{
					crewsPlayerUI = UnityEngine.Object.Instantiate<CrewsV3PlayerUI>(this.CrewsPlayerV3Prefab);
				}
				crewsPlayerUI.Initialize(config, playerSelectionInfo, team, side);
				if (this.GridGroup != null)
				{
					crewsPlayerUI.Transform.SetParent(this.GridGroup.transform, false);
				}
				else
				{
					crewsPlayerUI.Transform.SetParent(this.LayoutGroup.transform, false);
				}
				this.guiElements.Add(crewsPlayerUI);
			}
		}
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x0010654C File Offset: 0x0010494C
	public void TickFrame()
	{
		for (int i = 0; i < this.guiElements.Count; i++)
		{
			this.guiElements[i].TickFrame();
		}
	}

	// Token: 0x04002625 RID: 9765
	public HorizontalOrVerticalLayoutGroup LayoutGroup;

	// Token: 0x04002626 RID: 9766
	public GridLayoutGroup GridGroup;

	// Token: 0x04002627 RID: 9767
	public CrewsPlayerGUI CrewsPlayerPrefab;

	// Token: 0x04002628 RID: 9768
	public CrewsV3PlayerUI CrewsPlayerV3Prefab;

	// Token: 0x04002629 RID: 9769
	private List<ICrewsPlayerUI> guiElements = new List<ICrewsPlayerUI>();
}
