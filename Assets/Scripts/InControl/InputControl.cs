// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace InControl
{
	public class InputControl : OneAxisInputControl
	{
		public static readonly InputControl Null = new InputControl();

		public bool Passive;

		private ulong zeroTick;

		public string Handle
		{
			get;
			protected set;
		}

		public InputControlType Target
		{
			get;
			protected set;
		}

		public bool IsButton
		{
			get;
			protected set;
		}

		public bool IsAnalog
		{
			get;
			protected set;
		}

		internal bool IsOnZeroTick
		{
			get
			{
				return base.UpdateTick == this.zeroTick;
			}
		}

		public bool IsStandard
		{
			get
			{
				return Utility.TargetIsStandard(this.Target);
			}
		}

		private InputControl()
		{
			this.Handle = "None";
			this.Target = InputControlType.None;
			this.Passive = false;
			this.IsButton = false;
			this.IsAnalog = false;
		}

		public InputControl(string handle, InputControlType target)
		{
			this.Handle = handle;
			this.Target = target;
			this.Passive = false;
			this.IsButton = Utility.TargetIsButton(target);
			this.IsAnalog = !this.IsButton;
		}

		public InputControl(string handle, InputControlType target, bool passive) : this(handle, target)
		{
			this.Passive = passive;
		}

		internal void SetZeroTick()
		{
			this.zeroTick = base.UpdateTick;
		}
	}
}
