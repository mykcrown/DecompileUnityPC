// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class VictoryScreenPayload : Payload
{
	public List<PlayerStats> stats = new List<PlayerStats>(8);

	public List<int> endGameCharacterIndicies = new List<int>();

	public bool wasForfeited;

	public bool wasExited;

	public List<PlayerNum> victors = new List<PlayerNum>();

	public List<TeamNum> winningTeams = new List<TeamNum>();

	public GameLoadPayload gamePayload;

	public ScreenType nextScreen;
}
