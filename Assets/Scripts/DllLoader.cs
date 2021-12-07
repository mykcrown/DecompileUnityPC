// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

public class DllLoader
{
	public static void LoadNativeDLLs()
	{
		string environmentVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
		string text = Application.dataPath;
		text = text + Path.DirectorySeparatorChar + "Plugins";
		if (environmentVariable != null && !environmentVariable.Contains(text))
		{
			Environment.SetEnvironmentVariable("PATH", environmentVariable + Path.PathSeparator + text, EnvironmentVariableTarget.Process);
		}
	}
}
