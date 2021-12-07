// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class CustomMatchParamsChangedEvent : ServerEvent
	{
		public EIconStages[] stageList;

		public LobbyGameMode gameMode;

		public CustomMatchParamsChangedEvent(EIconStages[] stageList, LobbyGameMode gameMode)
		{
			this.stageList = stageList;
			this.gameMode = gameMode;
		}
	}
}
