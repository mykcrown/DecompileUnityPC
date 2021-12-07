using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000072 RID: 114
	public class TwoAxisInputControl : IInputControl
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x000172B0 File Offset: 0x000156B0
		public TwoAxisInputControl()
		{
			this.Left = new OneAxisInputControl();
			this.Right = new OneAxisInputControl();
			this.Up = new OneAxisInputControl();
			this.Down = new OneAxisInputControl();
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x00017305 File Offset: 0x00015705
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x0001730D File Offset: 0x0001570D
		public float X { get; protected set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00017316 File Offset: 0x00015716
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x0001731E File Offset: 0x0001571E
		public float Y { get; protected set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00017327 File Offset: 0x00015727
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x0001732F File Offset: 0x0001572F
		public OneAxisInputControl Left { get; protected set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00017338 File Offset: 0x00015738
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00017340 File Offset: 0x00015740
		public OneAxisInputControl Right { get; protected set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00017349 File Offset: 0x00015749
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00017351 File Offset: 0x00015751
		public OneAxisInputControl Up { get; protected set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0001735A File Offset: 0x0001575A
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x00017362 File Offset: 0x00015762
		public OneAxisInputControl Down { get; protected set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001736B File Offset: 0x0001576B
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x00017373 File Offset: 0x00015773
		public ulong UpdateTick { get; protected set; }

		// Token: 0x0600040F RID: 1039 RVA: 0x0001737C File Offset: 0x0001577C
		public void ClearInputState()
		{
			this.Left.ClearInputState();
			this.Right.ClearInputState();
			this.Up.ClearInputState();
			this.Down.ClearInputState();
			this.lastState = false;
			this.lastValue = Vector2.zero;
			this.thisState = false;
			this.thisValue = Vector2.zero;
			this.X = 0f;
			this.Y = 0f;
			this.clearInputState = true;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000173F6 File Offset: 0x000157F6
		public void Filter(TwoAxisInputControl twoAxisInputControl, float deltaTime)
		{
			this.UpdateWithAxes(twoAxisInputControl.X, twoAxisInputControl.Y, InputManager.CurrentTick, deltaTime);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00017410 File Offset: 0x00015810
		internal void UpdateWithAxes(float x, float y, ulong updateTick, float deltaTime)
		{
			this.lastState = this.thisState;
			this.lastValue = this.thisValue;
			this.thisValue = ((!this.Raw) ? Utility.ApplyCircularDeadZone(x, y, this.LowerDeadZone, this.UpperDeadZone) : new Vector2(x, y));
			this.X = this.thisValue.x;
			this.Y = this.thisValue.y;
			this.Left.CommitWithValue(Mathf.Max(0f, -this.X), updateTick, deltaTime);
			this.Right.CommitWithValue(Mathf.Max(0f, this.X), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.Up.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
				this.Down.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
			}
			else
			{
				this.Up.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
				this.Down.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
			}
			this.thisState = (this.Up.State || this.Down.State || this.Left.State || this.Right.State);
			if (this.clearInputState)
			{
				this.lastState = this.thisState;
				this.lastValue = this.thisValue;
				this.clearInputState = false;
				this.HasChanged = false;
				return;
			}
			if (this.thisValue != this.lastValue)
			{
				this.UpdateTick = updateTick;
				this.HasChanged = true;
			}
			else
			{
				this.HasChanged = false;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x000175F3 File Offset: 0x000159F3
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x000175FC File Offset: 0x000159FC
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			set
			{
				this.sensitivity = Mathf.Clamp01(value);
				this.Left.Sensitivity = this.sensitivity;
				this.Right.Sensitivity = this.sensitivity;
				this.Up.Sensitivity = this.sensitivity;
				this.Down.Sensitivity = this.sensitivity;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00017659 File Offset: 0x00015A59
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x00017664 File Offset: 0x00015A64
		public float StateThreshold
		{
			get
			{
				return this.stateThreshold;
			}
			set
			{
				this.stateThreshold = Mathf.Clamp01(value);
				this.Left.StateThreshold = this.stateThreshold;
				this.Right.StateThreshold = this.stateThreshold;
				this.Up.StateThreshold = this.stateThreshold;
				this.Down.StateThreshold = this.stateThreshold;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x000176C1 File Offset: 0x00015AC1
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x000176CC File Offset: 0x00015ACC
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
				this.Left.LowerDeadZone = this.lowerDeadZone;
				this.Right.LowerDeadZone = this.lowerDeadZone;
				this.Up.LowerDeadZone = this.lowerDeadZone;
				this.Down.LowerDeadZone = this.lowerDeadZone;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00017729 File Offset: 0x00015B29
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x00017734 File Offset: 0x00015B34
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
				this.Left.UpperDeadZone = this.upperDeadZone;
				this.Right.UpperDeadZone = this.upperDeadZone;
				this.Up.UpperDeadZone = this.upperDeadZone;
				this.Down.UpperDeadZone = this.upperDeadZone;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00017791 File Offset: 0x00015B91
		public bool State
		{
			get
			{
				return this.thisState;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00017799 File Offset: 0x00015B99
		public bool LastState
		{
			get
			{
				return this.lastState;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x000177A1 File Offset: 0x00015BA1
		public Vector2 Value
		{
			get
			{
				return this.thisValue;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x000177A9 File Offset: 0x00015BA9
		public Vector2 LastValue
		{
			get
			{
				return this.lastValue;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x000177B1 File Offset: 0x00015BB1
		public Vector2 Vector
		{
			get
			{
				return this.thisValue;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x000177B9 File Offset: 0x00015BB9
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x000177C1 File Offset: 0x00015BC1
		public bool HasChanged { get; protected set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x000177CA File Offset: 0x00015BCA
		public bool IsPressed
		{
			get
			{
				return this.thisState;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x000177D2 File Offset: 0x00015BD2
		public bool WasPressed
		{
			get
			{
				return this.thisState && !this.lastState;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x000177EB File Offset: 0x00015BEB
		public bool WasReleased
		{
			get
			{
				return !this.thisState && this.lastState;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00017801 File Offset: 0x00015C01
		public float Angle
		{
			get
			{
				return Utility.VectorToAngle(this.thisValue);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001780E File Offset: 0x00015C0E
		public static implicit operator bool(TwoAxisInputControl instance)
		{
			return instance.thisState;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00017816 File Offset: 0x00015C16
		public static implicit operator Vector2(TwoAxisInputControl instance)
		{
			return instance.thisValue;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001781E File Offset: 0x00015C1E
		public static implicit operator Vector3(TwoAxisInputControl instance)
		{
			return instance.thisValue;
		}

		// Token: 0x040003A5 RID: 933
		public static readonly TwoAxisInputControl Null = new TwoAxisInputControl();

		// Token: 0x040003AD RID: 941
		private float sensitivity = 1f;

		// Token: 0x040003AE RID: 942
		private float lowerDeadZone;

		// Token: 0x040003AF RID: 943
		private float upperDeadZone = 1f;

		// Token: 0x040003B0 RID: 944
		private float stateThreshold;

		// Token: 0x040003B1 RID: 945
		public bool Raw;

		// Token: 0x040003B2 RID: 946
		private bool thisState;

		// Token: 0x040003B3 RID: 947
		private bool lastState;

		// Token: 0x040003B4 RID: 948
		private Vector2 thisValue;

		// Token: 0x040003B5 RID: 949
		private Vector2 lastValue;

		// Token: 0x040003B6 RID: 950
		private bool clearInputState;
	}
}
