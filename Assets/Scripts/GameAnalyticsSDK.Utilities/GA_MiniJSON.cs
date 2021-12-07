// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameAnalyticsSDK.Utilities
{
	public class GA_MiniJSON
	{
		private sealed class Parser : IDisposable
		{
			private enum TOKEN
			{
				NONE,
				CURLY_OPEN,
				CURLY_CLOSE,
				SQUARED_OPEN,
				SQUARED_CLOSE,
				COLON,
				COMMA,
				STRING,
				NUMBER,
				TRUE,
				FALSE,
				NULL
			}

			private const string WORD_BREAK = "{}[],:\"";

			private StringReader json;

			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (!GA_MiniJSON.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			private GA_MiniJSON.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return GA_MiniJSON.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					switch (peekChar)
					{
					case ',':
						this.json.Read();
						return GA_MiniJSON.Parser.TOKEN.COMMA;
					case '-':
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						return GA_MiniJSON.Parser.TOKEN.NUMBER;
					case '.':
					case '/':
						IL_65:
						switch (peekChar)
						{
						case '[':
							return GA_MiniJSON.Parser.TOKEN.SQUARED_OPEN;
						case '\\':
							IL_7A:
							switch (peekChar)
							{
							case '{':
								return GA_MiniJSON.Parser.TOKEN.CURLY_OPEN;
							case '|':
								IL_8F:
								if (peekChar != '"')
								{
									string nextWord = this.NextWord;
									if (nextWord != null)
									{
										if (nextWord == "false")
										{
											return GA_MiniJSON.Parser.TOKEN.FALSE;
										}
										if (nextWord == "true")
										{
											return GA_MiniJSON.Parser.TOKEN.TRUE;
										}
										if (nextWord == "null")
										{
											return GA_MiniJSON.Parser.TOKEN.NULL;
										}
									}
									return GA_MiniJSON.Parser.TOKEN.NONE;
								}
								return GA_MiniJSON.Parser.TOKEN.STRING;
							case '}':
								this.json.Read();
								return GA_MiniJSON.Parser.TOKEN.CURLY_CLOSE;
							}
							goto IL_8F;
						case ']':
							this.json.Read();
							return GA_MiniJSON.Parser.TOKEN.SQUARED_CLOSE;
						}
						goto IL_7A;
					case ':':
						return GA_MiniJSON.Parser.TOKEN.COLON;
					}
					goto IL_65;
				}
			}

			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			public static object Parse(string jsonString)
			{
				object result;
				using (GA_MiniJSON.Parser parser = new GA_MiniJSON.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				while (true)
				{
					GA_MiniJSON.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case GA_MiniJSON.Parser.TOKEN.NONE:
						goto IL_37;
					case GA_MiniJSON.Parser.TOKEN.CURLY_OPEN:
					{
						IL_2B:
						if (nextToken == GA_MiniJSON.Parser.TOKEN.COMMA)
						{
							continue;
						}
						string text = this.ParseString();
						if (text == null)
						{
							goto Block_2;
						}
						if (this.NextToken != GA_MiniJSON.Parser.TOKEN.COLON)
						{
							goto Block_3;
						}
						this.json.Read();
						dictionary[text] = this.ParseValue();
						continue;
					}
					case GA_MiniJSON.Parser.TOKEN.CURLY_CLOSE:
						return dictionary;
					}
					goto IL_2B;
				}
				IL_37:
				return null;
				Block_2:
				return null;
				Block_3:
				return null;
			}

			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					GA_MiniJSON.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case GA_MiniJSON.Parser.TOKEN.SQUARED_CLOSE:
						flag = false;
						continue;
					case GA_MiniJSON.Parser.TOKEN.COLON:
						IL_34:
						if (nextToken != GA_MiniJSON.Parser.TOKEN.NONE)
						{
							object item = this.ParseByToken(nextToken);
							list.Add(item);
							continue;
						}
						return null;
					case GA_MiniJSON.Parser.TOKEN.COMMA:
						continue;
					}
					goto IL_34;
				}
				return list;
			}

			private object ParseValue()
			{
				GA_MiniJSON.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			private object ParseByToken(GA_MiniJSON.Parser.TOKEN token)
			{
				switch (token)
				{
				case GA_MiniJSON.Parser.TOKEN.STRING:
					return this.ParseString();
				case GA_MiniJSON.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case GA_MiniJSON.Parser.TOKEN.TRUE:
					return true;
				case GA_MiniJSON.Parser.TOKEN.FALSE:
					return false;
				case GA_MiniJSON.Parser.TOKEN.NULL:
					return null;
				default:
					switch (token)
					{
					case GA_MiniJSON.Parser.TOKEN.CURLY_OPEN:
						return this.ParseObject();
					case GA_MiniJSON.Parser.TOKEN.SQUARED_OPEN:
						return this.ParseArray();
					}
					return null;
				}
			}

			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char nextChar = this.NextChar;
					if (nextChar != '"')
					{
						if (nextChar != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else
						{
							if (this.json.Peek() != -1)
							{
								nextChar = this.NextChar;
								switch (nextChar)
								{
								case 'r':
									stringBuilder.Append('\r');
									continue;
								case 's':
									IL_8C:
									if (nextChar == '"' || nextChar == '/' || nextChar == '\\')
									{
										stringBuilder.Append(nextChar);
										continue;
									}
									if (nextChar == 'b')
									{
										stringBuilder.Append('\b');
										continue;
									}
									if (nextChar == 'f')
									{
										stringBuilder.Append('\f');
										continue;
									}
									if (nextChar != 'n')
									{
										continue;
									}
									stringBuilder.Append('\n');
									continue;
								case 't':
									stringBuilder.Append('\t');
									continue;
								case 'u':
								{
									char[] array = new char[4];
									for (int i = 0; i < 4; i++)
									{
										array[i] = this.NextChar;
									}
									stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
									continue;
								}
								}
								goto IL_8C;
							}
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}
		}

		private sealed class Serializer
		{
			private StringBuilder builder;

			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			public static string Serialize(object obj)
			{
				GA_MiniJSON.Serializer serializer = new GA_MiniJSON.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			private void SerializeValue(object value)
			{
				string str;
				IList anArray;
				IDictionary obj;
				if (value == null)
				{
					this.builder.Append("null");
				}
				else if ((str = (value as string)) != null)
				{
					this.SerializeString(str);
				}
				else if (value is bool)
				{
					this.builder.Append((!(bool)value) ? "false" : "true");
				}
				else if ((anArray = (value as IList)) != null)
				{
					this.SerializeArray(anArray);
				}
				else if ((obj = (value as IDictionary)) != null)
				{
					this.SerializeObject(obj);
				}
				else if (value is char)
				{
					this.SerializeString(new string((char)value, 1));
				}
				else
				{
					this.SerializeOther(value);
				}
			}

			private void SerializeObject(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				IEnumerator enumerator = obj.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						if (!flag)
						{
							this.builder.Append(',');
						}
						this.SerializeString(current.ToString());
						this.builder.Append(':');
						this.SerializeValue(obj[current]);
						flag = false;
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
				this.builder.Append('}');
			}

			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				IEnumerator enumerator = anArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						if (!flag)
						{
							this.builder.Append(',');
						}
						this.SerializeValue(current);
						flag = false;
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
				this.builder.Append(']');
			}

			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				char[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					char c = array2[i];
					switch (c)
					{
					case '\b':
						this.builder.Append("\\b");
						goto IL_14B;
					case '\t':
						this.builder.Append("\\t");
						goto IL_14B;
					case '\n':
						this.builder.Append("\\n");
						goto IL_14B;
					case '\v':
						IL_42:
						if (c == '"')
						{
							this.builder.Append("\\\"");
							goto IL_14B;
						}
						if (c != '\\')
						{
							int num = Convert.ToInt32(c);
							if (num >= 32 && num <= 126)
							{
								this.builder.Append(c);
							}
							else
							{
								this.builder.Append("\\u");
								this.builder.Append(num.ToString("x4"));
							}
							goto IL_14B;
						}
						this.builder.Append("\\\\");
						goto IL_14B;
					case '\f':
						this.builder.Append("\\f");
						goto IL_14B;
					case '\r':
						this.builder.Append("\\r");
						goto IL_14B;
					}
					goto IL_42;
					IL_14B:;
				}
				this.builder.Append('"');
			}

			private void SerializeOther(object value)
			{
				if (value is float)
				{
					this.builder.Append(((float)value).ToString("R"));
				}
				else if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
				{
					this.builder.Append(value);
				}
				else if (value is double || value is decimal)
				{
					this.builder.Append(Convert.ToDouble(value).ToString("R"));
				}
				else
				{
					this.SerializeString(value.ToString());
				}
			}
		}

		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return GA_MiniJSON.Parser.Parse(json);
		}

		public static string Serialize(object obj)
		{
			return GA_MiniJSON.Serializer.Serialize(obj);
		}
	}
}
