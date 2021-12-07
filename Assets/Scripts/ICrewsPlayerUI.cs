// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface ICrewsPlayerUI
{
	Transform Transform
	{
		get;
	}

	void Initialize(BattleSettings config, PlayerSelectionInfo playerInfo, TeamNum team, CrewsGUISide side);

	void TickFrame();
}
