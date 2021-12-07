using System;

namespace InControl
{
	// Token: 0x0200005F RID: 95
	public class MouseBindingSourceListener : BindingSourceListener
	{
		// Token: 0x06000318 RID: 792 RVA: 0x00015C08 File Offset: 0x00014008
		public void Reset()
		{
			this.detectFound = Mouse.None;
			this.detectPhase = 0;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00015C18 File Offset: 0x00014018
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (this.detectFound != Mouse.None && !this.IsPressed(this.detectFound) && this.detectPhase == 2)
			{
				MouseBindingSource result = new MouseBindingSource(this.detectFound);
				this.Reset();
				return result;
			}
			Mouse mouse = this.ListenForControl(listenOptions);
			if (mouse != Mouse.None)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = mouse;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00015CA1 File Offset: 0x000140A1
		private bool IsPressed(Mouse control)
		{
			if (control == Mouse.NegativeScrollWheel)
			{
				return MouseBindingSource.NegativeScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold);
			}
			if (control != Mouse.PositiveScrollWheel)
			{
				return MouseBindingSource.ButtonIsPressed(control);
			}
			return MouseBindingSource.PositiveScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00015CD4 File Offset: 0x000140D4
		private Mouse ListenForControl(BindingListenOptions listenOptions)
		{
			if (listenOptions.IncludeMouseButtons)
			{
				for (Mouse mouse = Mouse.None; mouse <= Mouse.Button9; mouse++)
				{
					if (MouseBindingSource.ButtonIsPressed(mouse))
					{
						return mouse;
					}
				}
			}
			if (listenOptions.IncludeMouseScrollWheel)
			{
				if (MouseBindingSource.NegativeScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold))
				{
					return Mouse.NegativeScrollWheel;
				}
				if (MouseBindingSource.PositiveScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold))
				{
					return Mouse.PositiveScrollWheel;
				}
			}
			return Mouse.None;
		}

		// Token: 0x04000299 RID: 665
		public static float ScrollWheelThreshold = 0.001f;

		// Token: 0x0400029A RID: 666
		private Mouse detectFound;

		// Token: 0x0400029B RID: 667
		private int detectPhase;
	}
}
