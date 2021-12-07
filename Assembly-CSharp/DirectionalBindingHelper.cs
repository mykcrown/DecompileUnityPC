using System;
using System.Collections.Generic;
using InControl;

// Token: 0x02000681 RID: 1665
public class DirectionalBindingHelper
{
	// Token: 0x06002929 RID: 10537 RVA: 0x000DAE94 File Offset: 0x000D9294
	public DirectionalBindingHelper.DirectionalDevice GetDeviceFromBinding(BindingSource binding)
	{
		DeviceBindingSource deviceBindingSource = binding as DeviceBindingSource;
		if (deviceBindingSource != null)
		{
			foreach (DirectionalBindingHelper.DirectionalDevice directionalDevice in this.directionBindingLookup.Keys)
			{
				Dictionary<AbsoluteDirection, InputControlType>.ValueCollection values = this.directionBindingLookup[directionalDevice].Values;
				foreach (InputControlType inputControlType in values)
				{
					if (inputControlType == deviceBindingSource.Control)
					{
						return directionalDevice;
					}
				}
			}
			return DirectionalBindingHelper.DirectionalDevice.None;
		}
		return DirectionalBindingHelper.DirectionalDevice.None;
	}

	// Token: 0x0600292A RID: 10538 RVA: 0x000DAF70 File Offset: 0x000D9370
	public BindingSource GetBindingFromDevice(DirectionalBindingHelper.DirectionalDevice directionalInput, AbsoluteDirection direction)
	{
		if (this.directionBindingLookup.ContainsKey(directionalInput))
		{
			InputControlType inputControlType = this.directionBindingLookup[directionalInput][direction];
			if (inputControlType != InputControlType.None)
			{
				return new DeviceBindingSource(inputControlType);
			}
		}
		return null;
	}

	// Token: 0x0600292B RID: 10539 RVA: 0x000DAFAF File Offset: 0x000D93AF
	public ButtonPress GetButtonFromButtonGroup(DirectionalBindingHelper.DirectionalButtonGroup buttonGroup, AbsoluteDirection direction)
	{
		if (this.directionalButtonLookup.ContainsKey(buttonGroup))
		{
			return this.directionalButtonLookup[buttonGroup][direction];
		}
		return ButtonPress.None;
	}

	// Token: 0x04001DC6 RID: 7622
	public List<DirectionalBindingHelper.DirectionalDevice> LegalDirectionalInputs = new List<DirectionalBindingHelper.DirectionalDevice>
	{
		DirectionalBindingHelper.DirectionalDevice.None,
		DirectionalBindingHelper.DirectionalDevice.LeftStick,
		DirectionalBindingHelper.DirectionalDevice.RightStick,
		DirectionalBindingHelper.DirectionalDevice.DPad
	};

	// Token: 0x04001DC7 RID: 7623
	private Dictionary<DirectionalBindingHelper.DirectionalDevice, Dictionary<AbsoluteDirection, InputControlType>> directionBindingLookup = new Dictionary<DirectionalBindingHelper.DirectionalDevice, Dictionary<AbsoluteDirection, InputControlType>>
	{
		{
			DirectionalBindingHelper.DirectionalDevice.None,
			new Dictionary<AbsoluteDirection, InputControlType>
			{
				{
					AbsoluteDirection.Up,
					InputControlType.None
				},
				{
					AbsoluteDirection.Down,
					InputControlType.None
				},
				{
					AbsoluteDirection.Left,
					InputControlType.None
				},
				{
					AbsoluteDirection.Right,
					InputControlType.None
				}
			}
		},
		{
			DirectionalBindingHelper.DirectionalDevice.LeftStick,
			new Dictionary<AbsoluteDirection, InputControlType>
			{
				{
					AbsoluteDirection.Up,
					InputControlType.LeftStickUp
				},
				{
					AbsoluteDirection.Down,
					InputControlType.LeftStickDown
				},
				{
					AbsoluteDirection.Left,
					InputControlType.LeftStickLeft
				},
				{
					AbsoluteDirection.Right,
					InputControlType.LeftStickRight
				}
			}
		},
		{
			DirectionalBindingHelper.DirectionalDevice.RightStick,
			new Dictionary<AbsoluteDirection, InputControlType>
			{
				{
					AbsoluteDirection.Up,
					InputControlType.RightStickUp
				},
				{
					AbsoluteDirection.Down,
					InputControlType.RightStickDown
				},
				{
					AbsoluteDirection.Left,
					InputControlType.RightStickLeft
				},
				{
					AbsoluteDirection.Right,
					InputControlType.RightStickRight
				}
			}
		},
		{
			DirectionalBindingHelper.DirectionalDevice.DPad,
			new Dictionary<AbsoluteDirection, InputControlType>
			{
				{
					AbsoluteDirection.Up,
					InputControlType.DPadUp
				},
				{
					AbsoluteDirection.Down,
					InputControlType.DPadDown
				},
				{
					AbsoluteDirection.Left,
					InputControlType.DPadLeft
				},
				{
					AbsoluteDirection.Right,
					InputControlType.DPadRight
				}
			}
		}
	};

