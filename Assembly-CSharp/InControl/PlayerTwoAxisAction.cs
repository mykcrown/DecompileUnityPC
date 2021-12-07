using System;
using System.Diagnostics;

namespace InControl
{
	// Token: 0x02000063 RID: 99
	public class PlayerTwoAxisAction : TwoAxisInputControl
	{
		// Token: 0x0600037B RID: 891 RVA: 0x00017837 File Offset: 0x00015C37
		internal PlayerTwoAxisAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
		{
			this.negativeXAction = negativeXAction;
			this.positiveXAction = positiveXAction;
			this.negativeYAction = negativeYAction;
			this.positiveYAction = positiveYAction;
			this.InvertXAxis = false;
			this.InvertYAxis = false;
			this.Raw = true;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00017871 File Offset: 0x00015C71
		// (set) Token: 0x0600037D RID: 893 RVA: 0x00017879 File Offset: 0x00015C79
		public bool InvertXAxis { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00017882 File Offset: 0x00015C82
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0001788A File Offset: 0x00015C8A
		public bool InvertYAxis { get; set; }

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000380 RID: 896 RVA: 0x00017894 File Offset: 0x00015C94
		// (remove) Token: 0x06000381 RID: 897 RVA: 0x000178CC File Offset: 0x00015CCC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00017902 File Offset: 0x00015D02
		// (set) Token: 0x06000383 RID: 899 RVA: 0x0001790A File Offset: 0x00015D0A
		public object UserData { get; set; }

		// Token: 0x06000384 RID: 900 RVA: 0x00017914 File Offset: 0x00015D14
		internal void Update(ulong updateTick, float deltaTime)
		{
			this.ProcessActionUpdate(this.negativeXAction);
			this.ProcessActionUpdate(this.positiveXAction);
			this.ProcessActionUpdate(this.negativeYAction);
			this.ProcessActionUpdate(this.positiveYAction);
			float x = Utility.ValueFromSides(this.negativeXAction, this.positiveXAction, this.InvertXAxis);
			float y = Utility.ValueFromSides(this.negativeYAction, this.positiveYAction, InputManager.InvertYAxis || this.InvertYAxis);
			base.UpdateWithAxes(x, y, updateTick, deltaTime);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000179AC File Offset: 0x00015DAC
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

		// Token: 0x040002C7 RID: 711
		private PlayerAction negativeXAction;

		// Token: 0x040002C8 RID: 712
		private PlayerAction positiveXAction;

		// Token: 0x040002C9 RID: 713
		private PlayerAction negativeYAction;

		// Token: 0x040002CA RID: 714
		private PlayerAction positiveYAction;

		// Token: 0x040002CD RID: 717
		public BindingSourceType LastInputType;
	}
}
