using System;
using UnityEngine;

// Token: 0x020002EB RID: 747
public class DataAlert
{
	// Token: 0x06000F7C RID: 3964 RVA: 0x0005DB88 File Offset: 0x0005BF88
	private void fatal(string text)
	{
		string message = "FATAL DATA CONFIGURATION: You cannot run the game until you fix the following error ---- \n" + text;
		Debug.LogError(message);
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x0005DBA7 File Offset: 0x0005BFA7
	public static void Fatal(string text)
	{
		DataAlert.instance.fatal(text);
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x0005DBB4 File Offset: 0x0005BFB4
	private void warning(string text)
	{
		string message = "WARNING: INVALID DATA CONFIGURATION ---- \n" + text;
		Debug.LogWarning(message);
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x0005DBD3 File Offset: 0x0005BFD3
	public static void Warning(string text)
	{
		DataAlert.instance.warning(text);
	}

	// Token: 0x04000A2F RID: 2607
	private static DataAlert instance = new DataAlert();
}
