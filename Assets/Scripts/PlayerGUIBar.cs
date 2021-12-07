// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGUIBar : GUIBar
{
	private GUIClearSignal theSignal;

	private List<PlayerGUI> guiList = new List<PlayerGUI>();

	private List<Rect> rectList = new List<Rect>();

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
					PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
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
			TeamNum[] array = values;
			for (int i = 0; i < array.Length; i++)
			{
				TeamNum key = array[i];
				if (dictionary.ContainsKey(key))
				{
					foreach (PlayerSelectionInfo current in dictionary[key])
					{
						if (current.type != PlayerType.None && !current.isSpectator)
						{
							GameObject gameObject = base.createNewElement();
							if (gameObject != null)
							{
								PlayerGUI component = gameObject.GetComponent<PlayerGUI>();
								component.Initialize(battleConfig, current);
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
					PlayerSelectionInfo playerSelectionInfo2 = (PlayerSelectionInfo)enumerator3.Current;
					if (playerSelectionInfo2.type != PlayerType.None && !playerSelectionInfo2.isSpectator)
					{
						GameObject gameObject2 = base.createNewElement();
						if (gameObject2 != null)
						{
							PlayerGUI component2 = gameObject2.GetComponent<PlayerGUI>();
							component2.Initialize(battleConfig, playerSelectionInfo2);
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

	private void onGuiClearSignal()
	{
		this.rectList.Clear();
		foreach (PlayerController current in base.gameController.currentGame.CharacterControllers)
		{
			Rect screenspaceClearRect = current.GetScreenspaceClearRect();
			this.rectList.Add(screenspaceClearRect);
		}
		foreach (PlayerGUI current2 in this.guiList)
		{
			current2.ClearScreenSpace(this.rectList);
		}
	}

	private void OnDrawGizmos()
	{
		if (base.gameController != null && base.gameController.currentGame != null)
		{
			foreach (PlayerController current in base.gameController.currentGame.CharacterControllers)
			{
				FixedRect boundsForScreenSpaceClear = current.GetBoundsForScreenSpaceClear();
				GizmoUtil.GizmosDrawRectangle(boundsForScreenSpaceClear, Color.black, false);
			}
		}
	}

	public override void OnDestroy()
	{
		if (this.theSignal != null)
		{
			this.theSignal.RemoveListener(new Action(this.onGuiClearSignal));
			this.theSignal = null;
		}
		base.OnDestroy();
	}
}
