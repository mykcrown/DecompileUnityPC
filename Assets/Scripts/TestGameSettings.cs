// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class TestGameSettings
{
	public bool enableAutoStartTestGame;

	public StageID stage = StageID.WavedashArena;

	public GameMode gameMode = GameMode.FreeForAll;

	public int lives = 3;

	public int durationSeconds = 480;

	public int assists = 3;

	public List<TestGamePlayerSettings> playerSettings = new List<TestGamePlayerSettings>();
}
