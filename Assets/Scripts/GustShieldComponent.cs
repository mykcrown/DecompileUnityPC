// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class GustShieldComponent : MoveComponent, IMoveRequirementValidator, ICloneable
{
	public GustShieldData data;

	private IShield shield;

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public override bool ValidateRequirements(MoveData move, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		this.shield = playerDelegate.Shield;
		return this.shield.CanBeginGusting;
	}

	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		this.shield = playerDelegate.Shield;
		if (!this.shield.CanBeginGusting)
		{
			UnityEngine.Debug.LogError("GustShieldComponent was initialized, but it is illegal to begin gusting now!");
		}
		else
		{
			this.shield.BeginGusting();
		}
	}

	public override void RegisterPreload(PreloadContext context)
	{
		base.RegisterPreload(context);
		if (this.data.gustParticle != null)
		{
			this.data.gustParticle.RegisterPreload(context);
		}
	}
}
