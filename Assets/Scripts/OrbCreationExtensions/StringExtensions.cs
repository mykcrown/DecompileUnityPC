// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace OrbCreationExtensions
{
	public static class StringExtensions
	{
		public static string MakeString(this string[] aPath)
		{
			string text = string.Empty;
			if (aPath != null)
			{
				if (aPath.Length > 0)
				{
					text = aPath[0];
				}
				for (int i = 1; i < aPath.Length; i++)
				{
					if (aPath.Length <= 0)
					{
						return text;
					}
					text = text + "/" + aPath[i];
				}
			}
			return text;
		}

		public static string[] TruncatePath(this string[] aPath, int newlen)
		{
			string[] array = new string[newlen];
			int num = 0;
			while (num < aPath.Length && num < newlen)
			{
				array[num] = aPath[num];
				num++;
			}
			return array;
		}

		public static string MakeString(this string aStr)
		{
			if (aStr == null)
			{
				return string.Empty;
			}
			return aStr;
		}

		public static int MakeInt(this string str)
		{
			int result = 0;
			if (str != null && int.TryParse(str, out result))
			{
				return result;
			}
			return 0;
		}

		public static float MakeFloat(this string aStr)
		{
			float result = 0f;
			if (aStr != null && float.TryParse(aStr, out result))
			{
				return result;
			}
			return 0f;
		}

		public static double MakeDouble(this string aStr)
		{
			double result = 0.0;
			if (aStr != null && double.TryParse(aStr, out result))
			{
				return result;
			}
			return 0.0;
		}

		public static bool MakeBool(this string aStr)
		{
			int num = 0;
			return aStr.ToLower() == "true" || (aStr != null && int.TryParse(aStr, out num) && num > 0);
		}

		public static Color MakeColor(this string aStr)
		{
			Color result = new Color(0f, 0f, 0f, 0f);
			if (aStr != null && aStr.Length > 0)
			{
				try
				{
					if (aStr.Substring(0, 1) == "#")
					{
						string text = aStr.Substring(1, aStr.Length - 1);
						try
						{
							result.r = (float)int.Parse(text.Substring(0, 2), NumberStyles.AllowHexSpecifier) / 255f;
							result.g = (float)int.Parse(text.Substring(2, 2), NumberStyles.AllowHexSpecifier) / 255f;
							result.b = (float)int.Parse(text.Substring(4, 2), NumberStyles.AllowHexSpecifier) / 255f;
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.Log(string.Concat(new object[]
							{
								"Could not convert ",
								aStr,
								" to Color. ",
								ex
							}));
						}
						if (text.Length == 8)
						{
							result.a = (float)int.Parse(text.Substring(6, 2), NumberStyles.AllowHexSpecifier) / 255f;
						}
						else
						{
							result.a = 1f;
						}
					}
					else if (aStr.IndexOf(",", 0) >= 0)
					{
						int num = 0;
						int num2 = 0;
						float num3 = 1f;
						if (aStr.IndexOf(".", 0) < 0)
						{
							num3 = 255f;
						}
						int num4 = aStr.IndexOf(",", num);
						while (num4 > num && num2 < 4)
						{
							result[num2++] = Mathf.Clamp01(aStr.Substring(num, num4 - num).MakeFloat() / num3);
							num = num4 + 1;
							if (num < aStr.Length)
							{
								num4 = aStr.IndexOf(",", num);
							}
							if (num4 < 0)
							{
								num4 = aStr.Length;
							}
						}
						if (num2 < 4)
						{
							result.a = 1f;
						}
					}
					else if (aStr.IndexOf(" ", 0) >= 0)
					{
						int num5 = 0;
						int num6 = 0;
						float num7 = 1f;
						if (aStr.IndexOf(".", 0) < 0)
						{
							num7 = 255f;
						}
						int num8 = aStr.IndexOf(" ", num5);
						while (num8 > num5 && num6 < 4)
						{
							result[num6++] = Mathf.Clamp01(aStr.Substring(num5, num8 - num5).MakeFloat() / num7);
							num5 = num8 + 1;
							if (num5 < aStr.Length)
							{
								num8 = aStr.IndexOf(" ", num5);
							}
							if (num8 < 0)
							{
								num8 = aStr.Length;
							}
						}
						if (num6 < 4)
						{
							result.a = 1f;
						}
					}
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Could not convert ",
						aStr,
						" to Color. ",
						ex2
					}));
				}
			}
			return result;
		}

		public static Vector3 MakeVector3(this string aStr)
		{
			Vector3 result = new Vector3(0f, 0f, 0f);
			if (aStr != null && aStr.Length > 0)
			{
				try
				{
					if (aStr.IndexOf(",", 0) >= 0)
					{
						int num = 0;
						int num2 = 0;
						int num3 = aStr.IndexOf(",", num);
						while (num3 > num && num2 <= 3)
						{
							result[num2++] = float.Parse(aStr.Substring(num, num3 - num));
							num = num3 + 1;
							if (num < aStr.Length)
							{
								num3 = aStr.IndexOf(",", num);
							}
							if (num3 < 0)
							{
								num3 = aStr.Length;
							}
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Could not convert ",
						aStr,
						" to Vector3. ",
						ex
					}));
					return new Vector3(0f, 0f, 0f);
				}
				return result;
			}
			return result;
		}

		public static Vector4 MakeVector4(this string aStr)
		{
			Vector4 result = new Vector4(0f, 0f, 0f, 0f);
			if (aStr != null && aStr.Length > 0)
			{
				try
				{
					if (aStr.IndexOf(",", 0) >= 0)
					{
						int num = 0;
						int num2 = 0;
						int num3 = aStr.IndexOf(",", num);
						while (num3 > num && num2 <= 4)
						{
							result[num2++] = float.Parse(aStr.Substring(num, num3 - num));
							num = num3 + 1;
							if (num < aStr.Length)
							{
								num3 = aStr.IndexOf(",", num);
							}
							if (num3 < 0)
							{
								num3 = aStr.Length;
							}
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Could not convert ",
						aStr,
						" to Vector3. ",
						ex
					}));
					return new Vector4(0f, 0f, 0f, 0f);
				}
				return result;
			}
			return result;
		}

		public static int IndexOfChars(this string str, string searchChars, int startAt)
		{
			for (int i = startAt; i < str.Length; i++)
			{
				char value = str[i];
				if (searchChars.IndexOf(value) >= 0)
				{
					return i;
				}
			}
			return -1;
		}

		public static int IndexOfEndOfLine(this string str, int startAt)
		{
			int num = str.IndexOf('\n', startAt);
			if (num < startAt)
			{
				return str.IndexOf('\r', startAt);
			}
			if (num > 0 && str[num - 1] == '\r')
			{
				return num - 1;
			}
			return num;
		}

		public static int EndOfCharRepetition(this string str, int startAt)
		{
			if (startAt < str.Length)
			{
				int i = startAt;
				char c = str[i];
				while (i < str.Length - 1)
				{
					i++;
					if (str[i] != c)
					{
						return i;
					}
				}
			}
			return str.Length;
		}

		public static string Truncate(this string str, int maxLength)
		{
			if (str.Length > maxLength)
			{
				return str.Substring(0, maxLength);
			}
			return str;
		}

		public static string TrimChars(this string str, string trimChars)
		{
			int i = 0;
			int j = str.Length;
			while (i < j)
			{
				char value = str[i];
				if (trimChars.IndexOf(value) < 0)
				{
					break;
				}
				i++;
			}
			while (j > i)
			{
				char value2 = str[j];
				if (trimChars.IndexOf(value2) < 0)
				{
					break;
				}
				j--;
			}
			if (i > 0 || j < str.Length)
			{
				return str.Substring(i, j - i);
			}
			return str;
		}

		public static string SubstringAfter(this string str, string after)
		{
			int num = str.IndexOf(after);
			if (num >= 0)
			{
				num += after.Length;
				return str.Substring(num, str.Length - num);
			}
			return str;
		}

		public static List<int> ToIntList(this string str, char separator)
		{
			List<int> list = new List<int>();
			int num = 0;
			bool flag = false;
			str += separator;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == separator || i == str.Length - 1)
				{
					if (num < i && flag)
					{
						int item = 0;
						if (int.TryParse(str.Substring(num, i - num), out item))
						{
							list.Add(item);
						}
						flag = false;
					}
					num = i;
				}
				if (c > '/' && c < ':')
				{
					flag = true;
				}
				if (!flag)
				{
					num = i;
				}
			}
			return list;
		}

		public static float[] ToFloatArray(this string str, char separator)
		{
			List<float> list = new List<float>();
			str.IntoFloatList(ref list, separator);
			return list.ToArray();
		}

		public static List<float> ToFloatList(this string str, char separator)
		{
			List<float> result = new List<float>();
			str.IntoFloatList(ref result, separator);
			return result;
		}

		public static void IntoFloatList(this string str, ref List<float> floats, char separator)
		{
			int num = 0;
			bool flag = false;
			str += separator;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == separator || c == '\n' || c == '\r' || i == str.Length - 1)
				{
					if (num < i && flag)
					{
						float item = 0f;
						if (float.TryParse(str.Substring(num, i - num), out item))
						{
							floats.Add(item);
						}
						flag = false;
					}
					num = i;
				}
				if (c > '/' && c < ':')
				{
					flag = true;
				}
				if (!flag)
				{
					num = i;
				}
			}
		}

		public static List<Vector2> ToVector2List(this string str, char separator)
		{
			List<Vector2> result = new List<Vector2>();
			str.IntoVector2List(ref result, separator, 2);
			return result;
		}

		public static List<Vector2> ToVector2List(this string str, char separator, int floatsPerValue)
		{
			List<Vector2> result = new List<Vector2>();
			str.IntoVector2List(ref result, separator, floatsPerValue);
			return result;
		}

		public static void IntoVector2List(this string str, ref List<Vector2> vectors, char separator, int floatsPerValue)
		{
			int num = 0;
			Vector2 item = new Vector2(0f, 0f);
			int num2 = 0;
			bool flag = false;
			str += separator;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == separator || c == '\n' || c == '\r' || i == str.Length - 1)
				{
					if (num2 < i && flag)
					{
						float value = 0f;
						if (float.TryParse(str.Substring(num2, i - num2), out value))
						{
							if (num < 2)
							{
								item[num] = value;
							}
							num++;
							if (num == floatsPerValue)
							{
								num = 0;
								vectors.Add(item);
								item = new Vector2(0f, 0f);
							}
						}
						flag = false;
					}
					num2 = i;
				}
				if (c > '/' && c < ':')
				{
					flag = true;
				}
				if (!flag)
				{
					num2 = i;
				}
			}
		}

		public static Vector3 ToVector3(this string str, char separator, Vector3 defaultValue)
		{
			List<Vector3> list = new List<Vector3>();
			str.IntoVector3List(ref list, separator);
			if (list.Count > 0)
			{
				return list[0];
			}
			return defaultValue;
		}

		public static List<Vector3> ToVector3List(this string str, char separator)
		{
			List<Vector3> result = new List<Vector3>();
			str.IntoVector3List(ref result, separator);
			return result;
		}

		public static void IntoVector3List(this string str, ref List<Vector3> vectors, char separator)
		{
			int num = 0;
			Vector3 item = new Vector3(0f, 0f, 0f);
			int num2 = 0;
			bool flag = false;
			str += separator;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == separator || c == '\n' || c == '\r' || i == str.Length - 1)
				{
					if (num2 < i && flag)
					{
						float value = 0f;
						if (float.TryParse(str.Substring(num2, i - num2), out value))
						{
							item[num++] = value;
							if (num == 3)
							{
								num = 0;
								vectors.Add(item);
								item = new Vector3(0f, 0f, 0f);
							}
						}
						flag = false;
					}
					num2 = i;
				}
				if (c > '/' && c < ':')
				{
					flag = true;
				}
				if (!flag)
				{
					num2 = i;
				}
			}
		}

		public static string FilterComments(this string str, string beginComment, string endComment, string replaceWith)
		{
			int num = 0;
			for (int i = str.IndexOf(beginComment, num); i > 0; i = str.IndexOf(beginComment, i))
			{
				num = str.IndexOf(endComment, i);
				if (num <= i)
				{
					break;
				}
				num += endComment.Length;
				str = str.Substring(0, i) + replaceWith + str.Substring(num, str.Length - num);
			}
			return str;
		}

		public static string XmlDecode(this string str)
		{
			str = str.Replace("&lt;", "<");
			str = str.Replace("&gt;", ">");
			str = str.Replace("&amp;", "&");
			str = str.Replace("&apos;", "'");
			str = str.Replace("&quot;", "\"");
			return str;
		}

		public static string XmlEncode(this string str)
		{
			str = str.Replace("&", "&amp;");
			str = str.Replace("<", "&lt;");
			str = str.Replace(">", "&gt;");
			str = str.Replace("'", "&apos;");
			str = str.Replace("\"", "&quot;");
			return str;
		}

		public static string JsonDecode(this string str)
		{
			str = str.Replace("\\/", "/");
			str = str.Replace("\\n", "\n");
			str = str.Replace("\\r", "\r");
			str = str.Replace("\\t", "\t");
			str = str.Replace("\\\"", "\"");
			str = str.Replace("\\\\", "\\");
			return str;
		}

		public static string JsonEncode(this string str)
		{
			str = str.Replace("\"", "\\\"");
			str = str.Replace("\\", "\\\\");
			str = str.Replace("\n", "\\n");
			str = str.Replace("\r", "\\r");
			str = str.Replace("\t", "\\t");
			return str;
		}
	}
}
