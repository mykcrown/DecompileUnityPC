using System;
using System.Collections;
using OrbCreationExtensions;

// Token: 0x02000019 RID: 25
public class SimpleJsonImporter
{
	// Token: 0x060000E2 RID: 226 RVA: 0x0000967C File Offset: 0x00007A7C
	public static Hashtable Import(string json)
	{
		return SimpleJsonImporter.Import(json, false);
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00009688 File Offset: 0x00007A88
	public static Hashtable Import(string json, bool caseInsensitive)
	{
		int length = json.Length;
		int num = 0;
		SimpleJsonImporter.MoveToNextNode(json, ref num, length);
		if (num < length)
		{
			int end = SimpleJsonImporter.FindMatchingEnd(json, num, length);
			if (json[num] == '{')
			{
				return SimpleJsonImporter.ReadHashtable(json, ref num, end, caseInsensitive);
			}
			if (json[num] == '[')
			{
				ArrayList arrayList = SimpleJsonImporter.ReadArrayList(json, ref num, end, caseInsensitive);
				if (arrayList != null && arrayList.Count > 0)
				{
					return new Hashtable
					{
						{
							"SimpleJSON",
							arrayList
						}
					};
				}
			}
		}
		return null;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00009714 File Offset: 0x00007B14
	private static Hashtable ReadHashtable(string json, ref int begin, int end, bool caseInsensitive)
	{
		Hashtable hashtable = new Hashtable();
		int num = 1;
		bool flag = false;
		string text = "\r\n\t ?\"'\\,:{}[]";
		string text2 = string.Empty;
		string text3 = string.Empty;
		int i;
		for (i = begin + 1; i < end; i++)
		{
			bool flag2 = false;
			char c = json[i];
			if (i == 0 || json[i - 1] != '\\')
			{
				if (c == '"')
				{
					flag = !flag;
				}
				if (!flag)
				{
					if (num != 1 && c == ',')
					{
						text3 = SimpleJsonImporter.TrimPropertyValue(text3);
						if (text2.Length > 0 && !hashtable.ContainsKey(text2) && text3.Length > 0)
						{
							hashtable[text2] = text3.JsonDecode();
						}
						num = 1;
						text2 = string.Empty;
						text3 = string.Empty;
						flag2 = true;
					}
					if (num == 1 && c == ':')
					{
						num = 2;
						text3 = string.Empty;
						flag2 = true;
					}
					if (num == 2 && c == '{')
					{
						int end2 = SimpleJsonImporter.FindMatchingEnd(json, i, end);
						hashtable[text2] = SimpleJsonImporter.ReadHashtable(json, ref i, end2, caseInsensitive);
						text3 = string.Empty;
						num = 0;
						flag2 = true;
					}
					if (num == 2 && c == '[')
					{
						int end3 = SimpleJsonImporter.FindMatchingEnd(json, i, end);
						hashtable[text2] = SimpleJsonImporter.ReadArrayList(json, ref i, end3, caseInsensitive);
						text3 = string.Empty;
						num = 0;
						flag2 = true;
					}
				}
			}
			if (!flag2)
			{
				if (num == 1 && text.IndexOf(c) < 0)
				{
					text2 += c;
				}
				if (num == 2)
				{
					text3 += c;
				}
			}
		}
		if (text2.Length > 0 && !hashtable.ContainsKey(text2))
		{
			text3 = SimpleJsonImporter.TrimPropertyValue(text3);
			if (text3.Length > 0)
			{
				hashtable[text2] = text3.JsonDecode();
			}
		}
		begin = i;
		return hashtable;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00009904 File Offset: 0x00007D04
	private static ArrayList ReadArrayList(string json, ref int begin, int end, bool caseInsensitive)
	{
		ArrayList arrayList = new ArrayList();
		bool flag = false;
		string text = string.Empty;
		int i;
		for (i = begin + 1; i < end; i++)
		{
			bool flag2 = false;
			char c = json[i];
			if (i == 0 || json[i - 1] != '\\')
			{
				if (c == '"')
				{
					flag = !flag;
				}
				if (!flag)
				{
					if (c == '{')
					{
						int end2 = SimpleJsonImporter.FindMatchingEnd(json, i, end);
						arrayList.Add(SimpleJsonImporter.ReadHashtable(json, ref i, end2, caseInsensitive));
						text = string.Empty;
						flag2 = true;
					}
					else if (c == '[')
					{
						int end3 = SimpleJsonImporter.FindMatchingEnd(json, i, end);
						arrayList.Add(SimpleJsonImporter.ReadArrayList(json, ref i, end3, caseInsensitive));
						text = string.Empty;
						flag2 = true;
					}
					else if (c == ',')
					{
						text = SimpleJsonImporter.TrimPropertyValue(text);
						if (text.Length > 0)
						{
							arrayList.Add(text.JsonDecode());
						}
						text = string.Empty;
						flag2 = true;
					}
				}
			}
			if (!flag2)
			{
				text += c;
			}
		}
		text = SimpleJsonImporter.TrimPropertyValue(text);
		if (text.Length > 0)
		{
			arrayList.Add(text.JsonDecode());
		}
		begin = i;
		return arrayList;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00009A3C File Offset: 0x00007E3C
	private static void MoveToNextNode(string json, ref int begin, int end)
	{
		bool flag = false;
		for (int i = begin; i < end; i++)
		{
			char c = json[i];
			if (c == '"' && i > 0 && json[i - 1] != '"')
			{
				flag = !flag;
			}
			if (!flag && (c == '{' || c == '['))
			{
				begin = i;
				return;
			}
		}
		begin = end;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00009AA8 File Offset: 0x00007EA8
	private static int FindMatchingEnd(string json, int begin, int end)
	{
		int num = 0;
		bool flag = false;
		for (int i = begin; i < end; i++)
		{
			char c = json[i];
			if (i == 0 || json[i - 1] != '\\')
			{
				if (c == '"')
				{
					flag = !flag;
				}
				else if (c == '{' || c == '[')
				{
					num++;
				}
				else if (c == '}' || c == ']')
				{
					num--;
					if (num == 0)
					{
						return i;
					}
				}
			}
		}
		return end;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00009B34 File Offset: 0x00007F34
	private static string TrimPropertyValue(string str)
	{
		str = str.Trim();
		if (str == null || str.Length == 0)
		{
			return string.Empty;
		}
		while ((str.Length > 1 && str[0] == '\r') || str[0] == '\n' || str[0] == '\t' || str[0] == ' ')
		{
			str = str.Substring(1, str.Length - 1);
		}
		while ((str.Length > 0 && str[str.Length - 1] == '\r') || str[str.Length - 1] == '\n' || str[str.Length - 1] == '\t' || str[str.Length - 1] == ' ')
		{
			str = str.Substring(0, str.Length - 1);
		}
		if (str == null)
		{
			return string.Empty;
		}
		if (str.Length >= 2 && str[0] == '"' && str[str.Length - 1] == '"')
		{
			return str.Substring(1, str.Length - 2);
		}
		return str;
	}
}
