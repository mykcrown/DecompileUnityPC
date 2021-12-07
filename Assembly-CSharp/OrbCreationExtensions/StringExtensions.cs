using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace OrbCreationExtensions
{
	// Token: 0x02000018 RID: 24
	public static class StringExtensions
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x0000877C File Offset: 0x00006B7C
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

		// Token: 0x060000C3 RID: 195 RVA: 0x000087D4 File Offset: 0x00006BD4
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

		// Token: 0x060000C4 RID: 196 RVA: 0x0000880C File Offset: 0x00006C0C
		public static string MakeString(this string aStr)
		{
			if (aStr == null)
			{
				return string.Empty;
			}
			return aStr;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000881C File Offset: 0x00006C1C
		public static int MakeInt(this string str)
		{
			int result = 0;
			if (str != null && int.TryParse(str, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00008844 File Offset: 0x00006C44
		public static float MakeFloat(this string aStr)
		{
			float result = 0f;
			if (aStr != null && float.TryParse(aStr, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00008874 File Offset: 0x00006C74
		public static double MakeDouble(this string aStr)
		{
			double result = 0.0;
			if (aStr != null && double.TryParse(aStr, out result))
			{
				return result;
			}
			return 0.0;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000088AC File Offset: 0x00006CAC
		public static bool MakeBool(this string aStr)
		{
			int num = 0;
			return aStr.ToLower() == "true" || (aStr != null && int.TryParse(aStr, out num) && num > 0);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000088EC File Offset: 0x00006CEC
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
							Debug.Log(string.Concat(new object[]
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
					Debug.Log(string.Concat(new object[]
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

		// Token: 0x060000CA RID: 202 RVA: 0x00008C28 File Offset: 0x00007028
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
					Debug.Log(string.Concat(new object[]
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

		// Token: 0x060000CB RID: 203 RVA: 0x00008D3C File Offset: 0x0000713C
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
					Debug.Log(string.Concat(new object[]
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

		// Token: 0x060000CC RID: 204 RVA: 0x00008E58 File Offset: 0x00007258
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

		// Token: 0x060000CD RID: 205 RVA: 0x00008E94 File Offset: 0x00007294
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

		// Token: 0x060000CE RID: 206 RVA: 0x00008ED8 File Offset: 0x000072D8
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

		// Token: 0x060000CF RID: 207 RVA: 0x00008F27 File Offset: 0x00007327
		public static string Truncate(this string str, int maxLength)
		{
			if (str.Length > maxLength)
			{
				return str.Substring(0, maxLength);
			}
			return str;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00008F40 File Offset: 0x00007340
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

		// Token: 0x060000D1 RID: 209 RVA: 0x00008FCC File Offset: 0x000073CC
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

		// Token: 0x060000D2 RID: 210 RVA: 0x00009004 File Offset: 0x00007404
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

		// Token: 0x060000D3 RID: 211 RVA: 0x000090B0 File Offset: 0x000074B0
		public static float[] ToFloatArray(this string str, char separator)
		{
			List<float> list = new List<float>();
			str.IntoFloatList(ref list, separator);
			return list.ToArray();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000090D4 File Offset: 0x000074D4
		public static List<float> ToFloatList(this string str, char separator)
		{
			List<float> result = new List<float>();
			str.IntoFloatList(ref result, separator);
			return result;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000090F4 File Offset: 0x000074F4
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

		// Token: 0x060000D6 RID: 214 RVA: 0x000091A8 File Offset: 0x000075A8
		public static List<Vector2> ToVector2List(this string str, char separator)
		{
			List<Vector2> result = new List<Vector2>();
			str.IntoVector2List(ref result, separator, 2);
			return result;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000091C8 File Offset: 0x000075C8
		public static List<Vector2> ToVector2List(this string str, char separator, int floatsPerValue)
		{
			List<Vector2> result = new List<Vector2>();
			str.IntoVector2List(ref result, separator, floatsPerValue);
			return result;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000091E8 File Offset: 0x000075E8
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

		// Token: 0x060000D9 RID: 217 RVA: 0x000092F0 File Offset: 0x000076F0
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

		// Token: 0x060000DA RID: 218 RVA: 0x00009324 File Offset: 0x00007724
		public static List<Vector3> ToVector3List(this string str, char separator)
		{
			List<Vector3> result = new List<Vector3>();
			str.IntoVector3List(ref result, separator);
			return result;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00009344 File Offset: 0x00007744
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

		// Token: 0x060000DC RID: 220 RVA: 0x0000944C File Offset: 0x0000784C
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

		// Token: 0x060000DD RID: 221 RVA: 0x000094C0 File Offset: 0x000078C0
		public static string XmlDecode(this string str)
		{
			str = str.Replace("&lt;", "<");
			str = str.Replace("&gt;", ">");
			str = str.Replace("&amp;", "&");
			str = str.Replace("&apos;", "'");
			str = str.Replace("&quot;", "\"");
			return str;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00009528 File Offset: 0x00007928
		public static string XmlEncode(this string str)
		{
			str = str.Replace("&", "&amp;");
			str = str.Replace("<", "&lt;");
			str = str.Replace(">", "&gt;");
			str = str.Replace("'", "&apos;");
			str = str.Replace("\"", "&quot;");
			return str;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00009590 File Offset: 0x00007990
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

		// Token: 0x060000E0 RID: 224 RVA: 0x0000960C File Offset: 0x00007A0C
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
