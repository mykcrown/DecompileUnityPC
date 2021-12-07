// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSimulator
{
	public static Fixed MAX_SLOPE_ALLOWING_LANDING;

	private static Fixed CLIFF_PROBE_DISTANCE;

	private static Fixed RAYCAST_PROBE_OFFSET;

	private static Fixed RAYCAST_PROBE_DIST;

	public static Fixed GROUNDED_RAYCAST_TOLERACE;

	private static Fixed GROUNDED_RAYCAST_SPREAD;

	public static readonly int GroundLayer;

	public static readonly int PlatformLayer;

	public static readonly int PlatformUndersideLayer;

	public static readonly int HazardLayer;

	private static List<CollisionData> sharedCollisions;

	private EdgeData previousBoundsBuffer = new EdgeData(new Vector2F[4], true, EdgeData.CacheFlag.NoSurface);

	private static List<ColliderSegmentReference> segmentBuffer;

	private static List<CollisionData> platformCollisionBuffer;

	[Inject]
	public IPhysicsCalculator physicsCalculator
	{
		get;
		set;
	}

	public static int GroundMask
	{
		get;
		private set;
	}

	public static int PlatformMask
	{
		get;
		private set;
	}

	public static int PlatformUndersideMask
	{
		get;
		private set;
	}

	public static int GroundAndPlatformMask
	{
		get
		{
			return PhysicsSimulator.GroundMask | PhysicsSimulator.PlatformMask;
		}
	}

	public static int HazardsMask
	{
		get;
		private set;
	}

	static PhysicsSimulator()
	{
		PhysicsSimulator.MAX_SLOPE_ALLOWING_LANDING = (Fixed)0.4;
		PhysicsSimulator.CLIFF_PROBE_DISTANCE = (Fixed)0.13300000131130219;
		PhysicsSimulator.RAYCAST_PROBE_OFFSET = (Fixed)0.10000000149011612;
		PhysicsSimulator.RAYCAST_PROBE_DIST = (Fixed)0.20000000298023224;
		PhysicsSimulator.GROUNDED_RAYCAST_TOLERACE = (Fixed)0.01;
		PhysicsSimulator.GROUNDED_RAYCAST_SPREAD = (Fixed)0.01;
		PhysicsSimulator.GroundLayer = LayerMask.NameToLayer(Layers.Ground);
		PhysicsSimulator.PlatformLayer = LayerMask.NameToLayer(Layers.Platform);
		PhysicsSimulator.PlatformUndersideLayer = LayerMask.NameToLayer(Layers.PlatformUnderside);
		PhysicsSimulator.HazardLayer = LayerMask.NameToLayer(Layers.Hazards);
		PhysicsSimulator.sharedCollisions = new List<CollisionData>();
		PhysicsSimulator.segmentBuffer = new List<ColliderSegmentReference>(64);
		PhysicsSimulator.platformCollisionBuffer = new List<CollisionData>();
		PhysicsSimulator.GroundMask = 1 << PhysicsSimulator.GroundLayer;
		PhysicsSimulator.PlatformMask = 1 << PhysicsSimulator.PlatformLayer;
		PhysicsSimulator.PlatformUndersideMask = 1 << PhysicsSimulator.PlatformUndersideLayer;
		PhysicsSimulator.HazardsMask = 1 << PhysicsSimulator.HazardLayer;
	}

	public void AdvanceState(PhysicsContext context, int iterations)
	{
		for (int i = 0; i < iterations; i++)
		{
			this.tickAdvanceState(context);
		}
	}

	private void tickAdvanceState(PhysicsContext context)
	{
		PhysicsModel model = context.model;
		if (model.isGrounded && model.groundedMovingStageObject != null && model.groundedMovingStageObject.DeltaPosition != Vector3F.zero)
		{
			PhysicsMotionContext motionContext = context.motionContext;
			motionContext.travelDelta = model.groundedMovingStageObject.DeltaPosition;
			motionContext.maxTravelDist = 100;
			motionContext.distanceTraveled = 0;
			context.model.RestoreVelocity = RestoreVelocityType.None;
			if (context.playerPhysicsModel.teeteringDirection != HorizontalDirection.None)
			{
				context.playerPhysicsModel.teeteringPosition += model.movingPlatformDeltaPosition;
			}
			this.Translate(context, PhysicsSimulator.sharedCollisions, true);
		}
		if (context.shouldIgnoreForcesCallback != null && context.shouldIgnoreForcesCallback())
		{
			return;
		}
		Vector3F knockbackVelocity = model.knockbackVelocity;
		Vector3F movementVelocity = model.movementVelocity;
		Vector3F windVelocity = model.windVelocity;
		this.CalculateNextModelVelocity(context, !model.isGrounded, ref knockbackVelocity, ref movementVelocity, ref windVelocity);
		model.SetVelocity(knockbackVelocity, VelocityType.Knockback);
		model.SetVelocity(movementVelocity, VelocityType.Movement);
		model.SetVelocity(windVelocity, VelocityType.Wind);
		this.advancePosition(context);
		if (context.debugPrediction != null)
		{
			int debugPredictionFrame = context.debugPredictionFrame;
			if (debugPredictionFrame < context.debugPrediction.Length && context.debugPrediction[debugPredictionFrame] != context.model.position)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"PREDICTION MISMATCH, expected ",
					context.debugPrediction[debugPredictionFrame],
					" but got ",
					context.model.position
				}));
			}
			context.debugPredictionFrame++;
		}
	}

	public void CalculateNextModelVelocity(PhysicsContext context, bool applyGravity, ref Vector3F knockbackVelocity, ref Vector3F movementVelocity, ref Vector3F windVelocity)
	{
		PhysicsModel model = context.model;
		Fixed @fixed = Fixed.MaxValue;
		if (context.calculateMaxHorizontalSpeedCallback != null)
		{
			@fixed = context.calculateMaxHorizontalSpeedCallback();
		}
		Fixed fixed2 = Fixed.MaxValue;
		if (context.calculateMaxVerticalSpeedCallback != null)
		{
			fixed2 = context.calculateMaxVerticalSpeedCallback();
		}
		Vector3F b = model.acceleration * WTime.fixedDeltaTime;
		if (MathUtil.SignsMatch(movementVelocity.x, b.x))
		{
			Fixed one = movementVelocity.x + b.x;
			if (one > @fixed)
			{
				b.x = FixedMath.Max(@fixed - movementVelocity.x, 0);
			}
			else if (one < -@fixed)
			{
				b.x = FixedMath.Min(-@fixed - movementVelocity.x, 0);
			}
		}
		if (MathUtil.SignsMatch(movementVelocity.y, b.y))
		{
			Fixed one2 = movementVelocity.y + b.y;
			if (one2 > fixed2)
			{
				b.y = FixedMath.Max(fixed2 - movementVelocity.y, 0);
			}
			else if (one2 < -fixed2)
			{
				b.y = FixedMath.Min(-fixed2 - movementVelocity.y, 0);
			}
		}
		movementVelocity += b;
		if (knockbackVelocity.sqrMagnitude > 0)
		{
			Fixed d = FixedMath.Max(0, knockbackVelocity.magnitude - context.knockbackConfig.knockbackAirDrag * WTime.fixedDeltaTime);
			knockbackVelocity.Normalize();
			knockbackVelocity *= d;
		}
		if (windVelocity.sqrMagnitude > 0)
		{
			Fixed d2 = FixedMath.Max(0, windVelocity.magnitude - context.knockbackConfig.knockbackAirDrag * WTime.fixedDeltaTime);
			windVelocity.Normalize();
			windVelocity *= d2;
		}
		if (context.physicsState.PhysicsOverride != null && (context.physicsState.PhysicsOverride.dragValue != 0f || context.physicsState.PhysicsOverride.velocityMultiplier != 1))
		{
			Fixed fixed3 = FixedMath.Max(0, movementVelocity.magnitude - context.physicsState.PhysicsOverride.dragValue * WTime.fixedDeltaTime);
			fixed3 = FixedMath.Max(0, fixed3 * context.physicsState.PhysicsOverride.velocityMultiplier);
			movementVelocity.Normalize();
			movementVelocity *= fixed3;
		}
		if (context.knockbackConfig.cancelKnockbackAtZeroVelocity)
		{
			if (MathUtil.SignsMatch(-movementVelocity.x, knockbackVelocity.x) && FixedMath.Abs(movementVelocity.x) > FixedMath.Abs(knockbackVelocity.x))
			{
				movementVelocity.x += knockbackVelocity.x;
				knockbackVelocity.x = 0;
			}
			if (MathUtil.SignsMatch(-movementVelocity.y, knockbackVelocity.y) && FixedMath.Abs(movementVelocity.y) > FixedMath.Abs(knockbackVelocity.y))
			{
				movementVelocity.y += knockbackVelocity.y;
				knockbackVelocity.y = 0;
			}
		}
		if (movementVelocity.x != 0 || movementVelocity.y != 0)
		{
			movementVelocity = this.ApplyFrictionToVelocity(movementVelocity, context, @fixed, fixed2);
		}
		if (context.model.isGrounded && (knockbackVelocity.x != 0 || knockbackVelocity.y != 0))
		{
			knockbackVelocity = this.ApplyFrictionToVelocity(knockbackVelocity, context, 0, 0);
		}
		Fixed fixed4 = -this.physicsCalculator.GetGravity(context) * WTime.fixedDeltaTime;
		if (context.physicsState.PhysicsOverride != null)
		{
			fixed4 *= context.physicsState.PhysicsOverride.gravityMultiplier;
		}
		if (applyGravity)
		{
			bool flag = context.physicsState.PhysicsOverride != null && context.physicsState.PhysicsOverride.ignoreMaxFallSpeed;
			movementVelocity.y += fixed4;
			if (context.playerPhysicsModel != null && context.playerPhysicsModel.gravityAssistFrames > 0)
			{
				Fixed fixed5 = context.playerPhysicsModel.gravityAssistFrames / context.playerPhysicsModel.gravityAssistTotalFrames;
				fixed5 = FixedMath.Min(fixed5, 1);
				fixed5 *= fixed5;
				Fixed fixed6 = this.physicsCalculator.GetMaxDownwardVelocity(context);
				fixed6 -= fixed6 * fixed5;
				movementVelocity.y = FixedMath.Max(fixed6, movementVelocity.y);
			}
			else if (!flag)
			{
				movementVelocity.y = FixedMath.Max(this.physicsCalculator.GetMaxDownwardVelocity(context), movementVelocity.y);
			}
		}
	}

	private Vector3F ApplyFrictionToVelocity(Vector3F velocity, PhysicsContext context, Fixed maxHorizontalSpeed, Fixed maxVerticalSpeed)
	{
		Vector2F vector2F = Vector2F.zero;
		if (context.model.isGrounded)
		{
			if (context.knockbackConfig.ignoreFrictionInJumpSquat && context.playerDelegate != null && context.playerDelegate.State.ActionState == ActionState.TakeOff)
			{
				vector2F.x = 0;
			}
			else
			{
				vector2F.x = this.physicsCalculator.GetGroundFriction(context, ref velocity);
			}
		}
		else
		{
			vector2F = this.physicsCalculator.GetAirFriction(context);
		}
		if (context.physicsState.PhysicsOverride != null)
		{
			if (context.physicsState.PhysicsOverride.overrideFriction)
			{
				vector2F.x = (Fixed)((double)context.physicsState.PhysicsOverride.horizontalFrictionOverride);
			}
			else
			{
				vector2F.x *= context.physicsState.PhysicsOverride.horizontalFrictionMultiplier;
			}
		}
		if (velocity.x > maxHorizontalSpeed)
		{
			velocity.x = FixedMath.Max(velocity.x - vector2F.x * WTime.fixedDeltaTime, maxHorizontalSpeed);
			velocity.x = FixedMath.Max(0, velocity.x);
		}
		else if (velocity.x < -maxHorizontalSpeed)
		{
			velocity.x = FixedMath.Min(velocity.x + vector2F.x * WTime.fixedDeltaTime, -maxHorizontalSpeed);
			velocity.x = FixedMath.Min(0, velocity.x);
		}
		if (velocity.y > maxVerticalSpeed)
		{
			velocity.y = FixedMath.Max(velocity.y - vector2F.y * WTime.fixedDeltaTime, maxVerticalSpeed);
			velocity.y = FixedMath.Max(0, velocity.y);
		}
		else if (velocity.y < -maxVerticalSpeed)
		{
			velocity.y = FixedMath.Min(velocity.y + vector2F.y * WTime.fixedDeltaTime, -maxVerticalSpeed);
			velocity.y = FixedMath.Min(0, velocity.y);
		}
		return velocity;
	}

	private void advancePosition(PhysicsContext context)
	{
		PhysicsMotionContext motionContext = context.motionContext;
		motionContext.initialVelocity = context.model.totalVelocity;
		motionContext.initialMovementVelocity = context.model.movementVelocity;
		motionContext.initialKnockbackVelocity = context.model.knockbackVelocity;
		motionContext.travelDelta = motionContext.initialVelocity * WTime.fixedDeltaTime;
		motionContext.maxTravelDist = motionContext.travelDelta.magnitude;
		motionContext.distanceTraveled = 0;
		int num = 3;
		int num2 = 0;
		bool flag = false;
		bool flag2 = false;
		PhysicsSimulator.sharedCollisions.Clear();
		while (motionContext.distanceTraveled < motionContext.maxTravelDist && num2 < num)
		{
			num2++;
			context.model.RestoreVelocity = RestoreVelocityType.None;
			bool flag3 = false;
			motionContext.completedMovement = this.Translate(context, PhysicsSimulator.sharedCollisions, true);
			if (this.physicsCalculator.IsCheckGrounded(context))
			{
				this.CheckIfGrounded(context, PhysicsSimulator.sharedCollisions, motionContext.initialVelocity);
				flag2 = true;
			}
			flag |= (context.model.RestoreVelocity == RestoreVelocityType.Restore);
			if (PhysicsSimulator.sharedCollisions.Count > 0)
			{
				flag3 |= context.collisionMotion.HandleMotion(context, PhysicsSimulator.sharedCollisions);
			}
			if (motionContext.completedMovement || flag3)
			{
				break;
			}
		}
		if (num2 > num)
		{
			UnityEngine.Debug.LogError("Translation reached max iterations");
		}
		if (this.physicsCalculator.IsCheckGrounded(context) && !flag2)
		{
			this.CheckIfGrounded(context, PhysicsSimulator.sharedCollisions, motionContext.initialVelocity);
		}
		if (flag)
		{
			context.model.SetVelocity(motionContext.initialMovementVelocity, VelocityType.Movement);
			context.model.SetVelocity(motionContext.initialKnockbackVelocity, VelocityType.Knockback);
		}
	}

	public bool Translate(PhysicsContext context, List<CollisionData> collisions, bool detectCliffs = true)
	{
		PhysicsMotionContext motionContext = context.motionContext;
		if (detectCliffs && (motionContext.travelDelta.x != 0 || motionContext.travelDelta.y != 0) && context.cliffProtectionCallback != null && context.cliffProtectionCallback() && this.DetectCliff(context, motionContext.travelDelta))
		{
			context.onCliffProtection(motionContext.travelDelta, context.model.position);
			motionContext.travelDelta = Vector3F.zero;
			return false;
		}
		bool flag = false;
		if (!this.physicsCalculator.IgnoreCollisions(context))
		{
			flag = this.findCollisions(context, ref motionContext.travelDelta, collisions);
		}
		if (flag)
		{
			CollisionData collision = collisions[0];
			motionContext.distanceTraveled += this.handleCollision(context, collision);
			if (context.playerPhysicsModel != null && collision.CollisionSurfaceType != SurfaceType.Floor)
			{
				context.model.RestoreVelocity = RestoreVelocityType.Restore;
			}
			if (collision.collisionType == CollisionType.TerrainCorner)
			{
				context.model.RestoreVelocity = RestoreVelocityType.Restore;
			}
			motionContext.maxTravelDist = context.model.totalVelocity.magnitude * WTime.fixedDeltaTime;
			motionContext.travelDelta = FixedMath.Max(motionContext.maxTravelDist - motionContext.distanceTraveled, 0) * context.model.totalVelocity.normalized;
			return motionContext.travelDelta.magnitude == 0;
		}
		context.model.position += motionContext.travelDelta;
		motionContext.distanceTraveled += motionContext.travelDelta.magnitude;
		return true;
	}

	private Fixed handleCollision(PhysicsContext context, CollisionData collision)
	{
		Fixed result = 0;
		context.model.position += collision.deltaToCollision;
		PhysicsUtil.UpdateContextCollider(context);
		if (Vector3F.Dot(collision.deltaToCollision, context.model.totalVelocity) > 0)
		{
			result = collision.deltaToCollision.magnitude;
		}
		if (context.impactHandler != null)
		{
			context.impactHandler.HandleImpact(context, collision);
		}
		return result;
	}

	public void CheckIfGrounded(PhysicsContext context, List<CollisionData> collisions, Vector3F initialVelocity)
	{
		bool flag = false;
		if (collisions != null && context.shouldCheckGrounded != null && context.shouldCheckGrounded())
		{
			for (int i = 0; i < collisions.Count; i++)
			{
				if (collisions[i].bottomBoundGrounded)
				{
					Fixed one = Vector3F.Dot(collisions[i].normal, Vector3F.up);
					if (one > PhysicsSimulator.MAX_SLOPE_ALLOWING_LANDING)
					{
						flag = true;
						this.makeGrounded(context, collisions[i].normal, initialVelocity, collisions[i].MovingObject, collisions[i].point);
					}
				}
			}
		}
		if (!flag && context.shouldCheckGrounded != null && context.shouldCheckGrounded())
		{
			Vector3F zero = Vector3F.zero;
			IMovingObject movingObject = null;
			Vector3F zero2 = Vector3F.zero;
			flag |= this.checkIfRaycastGrounded(context, initialVelocity, Vector2F.zero, out zero, out movingObject, out zero2);
			if (!flag)
			{
				flag |= this.checkIfRaycastGrounded(context, initialVelocity, Vector2F.left * PhysicsSimulator.GROUNDED_RAYCAST_SPREAD, out zero, out movingObject, out zero2);
			}
			if (!flag)
			{
				flag |= this.checkIfRaycastGrounded(context, initialVelocity, Vector2F.right * PhysicsSimulator.GROUNDED_RAYCAST_SPREAD, out zero, out movingObject, out zero2);
			}
			if (flag)
			{
				if (!context.model.isGrounded && FixedMath.Abs(Vector3F.Dot(context.model.totalVelocity.normalized, context.model.groundedNormal)) < (Fixed)0.01)
				{
					this.makeGrounded(context, zero, initialVelocity, movingObject, zero2);
				}
				else if (context.model.groundedMovingStageObject != movingObject && movingObject != null)
				{
					context.model.groundedMovingStageObject = movingObject;
				}
			}
		}
		if (!flag)
		{
			if (context.model.isGrounded)
			{
				if (context.beginFallCallback != null)
				{
					context.beginFallCallback();
				}
				if (context.physicsState.CurrentMove == null)
				{
					this.CapHorizontalSpeed(context.model, this.physicsCalculator.GetMaxHorizontalAirVelocity(context), VelocityType.Movement);
				}
				if (context.fallCallback != null)
				{
					context.fallCallback();
				}
			}
			context.model.IsGrounded = false;
		}
	}

	private bool checkIfRaycastGrounded(PhysicsContext context, Vector3F initialVelocity, Vector2F originOffset, out Vector3F groundedNormalOut, out IMovingObject movingObjectOut, out Vector3F collisionPointOut)
	{
		RaycastData raycastData = default(RaycastData);
		int num = PhysicsSimulator.GroundMask;
		if (this.physicsCalculator.IsCheckPlatformForGrounded(context, initialVelocity))
		{
			num |= PhysicsSimulator.PlatformMask;
		}
		Fixed d = PhysicsSimulator.RAYCAST_PROBE_OFFSET;
		Fixed distance = PhysicsSimulator.RAYCAST_PROBE_DIST;
		if (context.physicsState.PhysicsOverride != null)
		{
			d = context.physicsState.PhysicsOverride.groundCheckUp;
			distance = context.physicsState.PhysicsOverride.groundCheckUp + context.physicsState.PhysicsOverride.groundCheckDown;
		}
		Vector2F a = Vector2F.up * d;
		Vector2F vector2F = context.model.position + context.model.bounds.centerOffset + context.model.bounds.down;
		vector2F += a + originOffset;
		bool firstRaycastHit = PhysicsRaycastCalculator.GetFirstRaycastHit(context, vector2F, Vector2F.down, distance, num, out raycastData.hit, PhysicsSimulator.GROUNDED_RAYCAST_TOLERACE);
		if (firstRaycastHit)
		{
			movingObjectOut = raycastData.hit.collider.MovingObject;
			groundedNormalOut = raycastData.hit.normal;
			Fixed one = Vector3F.Dot(groundedNormalOut, Vector3F.up);
			collisionPointOut = raycastData.hit.point;
			if (one > PhysicsSimulator.MAX_SLOPE_ALLOWING_LANDING && !context.IsFallingThroughPlatform(raycastData.hit.collider))
			{
				return Vector2F.Dot(raycastData.hit.normal, Vector2F.up) > PhysicsSimulator.MAX_SLOPE_ALLOWING_LANDING;
			}
		}
		groundedNormalOut = Vector3F.zero;
		movingObjectOut = null;
		collisionPointOut = Vector3F.zero;
		return false;
	}

	private void makeGrounded(PhysicsContext context, Vector3F groundedNormal, Vector3F initialVelocity, IMovingObject stageObject, Vector3F collisionPoint)
	{
		if (!context.model.isGrounded)
		{
			context.model.groundedNormal.Set(groundedNormal.x, groundedNormal.y, groundedNormal.z);
			context.model.groundedMovingStageObject = stageObject;
			if (context.landCallback != null && Vector3F.Dot(context.model.totalVelocity.normalized, context.model.groundedNormal) < (Fixed)0.01)
			{
				Vector3F b = collisionPoint - (context.model.bounds.down + context.model.center);
				context.model.position += b;
				context.model.ClearVelocity(false, true, true, VelocityType.Total);
				context.landCallback(ref initialVelocity);
			}
		}
	}

	private bool findCollisions(PhysicsContext context, ref Vector3F delta, List<CollisionData> outCollisions)
	{
		outCollisions.Clear();
		return this.detectCollisions(context, delta, context.model.center + delta, outCollisions);
	}

	public bool OnCollisionBoundsChanged(PhysicsContext context, EdgeData previousBoundsEdge, bool checkIfGrounded)
	{
		bool result = false;
		int num = 0;
		CollisionData collisionData;
		do
		{
			num++;
			PhysicsUtil.UpdateContextCollider(context);
			this.previousBoundsBuffer.SetPoints(context.collider.Edge);
			EdgeData points = this.previousBoundsBuffer;
			collisionData = PhysicsCollisionCalculator.CalculateExtraction(context, context.world.GetRelevantColliders(), previousBoundsEdge, PhysicsSimulator.GroundMask);
			previousBoundsEdge.SetPoints(points);
			if (collisionData.collisionType != CollisionType.None)
			{
				if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Physics))
				{
					DebugDraw.Instance.CreateArrow(collisionData.point, collisionData.deltaToCollision.normalized, 0.5f, new Color(1f, 0.5f, 0f), 30);
				}
				context.model.position += collisionData.deltaToCollision;
				if (checkIfGrounded)
				{
					this.CheckIfGrounded(context, null, context.model.totalVelocity);
				}
				result = true;
			}
		}
		while (collisionData.collisionType != CollisionType.None && num < 3);
		return result;
	}

	public bool detectCollisions(PhysicsContext context, Vector3F delta, Vector3F center, List<CollisionData> collisions)
	{
		collisions.Clear();
		bool flag = false;
		PhysicsUtil.UpdateContextCollider(context);
		FixedRect bounds = PhysicsUtil.ExtendBoundingBox(context.collider.BoundingBox, delta * 2);
		context.world.GetRelevantSegments(bounds, PhysicsSimulator.segmentBuffer);
		if (this.physicsCalculator.IsGroundCollision(context, delta))
		{
			flag |= PhysicsCollisionCalculator.DetectCollisions(context, PhysicsSimulator.segmentBuffer, delta, PhysicsSimulator.GroundMask, collisions);
		}
		if (this.physicsCalculator.IsPlatformCollision(context, delta))
		{
			int num = PhysicsSimulator.PlatformMask;
			if (this.physicsCalculator.IsPlatformUndersideCollision(context, delta))
			{
				num |= PhysicsSimulator.PlatformUndersideMask;
			}
			bool flag2 = PhysicsCollisionCalculator.DetectCollisions(context, PhysicsSimulator.segmentBuffer, delta, num, collisions);
			if (flag2)
			{
				IPhysicsCollider platformCollider = null;
				for (int i = collisions.Count - 1; i >= 0; i--)
				{
					if (collisions[i].IsPlatformCollision)
					{
						platformCollider = collisions[i].terrainCollider;
						break;
					}
				}
				if (context.IsFallingThroughPlatform(platformCollider))
				{
					for (int j = collisions.Count - 1; j >= 0; j--)
					{
						if (collisions[j].IsPlatformCollision)
						{
							collisions.RemoveAt(j);
						}
					}
				}
				if (context.isFallThroughPlatformsInput)
				{
					flag2 = false;
					this.removePlatformCollisions(collisions, PhysicsSimulator.platformCollisionBuffer);
				}
			}
			flag |= flag2;
		}
		return flag && collisions.Count > 0;
	}

	public bool DetectCliff(PhysicsContext context, Vector3F travelVec)
	{
		PhysicsModel model = context.model;
		Vector3F v = model.position + model.bounds.centerOffset + travelVec;
		Vector2F normalizedDirection = (!model.isGrounded) ? Vector3F.down : (model.groundedNormal * -1);
		Fixed maxDistance = FixedMath.Abs(model.bounds.down.y) + PhysicsSimulator.CLIFF_PROBE_DISTANCE;
		int groundAndPlatformMask = PhysicsSimulator.GroundAndPlatformMask;
		int num = context.world.RaycastTerrain(v, normalizedDirection, maxDistance, groundAndPlatformMask, null, RaycastFlags.Default, default(Fixed));
		return num == 0;
	}

	public void CapHorizontalSpeed(PhysicsModel model, Fixed speed, VelocityType velocityType)
	{
		if (FixedMath.Abs(model.GetVelocity(velocityType).x) > speed)
		{
			Vector3F velocity = model.GetVelocity(velocityType);
			velocity.x = ((!(velocity.x > 1)) ? (-1) : 1) * speed;
			model.SetVelocity(velocity, velocityType);
		}
	}

	public bool CheckIfAboveStage(PhysicsContext context)
	{
		return this.checkIfAboveStage(context);
	}

	private bool checkIfAboveStage(PhysicsContext context)
	{
		RaycastData raycastData = default(RaycastData);
		int groundMask = PhysicsSimulator.GroundMask;
		Fixed distance = 100;
		Vector2F up = Vector2F.up;
		Vector2F vector2F = context.model.position + context.model.bounds.centerOffset + context.model.bounds.down;
		vector2F += up;
		return PhysicsRaycastCalculator.GetFirstRaycastHit(context, vector2F, Vector2F.down, distance, groundMask, out raycastData.hit, PhysicsSimulator.GROUNDED_RAYCAST_TOLERACE);
	}

	private void removePlatformCollisions(List<CollisionData> collisions, List<CollisionData> removedCollisionsOut)
	{
		removedCollisionsOut.Clear();
		for (int i = collisions.Count - 1; i >= 0; i--)
		{
			if (collisions[i].IsPlatformCollision)
			{
				removedCollisionsOut.Add(collisions[i]);
				collisions.RemoveAt(i);
			}
		}
	}
}
