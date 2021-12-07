// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class DebugConfig : ScriptableObject
{
	public bool displayUnitsSeconds;

	public PlayerNum debugText2Player = PlayerNum.Player2;

	public Color debugTextColor = Color.white;

	public bool beginDebugPaused;

	public TestGameSettings testGameSettings = new TestGameSettings();

	public OfflineModeSettings offlineModeSettings = new OfflineModeSettings();

	public TestPlayerProgressionSettings testPlayerProgressionSettings = new TestPlayerProgressionSettings();

	private const string settingsPath = "Assets/Wavedash/Config/DebugConfig.asset";

	public static DebugConfig LoadConfig()
	{
		return (DebugConfig)ScriptableObject.CreateInstance("DebugConfig");
	}
}
