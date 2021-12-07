using System;

namespace InControl
{
	// Token: 0x02000057 RID: 87
	public class DeviceBindingSourceListener : BindingSourceListener
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x00013D61 File Offset: 0x00012161
		public void Reset()
		{
			this.detectFound = InputControlType.None;
			this.detectPhase = 0;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00013D74 File Offset: 0x00012174
		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeControllers || device.IsUnknown)
			{
				return null;
			}
			if (this.detectFound != InputControlType.None && !this.IsPressed(this.detectFound, device) && this.detectPhase == 2)
			{
				DeviceBindingSource result = new DeviceBindingSource(this.detectFound);
				this.Reset();
				return result;
			}
			InputControlType inputControlType = this.ListenForControl(listenOptions, device);
			if (inputControlType != InputControlType.None)
			{
				if (this.detectPhase == 1)
				{
					this.detectFound = inputControlType;
					this.detectPhase = 2;
				}
			}
			else if (this.detectPhase == 0)
			{
				this.detectPhase = 1;
			}
			return null;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00013E17 File Offset: 0x00012217
		private bool IsPressed(InputControl control)
		{
			return Utility.AbsoluteIsOverThreshold(control.Value, 0.5f);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00013E29 File Offset: 0x00012229
		private bool IsPressed(InputControlType control, InputDevice device)
		{
			return this.IsPressed(device.GetControl(control));
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00013E38 File Offset: 0x00012238
		private InputControlType ListenForControl(BindingListenOptions listenOptions, InputDevice device)
		{
			if (device.IsKnown)
			{
				int count = device.Controls.Count;
				for (int i = 0; i < count; i++)
				{
					InputControl inputControl = device.Controls[i];
					if (inputControl != null && this.IsPressed(inputControl) && (listenOptions.IncludeNonStandardControls || inputControl.IsStandard))
					{
						InputControlType target = inputControl.Target;
						if (target != InputControlType.Command || !listenOptions.IncludeNonStandardControls)
						{
							return target;
						}
					}
				}
			}
			return InputControlType.None;
		}

		// Token: 0x04000201 RID: 513
		private InputControlType detectFound;

		// Token: 0x04000202 RID: 514
		private int detectPhase;
	}
}
