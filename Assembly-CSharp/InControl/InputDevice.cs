using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000077 RID: 119
	public class InputDevice : IControllerInputDevice, IInputDevice
	{
		// Token: 0x06000432 RID: 1074 RVA: 0x000116F2 File Offset: 0x0000FAF2
		public InputDevice() : this(string.Empty)
		{
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000116FF File Offset: 0x0000FAFF
		public InputDevice(string name) : this(name, false)
		{
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001170C File Offset: 0x0000FB0C
		public InputDevice(string name, bool rawSticks)
		{
			this.Name = name;
			this.RawSticks = rawSticks;
			this.Meta = string.Empty;
			this.GUID = Guid.NewGuid();
			this.LastChangeTick = 0UL;
			this.SortOrder = int.MaxValue;
			this.DeviceClass = InputDeviceClass.Unknown;
			this.DeviceStyle = InputDeviceStyle.Unknown;
			this.Passive = false;
			this.ControlsByTarget = new InputControl[521];
			this.controls = new List<InputControl>(32);
			this.Controls = new ReadOnlyCollection<InputControl>(this.controls);
			this.RemoveAliasControls();
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001179F File Offset: 0x0000FB9F
		public void MangleDefaultSettings(InputSettingsData settings)
		{
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x000117A1 File Offset: 0x0000FBA1
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x000117A9 File Offset: 0x0000FBA9
		public string Name { get; protected set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x000117B2 File Offset: 0x0000FBB2
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x000117BA File Offset: 0x0000FBBA
		public string Meta { get; protected set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x000117C3 File Offset: 0x0000FBC3
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x000117CB File Offset: 0x0000FBCB
		public int SortOrder { get; protected set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x000117D4 File Offset: 0x0000FBD4
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x000117DC File Offset: 0x0000FBDC
		public InputDeviceClass DeviceClass { get; protected set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x000117E5 File Offset: 0x0000FBE5
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x000117ED File Offset: 0x0000FBED
		public InputDeviceStyle DeviceStyle { get; protected set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x000117F6 File Offset: 0x0000FBF6
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x000117FE File Offset: 0x0000FBFE
		public Guid GUID { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00011807 File Offset: 0x0000FC07
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0001180F File Offset: 0x0000FC0F
		public ulong LastChangeTick { get; private set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00011818 File Offset: 0x0000FC18
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00011820 File Offset: 0x0000FC20
		public bool IsAttached { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00011829 File Offset: 0x0000FC29
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x00011831 File Offset: 0x0000FC31
		private protected bool RawSticks { protected get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0001183A File Offset: 0x0000FC3A
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00011842 File Offset: 0x0000FC42
		public ReadOnlyCollection<InputControl> Controls { get; protected set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0001184B File Offset: 0x0000FC4B
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00011853 File Offset: 0x0000FC53
		private protected InputControl[] ControlsByTarget { protected get; private set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0001185C File Offset: 0x0000FC5C
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00011864 File Offset: 0x0000FC64
		public TwoAxisInputControl LeftStick { get; private set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x0001186D File Offset: 0x0000FC6D
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00011875 File Offset: 0x0000FC75
		public TwoAxisInputControl RightStick { get; private set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0001187E File Offset: 0x0000FC7E
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x00011886 File Offset: 0x0000FC86
		public TwoAxisInputControl DPad { get; private set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0001188F File Offset: 0x0000FC8F
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00011897 File Offset: 0x0000FC97
		protected InputDevice.AnalogSnapshotEntry[] AnalogSnapshot { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x000118A0 File Offset: 0x0000FCA0
		protected virtual float MaxLeftStickUpperDeadZone
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x000118A7 File Offset: 0x0000FCA7
		protected virtual float MaxRightStickUpperDeadZone
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x000118AE File Offset: 0x0000FCAE
		protected virtual float MaxTriggerUpperDeadZone
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x000118B5 File Offset: 0x0000FCB5
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x000118C4 File Offset: 0x0000FCC4
		public float LeftStickLowerDeadZone
		{
			get
			{
				return this.leftLowerDeadZone / this.MaxLeftStickUpperDeadZone;
			}
			set
			{
				this.leftLowerDeadZone = value * this.MaxLeftStickUpperDeadZone;
				this.GetControl(InputControlType.LeftStickX).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickY).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickLeft).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickRight).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickUp).LowerDeadZone = this.leftLowerDeadZone;
				this.GetControl(InputControlType.LeftStickDown).LowerDeadZone = this.leftLowerDeadZone;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00011953 File Offset: 0x0000FD53
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x00011964 File Offset: 0x0000FD64
		public float RightStickLowerDeadZone
		{
			get
			{
				return this.rightLowerDeadZone / this.MaxRightStickUpperDeadZone;
			}
			set
			{
				this.rightLowerDeadZone = value * this.MaxRightStickUpperDeadZone;
				this.GetControl(InputControlType.RightStickX).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickY).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickLeft).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickRight).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickUp).LowerDeadZone = this.rightLowerDeadZone;
				this.GetControl(InputControlType.RightStickDown).LowerDeadZone = this.rightLowerDeadZone;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x000119F4 File Offset: 0x0000FDF4
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x00011A03 File Offset: 0x0000FE03
		public float LeftTriggerDeadZone
		{
			get
			{
				return this.leftTriggerLowerDeadZone / this.MaxTriggerUpperDeadZone;
			}
			set
			{
				this.leftTriggerLowerDeadZone = value * this.MaxTriggerUpperDeadZone;
				this.GetControl(InputControlType.LeftTrigger).LowerDeadZone = this.leftTriggerLowerDeadZone;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00011A26 File Offset: 0x0000FE26
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x00011A35 File Offset: 0x0000FE35
		public float RightTriggerDeadZone
		{
			get
			{
				return this.rightTriggerLowerDeadZone / this.MaxTriggerUpperDeadZone;
			}
			set
			{
				this.rightTriggerLowerDeadZone = value * this.MaxTriggerUpperDeadZone;
				this.GetControl(InputControlType.RightTrigger).LowerDeadZone = this.rightTriggerLowerDeadZone;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00011A58 File Offset: 0x0000FE58
		public virtual float DefaultLeftStickLowerDeadZone
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00011A5F File Offset: 0x0000FE5F
		public virtual float DefaultRightStickLowerDeadZone
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00011A66 File Offset: 0x0000FE66
		public virtual float DefaultLeftTriggerDeadZone
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00011A6D File Offset: 0x0000FE6D
		public virtual float DefaultRightTriggerDeadZone
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00011A74 File Offset: 0x0000FE74
		internal void OnAttached()
		{
			this.IsAttached = true;
			this.AddAliasControls();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00011A83 File Offset: 0x0000FE83
		internal void OnDetached()
		{
			this.IsAttached = false;
			this.StopVibration();
			this.RemoveAliasControls();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00011A98 File Offset: 0x0000FE98
		private void AddAliasControls()
		{
			this.RemoveAliasControls();
			if (this.IsKnown)
			{
				this.LeftStick = new TwoAxisInputControl();
				this.RightStick = new TwoAxisInputControl();
				this.DPad = new TwoAxisInputControl();
				this.AddControl(InputControlType.LeftStickX, "Left Stick X");
				this.AddControl(InputControlType.LeftStickY, "Left Stick Y");
				this.AddControl(InputControlType.RightStickX, "Right Stick X");
				this.AddControl(InputControlType.RightStickY, "Right Stick Y");
				this.AddControl(InputControlType.DPadX, "DPad X");
				this.AddControl(InputControlType.DPadY, "DPad Y");
				this.AddControl(InputControlType.Command, "Command");
				this.ExpireControlCache();
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00011B54 File Offset: 0x0000FF54
		private void RemoveAliasControls()
		{
			this.LeftStick = TwoAxisInputControl.Null;
			this.RightStick = TwoAxisInputControl.Null;
			this.DPad = TwoAxisInputControl.Null;
			this.RemoveControl(InputControlType.LeftStickX);
			this.RemoveControl(InputControlType.LeftStickY);
			this.RemoveControl(InputControlType.RightStickX);
			this.RemoveControl(InputControlType.RightStickY);
			this.RemoveControl(InputControlType.DPadX);
			this.RemoveControl(InputControlType.DPadY);
			this.RemoveControl(InputControlType.Command);
			this.ExpireControlCache();
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00011BD5 File Offset: 0x0000FFD5
		protected void ClearControls()
		{
			Array.Clear(this.ControlsByTarget, 0, this.ControlsByTarget.Length);
			this.controls.Clear();
			this.ExpireControlCache();
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00011BFC File Offset: 0x0000FFFC
		public bool HasControl(InputControlType controlType)
		{
			return this.ControlsByTarget[(int)controlType] != null;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00011C0C File Offset: 0x0001000C
		public InputControl GetControl(InputControlType controlType)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			return inputControl ?? InputControl.Null;
		}

		// Token: 0x170000BA RID: 186
		public InputControl this[InputControlType controlType]
		{
			get
			{
				return this.GetControl(controlType);
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00011C38 File Offset: 0x00010038
		public static InputControlType GetInputControlTypeByName(string inputControlName)
		{
			return (InputControlType)Enum.Parse(typeof(InputControlType), inputControlName);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00011C50 File Offset: 0x00010050
		public InputControl GetControlByName(string controlName)
		{
			InputControlType inputControlTypeByName = InputDevice.GetInputControlTypeByName(controlName);
			return this.GetControl(inputControlTypeByName);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00011C6C File Offset: 0x0001006C
		public InputControl AddControl(InputControlType controlType, string handle)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			if (inputControl == null)
			{
				inputControl = new InputControl(handle, controlType);
				this.ControlsByTarget[(int)controlType] = inputControl;
				this.controls.Add(inputControl);
				this.ExpireControlCache();
			}
			return inputControl;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00011CAC File Offset: 0x000100AC
		public InputControl AddControl(InputControlType controlType, string handle, float lowerDeadZone, float upperDeadZone)
		{
			InputControl inputControl = this.AddControl(controlType, handle);
			inputControl.LowerDeadZone = lowerDeadZone;
			inputControl.UpperDeadZone = upperDeadZone;
			return inputControl;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00011CD4 File Offset: 0x000100D4
		private void RemoveControl(InputControlType controlType)
		{
			InputControl inputControl = this.ControlsByTarget[(int)controlType];
			if (inputControl != null)
			{
				this.ControlsByTarget[(int)controlType] = null;
				this.controls.Remove(inputControl);
				this.ExpireControlCache();
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00011D0C File Offset: 0x0001010C
		public void ClearInputState()
		{
			this.LeftStick.ClearInputState();
			this.RightStick.ClearInputState();
			this.DPad.ClearInputState();
			int count = this.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.ClearInputState();
				}
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00011D71 File Offset: 0x00010171
		protected void UpdateWithState(InputControlType controlType, bool state, ulong updateTick, float deltaTime)
		{
			this.GetControl(controlType).UpdateWithState(state, updateTick, deltaTime);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00011D84 File Offset: 0x00010184
		protected void UpdateWithValue(InputControlType controlType, float value, ulong updateTick, float deltaTime)
		{
			this.GetControl(controlType).UpdateWithValue(value, updateTick, deltaTime);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00011D98 File Offset: 0x00010198
		internal void UpdateLeftStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00011E74 File Offset: 0x00010274
		internal void UpdateLeftStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00011F4D File Offset: 0x0001034D
		internal void CommitLeftStick()
		{
			this.LeftStickUp.Commit();
			this.LeftStickDown.Commit();
			this.LeftStickLeft.Commit();
			this.LeftStickRight.Commit();
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00011F7C File Offset: 0x0001037C
		internal void UpdateRightStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.RightStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00012058 File Offset: 0x00010458
		internal void UpdateRightStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00012131 File Offset: 0x00010531
		internal void CommitRightStick()
		{
			this.RightStickUp.Commit();
			this.RightStickDown.Commit();
			this.RightStickLeft.Commit();
			this.RightStickRight.Commit();
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001215F File Offset: 0x0001055F
		public virtual void Update(ulong updateTick, float deltaTime)
		{
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00012164 File Offset: 0x00010564
		private bool AnyCommandControlIsPressed()
		{
			for (int i = 100; i <= 113; i++)
			{
				InputControl inputControl = this.ControlsByTarget[i];
				if (inputControl != null && inputControl.IsPressed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000121A4 File Offset: 0x000105A4
		private void ProcessLeftStick(ulong updateTick, float deltaTime)
		{
			float x = Utility.ValueFromSides(this.LeftStickLeft.NextRawValue, this.LeftStickRight.NextRawValue);
			float y = Utility.ValueFromSides(this.LeftStickDown.NextRawValue, this.LeftStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.LeftStickLeft.Raw || this.LeftStickRight.Raw || this.LeftStickUp.Raw || this.LeftStickDown.Raw)
			{
				vector = new Vector2(x, y);
			}
			else
			{
				float lowerDeadZone = Utility.Max(this.LeftStickLeft.LowerDeadZone, this.LeftStickRight.LowerDeadZone, this.LeftStickUp.LowerDeadZone, this.LeftStickDown.LowerDeadZone);
				float upperDeadZone = Utility.Min(this.LeftStickLeft.UpperDeadZone, this.LeftStickRight.UpperDeadZone, this.LeftStickUp.UpperDeadZone, this.LeftStickDown.UpperDeadZone);
				vector = Utility.ApplySeparateDeadZone(x, y, lowerDeadZone, upperDeadZone);
			}
			this.LeftStick.Raw = true;
			this.LeftStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.LeftStickX.Raw = true;
			this.LeftStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.LeftStickY.Raw = true;
			this.LeftStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.LeftStickLeft.SetValue(this.LeftStick.Left.Value, updateTick);
			this.LeftStickRight.SetValue(this.LeftStick.Right.Value, updateTick);
			this.LeftStickUp.SetValue(this.LeftStick.Up.Value, updateTick);
			this.LeftStickDown.SetValue(this.LeftStick.Down.Value, updateTick);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00012390 File Offset: 0x00010790
		private void ProcessRightStick(ulong updateTick, float deltaTime)
		{
			float x = Utility.ValueFromSides(this.RightStickLeft.NextRawValue, this.RightStickRight.NextRawValue);
			float y = Utility.ValueFromSides(this.RightStickDown.NextRawValue, this.RightStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.RightStickLeft.Raw || this.RightStickRight.Raw || this.RightStickUp.Raw || this.RightStickDown.Raw)
			{
				vector = new Vector2(x, y);
			}
			else
			{
				float lowerDeadZone = Utility.Max(this.RightStickLeft.LowerDeadZone, this.RightStickRight.LowerDeadZone, this.RightStickUp.LowerDeadZone, this.RightStickDown.LowerDeadZone);
				float upperDeadZone = Utility.Min(this.RightStickLeft.UpperDeadZone, this.RightStickRight.UpperDeadZone, this.RightStickUp.UpperDeadZone, this.RightStickDown.UpperDeadZone);
				vector = Utility.ApplySeparateDeadZone(x, y, lowerDeadZone, upperDeadZone);
			}
			this.RightStick.Raw = true;
			this.RightStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.RightStickX.Raw = true;
			this.RightStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.RightStickY.Raw = true;
			this.RightStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.RightStickLeft.SetValue(this.RightStick.Left.Value, updateTick);
			this.RightStickRight.SetValue(this.RightStick.Right.Value, updateTick);
			this.RightStickUp.SetValue(this.RightStick.Up.Value, updateTick);
			this.RightStickDown.SetValue(this.RightStick.Down.Value, updateTick);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001257C File Offset: 0x0001097C
		private void ProcessDPad(ulong updateTick, float deltaTime)
		{
			float x = Utility.ValueFromSides(this.DPadLeft.NextRawValue, this.DPadRight.NextRawValue);
			float y = Utility.ValueFromSides(this.DPadDown.NextRawValue, this.DPadUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks || this.DPadLeft.Raw || this.DPadRight.Raw || this.DPadUp.Raw || this.DPadDown.Raw)
			{
				vector = new Vector2(x, y);
			}
			else
			{
				float lowerDeadZone = Utility.Max(this.DPadLeft.LowerDeadZone, this.DPadRight.LowerDeadZone, this.DPadUp.LowerDeadZone, this.DPadDown.LowerDeadZone);
				float upperDeadZone = Utility.Min(this.DPadLeft.UpperDeadZone, this.DPadRight.UpperDeadZone, this.DPadUp.UpperDeadZone, this.DPadDown.UpperDeadZone);
				vector = Utility.ApplySeparateDeadZone(x, y, lowerDeadZone, upperDeadZone);
			}
			this.DPad.Raw = true;
			this.DPad.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.DPadX.Raw = true;
			this.DPadX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.DPadY.Raw = true;
			this.DPadY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.DPadLeft.SetValue(this.DPad.Left.Value, updateTick);
			this.DPadRight.SetValue(this.DPad.Right.Value, updateTick);
			this.DPadUp.SetValue(this.DPad.Up.Value, updateTick);
			this.DPadDown.SetValue(this.DPad.Down.Value, updateTick);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00012768 File Offset: 0x00010B68
		public void Commit(ulong updateTick, float deltaTime)
		{
			if (this.IsKnown)
			{
				this.ProcessLeftStick(updateTick, deltaTime);
				this.ProcessRightStick(updateTick, deltaTime);
				this.ProcessDPad(updateTick, deltaTime);
			}
			int count = this.Controls.Count;
			for (int i = 0; i < count; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.Commit();
					if (inputControl.HasChanged && !inputControl.Passive)
					{
						this.LastChangeTick = updateTick;
					}
				}
			}
			if (this.IsKnown)
			{
				this.Command.CommitWithState(this.AnyCommandControlIsPressed(), updateTick, deltaTime);
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001280A File Offset: 0x00010C0A
		public bool LastChangedAfter(InputDevice device)
		{
			return device == null || this.LastChangeTick > device.LastChangeTick;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00012823 File Offset: 0x00010C23
		internal void RequestActivation()
		{
			this.LastChangeTick = InputManager.CurrentTick;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00012830 File Offset: 0x00010C30
		public virtual void Vibrate(float leftMotor, float rightMotor)
		{
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00012832 File Offset: 0x00010C32
		public void Vibrate(float intensity)
		{
			this.Vibrate(intensity, intensity);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001283C File Offset: 0x00010C3C
		public void StopVibration()
		{
			this.Vibrate(0f);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00012849 File Offset: 0x00010C49
		public virtual void SetLightColor(float red, float green, float blue)
		{
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0001284B File Offset: 0x00010C4B
		public void SetLightColor(Color color)
		{
			this.SetLightColor(color.r * color.a, color.g * color.a, color.b * color.a);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00012880 File Offset: 0x00010C80
		public virtual void SetLightFlash(float flashOnDuration, float flashOffDuration)
		{
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00012882 File Offset: 0x00010C82
		public void StopLightFlash()
		{
			this.SetLightFlash(1f, 0f);
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00012894 File Offset: 0x00010C94
		public virtual bool IsSupportedOnThisPlatform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00012897 File Offset: 0x00010C97
		public virtual bool IsKnown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0001289A File Offset: 0x00010C9A
		public bool IsUnknown
		{
			get
			{
				return !this.IsKnown;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x000128A5 File Offset: 0x00010CA5
		[Obsolete("Use InputDevice.CommandIsPressed instead.", false)]
		public bool MenuIsPressed
		{
			get
			{
				return this.IsKnown && this.Command.IsPressed;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x000128C0 File Offset: 0x00010CC0
		[Obsolete("Use InputDevice.CommandWasPressed instead.", false)]
		public bool MenuWasPressed
		{
			get
			{
				return this.IsKnown && this.Command.WasPressed;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x000128DB File Offset: 0x00010CDB
		[Obsolete("Use InputDevice.CommandWasReleased instead.", false)]
		public bool MenuWasReleased
		{
			get
			{
				return this.IsKnown && this.Command.WasReleased;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x000128F6 File Offset: 0x00010CF6
		public bool CommandIsPressed
		{
			get
			{
				return this.IsKnown && this.Command.IsPressed;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00012911 File Offset: 0x00010D11
		public bool CommandWasPressed
		{
			get
			{
				return this.IsKnown && this.Command.WasPressed;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001292C File Offset: 0x00010D2C
		public bool CommandWasReleased
		{
			get
			{
				return this.IsKnown && this.Command.WasReleased;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00012948 File Offset: 0x00010D48
		public InputControl AnyButton
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.IsPressed)
					{
						return inputControl;
					}
				}
				return InputControl.Null;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x000129A4 File Offset: 0x00010DA4
		public bool AnyButtonIsPressed
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.IsPressed)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x000129FC File Offset: 0x00010DFC
		public bool AnyButtonWasPressed
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.WasPressed)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00012A54 File Offset: 0x00010E54
		public bool AnyButtonWasReleased
		{
			get
			{
				int count = this.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.WasReleased)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00012AAB File Offset: 0x00010EAB
		public TwoAxisInputControl Direction
		{
			get
			{
				return (this.DPad.UpdateTick <= this.LeftStick.UpdateTick) ? this.LeftStick : this.DPad;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00012ADC File Offset: 0x00010EDC
		public InputControl LeftStickUp
		{
			get
			{
				InputControl result;
				if ((result = this.cachedLeftStickUp) == null)
				{
					result = (this.cachedLeftStickUp = this.GetControl(InputControlType.LeftStickUp));
				}
				return result;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x00012B08 File Offset: 0x00010F08
		public InputControl LeftStickDown
		{
			get
			{
				InputControl result;
				if ((result = this.cachedLeftStickDown) == null)
				{
					result = (this.cachedLeftStickDown = this.GetControl(InputControlType.LeftStickDown));
				}
				return result;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00012B34 File Offset: 0x00010F34
		public InputControl LeftStickLeft
		{
			get
			{
				InputControl result;
				if ((result = this.cachedLeftStickLeft) == null)
				{
					result = (this.cachedLeftStickLeft = this.GetControl(InputControlType.LeftStickLeft));
				}
				return result;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00012B60 File Offset: 0x00010F60
		public InputControl LeftStickRight
		{
			get
			{
				InputControl result;
				if ((result = this.cachedLeftStickRight) == null)
				{
					result = (this.cachedLeftStickRight = this.GetControl(InputControlType.LeftStickRight));
				}
				return result;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x00012B8C File Offset: 0x00010F8C
		public InputControl RightStickUp
		{
			get
			{
				InputControl result;
				if ((result = this.cachedRightStickUp) == null)
				{
					result = (this.cachedRightStickUp = this.GetControl(InputControlType.RightStickUp));
				}
				return result;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00012BB8 File Offset: 0x00010FB8
		public InputControl RightStickDown
		{
			get
			{
				InputControl result;
				if ((result = this.cachedRightStickDown) == null)
				{
					result = (this.cachedRightStickDown = this.GetControl(InputControlType.RightStickDown));
				}
				return result;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x00012BE4 File Offset: 0x00010FE4
		public InputControl RightStickLeft
		{
			get
			{
				InputControl result;
				if ((result = this.cachedRightStickLeft) == null)
				{
					result = (this.cachedRightStickLeft = this.GetControl(InputControlType.RightStickLeft));
				}
				return result;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00012C10 File Offset: 0x00011010
		public InputControl RightStickRight
		{
			get
			{
				InputControl result;
				if ((result = this.cachedRightStickRight) == null)
				{
					result = (this.cachedRightStickRight = this.GetControl(InputControlType.RightStickRight));
				}
				return result;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00012C3C File Offset: 0x0001103C
		public InputControl DPadUp
		{
			get
			{
				InputControl result;
				if ((result = this.cachedDPadUp) == null)
				{
					result = (this.cachedDPadUp = this.GetControl(InputControlType.DPadUp));
				}
				return result;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00012C68 File Offset: 0x00011068
		public InputControl DPadDown
		{
			get
			{
				InputControl result;
				if ((result = this.cachedDPadDown) == null)
				{
					result = (this.cachedDPadDown = this.GetControl(InputControlType.DPadDown));
				}
				return result;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00012C94 File Offset: 0x00011094
		public InputControl DPadLeft
		{
			get
			{
				InputControl result;
				if ((result = this.cachedDPadLeft) == null)
				{
					result = (this.cachedDPadLeft = this.GetControl(InputControlType.DPadLeft));
				}
				return result;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00012CC0 File Offset: 0x000110C0
		public InputControl DPadRight
		{
			get
			{
				InputControl result;
				if ((result = this.cachedDPadRight) == null)
				{
					result = (this.cachedDPadRight = this.GetControl(InputControlType.DPadRight));
				}
				return result;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00012CEC File Offset: 0x000110EC
		public InputControl Action1
		{
			get
			{
				InputControl result;
				if ((result = this.cachedAction1) == null)
				{
					result = (this.cachedAction1 = this.GetControl(InputControlType.Action1));
				}
				return result;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00012D18 File Offset: 0x00011118
		public InputControl Action2
		{
			get
			{
				InputControl result;
				if ((result = this.cachedAction2) == null)
				{
					result = (this.cachedAction2 = this.GetControl(InputControlType.Action2));
				}
				return result;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00012D44 File Offset: 0x00011144
		public InputControl Action3
		{
			get
			{
				InputControl result;
				if ((result = this.cachedAction3) == null)
				{
					result = (this.cachedAction3 = this.GetControl(InputControlType.Action3));
				}
				return result;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00012D70 File Offset: 0x00011170
		public InputControl Action4
		{
			get
			{
				InputControl result;
				if ((result = this.cachedAction4) == null)
				{
					result = (this.cachedAction4 = this.GetControl(InputControlType.Action4));
				}
				return result;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00012D9C File Offset: 0x0001119C
		public InputControl LeftTrigger
		{
			get
			{
				InputControl result;
				if ((result = this.cachedLeftTrigger) == null)
				{
					result = (this.cachedLeftTrigger = this.GetControl(InputControlType.LeftTrigger));
				}
				return result;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00012DC8 File Offset: 0x000111C8
		public InputControl RightTrigger
		{
			get
			{
				InputControl result;
				if ((result = this.cachedRightTrigger) == null)
				{
					result = (this.cachedRightTrigger = this.GetControl(InputControlType.RightTrigger));
				}
				return result;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00012DF4 File Offset: 0x000111F4
		public InputControl LeftBumper
		{
			get
			{
				InputControl result;
				if ((result = this.cachedLeftBumper) == null)
				{
					result = (this.cachedLeftBumper = this.GetControl(InputControlType.LeftBumper));
				}
				return result;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00012E20 File Offset: 0x00011220
		public InputControl RightBumper
		{
			get
			{
				InputControl result;
				if ((result = this.cachedRightBumper) == null)
				{
					result = (this.cachedRightBumper = this.GetControl(InputControlType.RightBumper));
				}
				return result;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00012E4C File Offset: 0x0001124C
		public InputControl LeftStickButton
		{
			get
			{
				InputControl result;
				if ((result = this.cachedLeftStickButton) == null)
				{
					result = (this.cachedLeftStickButton = this.GetControl(InputControlType.LeftStickButton));
				}
				return result;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00012E78 File Offset: 0x00011278
		public InputControl RightStickButton
		{
			get
			{
				InputControl result;
				if ((result = this.cachedRightStickButton) == null)
				{
					result = (this.cachedRightStickButton = this.GetControl(InputControlType.RightStickButton));
				}
				return result;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00012EA4 File Offset: 0x000112A4
		public InputControl LeftStickX
		{
			get
			{
				InputControl result;
				if ((result = this.cachedLeftStickX) == null)
				{
					result = (this.cachedLeftStickX = this.GetControl(InputControlType.LeftStickX));
				}
				return result;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00012ED4 File Offset: 0x000112D4
		public InputControl LeftStickY
		{
			get
			{
				InputControl result;
				if ((result = this.cachedLeftStickY) == null)
				{
					result = (this.cachedLeftStickY = this.GetControl(InputControlType.LeftStickY));
				}
				return result;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00012F04 File Offset: 0x00011304
		public InputControl RightStickX
		{
			get
			{
				InputControl result;
				if ((result = this.cachedRightStickX) == null)
				{
					result = (this.cachedRightStickX = this.GetControl(InputControlType.RightStickX));
				}
				return result;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00012F34 File Offset: 0x00011334
		public InputControl RightStickY
		{
			get
			{
				InputControl result;
				if ((result = this.cachedRightStickY) == null)
				{
					result = (this.cachedRightStickY = this.GetControl(InputControlType.RightStickY));
				}
				return result;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00012F64 File Offset: 0x00011364
		public InputControl DPadX
		{
			get
			{
				InputControl result;
				if ((result = this.cachedDPadX) == null)
				{
					result = (this.cachedDPadX = this.GetControl(InputControlType.DPadX));
				}
				return result;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00012F94 File Offset: 0x00011394
		public InputControl DPadY
		{
			get
			{
				InputControl result;
				if ((result = this.cachedDPadY) == null)
				{
					result = (this.cachedDPadY = this.GetControl(InputControlType.DPadY));
				}
				return result;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00012FC4 File Offset: 0x000113C4
		public InputControl Command
		{
			get
			{
				InputControl result;
				if ((result = this.cachedCommand) == null)
				{
					result = (this.cachedCommand = this.GetControl(InputControlType.Command));
				}
				return result;
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00012FF4 File Offset: 0x000113F4
		private void ExpireControlCache()
		{
			this.cachedLeftStickUp = null;
			this.cachedLeftStickDown = null;
			this.cachedLeftStickLeft = null;
			this.cachedLeftStickRight = null;
			this.cachedRightStickUp = null;
			this.cachedRightStickDown = null;
			this.cachedRightStickLeft = null;
			this.cachedRightStickRight = null;
			this.cachedDPadUp = null;
			this.cachedDPadDown = null;
			this.cachedDPadLeft = null;
			this.cachedDPadRight = null;
			this.cachedAction1 = null;
			this.cachedAction2 = null;
			this.cachedAction3 = null;
			this.cachedAction4 = null;
			this.cachedLeftTrigger = null;
			this.cachedRightTrigger = null;
			this.cachedLeftBumper = null;
			this.cachedRightBumper = null;
			this.cachedLeftStickButton = null;
			this.cachedRightStickButton = null;
			this.cachedLeftStickX = null;
			this.cachedLeftStickY = null;
			this.cachedRightStickX = null;
			this.cachedRightStickY = null;
			this.cachedDPadX = null;
			this.cachedDPadY = null;
			this.cachedCommand = null;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x000130CC File Offset: 0x000114CC
		internal virtual int NumUnknownAnalogs
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x000130CF File Offset: 0x000114CF
		internal virtual int NumUnknownButtons
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000130D2 File Offset: 0x000114D2
		internal virtual bool ReadRawButtonState(int index)
		{
			return false;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x000130D5 File Offset: 0x000114D5
		internal virtual float ReadRawAnalogValue(int index)
		{
			return 0f;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000130DC File Offset: 0x000114DC
		internal void TakeSnapshot()
		{
			if (this.AnalogSnapshot == null)
			{
				this.AnalogSnapshot = new InputDevice.AnalogSnapshotEntry[this.NumUnknownAnalogs];
			}
			for (int i = 0; i < this.NumUnknownAnalogs; i++)
			{
				float value = Utility.ApplySnapping(this.ReadRawAnalogValue(i), 0.5f);
				this.AnalogSnapshot[i].value = value;
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00013140 File Offset: 0x00011540
		internal UnknownDeviceControl GetFirstPressedAnalog()
		{
			if (this.AnalogSnapshot != null)
			{
				for (int i = 0; i < this.NumUnknownAnalogs; i++)
				{
					InputControlType control = InputControlType.Analog0 + i;
					float num = Utility.ApplySnapping(this.ReadRawAnalogValue(i), 0.5f);
					float num2 = num - this.AnalogSnapshot[i].value;
					this.AnalogSnapshot[i].TrackMinMaxValue(num);
					if (num2 > 0.1f)
					{
						num2 = this.AnalogSnapshot[i].maxValue - this.AnalogSnapshot[i].value;
					}
					if (num2 < -0.1f)
					{
						num2 = this.AnalogSnapshot[i].minValue - this.AnalogSnapshot[i].value;
					}
					if (num2 > 1.9f)
					{
						return new UnknownDeviceControl(control, InputRangeType.MinusOneToOne);
					}
					if (num2 < -0.9f)
					{
						return new UnknownDeviceControl(control, InputRangeType.ZeroToMinusOne);
					}
					if (num2 > 0.9f)
					{
						return new UnknownDeviceControl(control, InputRangeType.ZeroToOne);
					}
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001324C File Offset: 0x0001164C
		internal UnknownDeviceControl GetFirstPressedButton()
		{
			for (int i = 0; i < this.NumUnknownButtons; i++)
			{
				if (this.ReadRawButtonState(i))
				{
					return new UnknownDeviceControl(InputControlType.Button0 + i, InputRangeType.ZeroToOne);
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x040003BF RID: 959
		public static readonly InputDevice Null = new InputDevice("None");

		// Token: 0x040003C9 RID: 969
		private List<InputControl> controls;

		// Token: 0x040003CF RID: 975
		public bool Passive;

		// Token: 0x040003D1 RID: 977
		private float leftLowerDeadZone;

		// Token: 0x040003D2 RID: 978
		private float rightLowerDeadZone;

		// Token: 0x040003D3 RID: 979
		private float leftTriggerLowerDeadZone;

		// Token: 0x040003D4 RID: 980
		private float rightTriggerLowerDeadZone;

		// Token: 0x040003D5 RID: 981
		private InputControl cachedLeftStickUp;

		// Token: 0x040003D6 RID: 982
		private InputControl cachedLeftStickDown;

		// Token: 0x040003D7 RID: 983
		private InputControl cachedLeftStickLeft;

		// Token: 0x040003D8 RID: 984
		private InputControl cachedLeftStickRight;

		// Token: 0x040003D9 RID: 985
		private InputControl cachedRightStickUp;

		// Token: 0x040003DA RID: 986
		private InputControl cachedRightStickDown;

		// Token: 0x040003DB RID: 987
		private InputControl cachedRightStickLeft;

		// Token: 0x040003DC RID: 988
		private InputControl cachedRightStickRight;

		// Token: 0x040003DD RID: 989
		private InputControl cachedDPadUp;

		// Token: 0x040003DE RID: 990
		private InputControl cachedDPadDown;

		// Token: 0x040003DF RID: 991
		private InputControl cachedDPadLeft;

		// Token: 0x040003E0 RID: 992
		private InputControl cachedDPadRight;

		// Token: 0x040003E1 RID: 993
		private InputControl cachedAction1;

		// Token: 0x040003E2 RID: 994
		private InputControl cachedAction2;

		// Token: 0x040003E3 RID: 995
		private InputControl cachedAction3;

		// Token: 0x040003E4 RID: 996
		private InputControl cachedAction4;

		// Token: 0x040003E5 RID: 997
		private InputControl cachedLeftTrigger;

		// Token: 0x040003E6 RID: 998
		private InputControl cachedRightTrigger;

		// Token: 0x040003E7 RID: 999
		private InputControl cachedLeftBumper;

		// Token: 0x040003E8 RID: 1000
		private InputControl cachedRightBumper;

		// Token: 0x040003E9 RID: 1001
		private InputControl cachedLeftStickButton;

		// Token: 0x040003EA RID: 1002
		private InputControl cachedRightStickButton;

		// Token: 0x040003EB RID: 1003
		private InputControl cachedLeftStickX;

		// Token: 0x040003EC RID: 1004
		private InputControl cachedLeftStickY;

		// Token: 0x040003ED RID: 1005
		private InputControl cachedRightStickX;

		// Token: 0x040003EE RID: 1006
		private InputControl cachedRightStickY;

		// Token: 0x040003EF RID: 1007
		private InputControl cachedDPadX;

		// Token: 0x040003F0 RID: 1008
		private InputControl cachedDPadY;

		// Token: 0x040003F1 RID: 1009
		private InputControl cachedCommand;

		// Token: 0x02000078 RID: 120
		protected struct AnalogSnapshotEntry
		{
			// Token: 0x060004BC RID: 1212 RVA: 0x000132A0 File Offset: 0x000116A0
			public void TrackMinMaxValue(float currentValue)
			{
				this.maxValue = Mathf.Max(this.maxValue, currentValue);
				this.minValue = Mathf.Min(this.minValue, currentValue);
			}

			// Token: 0x040003F2 RID: 1010
			public float value;

			// Token: 0x040003F3 RID: 1011
			public float maxValue;

			// Token: 0x040003F4 RID: 1012
			public float minValue;
		}
	}
}
