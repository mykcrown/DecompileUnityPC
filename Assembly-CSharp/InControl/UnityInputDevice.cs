using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020001CC RID: 460
	public class UnityInputDevice : InputDevice
	{
		// Token: 0x060007C8 RID: 1992 RVA: 0x000497D8 File Offset: 0x00047BD8
		public UnityInputDevice(UnityInputDeviceProfileBase deviceProfile) : this(deviceProfile, 0, string.Empty)
		{
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000497E7 File Offset: 0x00047BE7
		public UnityInputDevice(int joystickId, string joystickName) : this(null, joystickId, joystickName)
		{
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000497F4 File Offset: 0x00047BF4
		public UnityInputDevice(UnityInputDeviceProfileBase deviceProfile, int joystickId, string joystickName)
		{
			this.profile = deviceProfile;
			this.JoystickId = joystickId;
			if (joystickId != 0)
			{
				base.SortOrder = 100 + joystickId;
			}
			UnityInputDevice.SetupAnalogQueries();
			UnityInputDevice.SetupButtonQueries();
			base.AnalogSnapshot = null;
			if (this.IsKnown)
			{
				base.Name = this.profile.Name;
				base.Meta = this.profile.Meta;
				base.DeviceClass = this.profile.DeviceClass;
				base.DeviceStyle = this.profile.DeviceStyle;
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					if (Utility.TargetIsAlias(inputControlMapping.Target))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Cannot map control \"",
							inputControlMapping.Handle,
							"\" as InputControlType.",
							inputControlMapping.Target,
							" in profile \"",
							deviceProfile.Name,
							"\" because this target is reserved as an alias. The mapping will be ignored."
						}));
					}
					else
					{
						InputControl inputControl = base.AddControl(inputControlMapping.Target, inputControlMapping.Handle);
						inputControl.Sensitivity = Mathf.Min(this.profile.Sensitivity, inputControlMapping.Sensitivity);
						inputControl.LowerDeadZone = Mathf.Max(this.profile.LowerDeadZone, inputControlMapping.LowerDeadZone);
						inputControl.UpperDeadZone = Mathf.Min(this.profile.UpperDeadZone, inputControlMapping.UpperDeadZone);
						inputControl.Raw = inputControlMapping.Raw;
						inputControl.Passive = inputControlMapping.Passive;
					}
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					if (Utility.TargetIsAlias(inputControlMapping2.Target))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Cannot map control \"",
							inputControlMapping2.Handle,
							"\" as InputControlType.",
							inputControlMapping2.Target,
							" in profile \"",
							deviceProfile.Name,
							"\" because this target is reserved as an alias. The mapping will be ignored."
						}));
					}
					else
					{
						InputControl inputControl2 = base.AddControl(inputControlMapping2.Target, inputControlMapping2.Handle);
						inputControl2.Passive = inputControlMapping2.Passive;
					}
				}
			}
			else
			{
				base.Name = "Unknown Device";
				base.Meta = "\"" + joystickName + "\"";
				for (int k = 0; k < this.NumUnknownButtons; k++)
				{
					base.AddControl(InputControlType.Button0 + k, "Button " + k);
				}
				for (int l = 0; l < this.NumUnknownAnalogs; l++)
				{
					base.AddControl(InputControlType.Analog0 + l, "Analog " + l, 0.2f, 0.9f);
				}
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00049AFB File Offset: 0x00047EFB
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x00049B03 File Offset: 0x00047F03
		internal int JoystickId { get; private set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00049B0C File Offset: 0x00047F0C
		public override float DefaultLeftStickLowerDeadZone
		{
			get
			{
				return this.profile.LowerDeadZone;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x00049B19 File Offset: 0x00047F19
		public override float DefaultRightStickLowerDeadZone
		{
			get
			{
				return this.profile.LowerDeadZone;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00049B26 File Offset: 0x00047F26
		public override float DefaultLeftTriggerDeadZone
		{
			get
			{
				return this.profile.LowerDeadZone;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x00049B33 File Offset: 0x00047F33
		public override float DefaultRightTriggerDeadZone
		{
			get
			{
				return this.profile.LowerDeadZone;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00049B40 File Offset: 0x00047F40
		protected override float MaxLeftStickUpperDeadZone
		{
			get
			{
				return this.profile.UpperDeadZone;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00049B4D File Offset: 0x00047F4D
		protected override float MaxRightStickUpperDeadZone
		{
			get
			{
				return this.profile.UpperDeadZone;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00049B5A File Offset: 0x00047F5A
		protected override float MaxTriggerUpperDeadZone
		{
			get
			{
				return this.profile.UpperDeadZone;
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00049B68 File Offset: 0x00047F68
		public override void Update(ulong updateTick, float deltaTime)
		{
			if (this.IsKnown)
			{
				int analogCount = this.profile.AnalogCount;
				for (int i = 0; i < analogCount; i++)
				{
					InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
					float value = inputControlMapping.Source.GetValue(this);
					InputControl control = base.GetControl(inputControlMapping.Target);
					if (!inputControlMapping.IgnoreInitialZeroValue || !control.IsOnZeroTick || !Utility.IsZero(value))
					{
						float value2 = inputControlMapping.MapValue(value);
						control.UpdateWithValue(value2, updateTick, deltaTime);
					}
				}
				int buttonCount = this.profile.ButtonCount;
				for (int j = 0; j < buttonCount; j++)
				{
					InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
					bool state = inputControlMapping2.Source.GetState(this);
					base.UpdateWithState(inputControlMapping2.Target, state, updateTick, deltaTime);
				}
			}
			else
			{
				for (int k = 0; k < this.NumUnknownButtons; k++)
				{
					base.UpdateWithState(InputControlType.Button0 + k, this.ReadRawButtonState(k), updateTick, deltaTime);
				}
				for (int l = 0; l < this.NumUnknownAnalogs; l++)
				{
					base.UpdateWithValue(InputControlType.Analog0 + l, this.ReadRawAnalogValue(l), updateTick, deltaTime);
				}
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00049CC0 File Offset: 0x000480C0
		private static void SetupAnalogQueries()
		{
			if (UnityInputDevice.analogQueries == null)
			{
				UnityInputDevice.analogQueries = new string[10, 20];
				for (int i = 1; i <= 10; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						UnityInputDevice.analogQueries[i - 1, j] = string.Concat(new object[]
						{
							"joystick ",
							i,
							" analog ",
							j
						});
					}
				}
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00049D48 File Offset: 0x00048148
		private static void SetupButtonQueries()
		{
			if (UnityInputDevice.buttonQueries == null)
			{
				UnityInputDevice.buttonQueries = new string[10, 20];
				for (int i = 1; i <= 10; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						UnityInputDevice.buttonQueries[i - 1, j] = string.Concat(new object[]
						{
							"joystick ",
							i,
							" button ",
							j
						});
					}
				}
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00049DCE File Offset: 0x000481CE
		private static string GetAnalogKey(int joystickId, int analogId)
		{
			return UnityInputDevice.analogQueries[joystickId - 1, analogId];
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00049DDE File Offset: 0x000481DE
		private static string GetButtonKey(int joystickId, int buttonId)
		{
			return UnityInputDevice.buttonQueries[joystickId - 1, buttonId];
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00049DF0 File Offset: 0x000481F0
		internal override bool ReadRawButtonState(int index)
		{
			if (index < 20)
			{
				string name = UnityInputDevice.buttonQueries[this.JoystickId - 1, index];
				return Input.GetKey(name);
			}
			return false;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00049E24 File Offset: 0x00048224
		internal override float ReadRawAnalogValue(int index)
		{
			if (index < 20)
			{
				string axisName = UnityInputDevice.analogQueries[this.JoystickId - 1, index];
				return Input.GetAxisRaw(axisName);
			}
			return 0f;
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00049E59 File Offset: 0x00048259
		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return this.profile == null || this.profile.IsSupportedOnThisPlatform;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00049E74 File Offset: 0x00048274
		public override bool IsKnown
		{
			get
			{
				return this.profile != null;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00049E82 File Offset: 0x00048282
		internal override int NumUnknownButtons
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00049E86 File Offset: 0x00048286
		internal override int NumUnknownAnalogs
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x0400058C RID: 1420
		private static string[,] analogQueries;

		// Token: 0x0400058D RID: 1421
		private static string[,] buttonQueries;

		// Token: 0x0400058E RID: 1422
		public const int MaxDevices = 10;

		// Token: 0x0400058F RID: 1423
		public const int MaxButtons = 20;

		// Token: 0x04000590 RID: 1424
		public const int MaxAnalogs = 20;

		// Token: 0x04000592 RID: 1426
		private UnityInputDeviceProfileBase profile;
	}
}
