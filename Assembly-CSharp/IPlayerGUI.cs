using System;

// Token: 0x020008CF RID: 2255
public interface IPlayerGUI : IGUIBarElement, ITickable
{
	// Token: 0x06003909 RID: 14601
	void Initialize(BattleSettings config, PlayerSelectionInfo playerInfo);
}
