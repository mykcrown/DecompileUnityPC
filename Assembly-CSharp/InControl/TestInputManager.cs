using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x020001EC RID: 492
	public class TestInputManager : MonoBehaviour
	{
		// Token: 0x060008CD RID: 2253 RVA: 0x0004CFA0 File Offset: 0x0004B3A0
		private void OnEnable()
		{
			this.isPaused = false;
			Time.timeScale = 1f;
			Logger.OnLogMessage += delegate(LogMessage logMessage)
			{
				this.logMessages.Add(logMessage);
			};
			InputManager.OnDeviceAttached += delegate(InputDevice inputDevice)
			{
				Debug.Log("Attached: " + inputDevice.Name);
			};
			InputManager.OnDeviceDetached += delegate(InputDevice inputDevice)
			{
				Debug.Log("Detached: " + inputDevice.Name);
			};
			InputManager.OnActiveDeviceChanged += delegate(InputDevice inputDevice)
			{
				Debug.Log("Active device changed to: " + inputDevice.Name);
			};
			InputManager.OnUpdate += this.HandleInputUpdate;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0004D048 File Offset: 0x0004B448
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

		// Token: 0x060008CF RID: 2255 RVA: 0x0004D0A0 File Offset: 0x0004B4A0
		private void Start()
		{
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0004D0A2 File Offset: 0x0004B4A2
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

		// Token: 0x060008D1 RID: 2257 RVA: 0x0004D0D4 File Offset: 0x0004B4D4
		private void CheckForPauseButton()
		{
			if (Input.GetKeyDown(KeyCode.P) || InputManager.CommandWasPressed)
			{
				Time.timeScale = ((!this.isPaused) ? 0f : 1f);
				this.isPaused = !this.isPaused;
			}
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0004D125 File Offset: 0x0004B525
		private void SetColor(Color color)
		{
			this.style.normal.textColor = color;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0004D138 File Offset: 0x0004B538
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
			foreach (InputDevice inputDevice in InputManager.Devices)
			{
				bool flag = InputManager.ActiveDevice == inputDevice;
				Color color = (!flag) ? Color.white : Color.yellow;
				num3 = 35;
				if (inputDevice.IsUnknown)
				{
					this.SetColor(Color.red);
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "Unknown Device", this.style);
				}
				else
				{
					this.SetColor(color);
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), inputDevice.Name, this.style);
				}
				num3 += num4;
				this.SetColor(color);
				if (inputDevice.IsUnknown)
				{
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), inputDevice.Meta, this.style);
					num3 += num4;
				}
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "Style: " + inputDevice.DeviceStyle, this.style);
				num3 += num4;
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "GUID: " + inputDevice.GUID, this.style);
				num3 += num4;
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "SortOrder: " + inputDevice.SortOrder, this.style);
				num3 += num4;
				GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), "LastChangeTick: " + inputDevice.LastChangeTick, this.style);
				num3 += num4;
				NativeInputDevice nativeInputDevice = inputDevice as NativeInputDevice;
				if (nativeInputDevice != null)
				{
					string text2 = string.Format("VID = 0x{0:x}, PID = 0x{1:x}, VER = 0x{2:x}", nativeInputDevice.Info.vendorID, nativeInputDevice.Info.productID, nativeInputDevice.Info.versionNumber);
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text2, this.style);
					num3 += num4;
				}
				num3 += num4;
				foreach (InputControl inputControl in inputDevice.Controls)
				{
					if (inputControl != null && !Utility.TargetIsAlias(inputControl.Target))
					{
						string arg;
						if (inputDevice.IsKnown)
						{
							arg = string.Format("{0} ({1})", inputControl.Target, inputControl.Handle);
						}
						else
						{
							arg = inputControl.Handle;
						}
						this.SetColor((!inputControl.State) ? color : Color.green);
						string text3 = string.Format("{0} {1}", arg, (!inputControl.State) ? string.Empty : ("= " + inputControl.Value));
						GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text3, this.style);
						num3 += num4;
					}
				}
				num3 += num4;
				color = ((!flag) ? Color.white : new Color(1f, 0.7f, 0.2f));
				if (inputDevice.IsKnown)
				{
					InputControl inputControl2 = inputDevice.Command;
					this.SetColor((!inputControl2.State) ? color : Color.green);
					string text4 = string.Format("{0} {1}", "Command", (!inputControl2.State) ? string.Empty : ("= " + inputControl2.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl2 = inputDevice.LeftStickX;
					this.SetColor((!inputControl2.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Left Stick X", (!inputControl2.State) ? string.Empty : ("= " + inputControl2.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl2 = inputDevice.LeftStickY;
					this.SetColor((!inputControl2.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Left Stick Y", (!inputControl2.State) ? string.Empty : ("= " + inputControl2.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					this.SetColor((!inputDevice.LeftStick.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Left Stick A", (!inputDevice.LeftStick.State) ? string.Empty : ("= " + inputDevice.LeftStick.Angle));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl2 = inputDevice.RightStickX;
					this.SetColor((!inputControl2.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Right Stick X", (!inputControl2.State) ? string.Empty : ("= " + inputControl2.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl2 = inputDevice.RightStickY;
					this.SetColor((!inputControl2.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Right Stick Y", (!inputControl2.State) ? string.Empty : ("= " + inputControl2.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					this.SetColor((!inputDevice.RightStick.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "Right Stick A", (!inputDevice.RightStick.State) ? string.Empty : ("= " + inputDevice.RightStick.Angle));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl2 = inputDevice.DPadX;
					this.SetColor((!inputControl2.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "DPad X", (!inputControl2.State) ? string.Empty : ("= " + inputControl2.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
					inputControl2 = inputDevice.DPadY;
					this.SetColor((!inputControl2.State) ? color : Color.green);
					text4 = string.Format("{0} {1}", "DPad Y", (!inputControl2.State) ? string.Empty : ("= " + inputControl2.Value));
					GUI.Label(new Rect((float)num2, (float)num3, (float)(num2 + num), (float)(num3 + 10)), text4, this.style);
					num3 += num4;
				}
				this.SetColor(Color.cyan);
				InputControl anyButton = inputDevice.AnyButton;
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
					foreach (string text5 in logMessage.text.Split(new char[]
					{
						'\n'
					}))
					{
						GUI.Label(new Rect((float)num2, (float)num3, (float)Screen.width, (float)(num3 + 10)), text5, this.style);
						num3 -= num4;
					}
				}
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0004DC10 File Offset: 0x0004C010
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

		// Token: 0x060008D5 RID: 2261 RVA: 0x0004DE4C File Offset: 0x0004C24C
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

		// Token: 0x04000645 RID: 1605
		public Font font;

		// Token: 0x04000646 RID: 1606
		private GUIStyle style = new GUIStyle();

		// Token: 0x04000647 RID: 1607
		private List<LogMessage> logMessages = new List<LogMessage>();

		// Token: 0x04000648 RID: 1608
		private bool isPaused;
	}
}
