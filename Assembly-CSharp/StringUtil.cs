using System;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000B62 RID: 2914
public static class StringUtil
{
	// Token: 0x06005458 RID: 21592 RVA: 0x001B1C50 File Offset: 0x001B0050
	public static void Init()
	{
		StringUtil.intToString = new string[1000];
		for (int i = 0; i < StringUtil.intToString.Length; i++)
		{
			StringUtil.intToString[i] = i.ToString();
		}
		StringUtil.intToStringLen = StringUtil.intToString.Length;
	}

	// Token: 0x06005459 RID: 21593 RVA: 0x001B1CA4 File Offset: 0x001B00A4
	public static string FormatVector(Vector3 vector, int decimals = 1, float multiplier = 1f, string units = "m/s")
	{
		string str = string.Empty;
		string format = "N" + decimals;
		str += "(";
		str += (vector.x * multiplier).ToString(format);
		str += ", ";
		str += (vector.y * multiplier).ToString(format);
		str += ", ";
		str += (vector.z * multiplier).ToString(format);
		str += ")";
		return str + " " + units;
	}

	// Token: 0x0600545A RID: 21594 RVA: 0x001B1D52 File Offset: 0x001B0152
	public static string TrueOrFalse(bool value)
	{
		return (!value) ? "false" : "true";
	}

	// Token: 0x0600545B RID: 21595 RVA: 0x001B1D69 File Offset: 0x001B0169
	public static string OnOrOff(bool value)
	{
		return (!value) ? "off" : "on";
	}

	// Token: 0x0600545C RID: 21596 RVA: 0x001B1D80 File Offset: 0x001B0180
	public static bool EqualsIgnoreCase(this string str, string other)
	{
		return str.Equals(other, StringComparison.InvariantCultureIgnoreCase);
	}

	// Token: 0x0600545D RID: 21597 RVA: 0x001B1D8C File Offset: 0x001B018C
	public static string ReplaceIgnoreCase(this string input, string search, string replacement)
	{
		return Regex.Replace(input, Regex.Escape(search), replacement.Replace("$", "$$"), RegexOptions.IgnoreCase);
	}

	// Token: 0x04003567 RID: 13671
	public static string[] intToString;

	// Token: 0x04003568 RID: 13672
	public static int intToStringLen;
}
