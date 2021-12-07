// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IInvincibilityController : IRollbackStateOwner, ITickable
{
	bool IsInvincible
	{
		get;
	}

	bool IsProjectileInvincible
	{
		get;
	}

	bool IsFullyIntangible
	{
		get;
	}

	int InvincibilityFramesRemaining
	{
		get;
	}

	int ProjectileInvincibilityFramesRemaining
	{
		get;
	}

	void BeginInvincibility(int frames);

	void BeginLedgeIntangibility(int frames);

	void BeginGrabIntangibility(int frames);

	void BeginPlatformIntangibility();

	void EndPlatformIntangibility();

	void BeginMoveIntangibility(BodyPart[] parts = null);

	void EndMoveIntangibility();

	void EndGrabInvincibility();

	void BeginMoveProjectileIntangibility(BodyPart[] parts = null);

	void EndMoveProjectileIntangibility();

	void EndAllMoveIntangibility();

	void Clear();
}
