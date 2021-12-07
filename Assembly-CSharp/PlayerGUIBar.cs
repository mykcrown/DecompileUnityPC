using System;
using System.Collections;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x020008D2 RID: 2258
public class PlayerGUIBar : GUIBar
{
	// Token: 0x0600390D RID: 14605 RVA: 0x0010BD10 File Offset: 0x0010A110
	public void Initialize(BattleSettings battleConfig, PlayerSelectionList players)
	{
		bool flag = false;
		if (base.gameDataManager != null)
		{
			GameModeData dataByType = base.gameDataManager.GameModeData.GetDataByType(battleConfig.mode);
			if (dataByType != null && dataByType.settings.teamMode == TeamMode.TwoTeams)
			{
				flag = true;
			}
		}
		if (flag)
		{
			Dictionary<TeamNum, List<PlayerSelectionInfo>> dictionary = new Dictionary<TeamNum, List<PlayerSelectionInfo>>
			{
				{
					TeamNum.Team1,
					new List<PlayerSelectionInfo>()
				},
				{
					TeamNum.Team2,
					new List<PlayerSelectionInfo>()
				}
			};
			IEnumerator enumerator = ((IEnumerable)players).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
					dictionary[playerSelectionInfo.team].Add(playerSelectionInfo);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			TeamNum[] values = EnumUtil.GetValues<TeamNum>();
			foreach (TeamNum key in values)
			{
				if (dictionary.ContainsKey(key))
				{
					foreach (PlayerSelectionInfo playerSelectionInfo2 in dictionary[key])
					{
						if (playerSelectionInfo2.type != PlayerType.None && !playerSelectionInfo2.isSpectator)
						{
							GameObject gameObject = base.createNewElement();
							if (gameObject != null)
							{
								PlayerGUI component = gameObject.GetComponent<PlayerGUI>();
								component.Initialize(battleConfig, playerSelectionInfo2);
								this.guiList.Add(component);
							}
						}
					}
				}
			}
		}
		else
		{
			IEnumerator enumerator3 = ((IEnumerable)players).GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					object obj2 = enumerator3.Current;
					PlayerSelectionInfo playerSelectionInfo3 = (PlayerSelectionInfo)obj2;
					if (playerSelectionInfo3.type != PlayerType.None && !playerSelectionInfo3.isSpectator)
					{
						GameObject gameObject2 = base.createNewElement();
						if (gameObject2 != null)
						{
							PlayerGUI component2 = gameObject2.GetComponent<PlayerGUI>();
							component2.Initialize(battleConfig, playerSelectionInfo3);
							this.guiList.Add(component2);
						}
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator3 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}
		this.theSignal = base.signalBus.GetSignal<GUIClearSignal>();
		this.theSignal.AddListener(new Action(this.onGuiClearSignal));
	}

	// Token: 0x0600390E RID: 14606 RVA: 0x0010BF88 File Offset: 0x0010A388
	private void onGuiClearSignal()
	{
		this.rectList.Clear();
		foreach (PlayerController playerController in base.gameController.currentGame.CharacterControllers)
		{
			Rect screenspaceClearRect = playerController.GetScreenspaceClearRect();
			this.rectList.Add(screenspaceClearRect);
		}
		foreach (PlayerGUI playerGUI in this.guiList)
		{
			playerGUI.ClearScreenSpace(this.rectList);
		}
	}

	// Token: 0x0600390F RID: 14607 RVA: 0x0010C058 File Offset: 0x0010A458
	private void OnDrawGizmos()
	{
		if (base.gameController != null && base.gameController.currentGame != null)
		{
			foreach (PlayerController playerController in base.gameController.currentGame.CharacterControllers)
			{
				FixedRect boundsForScreenSpaceClear = playerController.GetBoundsForScreenSpaceClear();
				GizmoUtil.GizmosDrawRectangle(boundsForScreenSpaceClear, Color.black, false);
			}
		}
	}

	// Token: 0x06003910 RID: 14608 RVA: 0x0010C0EC File Offset: 0x0010A4EC
	public override void OnDestroy()
	{
		if (this.theSignal != null)
		{
			this.theSignal.RemoveListener(new Action(this.onGuiClearSignal));
			this.theSignal = null;
		}
		base.OnDestroy();
	}

	// Token: 0x04002756 RID: 10070
	private GUIClearSignal theSignal;

	// Token: 0x04002757 RID: 10071
	private List<PlayerGUI> guiList = new List<PlayerGUI>();

	// Token: 0x04002758 RID: 10072
	private List<Rect> rectList = new List<Rect>();
}
