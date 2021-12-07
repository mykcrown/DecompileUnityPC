using System;

// Token: 0x020008D5 RID: 2261
public class CharacterSelectCalculator
{
	// Token: 0x17000DBD RID: 3517
	// (get) Token: 0x06003916 RID: 14614 RVA: 0x0010C12D File Offset: 0x0010A52D
	// (set) Token: 0x06003917 RID: 14615 RVA: 0x0010C135 File Offset: 0x0010A535
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000DBE RID: 3518
	// (get) Token: 0x06003918 RID: 14616 RVA: 0x0010C13E File Offset: 0x0010A53E
	// (set) Token: 0x06003919 RID: 14617 RVA: 0x0010C146 File Offset: 0x0010A546
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x0600391A RID: 14618 RVA: 0x0010C150 File Offset: 0x0010A550
	public int GetMaxPlayers()
	{
		if (this.enterNewGame.GamePayload == null)
		{
			return 999999;
		}
		GameModeData dataByType = this.gameDataManager.GameModeData.GetDataByType(this.enterNewGame.GamePayload.battleConfig.mode);
		return dataByType.settings.maxPlayerCount;
	}
}
