// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class HitCollisionCalculator
{
	public enum HitCollisionPriority
	{
		Clank,
		Hit,
		ReceiveHit,
		Grab,
		ReceiveGrab
	}

	private struct InvokeHitSuccess : HitCollisionCalculator.ISortedHitCollisionInvoke
	{
		public Hit hit
		{
			get;
			private set;
		}

		public IHitOwner hitter
		{
			get;
			private set;
		}

		public IHitOwner receiver
		{
			get;
			private set;
		}

		public ImpactType impactType
		{
			get;
			private set;
		}

		public HitContext hitContext
		{
			get;
			private set;
		}

		public InvokeHitSuccess(IHitOwner hitter, Hit hit, IHitOwner receiver, ImpactType impactType, HitContext hitContext)
		{
			this.hit = hit;
			this.hitter = hitter;
			this.receiver = receiver;
			this.impactType = impactType;
			this.hitContext = hitContext;
		}

		public int Priority()
		{
			HitType hitType = this.hit.data.hitType;
			if (hitType != HitType.Grab && hitType != HitType.BlockableGrab)
			{
				return 1;
			}
			return 3;
		}

		public bool IsHitSuccess()
		{
			return true;
		}

		public void Invoke()
		{
			this.hitter.OnHitSuccess(this.hit, this.receiver, this.impactType, ref this.hitContext.collisionPosition, ref this.hitContext.collisionVelocity, this.hitContext);
		}
	}

	private class InvokeReceiveHit : HitCollisionCalculator.ISortedHitCollisionInvoke
	{
		public HitData hitData
		{
			get;
			private set;
		}

		public IHitOwner receiver
		{
			get;
			private set;
		}

		public IHitOwner hitter
		{
			get;
			private set;
		}

		public ImpactType impactType
		{
			get;
			private set;
		}

		public HitContext hitContext
		{
			get;
			private set;
		}

		public InvokeReceiveHit(IHitOwner receiver, HitData hitData, IHitOwner hitter, ImpactType impactType, HitContext hitContext)
		{
			this.hitData = hitData;
			this.receiver = receiver;
			this.hitter = hitter;
			this.impactType = impactType;
			this.hitContext = hitContext;
		}

		public int Priority()
		{
			HitType hitType = this.hitData.hitType;
			if (hitType != HitType.Grab && hitType != HitType.BlockableGrab)
			{
				return 2;
			}
			return 4;
		}

		public bool IsHitSuccess()
		{
			return false;
		}

		public void Invoke()
		{
			this.receiver.ReceiveHit(this.hitData, this.hitter, this.impactType, this.hitContext);
		}
	}

	private struct InvokeDualHitBoxCollision : HitCollisionCalculator.ISortedHitCollisionInvoke
	{
		public Hit hit1
		{
			get;
			private set;
		}

		public Hit hit2
		{
			get;
			private set;
		}

		public IHitOwner owner1
		{
			get;
			private set;
		}

		public IHitOwner owner2
		{
			get;
			private set;
		}

		public Vector3F collisionPoint
		{
			get;
			private set;
		}

		public bool cancel1
		{
			get;
			private set;
		}

		public bool cancel2
		{
			get;
			private set;
		}

		public HitContext hitContext
		{
			get;
			private set;
		}

		public ImpactType impactType
		{
			get
			{
				return ImpactType.Dual;
			}
		}

		public IHitOwner receiver
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IHitOwner hitter
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public InvokeDualHitBoxCollision(IHitOwner owner1, IHitOwner owner2, Hit hit1, Hit hit2, Vector3F collisionPoint, bool cancel1, bool cancel2, HitContext hitContext)
		{
			this.hit1 = hit1;
			this.hit2 = hit2;
			this.owner1 = owner1;
			this.owner2 = owner2;
			this.collisionPoint = collisionPoint;
			this.cancel1 = cancel1;
			this.cancel2 = cancel2;
			this.hitContext = hitContext;
		}

		public int Priority()
		{
			return 0;
		}

		public bool IsHitSuccess()
		{
			return false;
		}

		public void Invoke()
		{
			Vector3F collisionPoint = this.collisionPoint;
			this.owner1.OnHitBoxCollision(this.hit1, this.owner2, this.hit2, ref collisionPoint, this.cancel1, this.owner1.PlayerNum < this.owner2.PlayerNum);
			this.owner2.OnHitBoxCollision(this.hit2, this.owner1, this.hit1, ref collisionPoint, this.cancel2, this.owner2.PlayerNum < this.owner1.PlayerNum);
		}
	}

	public interface ISortedHitCollisionInvoke
	{
		HitContext hitContext
		{
			get;
		}

		ImpactType impactType
		{
			get;
		}

		IHitOwner hitter
		{
			get;
		}

		IHitOwner receiver
		{
			get;
		}

		int Priority();

		void Invoke();

		bool IsHitSuccess();
	}

	[Inject]
	public IHitContextPool hitContextPool
	{
		get;
		set;
	}

	private void checkCollision<T, U>(List<T> segments1, HitType type1, bool isSegment1Projectile, List<U> segments2, HitType type2, bool isSegment2Projectile, out HitContext hitContext, bool check3D = false) where T : ISegmentCollider where U : ISegmentCollider
	{
		hitContext = this.hitContextPool.GetNext();
		if (segments1 == null || segments2 == null)
		{
			return;
		}
		foreach (ISegmentCollider segmentCollider in segments1)
		{
			if (!segmentCollider.MatchesVisiblityState(HurtBoxVisibilityState.HIDDEN))
			{
				if (!isSegment2Projectile || !segmentCollider.MatchesVisiblityState(HurtBoxVisibilityState.HIDDEN_FROM_PROJECTILES))
				{
					if (type2 != HitType.Grab || !segmentCollider.MatchesVisiblityState(HurtBoxVisibilityState.HIDDEN_FROM_GRAB))
					{
						if (segmentCollider.InteractsWithType(type2))
						{
							if (!(segmentCollider.Radius == 0))
							{
								foreach (ISegmentCollider segmentCollider2 in segments2)
								{
									if (!segmentCollider2.MatchesVisiblityState(HurtBoxVisibilityState.HIDDEN))
									{
										if (!isSegment1Projectile || !segmentCollider2.MatchesVisiblityState(HurtBoxVisibilityState.HIDDEN_FROM_PROJECTILES))
										{
											if (type1 != HitType.Grab || !segmentCollider2.MatchesVisiblityState(HurtBoxVisibilityState.HIDDEN_FROM_GRAB))
											{
												if (segmentCollider2.InteractsWithType(type1))
												{
													if (!(segmentCollider2.Radius == 0))
													{
														Fixed one = 0;
														if (!segmentCollider.IsCircle && !segmentCollider2.IsCircle)
														{
															one = MathUtil.FindSqDistanceBetweenSegments(segmentCollider.Point1, segmentCollider.Point2, segmentCollider2.Point1, segmentCollider2.Point2);
														}
														else if (segmentCollider.IsCircle && segmentCollider2.IsCircle)
														{
															one = (segmentCollider.Point1 - segmentCollider2.Point1).sqrMagnitude;
														}
														else
														{
															Vector3F v = (!segmentCollider.IsCircle) ? segmentCollider2.Point1 : segmentCollider.Point1;
															ISegmentCollider segmentCollider3 = (!segmentCollider.IsCircle) ? segmentCollider : segmentCollider2;
															one = MathUtil.SqDistBetweenPointAndLineSegment(segmentCollider3.Point1, segmentCollider3.Point2, v);
														}
														if (one <= FixedMath.Pow(segmentCollider.Radius + segmentCollider2.Radius, 2))
														{
															hitContext.collisionPosition = (segmentCollider.Point1 + segmentCollider2.Point1) / (Fixed)2.0;
															hitContext.hurtBoxState = segmentCollider2;
															if (segmentCollider.Point1 != Vector3F.zero && segmentCollider.Point2 != Vector3F.zero)
															{
																hitContext.collisionVelocity = segmentCollider.Point1 - segmentCollider.Point2;
															}
															return;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public void CheckCollision(IHitOwner owner1, IHitOwner owner2, List<HitCollisionCalculator.ISortedHitCollisionInvoke> results)
	{
		if (owner1.Hits == null)
		{
			return;
		}
		for (int i = 0; i < owner1.Hits.Count; i++)
		{
			Hit hit = owner1.Hits[i];
			if (owner1.IsHitActive(hit, owner2, true))
			{
				HitData data = hit.data;
				if (owner2.Hits != null && this.DoesHitTypeInteractWithCollisionCheckType(data.hitType, CollisionCheckType.HitBox))
				{
					for (int j = 0; j < owner2.Hits.Count; j++)
					{
						Hit hit2 = owner2.Hits[j];
						HitData data2 = hit2.data;
						if (owner2.IsHitActive(hit2, owner1, true) && this.AllowHitTypeInteraction(data.hitType, data2.hitType) && this.AllowHitTypeInteraction(hit2.data.hitType, hit.data.hitType))
						{
							if (owner1.ForceCollisionChecks(CollisionCheckType.HitBox, data) || owner2.ForceCollisionChecks(CollisionCheckType.HitBox, data2) || owner2.CanReflect(data2) || (owner1.AllowClanking(data2, owner2) && owner2.AllowClanking(data, owner1)))
							{
								if (!this.isFilteredCounter(data, owner2) && !this.isFilteredCounter(data2, owner1))
								{
									HitContext hitContext;
									this.checkCollision<HitBoxState, HitBoxState>(hit.hitBoxes, data.hitType, owner1.IsProjectile, hit2.hitBoxes, data2.hitType, owner2.IsProjectile, out hitContext, false);
									if (hitContext.collisionPosition != Vector3F.zero)
									{
										if (owner1.HandleComponentHitInteraction(hit2, owner2, CollisionCheckType.HitBox, hitContext) || owner2.HandleComponentHitInteraction(hit, owner1, CollisionCheckType.HitBox, hitContext))
										{
											return;
										}
										if (owner2.ShouldReflect(owner1, ref hitContext.collisionPosition, CollisionCheckType.HitBox, hit2))
										{
											hitContext.reflectorHitData = hit2.data;
											results.Add(new HitCollisionCalculator.InvokeHitSuccess(owner1, hit, owner2, ImpactType.Reflect, hitContext));
											results.Add(new HitCollisionCalculator.InvokeReceiveHit(owner2, data, owner1, ImpactType.Reflect, hitContext));
											return;
										}
										if ((owner1.AllowClanking(data2, owner2) && owner2.AllowClanking(data, owner1)) || owner1.ForceCollisionChecks(CollisionCheckType.HitBox, data) || owner2.ForceCollisionChecks(CollisionCheckType.HitBox, data2))
										{
											bool flag = owner1.ShouldCancelClankedMove(hit, hit2, owner2);
											bool cancel = owner2.ShouldCancelClankedMove(hit2, hit, owner1);
											results.Add(new HitCollisionCalculator.InvokeDualHitBoxCollision(owner1, owner2, hit, hit2, hitContext.collisionPosition, flag, cancel, hitContext));
											if (flag)
											{
												return;
											}
										}
									}
								}
							}
						}
					}
				}
				if (!owner2.IsImmune(data, owner1))
				{
					if (this.DoesHitTypeInteractWithCollisionCheckType(data.hitType, CollisionCheckType.ShieldBox))
					{
						HitContext hitContext2;
						this.checkCollision<HitBoxState, HitBoxState>(hit.hitBoxes, data.hitType, owner1.IsProjectile, owner2.ShieldBoxes, HitType.None, false, out hitContext2, false);
						if (hitContext2.collisionPosition != Vector3F.zero)
						{
							ImpactType impactType = ImpactType.Shield;
							if (owner2.ShouldReflect(owner1, ref hitContext2.collisionPosition, CollisionCheckType.ShieldBox, null))
							{
								impactType = ImpactType.Reflect;
							}
							hitContext2.reflectorHitData = null;
							results.Add(new HitCollisionCalculator.InvokeHitSuccess(owner1, hit, owner2, impactType, hitContext2));
							results.Add(new HitCollisionCalculator.InvokeReceiveHit(owner2, data, owner1, impactType, hitContext2));
							return;
						}
					}
					if (this.DoesHitTypeInteractWithCollisionCheckType(data.hitType, CollisionCheckType.HurtBox))
					{
						HitContext hitContext3;
						this.checkCollision<HitBoxState, HurtBoxState>(hit.hitBoxes, data.hitType, owner1.IsProjectile, owner2.HurtBoxes, HitType.None, false, out hitContext3, false);
						if (hitContext3.collisionPosition != Vector3F.zero)
						{
							ImpactType impactType2 = ImpactType.Hit;
							if (data.hitType == HitType.Gust && !owner1.ShouldReflect(owner2, ref hitContext3.collisionPosition, CollisionCheckType.HitBox, hit))
							{
								return;
							}
							if (data.hitType == HitType.Grab || data.hitType == HitType.BlockableGrab)
							{
								impactType2 = ImpactType.Grab;
							}
							else if (owner2.ShouldReflect(owner1, ref hitContext3.collisionPosition, CollisionCheckType.HurtBox, null))
							{
								impactType2 = ImpactType.Reflect;
								hitContext3.reflectorHitData = null;
							}
							results.Add(new HitCollisionCalculator.InvokeHitSuccess(owner1, hit, owner2, impactType2, hitContext3));
							results.Add(new HitCollisionCalculator.InvokeReceiveHit(owner2, data, owner1, impactType2, hitContext3));
							return;
						}
					}
				}
			}
		}
	}

	public bool AllowHitTypeInteraction(HitType hitType1, HitType hitType2)
	{
		return hitType1 != HitType.Counter || (hitType2 != HitType.BlockableGrab && hitType2 != HitType.Gust && hitType2 != HitType.Grab);
	}

	public bool DoesHitTypeInteractWithCollisionCheckType(HitType hitType, CollisionCheckType collisionType)
	{
		switch (collisionType)
		{
		case CollisionCheckType.ShieldBox:
			return hitType != HitType.Grab && hitType != HitType.Counter;
		case CollisionCheckType.HurtBox:
			return hitType != HitType.Counter;
		case CollisionCheckType.HitBox:
			switch (hitType)
			{
			case HitType.Grab:
			case HitType.SelfHit:
				return false;
			case HitType.Projectile:
			case HitType.Counter:
				IL_56:
				if (hitType != HitType.BlockableGrab)
				{
					return true;
				}
				return false;
			}
			goto IL_56;
		default:
			return true;
		}
	}

	private bool isFilteredCounter(HitData hitData, IHitOwner other)
	{
		return hitData.hitType == HitType.Counter && hitData.counterFilter != HitCounterFilter.None && (((hitData.counterFilter & HitCounterFilter.Character) == HitCounterFilter.None || other.IsProjectile) && ((hitData.counterFilter & HitCounterFilter.Projectile) == HitCounterFilter.None || !other.IsProjectile));
	}
}
