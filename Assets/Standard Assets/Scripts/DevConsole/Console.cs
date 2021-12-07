// Decompile from assembly: Assembly-CSharp-firstpass.dll

using SickDev.CommandSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace DevConsole
{
	[Serializable]
	public class Console : MonoBehaviour, ISerializationCallbackReceiver
	{
		private sealed class _FadeInOut_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal float _duration___0;

			internal float _time___0;

			internal bool open;

			internal Console _this;

			internal object _current;

			internal bool _disposing;

			internal int _PC;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public _FadeInOut_c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					if (this._this.opening)
					{
						return false;
					}
					this._this.opening = true;
					this._this.maxConsoleHeight = (float)(Screen.height / 3);
					this._this.numLinesThreshold = this._this.maxConsoleHeight / this._this.lineHeight;
					this._this.closed = false;
					this._duration___0 = this._this.extraSettings.curve[this._this.extraSettings.curve.length - 1].time;
					this._time___0 = 0f;
					break;
				case 1u:
					this._time___0 = Mathf.Clamp(this._time___0 + Time.unscaledDeltaTime, 0f, this._duration___0);
					if (this._time___0 >= this._duration___0)
					{
						this._this.currentConsoleHeight = this._this.maxConsoleHeight * this._this.extraSettings.curve.Evaluate((!this.open) ? (this._duration___0 - this._time___0) : this._time___0);
						this._this.closed = !this.open;
						if (this._this.closed)
						{
							this._this.inputText = string.Empty;
						}
						this._this.opening = false;
						this._PC = -1;
						return false;
					}
					break;
				default:
					return false;
				}
				this._this.currentConsoleHeight = this._this.maxConsoleHeight * this._this.extraSettings.curve.Evaluate((!this.open) ? (this._duration___0 - this._time___0) : this._time___0);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}

			public void Dispose()
			{
				this._disposing = true;
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		[SerializeField]
		private bool dontDestroyOnLoad;

		[SerializeField]
		private KeyCode consoleKey = KeyCode.Backslash;

		[Range(8f, 20f)]
		public int fontSize;

		private const int TEXT_AREA_OFFSET = 7;

		private bool helpEnabled = true;

		private int numHelpCommandsToShow = 5;

		private float helpWindowMinWidth = 200f;

		private const int WARNING_THRESHOLD = 15000;

		private const int DANGER_THRESHOLD = 16000;

		private const int AUTOCLEAR_THRESHOLD = 18000;

		private List<CommandBase> candidates = new List<CommandBase>();

		private int selectedCandidate;

		private List<string> history = new List<string>();

		private int selectedHistory;

		private List<KeyValuePair<string, string>> buffer = new List<KeyValuePair<string, string>>();

		private CommandsManager _manager;

		private static Console _singleton;

		private bool opening;

		private bool closed = true;

		private bool showHelp = true;

		private bool inHistory;

		private float numLinesThreshold;

		private float maxConsoleHeight;

		private float currentConsoleHeight;

		private Vector2 consoleScroll = Vector2.zero;

		private Vector2 helpWindowScroll = Vector2.zero;

		[HideInInspector, SerializeField]
		private string serializedConsoleText = string.Empty;

		private StringBuilder consoleText = new StringBuilder();

		private string inputText = string.Empty;

		private string lastText = string.Empty;

		private int numLines;

		private float lineHeight;

		[SerializeField]
		private Settings extraSettings;

		private static CommandsManager.OnExceptionThrown __f__mg_cache0;

		private static CommandsManager.OnMessage __f__mg_cache1;

		private CommandsManager manager
		{
			get
			{
				if (this._manager == null)
				{
					this._manager = new CommandsManager();
					this.manager.AddAssemblyWithCommands("Assembly-CSharp.dll");
					this.manager.AddAssemblyWithCommands("Assembly-CSharp-firstpass.dll");
					this.manager.Load();
				}
				return this._manager;
			}
		}

		private static Console Singleton
		{
			get
			{
				if (Console._singleton == null)
				{
					Console._singleton = UnityEngine.Object.FindObjectOfType<Console>();
				}
				return Console._singleton;
			}
		}

		public static bool isOpen
		{
			get
			{
				return !Console.Singleton.closed;
			}
		}

		static Console()
		{
			if (Console.__f__mg_cache0 == null)
			{
				Console.__f__mg_cache0 = new CommandsManager.OnExceptionThrown(UnityEngine.Debug.LogException);
			}
			CommandsManager.onExceptionThrown += Console.__f__mg_cache0;
			if (Console.__f__mg_cache1 == null)
			{
				Console.__f__mg_cache1 = new CommandsManager.OnMessage(UnityEngine.Debug.Log);
			}
			CommandsManager.onMessage += Console.__f__mg_cache1;
		}

		private void Awake()
		{
			if (Console.Singleton != this)
			{
				UnityEngine.Debug.LogWarning("There can only be one Console per project");
				UnityEngine.Object.Destroy(this);
				return;
			}
			if (this.dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			if (this.extraSettings.showDebugLog)
			{
				Application.logMessageReceived += new Application.LogCallback(this.LogCallback);
			}
		}

		public void OnBeforeSerialize()
		{
			this.serializedConsoleText = this.consoleText.ToString();
		}

		public void OnAfterDeserialize()
		{
			this.consoleText.Append(this.serializedConsoleText);
		}

		private void OnGUI()
		{
			GUISkin skin = GUI.skin;
			if (this.extraSettings.skin != null)
			{
				GUI.skin = this.extraSettings.skin;
			}
			Event current = Event.current;
			GUI.skin.textArea.richText = true;
			if (this.extraSettings.font != null)
			{
				GUI.skin.font = this.extraSettings.font;
			}
			GUIStyle arg_8F_0 = GUI.skin.textArea;
			int num = this.fontSize;
			GUI.skin.textField.fontSize = num;
			arg_8F_0.fontSize = num;
			if (current.type == EventType.KeyDown && current.keyCode == this.consoleKey && this.consoleKey != KeyCode.None && (current.control || current.alt || current.shift))
			{
				GUIUtility.keyboardControl = 0;
				base.StartCoroutine(this.FadeInOut(this.closed));
			}
			this.lineHeight = GUI.skin.textArea.lineHeight;
			bool flag = this.currentConsoleHeight != this.maxConsoleHeight && this.currentConsoleHeight != 0f;
			float num2 = this.lineHeight * (float)this.numLines;
			float height = (num2 <= this.currentConsoleHeight) ? this.currentConsoleHeight : num2;
			if (!this.closed)
			{
				for (int i = 0; i < this.buffer.Count; i++)
				{
					this.BasePrintOnGUI(this.buffer[i].Key, this.buffer[i].Value);
				}
				this.buffer.Clear();
				if (!flag)
				{
					GUI.FocusControl("TextField");
				}
				if (current.type == EventType.KeyDown)
				{
					if (!string.IsNullOrEmpty(this.inputText))
					{
						KeyCode keyCode = current.keyCode;
						if (keyCode != KeyCode.Tab)
						{
							if (keyCode != KeyCode.Return)
							{
								if (keyCode != KeyCode.Escape)
								{
									if (keyCode == KeyCode.F1)
									{
										this.showHelp = true;
									}
								}
								else
								{
									this.showHelp = false;
									this.candidates.Clear();
								}
							}
							else if (this.candidates.Count == 0)
							{
								this.PrintInput(this.inputText);
							}
							else
							{
								this.SelectCurrentCandidate();
							}
						}
						else if (this.candidates.Count != 0)
						{
							this.SelectCurrentCandidate();
						}
					}
					KeyCode keyCode2 = current.keyCode;
					if (keyCode2 != KeyCode.UpArrow)
					{
						if (keyCode2 == KeyCode.DownArrow)
						{
							if ((this.inHistory || this.inputText == string.Empty) && this.history.Count != 0)
							{
								this.selectedHistory = Mathf.Clamp(this.selectedHistory - ((!this.inHistory) ? 0 : 1), 0, this.history.Count - 1);
								this.inputText = this.history[this.selectedHistory];
								this.showHelp = false;
								this.inHistory = true;
								this.lastText = this.inputText;
							}
							else if (this.inputText != string.Empty && !this.inHistory)
							{
								this.selectedCandidate = Mathf.Clamp(++this.selectedCandidate, 0, this.candidates.Count - 1);
								if ((float)this.selectedCandidate * this.lineHeight > this.helpWindowScroll.y + this.lineHeight * (float)(this.numHelpCommandsToShow - 2) || (float)this.selectedCandidate * this.lineHeight < this.helpWindowScroll.y)
								{
									this.helpWindowScroll = new Vector2(0f, (float)this.selectedCandidate * this.lineHeight - (float)(this.numHelpCommandsToShow - 2) * this.lineHeight);
								}
							}
							this.SetCursorPos(this.inputText, this.inputText.Length);
						}
					}
					else
					{
						if ((this.inHistory || this.inputText == string.Empty) && this.history.Count != 0)
						{
							this.selectedHistory = Mathf.Clamp(this.selectedHistory + ((!this.inHistory) ? 0 : 1), 0, this.history.Count - 1);
							this.inputText = this.history[this.selectedHistory];
							this.showHelp = false;
							this.inHistory = true;
							this.lastText = this.inputText;
						}
						else if (this.inputText != string.Empty && !this.inHistory)
						{
							this.selectedCandidate = Mathf.Clamp(--this.selectedCandidate, 0, this.candidates.Count - 1);
							if ((float)this.selectedCandidate * this.lineHeight <= this.helpWindowScroll.y || (float)this.selectedCandidate * this.lineHeight > this.helpWindowScroll.y + this.lineHeight * (float)(this.numHelpCommandsToShow - 1))
							{
								this.helpWindowScroll = new Vector2(0f, (float)this.selectedCandidate * this.lineHeight - 1f * this.lineHeight);
							}
						}
						this.SetCursorPos(this.inputText, this.inputText.Length);
					}
				}
				if (this.lastText != this.inputText)
				{
					this.inHistory = false;
					this.lastText = string.Empty;
				}
				GUI.Box(new Rect(0f, 0f, (float)Screen.width, this.currentConsoleHeight), new GUIContent());
				GUI.SetNextControlName("TextField");
				GUI.enabled = !this.opening;
				this.inputText = GUI.TextField(new Rect(0f, this.currentConsoleHeight, (float)Screen.width, 25f), this.inputText);
				GUI.enabled = true;
				GUI.skin.textArea.normal.background = null;
				GUI.skin.textArea.hover.background = null;
				this.consoleScroll = GUI.BeginScrollView(new Rect(0f, 0f, (float)Screen.width, this.currentConsoleHeight), this.consoleScroll, new Rect(0f, 0f, (float)(Screen.width - 20), height));
				GUI.TextArea(new Rect(0f, -5f + this.currentConsoleHeight - ((this.numLines != 0) ? num2 : this.lineHeight) + (((float)this.numLines < this.numLinesThreshold - 1f) ? 0f : (this.lineHeight * ((float)this.numLines - this.numLinesThreshold))), (float)Screen.width, 7f + ((this.numLines != 0) ? num2 : this.lineHeight)), this.consoleText.ToString());
				GUI.EndScrollView();
				if (this.inputText == string.Empty)
				{
					this.showHelp = true;
				}
			}
			if (this.showHelp && this.helpEnabled && this.inputText.Trim() != string.Empty)
			{
				this.ShowHelp();
				if (this.candidates.Count != 0)
				{
					GUI.skin.textArea.normal.background = GUI.skin.textField.normal.background;
					GUI.skin.textArea.hover.background = GUI.skin.textField.hover.background;
					StringBuilder stringBuilder = new StringBuilder();
					float num3 = this.helpWindowMinWidth;
					for (int j = 0; j < this.candidates.Count; j++)
					{
						string text = (this.candidates[this.selectedCandidate] != this.candidates[j]) ? this.candidates[j].signature.raw : ("<color=yellow>" + this.candidates[j].signature.raw + "</color>");
						float x = GUI.skin.textArea.CalcSize(new GUIContent(text)).x;
						num3 = Mathf.Max(num3, x);
						stringBuilder.Append(text + '\n');
					}
					if (this.candidates.Count > this.numHelpCommandsToShow)
					{
						this.helpWindowScroll = GUI.BeginScrollView(new Rect(0f, this.currentConsoleHeight - (float)this.numHelpCommandsToShow * this.lineHeight - 7f, num3, 5f + this.lineHeight * (float)this.numHelpCommandsToShow), this.helpWindowScroll, new Rect(0f, 0f, num3 - 20f, 7f + (float)this.candidates.Count * this.lineHeight));
						GUI.TextArea(new Rect(0f, 0f, num3, 7f + (float)this.candidates.Count * this.lineHeight), stringBuilder.ToString());
						GUI.EndScrollView();
					}
					else
					{
						GUI.TextArea(new Rect(0f, this.currentConsoleHeight - 7f - ((this.candidates.Count <= this.numHelpCommandsToShow) ? (this.lineHeight * (float)this.candidates.Count) : ((float)this.numHelpCommandsToShow * this.lineHeight)), num3, ((this.candidates.Count <= this.numHelpCommandsToShow) ? (this.lineHeight * (float)this.candidates.Count) : ((float)this.numHelpCommandsToShow * this.lineHeight)) + 7f), stringBuilder.ToString());
					}
				}
			}
			GUI.skin = skin;
		}

		private void SelectCurrentCandidate()
		{
			this.inputText = this.candidates[this.selectedCandidate].name;
			this.showHelp = false;
			this.candidates.Clear();
			this.SetCursorPos(this.inputText, this.inputText.Length);
		}

		public static void Open()
		{
			if (Console.isOpen)
			{
				return;
			}
			GUIUtility.keyboardControl = 0;
			Console.Singleton.StartCoroutine(Console.Singleton.FadeInOut(true));
		}

		public static void Close()
		{
			if (!Console.isOpen)
			{
				return;
			}
			GUIUtility.keyboardControl = 0;
			Console.Singleton.StartCoroutine(Console.Singleton.FadeInOut(false));
		}

		private IEnumerator FadeInOut(bool open)
		{
			Console._FadeInOut_c__Iterator0 _FadeInOut_c__Iterator = new Console._FadeInOut_c__Iterator0();
			_FadeInOut_c__Iterator.open = open;
			_FadeInOut_c__Iterator._this = this;
			return _FadeInOut_c__Iterator;
		}

		private void ShowHelp()
		{
			CommandBase[] commands = this.manager.GetCommands();
			CommandBase commandBase = null;
			if (this.candidates.Count != 0 && this.selectedCandidate >= 0 && this.candidates.Count > this.selectedCandidate)
			{
				commandBase = this.candidates[this.selectedCandidate];
			}
			this.candidates.Clear();
			for (int i = 0; i < commands.Length; i++)
			{
				if (commands[i].name.ToUpper().StartsWith(this.inputText.ToUpper()))
				{
					this.candidates.Add(commands[i]);
				}
			}
			if (commandBase == null)
			{
				this.selectedCandidate = 0;
				return;
			}
			for (int j = 0; j < this.candidates.Count; j++)
			{
				if (this.candidates[j] == commandBase)
				{
					this.selectedCandidate = j;
					return;
				}
			}
			this.selectedCandidate = 0;
		}

		private void SetCursorPos(string text, int pos)
		{
			TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
			textEditor.text = text;
			textEditor.cursorIndex = pos;
			textEditor.selectIndex = pos;
			GUIUtility.ExitGUI();
		}

		public static string ColorToHex(Color color)
		{
			string text = "0123456789ABCDEF";
			int num = (int)(color.r * 255f);
			int num2 = (int)(color.g * 255f);
			int num3 = (int)(color.b * 255f);
			return string.Concat(new string[]
			{
				text[(int)Mathf.Floor((float)(num / 16))].ToString(),
				text[(int)Mathf.Round((float)(num % 16))].ToString(),
				text[(int)Mathf.Floor((float)(num2 / 16))].ToString(),
				text[(int)Mathf.Round((float)(num2 % 16))].ToString(),
				text[(int)Mathf.Floor((float)(num3 / 16))].ToString(),
				text[(int)Mathf.Round((float)(num3 % 16))].ToString()
			});
		}

		public static void Log(string text)
		{
			Console.Singleton.BasePrint(text);
		}

		public static void Log(object obj)
		{
			Console.Log(obj.ToString());
		}

		public static void LogInfo(string text)
		{
			Console.Singleton.BasePrint(text, Color.cyan);
		}

		public static void LogInfo(object obj)
		{
			Console.LogInfo(obj.ToString());
		}

		public static void LogWarning(string text)
		{
			Console.Singleton.BasePrint(text, Color.yellow);
		}

		public static void LogWarning(object obj)
		{
			Console.LogWarning(obj.ToString());
		}

		public static void LogError(string text)
		{
			Console.Singleton.BasePrint(text, Color.red);
		}

		public static void LogError(object obj)
		{
			Console.LogError(obj.ToString());
		}

		public static void Log(string text, string color)
		{
			Console.Singleton.BasePrint(text, color);
		}

		public static void Log(object obj, string color)
		{
			Console.Log(obj.ToString(), color);
		}

		public static void Log(string text, Color color)
		{
			Console.Singleton.BasePrint(text, color);
		}

		public static void Log(object obj, Color color)
		{
			Console.Log(obj.ToString(), color);
		}

		private void BasePrint(string text)
		{
			this.BasePrint(text, Console.ColorToHex(Color.white));
		}

		private void BasePrint(string text, Color color)
		{
			this.BasePrint(text, Console.ColorToHex(color));
		}

		private void BasePrint(string text, string color)
		{
			this.buffer.Add(new KeyValuePair<string, string>(text, color));
		}

		private void BasePrintOnGUI(string text, string color)
		{
			text = "> " + text;
			int num = 1;
			string value = (!this.extraSettings.showTimeStamp) ? string.Empty : ("[" + DateTime.Now.ToShortTimeString() + "]  ");
			StringBuilder stringBuilder = new StringBuilder(value);
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '\n')
				{
					num++;
					stringBuilder = new StringBuilder(value);
				}
				else
				{
					stringBuilder.Append(text[i]);
				}
				if (GUI.skin.textArea.CalcSize(new GUIContent(stringBuilder.ToString())).x > (float)(Screen.width - 20))
				{
					text = text.Insert(i, "\n");
					i--;
				}
			}
			text += '\n';
			this.numLines += num;
			if ((float)this.numLines >= this.numLinesThreshold - 1f)
			{
				this.consoleScroll = new Vector2(0f, this.consoleScroll.y + 2.14748365E+09f);
			}
			this.AddText(text, color);
			if (this.consoleText.Length >= 18000)
			{
				Console.Clear();
				this.AddText("Buffer cleared automatically\n", Console.ColorToHex(Color.yellow));
			}
			else if (this.consoleText.Length >= 16000)
			{
				this.AddText("Buffer size too large. You should clear the console\n", Console.ColorToHex(Color.red));
			}
			else if (this.consoleText.Length >= 15000)
			{
				this.AddText("Buffer size too large. You should clear the console\n", Console.ColorToHex(Color.yellow));
			}
		}

		private void AddText(string text, string color)
		{
			this.consoleText.Append(string.Format("{0}<color=#{1}>{2}</color>", (!this.extraSettings.showTimeStamp) ? string.Empty : ("[" + DateTime.Now.ToShortTimeString() + "]  "), color, text));
		}

		private void PrintInput(string input)
		{
			this.inputText = string.Empty;
			if ((this.history.Count == 0 || this.history[0] != input) && input.Trim() != string.Empty)
			{
				this.history.Insert(0, input);
			}
			this.selectedHistory = 0;
			this.BasePrint(input);
			this.ExecuteCommandInternal(input);
		}

		private void LogCallback(string log, string stackTrace, LogType type)
		{
			Color color;
			if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
			{
				color = Color.red;
			}
			else if (type == LogType.Warning)
			{
				color = Color.yellow;
			}
			else
			{
				color = Color.cyan;
			}
			this.BasePrint(log, color);
			this.BasePrint(stackTrace, color);
			int i = (int)GUI.skin.textArea.CalcSize(new GUIContent(log)).x;
			while (i >= Screen.width)
			{
				i -= Screen.width;
				this.numLines++;
			}
			this.numLines++;
		}

		public static void ExecuteCommand(string command)
		{
			Console.Singleton.ExecuteCommandInternal(command);
		}

		public static void ExecuteCommand(string command, string args)
		{
			Console.Singleton.ExecuteCommandInternal(command + " " + args);
		}

		private void ExecuteCommandInternal(string command)
		{
			CommandExecuter commandExecuter = this.manager.GetCommandExecuter(command);
			if (commandExecuter.IsValidCommand())
			{
				commandExecuter.Execute();
			}
			else
			{
				UnityEngine.Debug.LogError("The command '" + command + "' is not valid");
			}
		}

		public static void AddCommands(params CommandBase[] cs)
		{
			for (int i = 0; i < cs.Length; i++)
			{
				CommandBase c = cs[i];
				Console.AddCommand(c);
			}
		}

		public static void AddCommand(CommandBase c)
		{
			Console.Singleton.manager.Add(c);
		}

		[Obsolete]
		public static void RemoveCommand(string commandName)
		{
		}

		[Command(alias = "clear", description = "Clears the console")]
		private static void Clear()
		{
			Console.Singleton.consoleText = new StringBuilder();
			Console.Singleton.numLines = 0;
		}

		[Command(alias = "help", description = "Shows this message")]
		private static void HelpCommand()
		{
			CommandBase[] commands = Console.Singleton.manager.GetCommands();
			StringBuilder stringBuilder = new StringBuilder("List of commands\n");
			for (int i = 0; i < commands.Length; i++)
			{
				stringBuilder.Append(commands[i].name + ((commands[i].description != null) ? (" - " + commands[i].description) : string.Empty));
				if (i < commands.Length - 1)
				{
					stringBuilder.Append('\n');
				}
			}
			Console.LogInfo(stringBuilder.ToString());
		}

		[Command(alias = "changeKey", description = "Changes de key used to open/close the console. Type \"changeKeyHelp\" for extra help")]
		private static void ChangeKey(string key)
		{
			int num;
			if (!int.TryParse(key, out num))
			{
				try
				{
					Console.Singleton.consoleKey = (KeyCode)Enum.Parse(typeof(KeyCode), key, true);
					Console.Log("Change successful", Color.green);
				}
				catch
				{
					Console.LogError("The entered value is not a valid KeyCode value");
				}
			}
			else
			{
				string[] names = Enum.GetNames(typeof(KeyCode));
				if (num < 0)
				{
					if (num >= names.Length)
					{
						Console.LogError("The entered value is not a valid KeyCode value");
						return;
					}
				}
				try
				{
					Console.Singleton.consoleKey = (KeyCode)Enum.Parse(typeof(KeyCode), names[num], true);
					Console.Log("Change successful", Color.green);
				}
				catch
				{
					Console.LogError("The entered value is not a valid KeyCode value");
				}
			}
		}

		[Command(alias = "changeKeyHelp", description = "Lists all of the possible keys to use with the \"changeKey\" command")]
		private static void ChangeKeyHelp()
		{
			string[] names = Enum.GetNames(typeof(KeyCode));
			StringBuilder stringBuilder = new StringBuilder("\nSPECIAL KEYS 1: ");
			int num = 0;
			for (int i = 0; i < names.Length; i++)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				if (i == 22)
				{
					stringBuilder2.Append("\n\nNUMERIC KEYS: ");
					num = 0;
				}
				else if (i == 32)
				{
					stringBuilder2.Append("\n\nSPECIAL KEYS 2: ");
					num = 0;
				}
				else if (i == 45)
				{
					stringBuilder2.Append("\n\nALPHA KEYS: ");
					num = 0;
				}
				else if (i == 71)
				{
					stringBuilder2.Append("\n\nKEYPAD KEYS: ");
					num = 0;
				}
				else if (i == 89)
				{
					stringBuilder2.Append("\n\nSPECIAL KEYS 3: ");
					num = 0;
				}
				else if (i == 98)
				{
					stringBuilder2.Append("\n\nF KEYS: ");
					num = 0;
				}
				else if (i == 113)
				{
					stringBuilder2.Append("\n\nSPECIAL KEYS 4: ");
					num = 0;
				}
				else if (i == 134)
				{
					stringBuilder2.Append("\n\nMOUSE: ");
					num = 0;
				}
				else if (i == 141)
				{
					stringBuilder2.Append("\n\nJOYSTICK KEYS: ");
					num = 0;
				}
				stringBuilder2.Append(string.Format("{0}[{1}]{2}", names[i], i, (i == names.Length - 1) ? string.Empty : ","));
				num += stringBuilder2.Length;
				stringBuilder.Append(stringBuilder2);
				if (num >= 65)
				{
					stringBuilder.Append('\n');
					num = 0;
				}
			}
			Console.LogInfo("Command Info: " + stringBuilder.ToString());
		}

		[Command(alias = "showLog", description = "Whether or not to show Debug.Log and its variants")]
		private static void ShowLog(bool value)
		{
			if (value)
			{
				Application.logMessageReceived += new Application.LogCallback(Console.Singleton.LogCallback);
			}
			else
			{
				Application.logMessageReceived -= new Application.LogCallback(Console.Singleton.LogCallback);
			}
			Console.Log("Change successful", Color.green);
		}

		[Command(alias = "showTimeStamp", description = "Whether or not to show a time stamp on each message")]
		private static void ShowTimeStamp(bool value)
		{
			Console.Singleton.extraSettings.showTimeStamp = value;
			Console.Log("Change successful", Color.green);
		}

		[Command(alias = "setFontSize", description = "Set the font size used in the console")]
		private static void SetFontSize(int size)
		{
			Console.Singleton.fontSize = size;
			Console.Log("Change successful", Color.green);
		}
	}
}
