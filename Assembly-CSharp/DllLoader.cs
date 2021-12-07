using System;
using System.IO;
using UnityEngine;

// Token: 0x0200089A RID: 2202
public class DllLoader
{
	// Token: 0x06003747 RID: 14151 RVA: 0x00101C88 File Offset: 0x00100088
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
