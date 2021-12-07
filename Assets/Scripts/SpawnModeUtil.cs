// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public static class SpawnModeUtil
{
	public static SpawnModeType GetSpawnModeFromPlayerReferences(List<PlayerReference> references)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		Dictionary<TeamNum, int> dictionary = new Dictionary<TeamNum, int>();
		for (int i = 0; i < references.Count; i++)
		{
			PlayerReference playerReference = references[i];
			if (playerReference.PlayerInfo.type != PlayerType.None && !playerReference.IsSpectating)
			{
				num++;
				if (!dictionary.ContainsKey(playerReference.Team))
				{
					dictionary.Add(playerReference.Team, 1);
					num2++;
				}
				else
				{
					Dictionary<TeamNum, int> dictionary2;
					TeamNum team;
					(dictionary2 = dictionary)[team = playerReference.Team] = dictionary2[team] + 1;
				}
				if (dictionary[playerReference.Team] > num3)
				{
					num3 = dictionary[playerReference.Team];
				}
			}
		}
		SpawnModeType result = SpawnModeType.FFA_4P;
		if (num2 == num)
		{
			switch (num)
			{
			case 1:
			case 2:
				result = SpawnModeType.FFA_2P;
				break;
			case 3:
				result = SpawnModeType.FFA_3P;
				break;
			case 4:
				result = SpawnModeType.FFA_4P;
				break;
			case 5:
				result = SpawnModeType.FFA_5P;
				break;
			case 6:
				result = SpawnModeType.FFA_6P;
				break;
			}
		}
		else if (num2 == 2 && num3 == 2)
		{
			result = SpawnModeType.TeamFFA_2v2;
		}
		else if (num2 == 2 && num3 == 3)
		{
			result = SpawnModeType.TeamFFA_3v3;
		}
		return result;
	}

	public static void AssignPlayerReferencesSpawnPoints(SpawnPointModeData spawnData, List<PlayerReference> references)
	{
		if (spawnData.setCount == 1)
		{
			for (int i = 0; i < references.Count; i++)
			{
				references[i].spawnReference.setIndex = 0;
				references[i].spawnReference.spawnerIndex = i;
			}
		}
		else
		{
			Dictionary<TeamNum, int> dictionary = new Dictionary<TeamNum, int>();
			Dictionary<TeamNum, int> dictionary2 = new Dictionary<TeamNum, int>();
			int num = 0;
			foreach (PlayerReference current in references)
			{
				if (!dictionary.ContainsKey(current.Team))
				{
					dictionary[current.Team] = num;
					dictionary2[current.Team] = 1;
					num++;
				}
				else
				{
					Dictionary<TeamNum, int> dictionary3;
					TeamNum team;
					(dictionary3 = dictionary2)[team = current.Team] = dictionary3[team] + 1;
				}
				current.spawnReference.setIndex = dictionary[current.Team];
				current.spawnReference.spawnerIndex = dictionary2[current.Team] - 1;
			}
		}
	}
}
