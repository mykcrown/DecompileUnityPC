// Decompile from assembly: Assembly-CSharp.dll

using SickDev.CommandSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DevConsole
{
	public class WavedashConsole : Console, IDevConsole
	{
		private sealed class _AddPlayerCommand_c__AnonStorey0
		{
			internal Action<PlayerNum> callback;
		}

		private sealed class _AddPlayerCommand_c__AnonStorey1
		{
			internal PlayerNum copy;

			internal WavedashConsole._AddPlayerCommand_c__AnonStorey0 __f__ref_0;

			internal void __m__0()
			{
				this.__f__ref_0.callback(this.copy);
			}
		}

		private sealed class _AddPlayerCommand_c__AnonStorey2<T>
		{
			internal Action<PlayerNum, T> callback;
		}

		private sealed class _AddPlayerCommand_c__AnonStorey3<T>
		{
			internal PlayerNum copy;

			internal WavedashConsole._AddPlayerCommand_c__AnonStorey2<T> __f__ref_2;

			internal void __m__0(T value)
			{
				this.__f__ref_2.callback(this.copy, value);
			}
		}

		private sealed class _AddConsoleVariable_c__AnonStorey4<T>
		{
			internal string label;

			internal string context;

			internal string name;

			internal Action<T> setCallback;

			internal Func<T> getCallback;

			internal WavedashConsole _this;

			internal void __m__0(string value)
			{
				if (value.Equals("?"))
				{
					this._this.PrintLn(string.Format("{0} can be set to:", this.label));
					T[] values = EnumUtil.GetValues<T>();
					for (int i = 0; i < values.Length; i++)
					{
						T t = values[i];
						this._this.PrintLn(string.Format(" {0}", t.ToString()));
					}
					return;
				}
				T t2 = default(T);
				if (!EnumUtil.TryParse<T>(value, ref t2, true, true, true))
				{
					this._this.PrintLn("Unable to parse {0} as value of Enum Type {1}.  Use '{2}.{3} ?' to view a list of available values.", new object[]
					{
						value,
						typeof(T).ToString(),
						this.context,
						this.name
					});
					return;
				}
				this.setCallback(t2);
				this._this.PrintLabel<T>(this.label, t2);
			}

			internal void __m__1()
			{
				this._this.PrintLn(string.Format("'{0}.{1} ?' to view list of available values\n{2}: {3}.", new object[]
				{
					this.context,
					this.name,
					this.label,
					this.getCallback()
				}));
			}

			internal void __m__2(string value)
			{
				T t;
				if (value.EqualsIgnoreCase("!") || value.EqualsIgnoreCase("toggle"))
				{
					bool flag = (bool)((object)this.getCallback());
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
						this._this.PrintLn("Unable to parse {0} as value of String Type {1}.", new object[]
						{
							value,
							typeof(T).ToString()
						});
						return;
					}
					t = (T)((object)false);
				}
				this.setCallback(t);
				this._this.PrintLabel<T>(this.label, t);
			}

			internal void __m__3()
			{
				this._this.PrintLabel<T>(this.label, this.getCallback());
			}

			internal void __m__4(T value)
			{
				this.setCallback(value);
				this._this.PrintLabel<T>(this.label, value);
			}

			internal void __m__5()
			{
				this._this.PrintLabel<T>(this.label, this.getCallback());
			}
		}

		private sealed class _AddPlayerConsoleVariable_c__AnonStorey5<T>
		{
			internal Func<PlayerNum, T> getCallback;

			internal Action<PlayerNum, T> setCallback;
		}

		private sealed class _AddPlayerConsoleVariable_c__AnonStorey6<T>
		{
			internal PlayerNum copy;

			internal WavedashConsole._AddPlayerConsoleVariable_c__AnonStorey5<T> __f__ref_5;

			internal T __m__0()
			{
				return this.__f__ref_5.getCallback(this.copy);
			}

			internal void __m__1(T value)
			{
				this.__f__ref_5.setCallback(this.copy, value);
			}
		}

		private HashSet<string> consoleVariables = new HashSet<string>();

		[Inject]
		public UIManager uiManager
		{
			get;
			set;
		}

		protected bool AllowConsole
		{
			get
			{
				return this.uiManager == null || !this.uiManager.IsTextEntryMode;
			}
		}

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

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

		private void executeAutoExec(string[] autoCommands)
		{
			for (int i = 0; i < autoCommands.Length; i++)
			{
				string command = autoCommands[i];
				this.ExecuteCommand(command, true);
			}
		}

		public void AddAdminCommand(Action callback, string text, string help = null)
		{
			Console.AddCommand(new ActionCommand(callback, "admin." + text, help));
		}

		public void AddAdminCommand<T>(Action<T> callback, string text, string help = null)
		{
			Console.AddCommand(new ActionCommand<T>(callback, "admin." + text, help));
		}

		public void AddCommand(Action callback, string category, string text, string help = null)
		{
			if (string.Compare(category, "admin", true) == 0)
			{
				this.AddAdminCommand(callback, text, help);
				return;
			}
			Console.AddCommand(new ActionCommand(callback, category + "." + text, help));
		}

		public void AddCommand<T>(Action<T> callback, string category, string text, string help = null)
		{
			if (string.Compare(category, "admin", true) == 0)
			{
				this.AddAdminCommand<T>(callback, text, help);
				return;
			}
			Console.AddCommand(new ActionCommand<T>(callback, category + "." + text, help));
		}

		public void AddPlayerCommand(Action<PlayerNum> callback, string text, string help = null)
		{
			WavedashConsole._AddPlayerCommand_c__AnonStorey0 _AddPlayerCommand_c__AnonStorey = new WavedashConsole._AddPlayerCommand_c__AnonStorey0();
			_AddPlayerCommand_c__AnonStorey.callback = callback;
			for (PlayerNum playerNum = PlayerNum.Player1; playerNum < PlayerNum.Player6; playerNum++)
			{
				WavedashConsole._AddPlayerCommand_c__AnonStorey1 _AddPlayerCommand_c__AnonStorey2 = new WavedashConsole._AddPlayerCommand_c__AnonStorey1();
				_AddPlayerCommand_c__AnonStorey2.__f__ref_0 = _AddPlayerCommand_c__AnonStorey;
				_AddPlayerCommand_c__AnonStorey2.copy = playerNum;
				this.AddCommand(new Action(_AddPlayerCommand_c__AnonStorey2.__m__0), playerNum.ToString().ToLower(), text, help);
			}
		}

		public void AddPlayerCommand<T>(Action<PlayerNum, T> callback, string text, string help = null)
		{
			WavedashConsole._AddPlayerCommand_c__AnonStorey2<T> _AddPlayerCommand_c__AnonStorey = new WavedashConsole._AddPlayerCommand_c__AnonStorey2<T>();
			_AddPlayerCommand_c__AnonStorey.callback = callback;
			for (PlayerNum playerNum = PlayerNum.Player1; playerNum < PlayerNum.Player6; playerNum++)
			{
				WavedashConsole._AddPlayerCommand_c__AnonStorey3<T> _AddPlayerCommand_c__AnonStorey2 = new WavedashConsole._AddPlayerCommand_c__AnonStorey3<T>();
				_AddPlayerCommand_c__AnonStorey2.__f__ref_2 = _AddPlayerCommand_c__AnonStorey;
				_AddPlayerCommand_c__AnonStorey2.copy = playerNum;
				this.AddCommand<T>(new Action<T>(_AddPlayerCommand_c__AnonStorey2.__m__0), playerNum.ToString().ToLower(), text, help);
			}
		}

		public void AddConsoleVariable<T>(string context, string name, string label, string help, Func<T> getCallback, Action<T> setCallback)
		{
			WavedashConsole._AddConsoleVariable_c__AnonStorey4<T> _AddConsoleVariable_c__AnonStorey = new WavedashConsole._AddConsoleVariable_c__AnonStorey4<T>();
			_AddConsoleVariable_c__AnonStorey.label = label;
			_AddConsoleVariable_c__AnonStorey.context = context;
			_AddConsoleVariable_c__AnonStorey.name = name;
			_AddConsoleVariable_c__AnonStorey.setCallback = setCallback;
			_AddConsoleVariable_c__AnonStorey.getCallback = getCallback;
			_AddConsoleVariable_c__AnonStorey._this = this;
			string item = string.Format("{0}.{1}", _AddConsoleVariable_c__AnonStorey.context, _AddConsoleVariable_c__AnonStorey.name);
			if (this.consoleVariables.Contains(item))
			{
				return;
			}
			this.consoleVariables.Add(item);
			if (typeof(T).IsEnum)
			{
				this.AddCommand<string>(new Action<string>(_AddConsoleVariable_c__AnonStorey.__m__0), _AddConsoleVariable_c__AnonStorey.context, _AddConsoleVariable_c__AnonStorey.name, help);
				this.AddCommand(new Action(_AddConsoleVariable_c__AnonStorey.__m__1), _AddConsoleVariable_c__AnonStorey.context, _AddConsoleVariable_c__AnonStorey.name, help);
			}
			else if (typeof(T) == typeof(bool))
			{
				this.AddCommand<string>(new Action<string>(_AddConsoleVariable_c__AnonStorey.__m__2), _AddConsoleVariable_c__AnonStorey.context, _AddConsoleVariable_c__AnonStorey.name, help);
				this.AddCommand(new Action(_AddConsoleVariable_c__AnonStorey.__m__3), _AddConsoleVariable_c__AnonStorey.context, _AddConsoleVariable_c__AnonStorey.name, help);
			}
			else
			{
				this.AddCommand<T>(new Action<T>(_AddConsoleVariable_c__AnonStorey.__m__4), _AddConsoleVariable_c__AnonStorey.context, _AddConsoleVariable_c__AnonStorey.name, help);
				this.AddCommand(new Action(_AddConsoleVariable_c__AnonStorey.__m__5), _AddConsoleVariable_c__AnonStorey.context, _AddConsoleVariable_c__AnonStorey.name, help);
			}
		}

		public void AddPlayerConsoleVariable<T>(string name, string label, string help, Func<PlayerNum, T> getCallback, Action<PlayerNum, T> setCallback)
		{
			WavedashConsole._AddPlayerConsoleVariable_c__AnonStorey5<T> _AddPlayerConsoleVariable_c__AnonStorey = new WavedashConsole._AddPlayerConsoleVariable_c__AnonStorey5<T>();
			_AddPlayerConsoleVariable_c__AnonStorey.getCallback = getCallback;
			_AddPlayerConsoleVariable_c__AnonStorey.setCallback = setCallback;
			for (PlayerNum playerNum = PlayerNum.Player1; playerNum < PlayerNum.Player6; playerNum++)
			{
				WavedashConsole._AddPlayerConsoleVariable_c__AnonStorey6<T> _AddPlayerConsoleVariable_c__AnonStorey2 = new WavedashConsole._AddPlayerConsoleVariable_c__AnonStorey6<T>();
				_AddPlayerConsoleVariable_c__AnonStorey2.__f__ref_5 = _AddPlayerConsoleVariable_c__AnonStorey;
				_AddPlayerConsoleVariable_c__AnonStorey2.copy = playerNum;
				string text = playerNum.ToString();
				this.AddConsoleVariable<T>(text.ToLower(), name, string.Format("{0} {1}", text, label), string.Format("{0} ({1})", help, text), new Func<T>(_AddPlayerConsoleVariable_c__AnonStorey2.__m__0), new Action<T>(_AddPlayerConsoleVariable_c__AnonStorey2.__m__1));
			}
		}

		public void PrintLabel<T>(string label, T value)
		{
			this.PrintLn(string.Format("{0}: {1}", label, value.ToString()));
		}

		private void Test()
		{
			this.print("Testing some shit.");
		}

		public void PrintLn(string text)
		{
			this.print(text);
		}

		public void PrintLn(string format, params object[] parameters)
		{
			this.PrintLn(string.Format(format, parameters));
		}

		private void print(string text)
		{
			Console.Log(text);
		}

		public new void ExecuteCommand(string command)
		{
			Console.ExecuteCommand(command);
		}

		public void ExecuteCommand(string command, bool echo)
		{
			this.ExecuteCommand(command);
			if (echo)
			{
				this.PrintLn(command);
			}
		}

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
	}
}
