// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface ICrewsGUI
{
	Transform Transform
	{
		get;
	}

	void Initialize(BattleSettings config, PlayerSelectionList players);

	void TickFrame();
}