	// Token: 0x04001DC8 RID: 7624
	private Dictionary<DirectionalBindingHelper.DirectionalButtonGroup, Dictionary<AbsoluteDirection, ButtonPress>> directionalButtonLookup = new Dictionary<DirectionalBindingHelper.DirectionalButtonGroup, Dictionary<AbsoluteDirection, ButtonPress>>
	{
		{
			DirectionalBindingHelper.DirectionalButtonGroup.None,
			new Dictionary<AbsoluteDirection, ButtonPress>
			{
				{
					AbsoluteDirection.Up,
					ButtonPress.None
				},
				{
					AbsoluteDirection.Down,
					ButtonPress.None
				},
				{
					AbsoluteDirection.Left,
					ButtonPress.None
				},
				{
					AbsoluteDirection.Right,
					ButtonPress.None
				}
			}
		},
		{
			DirectionalBindingHelper.DirectionalButtonGroup.Movement,
			new Dictionary<AbsoluteDirection, ButtonPress>
			{
				{
					AbsoluteDirection.Up,
					ButtonPress.Up
				},
				{
					AbsoluteDirection.Down,
					ButtonPress.Down
				},
				{
					AbsoluteDirection.Left,
					ButtonPress.Backward
				},
				{
					AbsoluteDirection.Right,
					ButtonPress.Forward
				}
			}
		},
		{
			DirectionalBindingHelper.DirectionalButtonGroup.Emotes,
			new Dictionary<AbsoluteDirection, ButtonPress>
			{
				{
					AbsoluteDirection.Up,
					ButtonPress.TauntUp
				},
				{
					AbsoluteDirection.Down,
					ButtonPress.TauntDown
				},
				{
					AbsoluteDirection.Left,
					ButtonPress.TauntLeft
				},
				{
					AbsoluteDirection.Right,
					ButtonPress.TauntRight
				}
			}
		},
		{
			DirectionalBindingHelper.DirectionalButtonGroup.Tilts,
			new Dictionary<AbsoluteDirection, ButtonPress>
			{
				{
					AbsoluteDirection.Up,
					ButtonPress.UpTilt
				},
				{
					AbsoluteDirection.Down,
					ButtonPress.DownTilt
				},
				{
					AbsoluteDirection.Left,
					ButtonPress.BackwardTilt
				},
				{
					AbsoluteDirection.Right,
					ButtonPress.ForwardTilt
				}
			}
		},
		{
			DirectionalBindingHelper.DirectionalButtonGroup.Strikes,
			new Dictionary<AbsoluteDirection, ButtonPress>
			{
				{
					AbsoluteDirection.Up,
					ButtonPress.UpStrike
				},
				{
					AbsoluteDirection.Down,
					ButtonPress.DownStrike
				},
				{
					AbsoluteDirection.Left,
					ButtonPress.BackwardStrike
				},
				{
					AbsoluteDirection.Right,
					ButtonPress.ForwardStrike
				}
			}
		},
		{
			DirectionalBindingHelper.DirectionalButtonGroup.Specials,
			new Dictionary<AbsoluteDirection, ButtonPress>
			{
				{
					AbsoluteDirection.Up,
					ButtonPress.UpSpecial
				},
				{
					AbsoluteDirection.Down,
					ButtonPress.DownSpecial
				},
				{
					AbsoluteDirection.Left,
					ButtonPress.BackwardSpecial
				},
				{
					AbsoluteDirection.Right,
					ButtonPress.ForwardSpecial
				}
			}
		}
	};

	// Token: 0x02000682 RID: 1666
	public enum DirectionalDevice
	{
		// Token: 0x04001DCA RID: 7626
		None,
		// Token: 0x04001DCB RID: 7627
		LeftStick,
		// Token: 0x04001DCC RID: 7628
		RightStick,
		// Token: 0x04001DCD RID: 7629
		DPad
	}

	// Token: 0x02000683 RID: 1667
	public enum DirectionalButtonGroup
	{
		// Token: 0x04001DCF RID: 7631
		None,
		// Token: 0x04001DD0 RID: 7632
		Movement,
		// Token: 0x04001DD1 RID: 7633
		Emotes,
		// Token: 0x04001DD2 RID: 7634
		Tilts,
		// Token: 0x04001DD3 RID: 7635
		Strikes,
		// Token: 0x04001DD4 RID: 7636
		Specials
	}
}
