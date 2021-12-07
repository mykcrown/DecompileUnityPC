using System;
using System.Collections.Generic;

// Token: 0x020009CA RID: 2506
public static class ScreenTextHelper
{
	// Token: 0x0600463A RID: 17978 RVA: 0x00132778 File Offset: 0x00130B78
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
				foreach (TeamNum team in victoryPayload.winningTeams)
				{
					string text2 = localization.GetText("team." + PlayerUtil.GetUIColorFromTeam(team).ToString());
					list.Add(text2.ToUpper());
				}
			}
			else
			{
				foreach (PlayerNum playerNum in victoryPayload.victors)
				{
					string playerNametag = PlayerUtil.GetPlayerNametag(localization, victoryPayload.gamePayload.players.GetPlayer(playerNum), true);
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

	// Token: 0x0600463B RID: 17979 RVA: 0x00132920 File Offset: 0x00130D20
	public static bool IsTeamVictory(VictoryScreenPayload victoryPayload, GameModeData modeData)
	{
		return modeData.settings.usesTeams && victoryPayload.winningTeams.Count > 0;
	}
}
