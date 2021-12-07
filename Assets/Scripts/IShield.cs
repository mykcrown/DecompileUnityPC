// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public interface IShield : IRollbackStateOwner, IDestroyable
{
	Fixed ShieldHealth
	{
		get;
		set;
	}

	bool IsActive
	{
		get;
	}

	bool IsBroken
	{
		get;
	}

	bool IsGusting
	{
		get;
	}

	bool GustSuccess
	{
		get;
	}

	int ShieldBeginFrame
	{
		get;
	}

	bool WasRunning
	{
		get;
	}

	bool CanBeginGusting
	{
		get;
	}

	ShieldConfig Data
	{
		get;
	}

	GustShieldData GustData
	{
		get;
	}

	List<Hit> ShieldHits
	{
		get;
	}

	Vector3F ShieldPosition
	{
		get;
	}

	Fixed ShieldPercentage
	{
		get;
	}

	void Initialize(IPlayerDelegate player, ShieldConfig data, MoveData[] gustShieldMoves, IFrameOwner frameOwner);

	void BeginGusting();

	void BreakShield();

	void OnShieldBegin(bool wasRunning);

	void OnShieldReleased();

	void ResetHealth();

	void OnEndGust(InputButtonsData input);

	void TickFrame(InputButtonsData input);

	bool TryToGustObject(IHitOwner other);

	void OnHit(HitData data, IHitOwner other);
}
