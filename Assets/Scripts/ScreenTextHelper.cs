// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public static class ScreenTextHelper
{
	public static string GetTitleText(VictoryScreenPayload victoryPayload, ILocalization localization, GameModeData modeData)
	{
		string text;
		if (victoryPayload.victors.Count == 0)
		{
			if (victoryPayload.wasForfeited && victoryPayload.gamePayload.isOnlineGame)
			{
				text = localization.GetText("victory.title.forfeit");
			}
			else
			{
				text = localization.GetText("victory.title.noVictor");
			}
		}
		else
		{
			List<string> list = new List<string>();
			if (ScreenTextHelper.IsTeamVictory(victoryPayload, modeData))
			{
				foreach (TeamNum current in victoryPayload.winningTeams)
				{
					string text2 = localization.GetText("team." + PlayerUtil.GetUIColorFromTeam(current).ToString());
					list.Add(text2.ToUpper());
				}
			}
			else
			{
				foreach (PlayerNum current2 in victoryPayload.victors)
				{
					string playerNametag = PlayerUtil.GetPlayerNametag(localization, victoryPayload.gamePayload.players.GetPlayer(current2), true);
					list.Add(playerNametag.ToUpper());
				}
			}
			text = string.Join(localization.GetText("victory.title.delimiter"), list.ToArray());
			if (list.Count > 1)
			{
				text += localization.GetText("victory.title.win.plural");
			}
			else
			{
				text += localization.GetText("victory.title.win.singular");
			}
		}
		return text;
	}

	public static bool IsTeamVictory(VictoryScreenPayload victoryPayload, GameModeData modeData)
	{
		return modeData.settings.usesTeams && victoryPayload.winningTeams.Count > 0;
	}
}
