// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;

public class DirectionalBindingHelper
{
	public enum DirectionalDevice
	{
		None,
		LeftStick,
		RightStick,
		DPad
	}

	public enum DirectionalButtonGroup
	{
		None,
		Movement,
		Emotes,
		Tilts,
		Strikes,
		Specials
	}

	public List<DirectionalBindingHelper.DirectionalDevice> LegalDirectionalInputs = new List<DirectionalBindingHelper.DirectionalDevice>
	{
		DirectionalBindingHelper.DirectionalDevice.None,
		DirectionalBindingHelper.DirectionalDevice.LeftStick,
		DirectionalBindingHelper.DirectionalDevice.RightStick,
		DirectionalBindingHelper.DirectionalDevice.DPad
	};

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

	public DirectionalBindingHelper.DirectionalDevice GetDeviceFromBinding(BindingSource binding)
	{
		DeviceBindingSource deviceBindingSource = binding as DeviceBindingSource;
		if (deviceBindingSource != null)
		{
			foreach (DirectionalBindingHelper.DirectionalDevice current in this.directionBindingLookup.Keys)
			{
				Dictionary<AbsoluteDirection, InputControlType>.ValueCollection values = this.directionBindingLookup[current].Values;
				foreach (InputControlType current2 in values)
				{
					if (current2 == deviceBindingSource.Control)
					{
						return current;
					}
				}
			}
			return DirectionalBindingHelper.DirectionalDevice.None;
		}
		return DirectionalBindingHelper.DirectionalDevice.None;
	}

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

	public ButtonPress GetButtonFromButtonGroup(DirectionalBindingHelper.DirectionalButtonGroup buttonGroup, AbsoluteDirection direction)
	{
		if (this.directionalButtonLookup.ContainsKey(buttonGroup))
		{
			return this.directionalButtonLookup[buttonGroup][direction];
		}
		return ButtonPress.None;
	}
}
