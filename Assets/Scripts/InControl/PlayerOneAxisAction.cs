// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Threading;

namespace InControl
{
	public class PlayerOneAxisAction : OneAxisInputControl
	{
		private PlayerAction negativeAction;

		private PlayerAction positiveAction;

		public BindingSourceType LastInputType;

		public event Action<BindingSourceType> OnLastInputTypeChanged;

		public object UserData
		{
			get;
			set;
		}

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

		internal PlayerOneAxisAction(PlayerAction negativeAction, PlayerAction positiveAction)
		{
			this.negativeAction = negativeAction;
			this.positiveAction = positiveAction;
			this.Raw = true;
		}

		internal void Update(ulong updateTick, float deltaTime)
		{
			this.ProcessActionUpdate(this.negativeAction);
			this.ProcessActionUpdate(this.positiveAction);
			float value = Utility.ValueFromSides(this.negativeAction, this.positiveAction);
			base.CommitWithValue(value, updateTick, deltaTime);
		}

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
	}
}
