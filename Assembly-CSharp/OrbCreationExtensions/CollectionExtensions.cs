using System;
using System.Collections;
using UnityEngine;

namespace OrbCreationExtensions
{
	// Token: 0x02000012 RID: 18
	public static class CollectionExtensions
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00006367 File Offset: 0x00004767
		public static object GetValue(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				return hash[key];
			}
			return null;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000637E File Offset: 0x0000477E
		public static Hashtable GetHashtable(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key) && hash[key].GetType() == typeof(Hashtable))
			{
				return (Hashtable)hash[key];
			}
			return null;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000063BA File Offset: 0x000047BA
		public static ArrayList GetArrayList(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key) && hash[key].GetType() == typeof(ArrayList))
			{
				return (ArrayList)hash[key];
			}
			return null;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000063F8 File Offset: 0x000047F8
		public static ArrayList GetArrayList(this Hashtable hash, object key, bool wrap)
		{
			if (hash.ContainsKey(key))
			{
				if (hash[key].GetType() == typeof(ArrayList))
				{
					return (ArrayList)hash[key];
				}
				if (wrap)
				{
					return new ArrayList
					{
						hash[key]
					};
				}
			}
			return null;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000645B File Offset: 0x0000485B
		public static string GetString(this Hashtable hash, object key)
		{
			return hash.GetString(key, null);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00006468 File Offset: 0x00004868
		public static string GetString(this Hashtable hash, object key, string defaultValue)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return defaultValue;
				}
				if (obj.GetType() == typeof(string))
				{
					return (string)obj;
				}
				if (obj.GetType() == typeof(bool))
				{
					return ((bool)obj).MakeString();
				}
				if (obj.GetType() == typeof(int) || obj.GetType() == typeof(long))
				{
					return ((int)obj).MakeString();
				}
				if (obj.GetType() == typeof(float) || obj.GetType() == typeof(double))
				{
					return ((float)obj).MakeString();
				}
			}
			return defaultValue;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000655B File Offset: 0x0000495B
		public static int GetInt(this Hashtable hash, object key)
		{
			return hash.GetInt(key, 0);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00006568 File Offset: 0x00004968
		public static int GetInt(this Hashtable hash, object key, int defaultValue)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return defaultValue;
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeInt();
				}
				if (obj.GetType() == typeof(bool) && (bool)obj)
				{
					return 1;
				}
				if (obj.GetType() == typeof(int) || obj.GetType() == typeof(long))
				{
					return (int)obj;
				}
				if (obj.GetType() == typeof(float) || obj.GetType() == typeof(double))
				{
					return Mathf.FloorToInt((float)obj);
				}
			}
			return defaultValue;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000665C File Offset: 0x00004A5C
		public static float GetFloat(this Hashtable hash, object key)
		{
			return hash.GetFloat(key, 0f);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000666C File Offset: 0x00004A6C
		public static float GetFloat(this Hashtable hash, object key, float defaultValue)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return defaultValue;
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeFloat();
				}
				if (obj.GetType() == typeof(bool) && (bool)obj)
				{
					return 1f;
				}
				if (obj.GetType() == typeof(int) || obj.GetType() == typeof(long))
				{
					return (float)obj;
				}
				if (obj.GetType() == typeof(float) || obj.GetType() == typeof(double))
				{
					return (float)obj;
				}
			}
			return defaultValue;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00006760 File Offset: 0x00004B60
		public static bool GetBool(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return false;
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeBool();
				}
				if (obj.GetType() == typeof(bool) && (bool)obj)
				{
					return (bool)obj;
				}
				if (obj.GetType() == typeof(int) || obj.GetType() == typeof(long))
				{
					return (int)obj > 0;
				}
				if (obj.GetType() == typeof(float) || obj.GetType() == typeof(double))
				{
					return (float)obj > 0f;
				}
			}
			return false;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006860 File Offset: 0x00004C60
		public static Color GetColor(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return new Color(0f, 0f, 0f, 0f);
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeColor();
				}
				if (obj.GetType() == typeof(Color))
				{
					return (Color)obj;
				}
			}
			return new Color(0f, 0f, 0f, 0f);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00006904 File Offset: 0x00004D04
		public static Vector3 GetVector3(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return Vector3.zero;
				}
				if (obj.GetType() == typeof(string))
				{
					return ((string)obj).MakeVector3();
				}
				if (obj.GetType() == typeof(Vector3))
				{
					return (Vector3)obj;
				}
			}
			return Vector3.zero;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006980 File Offset: 0x00004D80
		public static Texture2D GetTexture2D(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return null;
				}
				if (obj.GetType() == typeof(Texture2D))
				{
					return (Texture2D)obj;
				}
			}
			return null;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000069CC File Offset: 0x00004DCC
		public static byte[] GetBytes(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				object obj = hash[key];
				if (obj == null)
				{
					return null;
				}
				if (obj.GetType() == typeof(byte[]))
				{
					return (byte[])obj;
				}
			}
			return null;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00006A18 File Offset: 0x00004E18
		public static void AddHashtable(this Hashtable hash, Hashtable addHash, bool overwriteExistingKeys)
		{
			ICollection keys = addHash.Keys;
			IEnumerator enumerator = keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					string key = (string)obj;
					if (overwriteExistingKeys || !hash.ContainsKey(key))
					{
						hash[key] = addHash[key];
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00006A98 File Offset: 0x00004E98
		public static string XmlString(this Hashtable hash)
		{
			string empty = string.Empty;
			hash.XmlString(ref empty, 0);
			return empty;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00006AB8 File Offset: 0x00004EB8
		public static string XmlString(this ArrayList arr)
		{
			string empty = string.Empty;
			arr.XmlString(ref empty, 0);
			return empty;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006AD8 File Offset: 0x00004ED8
		private static void XmlString(this Hashtable hash, ref string str, int level)
		{
			bool flag = false;
			string text = null;
			if (hash.ContainsKey(".tag."))
			{
				text = (string)hash[".tag."];
				CollectionExtensions.MoveToNewLineIfNeeded(ref str, level);
				str = str + "<" + text;
				if (hash.Count < 6 && hash.ContainsKey(text))
				{
					flag = true;
					ICollection keys = hash.Keys;
					IEnumerator enumerator = keys.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							string text2 = (string)obj;
							if (text2 != text && (hash[text2].GetType() == typeof(Hashtable) || hash[text2].GetType() == typeof(ArrayList)))
							{
								flag = false;
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				if (flag)
				{
					ICollection keys2 = hash.Keys;
					IEnumerator enumerator2 = keys2.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							string text3 = (string)obj2;
							if (text3 != text && text3 != ".tag.")
							{
								str = string.Concat(new object[]
								{
									str,
									" ",
									text3,
									"=\"",
									hash[text3],
									"\""
								});
							}
						}
					}
					finally
					{
						IDisposable disposable2;
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
				}
				str += ">\n";
				level++;
			}
			if (flag)
			{
				CollectionExtensions.MoveToNewLineIfNeeded(ref str, level);
				str = str + hash[text] + "\n";
			}
			else
			{
				ICollection keys3 = hash.Keys;
				IEnumerator enumerator3 = keys3.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						string text4 = (string)obj3;
						if (text4 != ".tag.")
						{
							object obj4 = hash[text4];
							CollectionExtensions.MoveToNewLineIfNeeded(ref str, level);
							str = str + "<" + text4 + ">";
							level++;
							if (obj4 == null)
							{
								str += "NULL";
							}
							else if (obj4.GetType() == typeof(Hashtable))
							{
								((Hashtable)obj4).XmlString(ref str, level);
							}
							else if (obj4.GetType() == typeof(ArrayList))
							{
								((ArrayList)obj4).XmlString(ref str, level);
							}
							else if (obj4.GetType() == typeof(string))
							{
								str += ((string)obj4).XmlEncode();
							}
							else
							{
								str += obj4;
							}
							level--;
							CollectionExtensions.MoveToNewLineIfNeeded(ref str, level);
							str = str + "</" + text4 + ">\n";
						}
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = (enumerator3 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
			}
			if (hash.ContainsKey(".tag."))
			{
				level--;
				CollectionExtensions.MoveToNewLineIfNeeded(ref str, level);
				str = str + "</" + text + ">\n";
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00006E90 File Offset: 0x00005290
		private static void XmlString(this ArrayList arr, ref string str, int level)
		{
			CollectionExtensions.MoveToNewLineIfNeeded(ref str, level);
			for (int i = 0; i < arr.Count; i++)
			{
				object obj = arr[i];
				CollectionExtensions.MoveToNewLineIfNeeded(ref str, level);
				if (obj == null)
				{
					str += "NULL";
					str += "\n";
				}
				else if (obj.GetType() == typeof(Hashtable))
				{
					((Hashtable)obj).XmlString(ref str, level);
				}
				else if (obj.GetType() == typeof(ArrayList))
				{
					((ArrayList)obj).XmlString(ref str, level);
				}
				else if (obj.GetType() == typeof(string))
				{
					str += ((string)obj).XmlEncode();
					str += "\n";
				}
				else
				{
					str += obj;
					str += "\n";
				}
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00006FA4 File Offset: 0x000053A4
		private static void MoveToNewLineIfNeeded(ref string str, int level)
		{
			if (str.Length > 0 && str.Substring(str.Length - 1) == ">")
			{
				str += "\n";
				for (int i = 0; i < level; i++)
				{
					str += "\t";
				}
			}
			else if (str.Length > 0 && str.Substring(str.Length - 1) == "\n")
			{
				for (int j = 0; j < level; j++)
				{
					str += "\t";
				}
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000705C File Offset: 0x0000545C
		public static string JsonString(this Hashtable hash)
		{
			string empty = string.Empty;
			hash.JsonString(ref empty, 0, true);
			return empty;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000707C File Offset: 0x0000547C
		public static string JsonString(this ArrayList arr)
		{
			string empty = string.Empty;
			arr.JsonString(ref empty, 0, true);
			return empty;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000709C File Offset: 0x0000549C
		public static string JsonString(this Hashtable hash, bool encode)
		{
			string empty = string.Empty;
			hash.JsonString(ref empty, 0, encode);
			return empty;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000070BC File Offset: 0x000054BC
		public static string JsonString(this ArrayList arr, bool encode)
		{
			string empty = string.Empty;
			arr.JsonString(ref empty, 0, encode);
			return empty;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000070DC File Offset: 0x000054DC
		private static void JsonString(this Hashtable hash, ref string str, int level, bool encode)
		{
			str += "{\n";
			level++;
			int num = 0;
			ICollection keys = hash.Keys;
			IEnumerator enumerator = keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					string text = (string)obj;
					object obj2 = hash[text];
					for (int i = 0; i < level; i++)
					{
						str += "\t";
					}
					if (encode)
					{
						str = str + "\"" + text + "\": ";
					}
					else
					{
						str = str + text + ": ";
					}
					if (obj2 == null)
					{
						str += "NULL";
					}
					else if (obj2.GetType() == typeof(Hashtable))
					{
						((Hashtable)obj2).JsonString(ref str, level, encode);
					}
					else if (obj2.GetType() == typeof(ArrayList))
					{
						((ArrayList)obj2).JsonString(ref str, level, encode);
					}
					else if (obj2.GetType() == typeof(string))
					{
						if (encode)
						{
							str = str + "\"" + ((string)obj2).JsonEncode() + "\"";
						}
						else
						{
							str += (string)obj2;
						}
					}
					else
					{
						str = string.Concat(new object[]
						{
							str,
							"\"",
							obj2,
							"\""
						});
					}
					num++;
					if (num < hash.Count)
					{
						str += ",";
					}
					str += "\n";
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			level--;
			for (int j = 0; j < level; j++)
			{
				str += "\t";
			}
			str += "}";
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00007318 File Offset: 0x00005718
		private static void JsonString(this ArrayList arr, ref string str, int level, bool encode)
		{
			str += "[\n";
			level++;
			for (int i = 0; i < arr.Count; i++)
			{
				object obj = arr[i];
				for (int j = 0; j < level; j++)
				{
					str += "\t";
				}
				if (obj == null)
				{
					str += "NULL";
				}
				else if (obj.GetType() == typeof(Hashtable))
				{
					((Hashtable)obj).JsonString(ref str, level, encode);
				}
				else if (obj.GetType() == typeof(ArrayList))
				{
					((ArrayList)obj).JsonString(ref str, level, encode);
				}
				else if (obj.GetType() == typeof(string))
				{
					if (encode)
					{
						str = str + "\"" + ((string)obj).JsonEncode() + "\"";
					}
					else
					{
						str += (string)obj;
					}
				}
				else
				{
					str = string.Concat(new object[]
					{
						str,
						"\"",
						obj,
						"\""
					});
				}
				if (i < arr.Count - 1)
				{
					str += ",";
				}
				str += "\n";
			}
			level--;
			for (int k = 0; k < level; k++)
			{
				str += "\t";
			}
			str += "]";
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000074C4 File Offset: 0x000058C4
		public static object GetNodeAtPath(this ArrayList inNodeList, string[] path)
		{
			return CollectionExtensions.GetNodeAtPath(inNodeList, path, 0);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000074CE File Offset: 0x000058CE
		public static object GetNodeAtPath(this Hashtable inNodeHash, string[] path)
		{
			return CollectionExtensions.GetNodeAtPath(inNodeHash, path, 0);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000074D8 File Offset: 0x000058D8
		private static object GetNodeAtPath(ArrayList inNodeList, string[] path, int level)
		{
			if (inNodeList == null)
			{
				return null;
			}
			string b = path[level];
			for (int i = 0; i < inNodeList.Count; i++)
			{
				object obj = inNodeList[i];
				if (obj != null)
				{
					if (obj.GetType() == typeof(Hashtable))
					{
						if (!((Hashtable)obj).ContainsKey(".tag."))
						{
							return CollectionExtensions.GetNodeAtPath((Hashtable)obj, path, level);
						}
						if ((string)((Hashtable)obj)[".tag."] == b)
						{
							if (level == path.Length - 1)
							{
								return obj;
							}
							return CollectionExtensions.GetNodeAtPath((Hashtable)obj, path, level + 1);
						}
					}
					else if (obj.GetType() == typeof(ArrayList))
					{
						return CollectionExtensions.GetNodeAtPath((ArrayList)obj, path, level);
					}
				}
			}
			return null;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000075C4 File Offset: 0x000059C4
		private static object GetNodeAtPath(Hashtable inNodeHash, string[] path, int level)
		{
			if (inNodeHash == null)
			{
				return null;
			}
			string key = path[level];
			if (inNodeHash.ContainsKey(key))
			{
				object obj = inNodeHash[key];
				if (level == path.Length - 1)
				{
					return inNodeHash[key];
				}
				if (obj != null)
				{
					if (obj.GetType() == typeof(Hashtable))
					{
						return CollectionExtensions.GetNodeAtPath((Hashtable)obj, path, level + 1);
					}
					if (obj.GetType() == typeof(ArrayList))
					{
						return CollectionExtensions.GetNodeAtPath((ArrayList)obj, path, level + 1);
					}
				}
			}
			return null;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00007660 File Offset: 0x00005A60
		public static Hashtable GetNodeWithProperty(this ArrayList inNodeList, string aKey, string aValue)
		{
			if (inNodeList == null)
			{
				return null;
			}
			for (int i = 0; i < inNodeList.Count; i++)
			{
				object obj = inNodeList[i];
				if (obj != null)
				{
					Hashtable hashtable = null;
					if (obj.GetType() == typeof(Hashtable))
					{
						hashtable = ((Hashtable)obj).GetNodeWithProperty(aKey, aValue);
					}
					else if (obj.GetType() == typeof(ArrayList))
					{
						hashtable = ((ArrayList)obj).GetNodeWithProperty(aKey, aValue);
					}
					if (hashtable != null)
					{
						return hashtable;
					}
				}
			}
			return null;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000076FC File Offset: 0x00005AFC
		public static Hashtable GetNodeWithProperty(this Hashtable inNodeHash, string aKey, string aValue)
		{
			if (inNodeHash == null)
			{
				return null;
			}
			ICollection keys = inNodeHash.Keys;
			IEnumerator enumerator = keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					string text = (string)obj;
					object obj2 = inNodeHash[text];
					if (obj2 != null)
					{
						Hashtable hashtable = null;
						if (text == aKey)
						{
							string a = string.Empty + obj2;
							if (a == aValue)
							{
								hashtable = inNodeHash;
							}
						}
						else if (obj2.GetType() == typeof(Hashtable))
						{
							hashtable = ((Hashtable)obj2).GetNodeWithProperty(aKey, aValue);
						}
						else if (obj2.GetType() == typeof(ArrayList))
						{
							hashtable = ((ArrayList)obj2).GetNodeWithProperty(aKey, aValue);
						}
						if (hashtable != null)
						{
							return hashtable;
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return null;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00007810 File Offset: 0x00005C10
		public static Hashtable GetHashtable(this ArrayList arr, int index)
		{
			if (arr.Count > index && index >= 0 && arr[index].GetType() == typeof(Hashtable))
			{
				return (Hashtable)arr[index];
			}
			return null;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00007860 File Offset: 0x00005C60
		public static ArrayList GetArrayList(this ArrayList arr, int index)
		{
			if (arr.Count > index && index >= 0 && arr[index].GetType() == typeof(ArrayList))
			{
				return (ArrayList)arr[index];
			}
			return null;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000078B0 File Offset: 0x00005CB0
		public static ArrayList GetArrayList(this ArrayList arr, int index, bool wrap)
		{
			if (arr.Count > index && index >= 0)
			{
				if (arr[index].GetType() == typeof(ArrayList))
				{
					return (ArrayList)arr[index];
				}
				if (wrap)
				{
					return new ArrayList
					{
						arr[index]
					};
				}
			}
			return null;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000791C File Offset: 0x00005D1C
		public static string GetString(this ArrayList arr, int index)
		{
			if (arr.Count > index && index >= 0 && arr[index].GetType() == typeof(string))
			{
				return (string)arr[index];
			}
			return null;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000796C File Offset: 0x00005D6C
		public static float GetFloat(this ArrayList arr, int index, float defaultValue = 0f)
		{
			if (arr.Count > index && index >= 0)
			{
				if (arr[index].GetType() == typeof(float))
				{
					return (float)arr[index];
				}
				if (arr[index].GetType() == typeof(int))
				{
					return (float)arr[index];
				}
				if (arr[index].GetType() == typeof(string))
				{
					return ((string)arr[index]).MakeFloat();
				}
			}
			return defaultValue;
		}
	}
}
