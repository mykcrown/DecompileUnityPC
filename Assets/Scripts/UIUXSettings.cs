// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class UIUXSettings
{
	public bool emotiveStartup;

	public int inputFramesBuffer = 10;

	public int countdownAmt = 3;

	public Fixed countdownIntervalSeconds = (Fixed)0.800000011920929;

	public Fixed characterSelectIdleSeconds = (Fixed)5.0;

	public int endGameDelayFrames = 80;

	public bool skipVictoryPoseScreen;

	public CrewBattleGuiType crewsGuiType;

	public bool floatyNameHiding;

	public bool alwaysShowCustomNames = true;

	public int floatyNamesAtMatchBegin = 60;

	public Fixed floatyNameOnTumble = 30;

	public int floatyNameMinShow = 60;

	public int floatyNamesIdleShow = 60;

	public bool floatyNameShowRespawning = true;

	public float offscreenUIOffset = 25f;

	public float offscreenDetectionPadding = 25f;

	public float floatyNameLerpSpeed = 0.1f;

	public int floatyNameMaxSize = 10;

	public int MatchmakingPromptTime = 30;

	public string displayVersion = "0.03.0";

	public bool DemoModeEnabled
	{
		get
		{
			return DemoSettings.DemoModeEnabled;
		}
	}
}
