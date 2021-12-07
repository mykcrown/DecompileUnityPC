// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CharacterActionSet : IEnumerable
{
	private Dictionary<ActionState, CharacterActionData> lookup = new Dictionary<ActionState, CharacterActionData>(default(ActionStateComparer));

	public List<CharacterActionData> characterActions = new List<CharacterActionData>();

	public CharacterActionData idle
	{
		get
		{
			return this.GetAction(ActionState.Idle, true);
		}
	}

	public CharacterActionData walkFast
	{
		get
		{
			return this.GetAction(ActionState.WalkFast, true);
		}
	}

	public CharacterActionData walkMedium
	{
		get
		{
			return this.GetAction(ActionState.WalkMedium, true);
		}
	}

	public CharacterActionData walkSlow
	{
		get
		{
			return this.GetAction(ActionState.WalkSlow, true);
		}
	}

	public CharacterActionData wavedash
	{
		get
		{
			return this.GetAction(ActionState.Wavedash, true);
		}
	}

	public CharacterActionData run
	{
		get
		{
			return this.GetAction(ActionState.Run, true);
		}
	}

	public CharacterActionData takeOff
	{
		get
		{
			return this.GetAction(ActionState.TakeOff, true);
		}
	}

	public CharacterActionData jumpStraight
	{
		get
		{
			return this.GetAction(ActionState.JumpStraight, true);
		}
	}

	public CharacterActionData jumpBack
	{
		get
		{
			return this.GetAction(ActionState.JumpBack, true);
		}
	}

	public CharacterActionData jumpForward
	{
		get
		{
			return this.GetAction(ActionState.JumpForward, true);
		}
	}

	public CharacterActionData airJump
	{
		get
		{
			return this.GetAction(ActionState.AirJump, true);
		}
	}

	public CharacterActionData fallStraight
	{
		get
		{
			return this.GetAction(ActionState.FallStraight, true);
		}
	}

	public CharacterActionData fallBack
	{
		get
		{
			return this.GetAction(ActionState.FallBack, true);
		}
	}

	public CharacterActionData fallForward
	{
		get
		{
			return this.GetAction(ActionState.FallForward, true);
		}
	}

	public CharacterActionData fallHelpless
	{
		get
		{
			return this.GetAction(ActionState.FallHelpless, true);
		}
	}

	public CharacterActionData landing
	{
		get
		{
			return this.GetAction(ActionState.Landing, true);
		}
	}

	public CharacterActionData crouching
	{
		get
		{
			return this.GetAction(ActionState.Crouching, true);
		}
	}

	public CharacterActionData crouchBegin
	{
		get
		{
			return this.GetAction(ActionState.CrouchBegin, true);
		}
	}

	public CharacterActionData crouchEnd
	{
		get
		{
			return this.GetAction(ActionState.CrouchEnd, true);
		}
	}

	public CharacterActionData platformDrop
	{
		get
		{
			return this.GetAction(ActionState.PlatformDrop, true);
		}
	}

	public CharacterActionData shieldLoop
	{
		get
		{
			return this.GetAction(ActionState.ShieldLoop, true);
		}
	}

	public CharacterActionData shieldBegin
	{
		get
		{
			return this.GetAction(ActionState.ShieldBegin, true);
		}
	}

	public CharacterActionData shieldEnd
	{
		get
		{
			return this.GetAction(ActionState.ShieldEnd, true);
		}
	}

	public CharacterActionData fallDown
	{
		get
		{
			return this.GetAction(ActionState.FallDown, true);
		}
	}

	public CharacterActionData downedLoop
	{
		get
		{
			return this.GetAction(ActionState.DownedLoop, true);
		}
	}

	public CharacterActionData recoil
	{
		get
		{
			return this.GetAction(ActionState.Recoil, true);
		}
	}

	public CharacterActionData death
	{
		get
		{
			return this.GetAction(ActionState.Death, true);
		}
	}

	public CharacterActionData tumble
	{
		get
		{
			return this.GetAction(ActionState.Tumble, true);
		}
	}

	public CharacterActionData dazedBegin
	{
		get
		{
			return this.GetAction(ActionState.DazedBegin, true);
		}
	}

	public CharacterActionData dazedLoop
	{
		get
		{
			return this.GetAction(ActionState.DazedLoop, true);
		}
	}

	public CharacterActionData dazedEnd
	{
		get
		{
			return this.GetAction(ActionState.DazedEnd, true);
		}
	}

	public CharacterActionData edgeGrab
	{
		get
		{
			return this.GetAction(ActionState.EdgeGrab, true);
		}
	}

	public CharacterActionData edgeHang
	{
		get
		{
			return this.GetAction(ActionState.EdgeHang, true);
		}
	}

	public CharacterActionData teeterLoop
	{
		get
		{
			return this.GetAction(ActionState.TeeterLoop, true);
		}
	}

	public CharacterActionData teeterBegin
	{
		get
		{
			return this.GetAction(ActionState.TeeterBegin, true);
		}
	}

	public CharacterActionData grabbing
	{
		get
		{
			return this.GetAction(ActionState.Grabbing, true);
		}
	}

	public CharacterActionData grabbedBegin
	{
		get
		{
			return this.GetAction(ActionState.GrabbedBegin, true);
		}
	}

	public CharacterActionData grabbedLoop
	{
		get
		{
			return this.GetAction(ActionState.GrabbedLoop, true);
		}
	}

	public CharacterActionData grabbedPummelled
	{
		get
		{
			return this.GetAction(ActionState.GrabbedPummelled, true);
		}
	}

	public CharacterActionData grabEscapeGround
	{
		get
		{
			return this.GetAction(ActionState.GrabEscapeGround, true);
		}
	}

	public CharacterActionData grabEscapeAir
	{
		get
		{
			return this.GetAction(ActionState.GrabEscapeAir, true);
		}
	}

	public CharacterActionData grabRelease
	{
		get
		{
			return this.GetAction(ActionState.GrabRelease, true);
		}
	}

	public CharacterActionData thrown
	{
		get
		{
			return this.GetAction(ActionState.Thrown, true);
		}
	}

	public CharacterActionData dash
	{
		get
		{
			return this.GetAction(ActionState.Dash, true);
		}
	}

	public CharacterActionData dashPivot
	{
		get
		{
			return this.GetAction(ActionState.DashPivot, true);
		}
	}

	public CharacterActionData dashBrake
	{
		get
		{
			return this.GetAction(ActionState.DashBrake, true);
		}
	}

	public CharacterActionData brake
	{
		get
		{
			return this.GetAction(ActionState.Brake, true);
		}
	}

	public CharacterActionData runPivot
	{
		get
		{
			return this.GetAction(ActionState.RunPivot, true);
		}
	}

	public CharacterActionData runPivotBrake
	{
		get
		{
			return this.GetAction(ActionState.RunPivotBrake, true);
		}
	}

	public CharacterActionData pivot
	{
		get
		{
			return this.GetAction(ActionState.Pivot, true);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.characterActions.GetEnumerator();
	}

	public static bool isOptionalState(ActionState state)
	{
		return state == ActionState.JumpForward || state == ActionState.JumpBack;
	}

	public CharacterActionData GetAction(ActionState characterActionState, bool createIfNull = false)
	{
		if (this.lookup.ContainsKey(characterActionState))
		{
			return this.lookup[characterActionState];
		}
		foreach (CharacterActionData current in this.characterActions)
		{
			if (current.characterActionState == characterActionState)
			{
				this.lookup[characterActionState] = current;
				return current;
			}
		}
		if (createIfNull)
		{
			CharacterActionData characterActionData = new CharacterActionData(characterActionState, characterActionState.ToString());
			this.characterActions.Add(characterActionData);
			this.lookup[characterActionState] = characterActionData;
		}
		else
		{
			this.lookup[characterActionState] = null;
		}
		return this.lookup[characterActionState];
	}
}
