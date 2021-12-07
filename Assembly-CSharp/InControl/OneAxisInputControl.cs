using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000071 RID: 113
	public class OneAxisInputControl : IInputControl
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00015D82 File Offset: 0x00014182
		// (set) Token: 0x060003DF RID: 991 RVA: 0x00015D8A File Offset: 0x0001418A
		public ulong UpdateTick { get; protected set; }

		// Token: 0x060003E0 RID: 992 RVA: 0x00015D94 File Offset: 0x00014194
		private void PrepareForUpdate(ulong updateTick)
		{
			if (this.IsNull)
			{
				return;
			}
			if (updateTick < this.pendingTick)
			{
				throw new InvalidOperationException("Cannot be updated with an earlier tick.");
			}
			if (this.pendingCommit && updateTick != this.pendingTick)
			{
				throw new InvalidOperationException("Cannot be updated for a new tick until pending tick is committed.");
			}
			if (updateTick > this.pendingTick)
			{
				this.lastState = this.thisState;
				this.nextState.Reset();
				this.pendingTick = updateTick;
				this.pendingCommit = true;
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00015E17 File Offset: 0x00014217
		public bool UpdateWithState(bool state, ulong updateTick, float deltaTime)
		{
			if (this.IsNull)
			{
				return false;
			}
			this.PrepareForUpdate(updateTick);
			this.nextState.Set(state || this.nextState.State);
			return state;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00015E50 File Offset: 0x00014250
		public bool UpdateWithValue(float value, ulong updateTick, float deltaTime)
		{
			if (this.IsNull)
			{
				return false;
			}
			this.PrepareForUpdate(updateTick);
			if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
			{
				this.nextState.RawValue = value;
				if (!this.Raw)
				{
					value = Utility.ApplyDeadZone(value, this.lowerDeadZone, this.upperDeadZone);
				}
				this.nextState.Set(value, this.stateThreshold);
				return true;
			}
			return false;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00015ECC File Offset: 0x000142CC
		internal bool UpdateWithRawValue(float value, ulong updateTick, float deltaTime)
		{
			if (this.IsNull)
			{
				return false;
			}
			this.Raw = true;
			this.PrepareForUpdate(updateTick);
			if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
			{
				this.nextState.RawValue = value;
				this.nextState.Set(value, this.stateThreshold);
				return true;
			}
			return false;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00015F30 File Offset: 0x00014330
		internal void SetValue(float value, ulong updateTick)
		{
			if (this.IsNull)
			{
				return;
			}
			if (updateTick > this.pendingTick)
			{
				this.lastState = this.thisState;
				this.nextState.Reset();
				this.pendingTick = updateTick;
				this.pendingCommit = true;
			}
			this.nextState.RawValue = value;
			this.nextState.Set(value, this.StateThreshold);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00015F98 File Offset: 0x00014398
		public void ClearInputState()
		{
			this.lastState.Reset();
			this.thisState.Reset();
			this.nextState.Reset();
			this.wasRepeated = false;
			this.clearInputState = true;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00015FCC File Offset: 0x000143CC
		public void Commit()
		{
			if (this.IsNull)
			{
				return;
			}
			this.pendingCommit = false;
			this.thisState = this.nextState;
			if (this.clearInputState)
			{
				this.lastState = this.nextState;
				this.UpdateTick = this.pendingTick;
				this.clearInputState = false;
				return;
			}
			bool state = this.lastState.State;
			bool state2 = this.thisState.State;
			this.wasRepeated = false;
			if (state && !state2)
			{
				this.nextRepeatTime = 0f;
			}
			else if (state2)
			{
				if (state != state2)
				{
					this.nextRepeatTime = Time.realtimeSinceStartup + this.FirstRepeatDelay;
				}
				else if (Time.realtimeSinceStartup >= this.nextRepeatTime)
				{
					this.wasRepeated = true;
					this.nextRepeatTime = Time.realtimeSinceStartup + this.RepeatDelay;
				}
			}
			if (this.thisState != this.lastState)
			{
				this.UpdateTick = this.pendingTick;
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000160CD File Offset: 0x000144CD
		public void CommitWithState(bool state, ulong updateTick, float deltaTime)
		{
			this.UpdateWithState(state, updateTick, deltaTime);
			this.Commit();
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000160DF File Offset: 0x000144DF
		public void CommitWithValue(float value, ulong updateTick, float deltaTime)
		{
			this.UpdateWithValue(value, updateTick, deltaTime);
			this.Commit();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000160F4 File Offset: 0x000144F4
		internal void CommitWithSides(InputControl negativeSide, InputControl positiveSide, ulong updateTick, float deltaTime)
		{
			this.LowerDeadZone = Mathf.Max(negativeSide.LowerDeadZone, positiveSide.LowerDeadZone);
			this.UpperDeadZone = Mathf.Min(negativeSide.UpperDeadZone, positiveSide.UpperDeadZone);
			this.Raw = (negativeSide.Raw || positiveSide.Raw);
			float value = Utility.ValueFromSides(negativeSide.RawValue, positiveSide.RawValue);
			this.CommitWithValue(value, updateTick, deltaTime);
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00016165 File Offset: 0x00014565
		public bool State
		{
			get
			{
				return this.Enabled && this.thisState.State;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x00016180 File Offset: 0x00014580
		public bool LastState
		{
			get
			{
				return this.Enabled && this.lastState.State;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0001619B File Offset: 0x0001459B
		public float Value
		{
			get
			{
				return (!this.Enabled) ? 0f : this.thisState.Value;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000161BD File Offset: 0x000145BD
		public float LastValue
		{
			get
			{
				return (!this.Enabled) ? 0f : this.lastState.Value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x000161DF File Offset: 0x000145DF
		public float RawValue
		{
			get
			{
				return (!this.Enabled) ? 0f : this.thisState.RawValue;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x00016201 File Offset: 0x00014601
		internal float NextRawValue
		{
			get
			{
				return (!this.Enabled) ? 0f : this.nextState.RawValue;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00016223 File Offset: 0x00014623
		public bool HasChanged
		{
			get
			{
				return this.Enabled && this.thisState != this.lastState;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x00016244 File Offset: 0x00014644
		public bool IsPressed
		{
			get
			{
				return this.Enabled && this.thisState.State;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0001625F File Offset: 0x0001465F
		public bool WasPressed
		{
			get
			{
				return this.Enabled && this.thisState && !this.lastState;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0001628D File Offset: 0x0001468D
		public bool WasReleased
		{
			get
			{
				return this.Enabled && !this.thisState && this.lastState;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000162B8 File Offset: 0x000146B8
		public bool WasRepeated
		{
			get
			{
				return this.Enabled && this.wasRepeated;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x000162CE File Offset: 0x000146CE
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x000162D6 File Offset: 0x000146D6
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			set
			{
				this.sensitivity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x000162E4 File Offset: 0x000146E4
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x000162EC File Offset: 0x000146EC
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000162FA File Offset: 0x000146FA
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x00016302 File Offset: 0x00014702
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00016310 File Offset: 0x00014710
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x00016318 File Offset: 0x00014718
		public float StateThreshold
		{
			get
			{
				return this.stateThreshold;
			}
			set
			{
				this.stateThreshold = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00016326 File Offset: 0x00014726
		public bool IsNull
		{
			get
			{
				return object.ReferenceEquals(this, InputControl.Null);
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00016333 File Offset: 0x00014733
		public static implicit operator bool(OneAxisInputControl instance)
		{
			return instance.State;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001633B File Offset: 0x0001473B
		public static implicit operator float(OneAxisInputControl instance)
		{
			return instance.Value;
		}

		// Token: 0x04000394 RID: 916
		private float sensitivity = 1f;

		// Token: 0x04000395 RID: 917
		private float lowerDeadZone;

		// Token: 0x04000396 RID: 918
		private float upperDeadZone = 1f;

		// Token: 0x04000397 RID: 919
		private float stateThreshold;

		// Token: 0x04000398 RID: 920
		public float FirstRepeatDelay = 0.8f;

		// Token: 0x04000399 RID: 921
		public float RepeatDelay = 0.1f;

		// Token: 0x0400039A RID: 922
		public bool Raw;

		// Token: 0x0400039B RID: 923
		internal bool Enabled = true;

		// Token: 0x0400039C RID: 924
		private ulong pendingTick;

		// Token: 0x0400039D RID: 925
		private bool pendingCommit;

		// Token: 0x0400039E RID: 926
		private float nextRepeatTime;

		// Token: 0x0400039F RID: 927
		private float lastPressedTime;

		// Token: 0x040003A0 RID: 928
		private bool wasRepeated;

		// Token: 0x040003A1 RID: 929
		private bool clearInputState;

		// Token: 0x040003A2 RID: 930
		private InputControlState lastState;

		// Token: 0x040003A3 RID: 931
		private InputControlState nextState;

		// Token: 0x040003A4 RID: 932
		private InputControlState thisState;
	}
}
