using System;

namespace InControl
{
	// Token: 0x0200006A RID: 106
	public class InputControl : OneAxisInputControl
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x000186D1 File Offset: 0x00016AD1
		private InputControl()
		{
			this.Handle = "None";
			this.Target = InputControlType.None;
			this.Passive = false;
			this.IsButton = false;
			this.IsAnalog = false;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00018700 File Offset: 0x00016B00
		public InputControl(string handle, InputControlType target)
		{
			this.Handle = handle;
			this.Target = target;
			this.Passive = false;
			this.IsButton = Utility.TargetIsButton(target);
			this.IsAnalog = !this.IsButton;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00018738 File Offset: 0x00016B38
		public InputControl(string handle, InputControlType target, bool passive) : this(handle, target)
		{
			this.Passive = passive;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00018749 File Offset: 0x00016B49
		// (set) Token: 0x060003BD RID: 957 RVA: 0x00018751 File Offset: 0x00016B51
		public string Handle { get; protected set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0001875A File Offset: 0x00016B5A
		// (set) Token: 0x060003BF RID: 959 RVA: 0x00018762 File Offset: 0x00016B62
		public InputControlType Target { get; protected set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0001876B File Offset: 0x00016B6B
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x00018773 File Offset: 0x00016B73
		public bool IsButton { get; protected set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0001877C File Offset: 0x00016B7C
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x00018784 File Offset: 0x00016B84
		public bool IsAnalog { get; protected set; }

		// Token: 0x060003C4 RID: 964 RVA: 0x0001878D File Offset: 0x00016B8D
		internal void SetZeroTick()
		{
			this.zeroTick = base.UpdateTick;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0001879B File Offset: 0x00016B9B
		internal bool IsOnZeroTick
		{
			get
			{
				return base.UpdateTick == this.zeroTick;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x000187AB File Offset: 0x00016BAB
		public bool IsStandard
		{
			get
			{
				return Utility.TargetIsStandard(this.Target);
			}
		}

		// Token: 0x040002EF RID: 751
		public static readonly InputControl Null = new InputControl();

		// Token: 0x040002F2 RID: 754
		public bool Passive;

		// Token: 0x040002F5 RID: 757
		private ulong zeroTick;
	}
}
