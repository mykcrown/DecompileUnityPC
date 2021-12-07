// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Threading;

namespace InControl
{
	public class PlayerTwoAxisAction : TwoAxisInputControl
	{
		private PlayerAction negativeXAction;

		private PlayerAction positiveXAction;

		private PlayerAction negativeYAction;

		private PlayerAction positiveYAction;

		public BindingSourceType LastInputType;

		public event Action<BindingSourceType> OnLastInputTypeChanged;

		public bool InvertXAxis
		{
			get;
			set;
		}

		public bool InvertYAxis
		{
			get;
			set;
		}

		public object UserData
		{
			get;
			set;
		}

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
