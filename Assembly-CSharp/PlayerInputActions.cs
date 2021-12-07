using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;

// Token: 0x020006AB RID: 1707
public class PlayerInputActions : PlayerActionSet
{
	// Token: 0x06002A60 RID: 10848 RVA: 0x000DFB20 File Offset: 0x000DDF20
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

	// Token: 0x17000A5E RID: 2654
	// (get) Token: 0x06002A61 RID: 10849 RVA: 0x000E0072 File Offset: 0x000DE472
	public Dictionary<int, PlayerAction> ButtonMappings
	{
		get
		{
			return this.buttonToAction;
		}
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x000E007C File Offset: 0x000DE47C
	public PlayerAction GetButtonAction(ButtonPress press)
	{
		if (this.buttonToAction.ContainsKey((int)press))
		{
			return this.buttonToAction[(int)press];
		}
		if (press != ButtonPress.None)
		{
			Debug.LogWarning("Failed to find PlayerAction for " + press);
		}
		return null;
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x000E00C8 File Offset: 0x000DE4C8
	public ButtonPress GetButtonPressFromAction(PlayerAction action)
	{
		using (Dictionary<int, PlayerAction>.KeyCollection.Enumerator enumerator = this.buttonToAction.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ButtonPress buttonPress = (ButtonPress)enumerator.Current;
				if (this.buttonToAction[(int)buttonPress] == action)
				{
					return buttonPress;
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

	// Token: 0x06002A64 RID: 10852 RVA: 0x000E0178 File Offset: 0x000DE578
	private void calibrateRightStick(PlayerAction action)
	{
		action.Raw = false;
		action.LowerDeadZone = PlayerInputActions.RIGHT_STICK_LOWER_DEADZONE;
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x000E018C File Offset: 0x000DE58C
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

	// Token: 0x06002A66 RID: 10854 RVA: 0x000E01C6 File Offset: 0x000DE5C6
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

	// Token: 0x04001E78 RID: 7800
	private static float RIGHT_STICK_LOWER_DEADZONE = 0.7f;

	// Token: 0x04001E79 RID: 7801
	public PlayerAction Left;

	// Token: 0x04001E7A RID: 7802
	public PlayerAction Right;

	// Token: 0x04001E7B RID: 7803
	public PlayerAction Up;

	// Token: 0x04001E7C RID: 7804
	public PlayerAction Down;

	// Token: 0x04001E7D RID: 7805
	public PlayerOneAxisAction Horizontal;

	// Token: 0x04001E7E RID: 7806
	public PlayerOneAxisAction Vertical;

	// Token: 0x04001E7F RID: 7807
	public PlayerAction UpStrike;

	// Token: 0x04001E80 RID: 7808
	public PlayerAction DownStrike;

	// Token: 0x04001E81 RID: 7809
	public PlayerAction LeftStrike;

	// Token: 0x04001E82 RID: 7810
	public PlayerAction RightStrike;

	// Token: 0x04001E83 RID: 7811
	public PlayerAction UpTilt;

	// Token: 0x04001E84 RID: 7812
	public PlayerAction DownTilt;

	// Token: 0x04001E85 RID: 7813
	public PlayerAction LeftTilt;

	// Token: 0x04001E86 RID: 7814
	public PlayerAction RightTilt;

	// Token: 0x04001E87 RID: 7815
	public PlayerAction UpSpecial;

	// Token: 0x04001E88 RID: 7816
	public PlayerAction DownSpecial;

	// Token: 0x04001E89 RID: 7817
	public PlayerAction LeftSpecial;

	// Token: 0x04001E8A RID: 7818
	public PlayerAction RightSpecial;

	// Token: 0x04001E8B RID: 7819
	public PlayerAction Jump;

	// Token: 0x04001E8C RID: 7820
	public PlayerAction FullJump;

	// Token: 0x04001E8D RID: 7821
	public PlayerAction ShortJump;

	// Token: 0x04001E8E RID: 7822
	public PlayerAction Shield1;

	// Token: 0x04001E8F RID: 7823
	public PlayerAction Shield2;

	// Token: 0x04001E90 RID: 7824
	public PlayerAction Grab;

	// Token: 0x04001E91 RID: 7825
	public PlayerAction Wavedash;

	// Token: 0x04001E92 RID: 7826
	public PlayerAction Start;

	// Token: 0x04001E93 RID: 7827
	public PlayerAction Taunt;

	// Token: 0x04001E94 RID: 7828
	public PlayerAction TauntLeft;

	// Token: 0x04001E95 RID: 7829
	public PlayerAction TauntRight;

	// Token: 0x04001E96 RID: 7830
	public PlayerAction TauntUp;

	// Token: 0x04001E97 RID: 7831
	public PlayerAction TauntDown;

	// Token: 0x04001E98 RID: 7832
	public PlayerAction Hologram;

	// Token: 0x04001E99 RID: 7833
	public PlayerAction GustShield;

	// Token: 0x04001E9A RID: 7834
	public PlayerAction Strike;

	// Token: 0x04001E9B RID: 7835
	public PlayerAction LightAttack;

	// Token: 0x04001E9C RID: 7836
	public PlayerAction Tilt;

	// Token: 0x04001E9D RID: 7837
	public PlayerAction Attack;

	// Token: 0x04001E9E RID: 7838
	public PlayerAction Special;

	// Token: 0x04001E9F RID: 7839
	public PlayerAction Submit;

	// Token: 0x04001EA0 RID: 7840
	public PlayerAction Cancel;

	// Token: 0x04001EA1 RID: 7841
	public PlayerAction SoloAssist;

	// Token: 0x04001EA2 RID: 7842
	public PlayerAction SoloGust;

	// Token: 0x04001EA3 RID: 7843
	public PlayerAction Run;

	// Token: 0x04001EA4 RID: 7844
	private Dictionary<int, PlayerAction> buttonToAction;
}
