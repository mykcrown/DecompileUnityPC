// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewsHUDPlayerList : GameBehavior
{
	public HorizontalOrVerticalLayoutGroup LayoutGroup;

	public GridLayoutGroup GridGroup;

	public CrewsPlayerGUI CrewsPlayerPrefab;

	public CrewsV3PlayerUI CrewsPlayerV3Prefab;

	private List<ICrewsPlayerUI> guiElements = new List<ICrewsPlayerUI>();

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

	public void TickFrame()
	{
		for (int i = 0; i < this.guiElements.Count; i++)
		{
			this.guiElements[i].TickFrame();
		}
	}
}
