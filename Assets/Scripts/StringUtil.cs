// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringUtil
{
	public static string[] intToString;

	public static int intToStringLen;

	public static void Init()
	{
		StringUtil.intToString = new string[1000];
		for (int i = 0; i < StringUtil.intToString.Length; i++)
		{
			StringUtil.intToString[i] = i.ToString();
		}
		StringUtil.intToStringLen = StringUtil.intToString.Length;
	}

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

	public static string TrueOrFalse(bool value)
	{
		return (!value) ? "false" : "true";
	}

	public static string OnOrOff(bool value)
	{
		return (!value) ? "off" : "on";
	}

	public static bool EqualsIgnoreCase(this string str, string other)
	{
		return str.Equals(other, StringComparison.InvariantCultureIgnoreCase);
	}

	public static string ReplaceIgnoreCase(this string input, string search, string replacement)
	{
		return Regex.Replace(input, Regex.Escape(search), replacement.Replace("$", "$$"), RegexOptions.IgnoreCase);
	}
}
