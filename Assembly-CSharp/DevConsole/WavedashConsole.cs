using System;
using System.Collections.Generic;
using System.IO;
using SickDev.CommandSystem;
using UnityEngine;

namespace DevConsole
{
	// Token: 0x020002F5 RID: 757
	public class WavedashConsole : Console, IDevConsole
	{
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x00060A02 File Offset: 0x0005EE02
		// (set) Token: 0x06001079 RID: 4217 RVA: 0x00060A0A File Offset: 0x0005EE0A
		[Inject]
		public UIManager uiManager { get; set; }

		// Token: 0x0600107A RID: 4218 RVA: 0x00060A13 File Offset: 0x0005EE13
		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00060A20 File Offset: 0x0005EE20
		void IDevConsole.RunAutoExec()
		{
			string text = string.Format("{0}{1}.autoexec", "Config/", Environment.UserName);
			string[] autoExecCommands = this.getAutoExecCommands(text);
			if (autoExecCommands.Length > 0)
			{
				this.PrintLn("Loading auto exec file: '{0}'.", new object[]
				{
					text
				});
				this.executeAutoExec(autoExecCommands);
			}
			if (BuildConfig.autoExec != null && BuildConfig.autoExec.Length > 0)
			{
				string[] array = BuildConfig.autoExec.Split(new char[]
				{
					';'
				});
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].Trim();
				}
				if (array.Length > 0)
				{
					this.PrintLn("Running BuildConfig auto commands:\n{0}", new object[]
					{
						BuildConfig.autoExec
					});
					this.executeAutoExec(array);
				}
			}
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00060AE8 File Offset: 0x0005EEE8
		private void executeAutoExec(string[] autoCommands)
		{
			foreach (string command in autoCommands)
			{
				this.ExecuteCommand(command, true);
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x00060B17 File Offset: 0x0005EF17
		protected bool AllowConsole
		{
			get
			{
				return this.uiManager == null || !this.uiManager.IsTextEntryMode;
			}
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00060B38 File Offset: 0x0005EF38
		public void AddAdminCommand(Action callback, string text, string help = null)
		{
			Console.AddCommand(new ActionCommand(callback, "admin." + text, help));
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00060B51 File Offset: 0x0005EF51
		public void AddAdminCommand<T>(Action<T> callback, string text, string help = null)
		{
			Console.AddCommand(new ActionCommand<T>(callback, "admin." + text, help));
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00060B6A File Offset: 0x0005EF6A
		public void AddCommand(Action callback, string category, string text, string help = null)
		{
			if (string.Compare(category, "admin", true) == 0)
			{
				this.AddAdminCommand(callback, text, help);
				return;
			}
			Console.AddCommand(new ActionCommand(callback, category + "." + text, help));
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00060BA1 File Offset: 0x0005EFA1
		public void AddCommand<T>(Action<T> callback, string category, string text, string help = null)
		{
			if (string.Compare(category, "admin", true) == 0)
			{
				this.AddAdminCommand<T>(callback, text, help);
				return;
			}
			Console.AddCommand(new ActionCommand<T>(callback, category + "." + text, help));
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00060BD8 File Offset: 0x0005EFD8
		public void AddPlayerCommand(Action<PlayerNum> callback, string text, string help = null)
		{
			for (PlayerNum playerNum = PlayerNum.Player1; playerNum < PlayerNum.Player6; playerNum++)
			{
				PlayerNum copy = playerNum;
				this.AddCommand(delegate()
				{
					callback(copy);
				}, playerNum.ToString().ToLower(), text, help);
			}
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00060C40 File Offset: 0x0005F040
		public void AddPlayerCommand<T>(Action<PlayerNum, T> callback, string text, string help = null)
		{
			for (PlayerNum playerNum = PlayerNum.Player1; playerNum < PlayerNum.Player6; playerNum++)
			{
				PlayerNum copy = playerNum;
				this.AddCommand<T>(delegate(T value)
				{
					callback(copy, value);
				}, playerNum.ToString().ToLower(), text, help);
			}
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00060CA8 File Offset: 0x0005F0A8
		public void AddConsoleVariable<T>(string context, string name, string label, string help, Func<T> getCallback, Action<T> setCallback)
		{
			string item = string.Format("{0}.{1}", context, name);
			if (this.consoleVariables.Contains(item))
			{
				return;
			}
			this.consoleVariables.Add(item);
			if (typeof(T).IsEnum)
			{
				this.AddCommand<string>(delegate(string value)
				{
					if (value.Equals("?"))
					{
						this.PrintLn(string.Format("{0} can be set to:", label));
						foreach (T t in EnumUtil.GetValues<T>())
						{
							this.PrintLn(string.Format(" {0}", t.ToString()));
						}
						return;
					}
					T t2 = default(T);
					if (!EnumUtil.TryParse<T>(value, ref t2, true, true, true))
					{
						this.PrintLn("Unable to parse {0} as value of Enum Type {1}.  Use '{2}.{3} ?' to view a list of available values.", new object[]
						{
							value,
							typeof(T).ToString(),
							context,
							name
						});
						return;
					}
					setCallback(t2);
					this.PrintLabel<T>(label, t2);
				}, context, name, help);
				this.AddCommand(delegate()
				{
					this.PrintLn(string.Format("'{0}.{1} ?' to view list of available values\n{2}: {3}.", new object[]
					{
						context,
						name,
						label,
						getCallback()
					}));
				}, context, name, help);
			}
			else if (typeof(T) == typeof(bool))
			{
				this.AddCommand<string>(delegate(string value)
				{
					T t;
					if (value.EqualsIgnoreCase("!") || value.EqualsIgnoreCase("toggle"))
					{
						bool flag = (bool)((object)getCallback());
						t = (T)((object)(!flag));
					}
					else if (value.EqualsIgnoreCase("true") || value.EqualsIgnoreCase("1") || value.EqualsIgnoreCase("yes"))
					{
						t = (T)((object)true);
					}
					else
					{
						if (!value.EqualsIgnoreCase("false") && !value.EqualsIgnoreCase("0") && !value.EqualsIgnoreCase("no"))
						{
							this.PrintLn("Unable to parse {0} as value of String Type {1}.", new object[]
							{
								value,
								typeof(T).ToString()
							});
							return;
						}
						t = (T)((object)false);
					}
					setCallback(t);
					this.PrintLabel<T>(label, t);
				}, context, name, help);
				this.AddCommand(delegate()
				{
					this.PrintLabel<T>(label, getCallback());
				}, context, name, help);
			}
			else
			{
				this.AddCommand<T>(delegate(T value)
				{
					setCallback(value);
					this.PrintLabel<T>(label, value);
				}, context, name, help);
				this.AddCommand(delegate()
				{
					this.PrintLabel<T>(label, getCallback());
				}, context, name, help);
			}
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00060E1C File Offset: 0x0005F21C
		public void AddPlayerConsoleVariable<T>(string name, string label, string help, Func<PlayerNum, T> getCallback, Action<PlayerNum, T> setCallback)
		{
			for (PlayerNum playerNum = PlayerNum.Player1; playerNum < PlayerNum.Player6; playerNum++)
			{
				PlayerNum copy = playerNum;
				string text = playerNum.ToString();
				this.AddConsoleVariable<T>(text.ToLower(), name, string.Format("{0} {1}", text, label), string.Format("{0} ({1})", help, text), () => getCallback(copy), delegate(T value)
				{
					setCallback(copy, value);
				});
			}
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00060EB0 File Offset: 0x0005F2B0
		public void PrintLabel<T>(string label, T value)
		{
			this.PrintLn(string.Format("{0}: {1}", label, value.ToString()));
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00060ED0 File Offset: 0x0005F2D0
		private void Test()
		{
			this.print("Testing some shit.");
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00060EDD File Offset: 0x0005F2DD
		public void PrintLn(string text)
		{
			this.print(text);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00060EE6 File Offset: 0x0005F2E6
		public void PrintLn(string format, params object[] parameters)
		{
			this.PrintLn(string.Format(format, parameters));
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00060EF5 File Offset: 0x0005F2F5
		private void print(string text)
		{
			Console.Log(text);
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00060EFD File Offset: 0x0005F2FD
		public void ExecuteCommand(string command)
		{
			Console.ExecuteCommand(command);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00060F05 File Offset: 0x0005F305
		public void ExecuteCommand(string command, bool echo)
		{
			this.ExecuteCommand(command);
			if (echo)
			{
				this.PrintLn(command);
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00060F1C File Offset: 0x0005F31C
		private string[] getAutoExecCommands(string pathToFile)
		{
			TextAsset textAsset = Resources.Load(pathToFile) as TextAsset;
			if (textAsset == null)
			{
				return new string[0];
			}
			List<string> list = new List<string>();
			string[] result;
			using (StringReader stringReader = new StringReader(textAsset.text))
			{
				int num = 1000;
				string text;
				while ((text = stringReader.ReadLine()) != string.Empty && num > 0)
				{
					num--;
					if (text == null)
					{
						break;
					}
					string text2 = text.Trim();
					if (!text2.StartsWith("#"))
					{
						list.Add(text);
					}
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x04000A88 RID: 2696
		private HashSet<string> consoleVariables = new HashSet<string>();
	}
}
