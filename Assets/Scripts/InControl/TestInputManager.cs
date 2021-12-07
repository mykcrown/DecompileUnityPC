// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace InControl
{
	public class TestInputManager : MonoBehaviour
	{
		public Font font;

		private GUIStyle style = new GUIStyle();

		private List<LogMessage> logMessages = new List<LogMessage>();

		private bool isPaused;

		private static Action<InputDevice> __f__am_cache0;

		private static Action<InputDevice> __f__am_cache1;

		private static Action<InputDevice> __f__am_cache2;

		private void OnEnable()
		{
			this.isPaused = false;
			Time.timeScale = 1f;
			Logger.OnLogMessage += new Action<LogMessage>(this._OnEnable_m__0);
			if (TestInputManager.__f__am_cache0 == null)
			{
				TestInputManager.__f__am_cache0 = new Action<InputDevice>(TestInputManager._OnEnable_m__1);
			}
			InputManager.OnDeviceAttached += TestInputManager.__f__am_cache0;
			if (TestInputManager.__f__am_cache1 == null)
			{
				TestInputManager.__f__am_cache1 = new Action<InputDevice>(TestInputManager._OnEnable_m__2);
			}
			InputManager.OnDeviceDetached += TestInputManager.__f__am_cache1;
			if (TestInputManager.__f__am_cache2 == null)
			{
				TestInputManager.__f__am_cache2 = new Action<InputDevice>(TestInputManager._OnEnable_m__3);
			}
			InputManager.OnActiveDeviceChanged += TestInputManager.__f__am_cache2;
			InputManager.OnUpdate += new Action<ulong, float>(this.HandleInputUpdate);
		}

		private void HandleInputUpdate(ulong updateTick, float deltaTime)
		{
			this.CheckForPauseButton();
			int count = InputManager.Devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.Devices[i];
				inputDevice.Vibrate(inputDevice.LeftTrigger, inputDevice.RightTrigger);
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				Utility.LoadScene("TestInputManager");
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				InputManager.Enabled = !InputManager.Enabled;
			}
		}

		private void CheckForPauseButton()
		{
			if (Input.GetKeyDown(KeyCode.P) || InputManager.CommandWasPressed)
			{
				Time.timeScale = ((!this.isPaused) ? 0f : 1f);
				this.isPaused = !this.isPaused;
			}
		}

		private void SetColor(Color color)
		{
			this.style.normal.textColor = color;
		}

		private void OnGUI()
		{
			int num = Mathf.FloorToInt((float)(Screen.width / Mathf.Max(1, InputManager.Devices.Count)));
			int num2 = 10;
			int num3 = 10;
			int num4 = 15;
			GUI.skin.font = this.font;
			this.SetColor(Color.white);
			string text = "Devices:";
			text = text + " (Platform: " + InputManager.Platform + ")";
			text = text + " " + InputManager.ActiveDevice.Direction.Vector;
			if (this.isPaused)
			{
				this.SetColor(Color.red);
				text = "+++ PAUSED +++";
			}
			GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text, this.style);
			this.SetColor(Color.white);
			foreach (InputDevice current in InputManager.Devices)
			{
				bool flag = InputManager.ActiveDevice == current;
				Color color = (!flag) ? Color.white : Color.yellow;
				num3 = 35;
				if (current.IsUnknown)
				{
					this.SetColor(Color.red);
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "Unknown Device", this.style);
				}
				else
				{
					this.SetColor(color);
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), current.Name, this.style);
				}
				num3 += num4;
				this.SetColor(color);
				if (current.IsUnknown)
				{
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), current.Meta, this.style);
					num3 += num4;
				}
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "Style: " + current.DeviceStyle, this.style);
				num3 += num4;
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "GUID: " + current.GUID, this.style);
				num3 += num4;
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "SortOrder: " + current.SortOrder, this.style);
				num3 += num4;
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "LastChangeTick: " + current.LastChangeTick, this.style);
				num3 += num4;
				NativeInputDevice nativeInputDevice = current as NativeInputDevice;
				if (nativeInputDevice != null)
				{
					string text2 = string.Format("VID = 0x{0:x}, PID = 0x{1:x}, VER = 0x{2:x}", nativeInputDevice.Info.vendorID, nativeInputDevice.Info.productID, nativeInputDevice.Info.versionNumber);
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text2, this.style);
					num3 += num4;
				}
				num3 += num4;
				foreach (InputControl current2 in current.Controls)
				{
					if (current2 != null && !Utility.TargetIsAlias(current2.Target))
					{
						string arg;
						if (current.IsKnown)
						{
							arg = string.Format("{0} ({1})", current2.Target, current2.Handle);
						}
						else
						{
							arg = current2.Handle;
						}
						this.SetColor((!current2.State) ? color : Color.green);
						string text3 = string.Format("{0} {1}", arg, (!current2.State) ? string.Empty : ("= " + current2.Value));
						GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text3, this.style);
						num3 += num4;
					}
				}
				num3 += num4;
				color = ((!flag) ? Color.white : new Color(1f, 0.7f, 0.2f));
				if (current.IsKnown)
				{
					InputControl inputControl = current.Command;
					this.SetColor((!inputControl.State) ? color : Color.green);
					string text4 = string.Format("{0} {1}", "Command", (!inputControl.State) ? string.Empty : ("= " + inputControl.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl = current.LeftStickX;
					this.SetColor((!inputControl.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Left Stick X", (!inputControl.State) ? string.Empty : ("= " + inputControl.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl = current.LeftStickY;
					this.SetColor((!inputControl.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Left Stick Y", (!inputControl.State) ? string.Empty : ("= " + inputControl.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					this.SetColor((!current.LeftStick.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Left Stick A", (!current.LeftStick.State) ? string.Empty : ("= " + current.LeftStick.Angle));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl = current.RightStickX;
					this.SetColor((!inputControl.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Right Stick X", (!inputControl.State) ? string.Empty : ("= " + inputControl.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl = current.RightStickY;
					this.SetColor((!inputControl.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Right Stick Y", (!inputControl.State) ? string.Empty : ("= " + inputControl.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					this.SetColor((!current.RightStick.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Right Stick A", (!current.RightStick.State) ? string.Empty : ("= " + current.RightStick.Angle));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl = current.DPadX;
					this.SetColor((!inputControl.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "DPad X", (!inputControl.State) ? string.Empty : ("= " + inputControl.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl = current.DPadY;
					this.SetColor((!inputControl.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "DPad Y", (!inputControl.State) ? string.Empty : ("= " + inputControl.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
				}
				this.SetColor(Color.cyan);
				InputControl anyButton = current.AnyButton;
				if (anyButton)
				{
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "AnyButton = " + anyButton.Handle, this.style);
				}
				num2 += num;
			}
			Color[] array = new Color[]
			{
				Color.gray,
				Color.yellow,
				Color.white
			};
			this.SetColor(Color.white);
			num2 = 10;
			num3 = Screen.height - (10 + num4);
			for (int i = this.logMessages.Count - 1; i >= 0; i--)
			{
				LogMessage logMessage = this.logMessages[i];
				if (logMessage.type != LogMessageType.Info)
				{
					this.SetColor(array[(int)logMessage.type]);
					string[] array2 = logMessage.text.Split(new char[]
					{
						'\n'
					});
					for (int j = 0; j < array2.Length; j++)
					{
						string text5 = array2[j];
						GUI.Label(new Rect((float)num2, (float)num3, (float)Screen.width, (float)(num3 + 10)), text5, this.style);
						num3 -= num4;
					}
				}
			}
		}

		private void DrawUnityInputDebugger()
		{
			int num = 300;
			int num2 = Screen.width / 2;
			int num3 = 10;
			int num4 = 20;
			this.SetColor(Color.white);
			string[] joystickNames = Input.GetJoystickNames();
			int num5 = joystickNames.Length;
			for (int i = 0; i < num5; i++)
			{
				string text = joystickNames[i];
				int num6 = i + 1;
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), string.Concat(new object[]
				{
					"Joystick ",
					num6,
					": \"",
					text,
					"\""
				}), this.style);
				num3 += num4;
				string text2 = "Buttons: ";
				for (int j = 0; j < 20; j++)
				{
					string name = string.Concat(new object[]
					{
						"joystick ",
						num6,
						" button ",
						j
					});
					bool key = Input.GetKey(name);
					if (key)
					{
						string text3 = text2;
						text2 = string.Concat(new object[]
						{
							text3,
							"B",
							j,
							"  "
						});
					}
				}
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text2, this.style);
				num3 += num4;
				string text4 = "Analogs: ";
				for (int k = 0; k < 20; k++)
				{
					string axisName = string.Concat(new object[]
					{
						"joystick ",
						num6,
						" analog ",
						k
					});
					float axisRaw = Input.GetAxisRaw(axisName);
					if (Utility.AbsoluteIsOverThreshold(axisRaw, 0.2f))
					{
						string text3 = text4;
						text4 = string.Concat(new object[]
						{
							text3,
							"A",
							k,
							": ",
							axisRaw.ToString("0.00"),
							"  "
						});
					}
				}
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
				num3 += num4;
				num3 += 25;
			}
		}

		private void OnDrawGizmos()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			Vector2 vector = activeDevice.Direction.Vector;
			Gizmos.color = Color.blue;
			Vector2 vector2 = new Vector2(-3f, -1f);
			Vector2 v = vector2 + vector * 2f;
			Gizmos.DrawSphere(vector2, 0.1f);
			Gizmos.DrawLine(vector2, v);
			Gizmos.DrawSphere(v, 1f);
			Gizmos.color = Color.red;
			Vector2 vector3 = new Vector2(3f, -1f);
			Vector2 v2 = vector3 + activeDevice.RightStick.Vector * 2f;
			Gizmos.DrawSphere(vector3, 0.1f);
			Gizmos.DrawLine(vector3, v2);
			Gizmos.DrawSphere(v2, 1f);
		}

		private void _OnEnable_m__0(LogMessage logMessage)
		{
			this.logMessages.Add(logMessage);
		}

		private static void _OnEnable_m__1(InputDevice inputDevice)
		{
			UnityEngine.Debug.Log("Attached: " + inputDevice.Name);
		}

		private static void _OnEnable_m__2(InputDevice inputDevice)
		{
			UnityEngine.Debug.Log("Detached: " + inputDevice.Name);
		}

		private static void _OnEnable_m__3(InputDevice inputDevice)
		{
			UnityEngine.Debug.Log("Active device changed to: " + inputDevice.Name);
		}
	}
}
