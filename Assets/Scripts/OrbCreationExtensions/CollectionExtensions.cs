// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

namespace OrbCreationExtensions
{
	public static class CollectionExtensions
	{
		public static object GetValue(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key))
			{
				return hash[key];
			}
			return null;
		}

		public static Hashtable GetHashtable(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key) && hash[key].GetType() == typeof(Hashtable))
			{
				return (Hashtable)hash[key];
			}
			return null;
		}

		public static ArrayList GetArrayList(this Hashtable hash, object key)
		{
			if (hash.ContainsKey(key) && hash[key].GetType() == typeof(ArrayList))
			{
				return (ArrayList)hash[key];
			}
			return null;
		}

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

		public static string GetString(this Hashtable hash, object key)
		{
			return hash.GetString(key, null);
		}

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

		public static int GetInt(this Hashtable hash, object key)
		{
			return hash.GetInt(key, 0);
		}

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

		public static float GetFloat(this Hashtable hash, object key)
		{
			return hash.GetFloat(key, 0f);
		}

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

		public static void AddHashtable(this Hashtable hash, Hashtable addHash, bool overwriteExistingKeys)
		{
			ICollection keys = addHash.Keys;
			IEnumerator enumerator = keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string key = (string)enumerator.Current;
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

		public static string XmlString(this Hashtable hash)
		{
			string empty = string.Empty;
			hash.XmlString(ref empty, 0);
			return empty;
		}

		public static string XmlString(this ArrayList arr)
		{
			string empty = string.Empty;
			arr.XmlString(ref empty, 0);
			return empty;
		}

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
							string text2 = (string)enumerator.Current;
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
							string text3 = (string)enumerator2.Current;
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
						string text4 = (string)enumerator3.Current;
						if (text4 != ".tag.")
						{
							object obj = hash[text4];
							CollectionExtensions.MoveToNewLineIfNeeded(ref str, level);
							str = str + "<" + text4 + ">";
							level++;
							if (obj == null)
							{
								str += "NULL";
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
							}
							else
							{
								str += obj;
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

		public static string JsonString(this Hashtable hash)
		{
			string empty = string.Empty;
			hash.JsonString(ref empty, 0, true);
			return empty;
		}

		public static string JsonString(this ArrayList arr)
		{
			string empty = string.Empty;
			arr.JsonString(ref empty, 0, true);
			return empty;
		}

		public static string JsonString(this Hashtable hash, bool encode)
		{
			string empty = string.Empty;
			hash.JsonString(ref empty, 0, encode);
			return empty;
		}

		public static string JsonString(this ArrayList arr, bool encode)
		{
			string empty = string.Empty;
			arr.JsonString(ref empty, 0, encode);
			return empty;
		}

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
					string text = (string)enumerator.Current;
					object obj = hash[text];
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

		public static object GetNodeAtPath(this ArrayList inNodeList, string[] path)
		{
			return CollectionExtensions.GetNodeAtPath(inNodeList, path, 0);
		}

		public static object GetNodeAtPath(this Hashtable inNodeHash, string[] path)
		{
			return CollectionExtensions.GetNodeAtPath(inNodeHash, path, 0);
		}

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
					string text = (string)enumerator.Current;
					object obj = inNodeHash[text];
					if (obj != null)
					{
						Hashtable hashtable = null;
						if (text == aKey)
						{
							string a = string.Empty + obj;
							if (a == aValue)
							{
								hashtable = inNodeHash;
							}
						}
						else if (obj.GetType() == typeof(Hashtable))
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

		public static Hashtable GetHashtable(this ArrayList arr, int index)
		{
			if (arr.Count > index && index >= 0 && arr[index].GetType() == typeof(Hashtable))
			{
				return (Hashtable)arr[index];
			}
			return null;
		}

		public static ArrayList GetArrayList(this ArrayList arr, int index)
		{
			if (arr.Count > index && index >= 0 && arr[index].GetType() == typeof(ArrayList))
			{
				return (ArrayList)arr[index];
			}
			return null;
		}

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

		public static string GetString(this ArrayList arr, int index)
		{
			if (arr.Count > index && index >= 0 && arr[index].GetType() == typeof(string))
			{
				return (string)arr[index];
			}
			return null;
		}

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
