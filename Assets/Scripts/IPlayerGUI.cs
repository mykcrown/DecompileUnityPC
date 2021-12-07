// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPlayerGUI : IGUIBarElement, ITickable
{
	void Initialize(BattleSettings config, PlayerSelectionInfo playerInfo);
}
