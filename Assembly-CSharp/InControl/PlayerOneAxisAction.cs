using System;
using System.Diagnostics;

namespace InControl
{
	// Token: 0x02000062 RID: 98
	public class PlayerOneAxisAction : OneAxisInputControl
	{
		// Token: 0x06000370 RID: 880 RVA: 0x00017131 File Offset: 0x00015531
		internal PlayerOneAxisAction(PlayerAction negativeAction, PlayerAction positiveAction)
		{
			this.negativeAction = negativeAction;
			this.positiveAction = positiveAction;
			this.Raw = true;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000371 RID: 881 RVA: 0x00017150 File Offset: 0x00015550
		// (remove) Token: 0x06000372 RID: 882 RVA: 0x00017188 File Offset: 0x00015588
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000373 RID: 883 RVA: 0x000171BE File Offset: 0x000155BE
		// (set) Token: 0x06000374 RID: 884 RVA: 0x000171C6 File Offset: 0x000155C6
		public object UserData { get; set; }

		// Token: 0x06000375 RID: 885 RVA: 0x000171D0 File Offset: 0x000155D0
		internal void Update(ulong updateTick, float deltaTime)
		{
			this.ProcessActionUpdate(this.negativeAction);
			this.ProcessActionUpdate(this.positiveAction);
			float value = Utility.ValueFromSides(this.negativeAction, this.positiveAction);
			base.CommitWithValue(value, updateTick, deltaTime);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0001721C File Offset: 0x0001561C
		private void ProcessActionUpdate(PlayerAction action)
		{
			BindingSourceType lastInputType = this.LastInputType;
			if (action.UpdateTick > base.UpdateTick)
			{
				base.UpdateTick = action.UpdateTick;
				lastInputType = action.LastInputType;
			}
			if (this.LastInputType != lastInputType)
			{
				this.LastInputType = lastInputType;
				if (this.OnLastInputTypeChanged != null)
				{
					this.OnLastInputTypeChanged(lastInputType);
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000377 RID: 887 RVA: 0x0001727E File Offset: 0x0001567E
		// (set) Token: 0x06000378 RID: 888 RVA: 0x00017288 File Offset: 0x00015688
		[Obsolete("Please set this property on device controls directly. It does nothing here.")]
		public new float LowerDeadZone
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00017297 File Offset: 0x00015697
		// (set) Token: 0x0600037A RID: 890 RVA: 0x000172A0 File Offset: 0x000156A0
		[Obsolete("Please set this property on device controls directly. It does nothing here.")]
		public new float UpperDeadZone
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x040002C2 RID: 706
		private PlayerAction negativeAction;

		// Token: 0x040002C3 RID: 707
		private PlayerAction positiveAction;

		// Token: 0x040002C4 RID: 708
		public BindingSourceType LastInputType;
	}
}
