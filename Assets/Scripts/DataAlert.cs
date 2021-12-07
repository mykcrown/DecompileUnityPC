// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class DataAlert
{
	private static DataAlert instance = new DataAlert();

	private void fatal(string text)
	{
		string message = "FATAL DATA CONFIGURATION: You cannot run the game until you fix the following error ---- \n" + text;
		UnityEngine.Debug.LogError(message);
	}

	public static void Fatal(string text)
	{
		DataAlert.instance.fatal(text);
	}

	private void warning(string text)
	{
		string message = "WARNING: INVALID DATA CONFIGURATION ---- \n" + text;
		UnityEngine.Debug.LogWarning(message);
	}

	public static void Warning(string text)
	{
		DataAlert.instance.warning(text);
	}
}
