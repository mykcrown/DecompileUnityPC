using System;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200005E RID: 94
	public class MouseBindingSource : BindingSource
	{
		// Token: 0x06000301 RID: 769 RVA: 0x000158CB File Offset: 0x00013CCB
		internal MouseBindingSource()
		{
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000158D3 File Offset: 0x00013CD3
		public MouseBindingSource(Mouse mouseControl)
		{
			this.Control = mouseControl;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000303 RID: 771 RVA: 0x000158E2 File Offset: 0x00013CE2
		// (set) Token: 0x06000304 RID: 772 RVA: 0x000158EA File Offset: 0x00013CEA
		public Mouse Control { get; protected set; }

		// Token: 0x06000305 RID: 773 RVA: 0x000158F4 File Offset: 0x00013CF4
		internal static bool SafeGetMouseButton(int button)
		{
			try
			{
				return Input.GetMouseButton(button);
			}
			catch (ArgumentException)
			{
			}
			return false;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00015928 File Offset: 0x00013D28
		internal static bool ButtonIsPressed(Mouse control)
		{
			int num = MouseBindingSource.buttonTable[(int)control];
			return num >= 0 && MouseBindingSource.SafeGetMouseButton(num);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0001594C File Offset: 0x00013D4C
		internal static bool NegativeScrollWheelIsActive(float threshold)
		{
			float num = Mathf.Min(Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ, 0f);
			return num < -threshold;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0001597C File Offset: 0x00013D7C
		internal static bool PositiveScrollWheelIsActive(float threshold)
		{
			float num = Mathf.Max(0f, Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ);
			return num > threshold;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000159A8 File Offset: 0x00013DA8
		internal static float GetValue(Mouse mouseControl)
		{
			int num = MouseBindingSource.buttonTable[(int)mouseControl];
			if (num >= 0)
			{
				return (!MouseBindingSource.SafeGetMouseButton(num)) ? 0f : 1f;
			}
			switch (mouseControl)
			{
			case Mouse.NegativeX:
				return -Mathf.Min(Input.GetAxisRaw("mouse x") * MouseBindingSource.ScaleX, 0f);
			case Mouse.PositiveX:
				return Mathf.Max(0f, Input.GetAxisRaw("mouse x") * MouseBindingSource.ScaleX);
			case Mouse.NegativeY:
				return -Mathf.Min(Input.GetAxisRaw("mouse y") * MouseBindingSource.ScaleY, 0f);
			case Mouse.PositiveY:
				return Mathf.Max(0f, Input.GetAxisRaw("mouse y") * MouseBindingSource.ScaleY);
			case Mouse.PositiveScrollWheel:
				return Mathf.Max(0f, Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ);
			case Mouse.NegativeScrollWheel:
				return -Mathf.Min(Input.GetAxisRaw("mouse z") * MouseBindingSource.ScaleZ, 0f);
			default:
				return 0f;
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00015AAE File Offset: 0x00013EAE
		public override float GetValue(InputDevice inputDevice)
		{
			return MouseBindingSource.GetValue(this.Control);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00015ABB File Offset: 0x00013EBB
		public override bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00015ACC File Offset: 0x00013ECC
		public override string Name
		{
			get
			{
				return this.Control.ToString();
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00015AED File Offset: 0x00013EED
		public override string DeviceName
		{
			get
			{
				return "Mouse";
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600030E RID: 782 RVA: 0x00015AF4 File Offset: 0x00013EF4
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return InputDeviceClass.Mouse;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00015AF7 File Offset: 0x00013EF7
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00015AFC File Offset: 0x00013EFC
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			MouseBindingSource mouseBindingSource = other as MouseBindingSource;
			return mouseBindingSource != null && this.Control == mouseBindingSource.Control;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00015B3C File Offset: 0x00013F3C
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			MouseBindingSource mouseBindingSource = other as MouseBindingSource;
			return mouseBindingSource != null && this.Control == mouseBindingSource.Control;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00015B74 File Offset: 0x00013F74
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00015B95 File Offset: 0x00013F95
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.MouseBindingSource;
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00015B98 File Offset: 0x00013F98
		internal override void Save(BinaryWriter writer)
		{
			writer.Write((int)this.Control);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00015BA6 File Offset: 0x00013FA6
		internal override void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			this.Control = (Mouse)reader.ReadInt32();
		}

		// Token: 0x04000294 RID: 660
		public static float ScaleX = 0.05f;

		// Token: 0x04000295 RID: 661
		public static float ScaleY = 0.05f;

		// Token: 0x04000296 RID: 662
		public static float ScaleZ = 0.05f;

		// Token: 0x04000297 RID: 663
		public static float JitterThreshold = 0.05f;

		// Token: 0x04000298 RID: 664
		private static readonly int[] buttonTable = new int[]
		{
			-1,
			0,
			1,
			2,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			3,
			4,
			5,
			6,
			7,
			8
		};
	}
}
