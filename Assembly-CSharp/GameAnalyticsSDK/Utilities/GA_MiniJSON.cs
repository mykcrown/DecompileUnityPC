using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameAnalyticsSDK.Utilities
{
	// Token: 0x02000039 RID: 57
	public class GA_MiniJSON
	{
		// Token: 0x060001DB RID: 475 RVA: 0x0000E527 File Offset: 0x0000C927
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return GA_MiniJSON.Parser.Parse(json);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000E537 File Offset: 0x0000C937
		public static string Serialize(object obj)
		{
			return GA_MiniJSON.Serializer.Serialize(obj);
		}

		// Token: 0x0200003A RID: 58
		private sealed class Parser : IDisposable
		{
			// Token: 0x060001DD RID: 477 RVA: 0x0000E53F File Offset: 0x0000C93F
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x060001DE RID: 478 RVA: 0x0000E553 File Offset: 0x0000C953
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x060001DF RID: 479 RVA: 0x0000E574 File Offset: 0x0000C974
			public static object Parse(string jsonString)
			{
				object result;
				using (GA_MiniJSON.Parser parser = new GA_MiniJSON.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x060001E0 RID: 480 RVA: 0x0000E5B4 File Offset: 0x0000C9B4
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x060001E1 RID: 481 RVA: 0x0000E5C8 File Offset: 0x0000C9C8
			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					GA_MiniJSON.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case GA_MiniJSON.Parser.TOKEN.NONE:
						goto IL_37;
					default:
						if (nextToken != GA_MiniJSON.Parser.TOKEN.COMMA)
						{
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
						}
						break;
					case GA_MiniJSON.Parser.TOKEN.CURLY_CLOSE:
						return dictionary;
					}
				}
				IL_37:
				return null;
				Block_2:
				return null;
				Block_3:
				return null;
			}

			// Token: 0x060001E2 RID: 482 RVA: 0x0000E654 File Offset: 0x0000CA54
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
						break;
					default:
					{
						if (nextToken == GA_MiniJSON.Parser.TOKEN.NONE)
						{
							return null;
						}
						object item = this.ParseByToken(nextToken);
						list.Add(item);
						break;
					}
					case GA_MiniJSON.Parser.TOKEN.COMMA:
						break;
					}
				}
				return list;
			}

			// Token: 0x060001E3 RID: 483 RVA: 0x0000E6CC File Offset: 0x0000CACC
			private object ParseValue()
			{
				GA_MiniJSON.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x060001E4 RID: 484 RVA: 0x0000E6E8 File Offset: 0x0000CAE8
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

			// Token: 0x060001E5 RID: 485 RVA: 0x0000E758 File Offset: 0x0000CB58
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
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							nextChar = this.NextChar;
							switch (nextChar)
							{
							case 'r':
								stringBuilder.Append('\r');
								break;
							default:
								if (nextChar != '"' && nextChar != '/' && nextChar != '\\')
								{
									if (nextChar != 'b')
									{
										if (nextChar != 'f')
										{
											if (nextChar == 'n')
											{
												stringBuilder.Append('\n');
											}
										}
										else
										{
											stringBuilder.Append('\f');
										}
									}
									else
									{
										stringBuilder.Append('\b');
									}
								}
								else
								{
									stringBuilder.Append(nextChar);
								}
								break;
							case 't':
								stringBuilder.Append('\t');
								break;
							case 'u':
							{
								char[] array = new char[4];
								for (int i = 0; i < 4; i++)
								{
									array[i] = this.NextChar;
								}
								stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
								break;
							}
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060001E6 RID: 486 RVA: 0x0000E8D8 File Offset: 0x0000CCD8
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

			// Token: 0x060001E7 RID: 487 RVA: 0x0000E919 File Offset: 0x0000CD19
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

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000E952 File Offset: 0x0000CD52
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000E964 File Offset: 0x0000CD64
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x1700001E RID: 30
			// (get) Token: 0x060001EA RID: 490 RVA: 0x0000E978 File Offset: 0x0000CD78
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

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x060001EB RID: 491 RVA: 0x0000E9CC File Offset: 0x0000CDCC
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
					default:
						switch (peekChar)
						{
						case '[':
							return GA_MiniJSON.Parser.TOKEN.SQUARED_OPEN;
						default:
							switch (peekChar)
							{
							case '{':
								return GA_MiniJSON.Parser.TOKEN.CURLY_OPEN;
							default:
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
							break;
						case ']':
							this.json.Read();
							return GA_MiniJSON.Parser.TOKEN.SQUARED_CLOSE;
						}
						break;
					case ':':
						return GA_MiniJSON.Parser.TOKEN.COLON;
					}
				}
			}

			// Token: 0x0400019B RID: 411
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x0400019C RID: 412
			private StringReader json;

			// Token: 0x0200003B RID: 59
			private enum TOKEN
			{
				// Token: 0x0400019E RID: 414
				NONE,
				// Token: 0x0400019F RID: 415
				CURLY_OPEN,
				// Token: 0x040001A0 RID: 416
				CURLY_CLOSE,
				// Token: 0x040001A1 RID: 417
				SQUARED_OPEN,
				// Token: 0x040001A2 RID: 418
				SQUARED_CLOSE,
				// Token: 0x040001A3 RID: 419
				COLON,
				// Token: 0x040001A4 RID: 420
				COMMA,
				// Token: 0x040001A5 RID: 421
				STRING,
				// Token: 0x040001A6 RID: 422
				NUMBER,
				// Token: 0x040001A7 RID: 423
				TRUE,
				// Token: 0x040001A8 RID: 424
				FALSE,
				// Token: 0x040001A9 RID: 425
				NULL
			}
		}

		// Token: 0x0200003C RID: 60
		private sealed class Serializer
		{
			// Token: 0x060001EC RID: 492 RVA: 0x0000EAF5 File Offset: 0x0000CEF5
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x060001ED RID: 493 RVA: 0x0000EB08 File Offset: 0x0000CF08
			public static string Serialize(object obj)
			{
				GA_MiniJSON.Serializer serializer = new GA_MiniJSON.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x060001EE RID: 494 RVA: 0x0000EB30 File Offset: 0x0000CF30
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

			// Token: 0x060001EF RID: 495 RVA: 0x0000EC04 File Offset: 0x0000D004
			private void SerializeObject(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				IEnumerator enumerator = obj.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						if (!flag)
						{
							this.builder.Append(',');
						}
						this.SerializeString(obj2.ToString());
						this.builder.Append(':');
						this.SerializeValue(obj[obj2]);
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

			// Token: 0x060001F0 RID: 496 RVA: 0x0000ECB8 File Offset: 0x0000D0B8
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				IEnumerator enumerator = anArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object value = enumerator.Current;
						if (!flag)
						{
							this.builder.Append(',');
						}
						this.SerializeValue(value);
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

			// Token: 0x060001F1 RID: 497 RVA: 0x0000ED48 File Offset: 0x0000D148
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				foreach (char c in array)
				{
					switch (c)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					default:
						if (c != '"')
						{
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
							}
							else
							{
								this.builder.Append("\\\\");
							}
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					}
				}
				this.builder.Append('"');
			}

			// Token: 0x060001F2 RID: 498 RVA: 0x0000EEBC File Offset: 0x0000D2BC
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

			// Token: 0x040001AA RID: 426
			private StringBuilder builder;
		}
	}
}
