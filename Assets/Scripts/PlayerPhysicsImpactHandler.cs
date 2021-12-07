// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class PlayerPhysicsImpactHandler : IPhysicsImpactHandler
{
	public void HandleImpact(PhysicsContext context, CollisionData collision)
	{
		PhysicsModel model = context.model;
		bool flag = true;
		if (model.totalVelocity.magnitude == 0)
		{
			model.ClearVelocity(true, true, true, VelocityType.Total);
			flag = false;
		}
		if (Vector3F.Dot(model.totalVelocity.normalized, collision.normal) > (Fixed)0.1)
		{
			flag = false;
		}
		if (flag)
		{
			bool flag2 = collision.collisionType == CollisionType.TerrainCorner;
			TechType techType = this.getTechType(context, collision);
			if (techType != TechType.None && !flag2)
			{
				switch (techType)
				{
				case TechType.Ground:
					IL_AB:
					model.isGrounded = true;
					this.updateVelocityOnLand(model, collision);
					goto IL_CE;
				case TechType.Wall:
				case TechType.Ceiling:
					model.ClearVelocity(true, true, true, VelocityType.Total);
					goto IL_CE;
				}
				goto IL_AB;
				IL_CE:
				context.performTechCallback(techType, collision);
				context.model.RestoreVelocity = RestoreVelocityType.PreventRestore;
			}
			else if (context.shouldBounceCallback != null && context.shouldBounceCallback() && !flag2)
			{
				Vector3F vector3F = model.totalVelocity - 2 * Vector3F.Dot(model.totalVelocity, collision.normal) * collision.normal;
				if (!model.isGrounded)
				{
					Fixed other = 0;
					SurfaceType collisionSurfaceType = collision.CollisionSurfaceType;
					if (collisionSurfaceType != SurfaceType.Floor)
					{
						other = context.knockbackConfig.knockbackNonGroundBounceReduction;
					}
					else
					{
						other = context.knockbackConfig.knockbackGroundBounceReduction;
					}
					vector3F *= (Fixed)1.0 - other;
				}
				Fixed other2 = (!model.isGrounded) ? context.knockbackConfig.knockbackBounceThreshold : context.knockbackConfig.knockbackLiftOffThreshold;
				if (collision.CollisionSurfaceType == SurfaceType.Floor && vector3F.y < other2)
				{
					if (context.playerPhysicsModel.wasHit && context.landCallback != null)
					{
						context.playerPhysicsModel.wasHit = false;
						Vector3F totalVelocity = model.totalVelocity;
						context.landCallback(ref totalVelocity);
					}
					model.ClearVelocity(false, true, true, VelocityType.Total);
					this.updateVelocityOnLand(model, collision);
				}
				else
				{
					model.ClearVelocity(true, true, true, VelocityType.Movement);
					model.SetVelocity(vector3F, VelocityType.Knockback);
					model.ClearVelocity(false, false, true, VelocityType.Total);
					context.model.RestoreVelocity = RestoreVelocityType.PreventRestore;
					context.onKnockbackBounceCallback(collision);
				}
				if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Physics))
				{
					DebugDraw.Instance.CreateArrow(collision.point, collision.normal, 1f, Color.cyan, 30);
				}
			}
			else if (context.shouldMaintainVelocityCallback == null || !context.shouldMaintainVelocityCallback())
			{
				this.updateVelocityOnLand(model, collision);
				if (model.totalVelocity.magnitude > (Fixed)0.03 && DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Physics))
				{
					DebugDraw.Instance.CreateArrow(collision.point, collision.normal, 0.5f, Color.yellow, 30);
				}
				if (model.isGrounded && model.totalVelocity.y > 0 && model.totalVelocity.y < context.knockbackConfig.knockbackLiftOffThreshold)
				{
					model.ClearVelocity(false, true, false, VelocityType.Total);
				}
				if (FixedMath.Abs(model.totalVelocity.x) < (Fixed)0.01)
				{
					model.ClearVelocity(true, false, false, VelocityType.Total);
				}
				if (FixedMath.Abs(model.totalVelocity.y) < (Fixed)0.01)
				{
					model.ClearVelocity(false, true, false, VelocityType.Total);
				}
				if (model.isGrounded && model.acceleration.x == 0 && Vector3F.Dot(model.totalVelocity, model.groundedNormal) == 0 && FixedMath.Abs(model.totalVelocity.sqrMagnitude) < FixedMath.Pow(context.characterData.Friction * WTime.fixedDeltaTime, 2))
				{
					model.ClearVelocity(true, true, true, VelocityType.Total);
				}
			}
		}
	}

	private void updateVelocityOnLand(PhysicsModel model, CollisionData collision)
	{
		Vector3F vector3F = model.GetVelocity(VelocityType.Knockback);
		Vector3F vector3F2 = model.GetVelocity(VelocityType.Movement);
		Vector3F vector3F3 = model.GetVelocity(VelocityType.Forced);
		if (vector3F != Vector3F.zero)
		{
			Fixed d = Vector3F.Dot(vector3F, collision.normal);
			Vector3F b = d * collision.normal;
			vector3F -= b;
			model.ClearVelocity(true, true, true, VelocityType.Knockback);
			model.AddVelocity(ref vector3F, VelocityType.Knockback);
		}
		if (vector3F2 != Vector3F.zero)
		{
			Fixed d2 = Vector3F.Dot(vector3F2, collision.normal);
			Vector3F b2 = d2 * collision.normal;
			vector3F2 -= b2;
			model.ClearVelocity(true, true, true, VelocityType.Movement);
			model.AddVelocity(ref vector3F2, VelocityType.Movement);
		}
		if (vector3F3 != Vector3F.zero)
		{
			Fixed d3 = Vector3F.Dot(vector3F3, collision.normal);
			Vector3F b3 = d3 * collision.normal;
			vector3F3 -= b3;
			model.ClearVelocity(true, true, true, VelocityType.Forced);
			model.AddVelocity(ref vector3F3, VelocityType.Forced);
		}
	}

	private TechType getTechType(PhysicsContext context, CollisionData collisionData)
	{
		if (context.availableTechCallback == null)
		{
			return TechType.None;
		}
		if (context.performTechCallback == null)
		{
			return TechType.None;
		}
		return context.availableTechCallback(collisionData);
	}
}
