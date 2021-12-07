using System;
using System.IO;

namespace InControl
{
	// Token: 0x02000059 RID: 89
	public class KeyBindingSource : BindingSource
	{
		// Token: 0x060002CB RID: 715 RVA: 0x00013EC8 File Offset: 0x000122C8
		internal KeyBindingSource()
		{
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00013ED0 File Offset: 0x000122D0
		public KeyBindingSource(KeyCombo keyCombo)
		{
			this.Control = keyCombo;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00013EDF File Offset: 0x000122DF
		public KeyBindingSource(params Key[] keys)
		{
			this.Control = new KeyCombo(keys);
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060002CE RID: 718 RVA: 0x00013EF3 File Offset: 0x000122F3
		// (set) Token: 0x060002CF RID: 719 RVA: 0x00013EFB File Offset: 0x000122FB
		public KeyCombo Control { get; protected set; }

		// Token: 0x060002D0 RID: 720 RVA: 0x00013F04 File Offset: 0x00012304
		public override float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00013F24 File Offset: 0x00012324
		public override bool GetState(InputDevice inputDevice)
		{
			return this.Control.IsPressed;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00013F40 File Offset: 0x00012340
		public override string Name
		{
			get
			{
				return this.Control.ToString();
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00013F61 File Offset: 0x00012361
		public override string DeviceName
		{
			get
			{
				return "Keyboard";
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00013F68 File Offset: 0x00012368
		public override InputDeviceClass DeviceClass
		{
			get
			{
				return InputDeviceClass.Keyboard;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00013F6B File Offset: 0x0001236B
		public override InputDeviceStyle DeviceStyle
		{
			get
			{
				return InputDeviceStyle.Unknown;
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00013F70 File Offset: 0x00012370
		public override bool Equals(BindingSource other)
		{
			if (other == null)
			{
				return false;
			}
			KeyBindingSource keyBindingSource = other as KeyBindingSource;
			return keyBindingSource != null && this.Control == keyBindingSource.Control;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00013FB4 File Offset: 0x000123B4
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			KeyBindingSource keyBindingSource = other as KeyBindingSource;
			return keyBindingSource != null && this.Control == keyBindingSource.Control;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00013FF0 File Offset: 0x000123F0
		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x00014011 File Offset: 0x00012411
		public override BindingSourceType BindingSourceType
		{
			get
			{
				return BindingSourceType.KeyBindingSource;
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00014014 File Offset: 0x00012414
		internal override void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			KeyCombo control = default(KeyCombo);
			control.Load(reader, dataFormatVersion);
			this.Control = control;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001403C File Offset: 0x0001243C
		internal override void Save(BinaryWriter writer)
		{
			this.Control.Save(writer);
		}
	}
}
