// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputActions : PlayerActionSet
{
	private static float RIGHT_STICK_LOWER_DEADZONE = 0.7f;

	public PlayerAction Left;

	public PlayerAction Right;

	public PlayerAction Up;

	public PlayerAction Down;

	public PlayerOneAxisAction Horizontal;

	public PlayerOneAxisAction Vertical;

	public PlayerAction UpStrike;

	public PlayerAction DownStrike;

	public PlayerAction LeftStrike;

	public PlayerAction RightStrike;

	public PlayerAction UpTilt;

	public PlayerAction DownTilt;

	public PlayerAction LeftTilt;

	public PlayerAction RightTilt;

	public PlayerAction UpSpecial;

	public PlayerAction DownSpecial;

	public PlayerAction LeftSpecial;

	public PlayerAction RightSpecial;

	public PlayerAction Jump;

	public PlayerAction FullJump;

	public PlayerAction ShortJump;

	public PlayerAction Shield1;

	public PlayerAction Shield2;

	public PlayerAction Grab;

	public PlayerAction Wavedash;

	public PlayerAction Start;

	public PlayerAction Taunt;

	public PlayerAction TauntLeft;

	public PlayerAction TauntRight;

	public PlayerAction TauntUp;

	public PlayerAction TauntDown;

	public PlayerAction Hologram;

	public PlayerAction GustShield;

	public PlayerAction Strike;

	public PlayerAction LightAttack;

	public PlayerAction Tilt;

	public PlayerAction Attack;

	public PlayerAction Special;

	public PlayerAction Submit;

	public PlayerAction Cancel;

	public PlayerAction SoloAssist;

	public PlayerAction SoloGust;

	public PlayerAction Run;

	private Dictionary<int, PlayerAction> buttonToAction;

	public Dictionary<int, PlayerAction> ButtonMappings
	{
		get
		{
			return this.buttonToAction;
		}
	}

	public PlayerInputActions()
	{
		this.Left = base.CreatePlayerAction("Move Left");
		this.Right = base.CreatePlayerAction("Move Right");
		this.Up = base.CreatePlayerAction("Move Up");
		this.Down = base.CreatePlayerAction("Move Down");
		this.Horizontal = base.CreateOneAxisPlayerAction(this.Left, this.Right);
		this.Vertical = base.CreateOneAxisPlayerAction(this.Down, this.Up);
		this.Horizontal.Raw = false;
		this.Vertical.Raw = false;
		this.UpStrike = base.CreatePlayerAction("Up Strike");
		this.DownStrike = base.CreatePlayerAction("Down Strike");
		this.LeftStrike = base.CreatePlayerAction("Left Strike");
		this.RightStrike = base.CreatePlayerAction("Right Strike");
		this.calibrateRightStick(this.UpStrike);
		this.calibrateRightStick(this.DownStrike);
		this.calibrateRightStick(this.LeftStrike);
		this.calibrateRightStick(this.RightStrike);
		this.UpTilt = base.CreatePlayerAction("Up Tilt");
		this.DownTilt = base.CreatePlayerAction("Down Tilt");
		this.LeftTilt = base.CreatePlayerAction("Left Tilt");
		this.RightTilt = base.CreatePlayerAction("Right Tilt");
		this.UpSpecial = base.CreatePlayerAction("Up Special");
		this.DownSpecial = base.CreatePlayerAction("Down Special");
		this.LeftSpecial = base.CreatePlayerAction("Left Special");
		this.RightSpecial = base.CreatePlayerAction("Right Special");
		this.Jump = base.CreatePlayerAction("Jump");
		this.FullJump = base.CreatePlayerAction("FullJump");
		this.ShortJump = base.CreatePlayerAction("ShortJump");
		this.Attack = base.CreatePlayerAction("Attack");
		this.Special = base.CreatePlayerAction("Special");
		this.Shield1 = base.CreatePlayerAction("Shield1");
		this.Shield2 = base.CreatePlayerAction("Shield2");
		this.Grab = base.CreatePlayerAction("Grab");
		this.GustShield = base.CreatePlayerAction("GustShield");
		this.Wavedash = base.CreatePlayerAction("Wavedash");
		this.Start = base.CreatePlayerAction("Start");
		this.Taunt = base.CreatePlayerAction("Taunt");
		this.Hologram = base.CreatePlayerAction("Hologram");
		this.Tilt = base.CreatePlayerAction("Tilt");
		this.Strike = base.CreatePlayerAction("Strike");
		this.LightAttack = base.CreatePlayerAction("LightAttack");
		this.TauntLeft = base.CreatePlayerAction("TauntLeft");
		this.TauntRight = base.CreatePlayerAction("TauntRight");
		this.TauntUp = base.CreatePlayerAction("TauntUp");
		this.TauntDown = base.CreatePlayerAction("TauntDown");
		this.Submit = base.CreatePlayerAction("Submit");
		this.Cancel = base.CreatePlayerAction("Cancel");
		this.SoloAssist = base.CreatePlayerAction("Solo Assist");
		this.SoloGust = base.CreatePlayerAction("Solo Gust");
		this.Run = base.CreatePlayerAction("Run");
		this.buttonToAction = new Dictionary<int, PlayerAction>
		{
			{
				5,
				this.Attack
			},
			{
				6,
				this.Special
			},
			{
				10,
				this.Jump
			},
			{
				45,
				this.FullJump
			},
			{
				26,
				this.ShortJump
			},
			{
				7,
				this.Shield1
			},
			{
				31,
				this.Shield2
			},
			{
				8,
				this.Grab
			},
			{
				20,
				this.Wavedash
			},
			{
				4,
				this.Start
			},
			{
				12,
				this.Tilt
			},
			{
				9,
				this.Taunt
			},
			{
				25,
				this.Hologram
			},
			{
				21,
				this.Submit
			},
			{
				22,
				this.Cancel
			},
			{
				23,
				this.GustShield
			},
			{
				36,
				this.Strike
			},
			{
				27,
				this.TauntLeft
			},
			{
				28,
				this.TauntRight
			},
			{
				29,
				this.TauntUp
			},
			{
				30,
				this.TauntDown
			},
			{
				34,
				this.LeftStrike
			},
			{
				35,
				this.RightStrike
			},
			{
				32,
				this.UpStrike
			},
			{
				33,
				this.DownStrike
			},
			{
				39,
				this.LeftTilt
			},
			{
				40,
				this.RightTilt
			},
			{
				37,
				this.UpTilt
			},
			{
				38,
				this.DownTilt
			},
			{
				43,
				this.LeftSpecial
			},
			{
				44,
				this.RightSpecial
			},
			{
				41,
				this.UpSpecial
			},
			{
				42,
				this.DownSpecial
			},
			{
				46,
				this.LightAttack
			},
			{
				47,
				this.SoloAssist
			},
			{
				48,
				this.SoloGust
			},
			{
				49,
				this.Run
			}
		};
	}

	public PlayerAction GetButtonAction(ButtonPress press)
	{
		if (this.buttonToAction.ContainsKey((int)press))
		{
			return this.buttonToAction[(int)press];
		}
		if (press != ButtonPress.None)
		{
			UnityEngine.Debug.LogWarning("Failed to find PlayerAction for " + press);
		}
		return null;
	}

	public ButtonPress GetButtonPressFromAction(PlayerAction action)
	{
		using (Dictionary<int, PlayerAction>.KeyCollection.Enumerator enumerator = this.buttonToAction.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ButtonPress current = (ButtonPress)enumerator.Current;
				if (this.buttonToAction[(int)current] == action)
				{
					ButtonPress result = current;
					return result;
				}
			}
		}
		if (action == this.Up)
		{
			return ButtonPress.Up;
		}
		if (action == this.Down)
		{
			return ButtonPress.Down;
		}
		if (action == this.Left)
		{
			return ButtonPress.Backward;
		}
		if (action == this.Right)
		{
			return ButtonPress.Forward;
		}
		return ButtonPress.None;
	}

	private void calibrateRightStick(PlayerAction action)
	{
		action.Raw = false;
		action.LowerDeadZone = PlayerInputActions.RIGHT_STICK_LOWER_DEADZONE;
	}

	public PlayerAction GetAxisAction(ButtonPress press)
	{
		switch (press)
		{
		case ButtonPress.Forward:
			return this.Right;
		case ButtonPress.Backward:
			return this.Left;
		case ButtonPress.Up:
			return this.Up;
		case ButtonPress.Down:
			return this.Down;
		default:
			return null;
		}
	}

	public static bool IsAxis(ButtonPress press)
	{
		switch (press)
		{
		case ButtonPress.Forward:
		case ButtonPress.Backward:
		case ButtonPress.Up:
		case ButtonPress.Down:
			return true;
		default:
			return false;
		}
	}
}
