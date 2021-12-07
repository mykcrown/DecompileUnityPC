using System;

namespace IconsServer
{
	// Token: 0x020007E2 RID: 2018
	public class CustomMatchParamsChangedEvent : ServerEvent
	{
		// Token: 0x060031FC RID: 12796 RVA: 0x000F2576 File Offset: 0x000F0976
		public CustomMatchParamsChangedEvent(EIconStages[] stageList, LobbyGameMode gameMode)
		{
			this.stageList = stageList;
			this.gameMode = gameMode;
		}

		// Token: 0x04002320 RID: 8992
		public EIconStages[] stageList;

		// Token: 0x04002321 RID: 8993
		public LobbyGameMode gameMode;
	}
}
