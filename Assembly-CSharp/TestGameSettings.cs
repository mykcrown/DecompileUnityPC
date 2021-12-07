using System;
using System.Collections.Generic;

// Token: 0x02000403 RID: 1027
[Serializable]
public class TestGameSettings
{
	// Token: 0x04001040 RID: 4160
	public bool enableAutoStartTestGame;

	// Token: 0x04001041 RID: 4161
	public StageID stage = StageID.WavedashArena;

	// Token: 0x04001042 RID: 4162
	public GameMode gameMode = GameMode.FreeForAll;

	// Token: 0x04001043 RID: 4163
	public int lives = 3;

	// Token: 0x04001044 RID: 4164
	public int durationSeconds = 480;

	// Token: 0x04001045 RID: 4165
	public int assists = 3;

	// Token: 0x04001046 RID: 4166
	public List<TestGamePlayerSettings> playerSettings = new List<TestGamePlayerSettings>();
}
