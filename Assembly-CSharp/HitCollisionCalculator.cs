using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x020003B3 RID: 947
public class HitCollisionCalculator
{
	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06001443 RID: 5187 RVA: 0x00071FB7 File Offset: 0x000703B7
	// (set) Token: 0x06001444 RID: 5188 RVA: 0x00071FBF File Offset: 0x000703BF
	[Inject]
	public IHitContextPool hitContextPool { get; set; }

	// Token: 0x06001445 RID: 5189 RVA: 0x00071FC8 File Offset: 0x000703C8
	private void checkCollision<T, U>(List<T> segments1, HitType type1, bool isSegment1Projectile, List<U> segments2, HitType type2, bool isSegment2Projectile, out HitContext hitContext, bool check3D = false) where T : ISegmentCollider where U : ISegmentCollider
	{
		hitContext = this.hitContextPool.GetNext();
		if (segments1 == null || segments2 == null)
		{
			return;
		}
		foreach (T t in segments1)
		{
			ISegmentCollider segmentCollider = t;
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
								foreach (U u in segments2)
								{
									ISegmentCollider segmentCollider2 = u;
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

	// Token: 0x06001446 RID: 5190 RVA: 0x000722F8 File Offset: 0x000706F8
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

	// Token: 0x06001447 RID: 5191 RVA: 0x00072704 File Offset: 0x00070B04
	public bool AllowHitTypeInteraction(HitType hitType1, HitType hitType2)
	{
		return hitType1 != HitType.Counter || (hitType2 != HitType.BlockableGrab && hitType2 != HitType.Gust && hitType2 != HitType.Grab);
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x00072734 File Offset: 0x00070B34
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
				break;
			default:
				if (hitType != HitType.BlockableGrab)
				{
					return true;
				}
				break;
			}
			return false;
		default:
			return true;
		}
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x000727A8 File Offset: 0x00070BA8
	private bool isFilteredCounter(HitData hitData, IHitOwner other)
	{
		return hitData.hitType == HitType.Counter && hitData.counterFilter != HitCounterFilter.None && (((hitData.counterFilter & HitCounterFilter.Character) == HitCounterFilter.None || other.IsProjectile) && ((hitData.counterFilter & HitCounterFilter.Projectile) == HitCounterFilter.None || !other.IsProjectile));
	}

	// Token: 0x020003B4 RID: 948
	public enum HitCollisionPriority
	{
		// Token: 0x04000D98 RID: 3480
		Clank,
		// Token: 0x04000D99 RID: 3481
		Hit,
		// Token: 0x04000D9A RID: 3482
		ReceiveHit,
		// Token: 0x04000D9B RID: 3483
		Grab,
		// Token: 0x04000D9C RID: 3484
		ReceiveGrab
	}

	// Token: 0x020003B5 RID: 949
	private struct InvokeHitSuccess : HitCollisionCalculator.ISortedHitCollisionInvoke
	{
		// Token: 0x0600144A RID: 5194 RVA: 0x00072804 File Offset: 0x00070C04
		public InvokeHitSuccess(IHitOwner hitter, Hit hit, IHitOwner receiver, ImpactType impactType, HitContext hitContext)
		{
			this.hit = hit;
			this.hitter = hitter;
			this.receiver = receiver;
			this.impactType = impactType;
			this.hitContext = hitContext;
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0007282C File Offset: 0x00070C2C
		public int Priority()
		{
			HitType hitType = this.hit.data.hitType;
			if (hitType != HitType.Grab && hitType != HitType.BlockableGrab)
			{
				return 1;
			}
			return 3;
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x00072860 File Offset: 0x00070C60
		// (set) Token: 0x0600144D RID: 5197 RVA: 0x00072868 File Offset: 0x00070C68
		public Hit hit { get; private set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x00072871 File Offset: 0x00070C71
		// (set) Token: 0x0600144F RID: 5199 RVA: 0x00072879 File Offset: 0x00070C79
		public IHitOwner hitter { get; private set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x00072882 File Offset: 0x00070C82
		// (set) Token: 0x06001451 RID: 5201 RVA: 0x0007288A File Offset: 0x00070C8A
		public IHitOwner receiver { get; private set; }

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x00072893 File Offset: 0x00070C93
		// (set) Token: 0x06001453 RID: 5203 RVA: 0x0007289B File Offset: 0x00070C9B
		public ImpactType impactType { get; private set; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x000728A4 File Offset: 0x00070CA4
		// (set) Token: 0x06001455 RID: 5205 RVA: 0x000728AC File Offset: 0x00070CAC
		public HitContext hitContext { get; private set; }

		// Token: 0x06001456 RID: 5206 RVA: 0x000728B5 File Offset: 0x00070CB5
		public bool IsHitSuccess()
		{
			return true;
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x000728B8 File Offset: 0x00070CB8
		public void Invoke()
		{
			this.hitter.OnHitSuccess(this.hit, this.receiver, this.impactType, ref this.hitContext.collisionPosition, ref this.hitContext.collisionVelocity, this.hitContext);
		}
	}

	// Token: 0x020003B6 RID: 950
	private class InvokeReceiveHit : HitCollisionCalculator.ISortedHitCollisionInvoke
	{
		// Token: 0x06001458 RID: 5208 RVA: 0x000728F4 File Offset: 0x00070CF4
		public InvokeReceiveHit(IHitOwner receiver, HitData hitData, IHitOwner hitter, ImpactType impactType, HitContext hitContext)
		{
			this.hitData = hitData;
			this.receiver = receiver;
			this.hitter = hitter;
			this.impactType = impactType;
			this.hitContext = hitContext;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x00072924 File Offset: 0x00070D24
		public int Priority()
		{
			HitType hitType = this.hitData.hitType;
			if (hitType != HitType.Grab && hitType != HitType.BlockableGrab)
			{
				return 2;
			}
			return 4;
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x00072953 File Offset: 0x00070D53
		// (set) Token: 0x0600145B RID: 5211 RVA: 0x0007295B File Offset: 0x00070D5B
		public HitData hitData { get; private set; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x00072964 File Offset: 0x00070D64
		// (set) Token: 0x0600145D RID: 5213 RVA: 0x0007296C File Offset: 0x00070D6C
		public IHitOwner receiver { get; private set; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x00072975 File Offset: 0x00070D75
		// (set) Token: 0x0600145F RID: 5215 RVA: 0x0007297D File Offset: 0x00070D7D
		public IHitOwner hitter { get; private set; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x00072986 File Offset: 0x00070D86
		// (set) Token: 0x06001461 RID: 5217 RVA: 0x0007298E File Offset: 0x00070D8E
		public ImpactType impactType { get; private set; }

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001462 RID: 5218 RVA: 0x00072997 File Offset: 0x00070D97
		// (set) Token: 0x06001463 RID: 5219 RVA: 0x0007299F File Offset: 0x00070D9F
		public HitContext hitContext { get; private set; }

		// Token: 0x06001464 RID: 5220 RVA: 0x000729A8 File Offset: 0x00070DA8
		public bool IsHitSuccess()
		{
			return false;
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x000729AB File Offset: 0x00070DAB
		public void Invoke()
		{
			this.receiver.ReceiveHit(this.hitData, this.hitter, this.impactType, this.hitContext);
		}
	}

	// Token: 0x020003B7 RID: 951
	private struct InvokeDualHitBoxCollision : HitCollisionCalculator.ISortedHitCollisionInvoke
	{
		// Token: 0x06001466 RID: 5222 RVA: 0x000729D0 File Offset: 0x00070DD0
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

		// Token: 0x06001467 RID: 5223 RVA: 0x00072A0F File Offset: 0x00070E0F
		public int Priority()
		{
			return 0;
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x00072A12 File Offset: 0x00070E12
		// (set) Token: 0x06001469 RID: 5225 RVA: 0x00072A1A File Offset: 0x00070E1A
		public Hit hit1 { get; private set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x00072A23 File Offset: 0x00070E23
		// (set) Token: 0x0600146B RID: 5227 RVA: 0x00072A2B File Offset: 0x00070E2B
		public Hit hit2 { get; private set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x00072A34 File Offset: 0x00070E34
		// (set) Token: 0x0600146D RID: 5229 RVA: 0x00072A3C File Offset: 0x00070E3C
		public IHitOwner owner1 { get; private set; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x00072A45 File Offset: 0x00070E45
		// (set) Token: 0x0600146F RID: 5231 RVA: 0x00072A4D File Offset: 0x00070E4D
		public IHitOwner owner2 { get; private set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x00072A56 File Offset: 0x00070E56
		// (set) Token: 0x06001471 RID: 5233 RVA: 0x00072A5E File Offset: 0x00070E5E
		public Vector3F collisionPoint { get; private set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x00072A67 File Offset: 0x00070E67
		// (set) Token: 0x06001473 RID: 5235 RVA: 0x00072A6F File Offset: 0x00070E6F
		public bool cancel1 { get; private set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x00072A78 File Offset: 0x00070E78
		// (set) Token: 0x06001475 RID: 5237 RVA: 0x00072A80 File Offset: 0x00070E80
		public bool cancel2 { get; private set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x00072A89 File Offset: 0x00070E89
		// (set) Token: 0x06001477 RID: 5239 RVA: 0x00072A91 File Offset: 0x00070E91
		public HitContext hitContext { get; private set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x00072A9A File Offset: 0x00070E9A
		public ImpactType impactType
		{
			get
			{
				return ImpactType.Dual;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x00072A9D File Offset: 0x00070E9D
		public IHitOwner receiver
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x00072AA4 File Offset: 0x00070EA4
		public IHitOwner hitter
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00072AAB File Offset: 0x00070EAB
		public bool IsHitSuccess()
		{
			return false;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00072AB0 File Offset: 0x00070EB0
		public void Invoke()
		{
			Vector3F collisionPoint = this.collisionPoint;
			this.owner1.OnHitBoxCollision(this.hit1, this.owner2, this.hit2, ref collisionPoint, this.cancel1, this.owner1.PlayerNum < this.owner2.PlayerNum);
			this.owner2.OnHitBoxCollision(this.hit2, this.owner1, this.hit1, ref collisionPoint, this.cancel2, this.owner2.PlayerNum < this.owner1.PlayerNum);
		}
	}

	// Token: 0x020003B8 RID: 952
	public interface ISortedHitCollisionInvoke
	{
		// Token: 0x0600147D RID: 5245
		int Priority();

		// Token: 0x0600147E RID: 5246
		void Invoke();

		// Token: 0x0600147F RID: 5247
		bool IsHitSuccess();

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001480 RID: 5248
		HitContext hitContext { get; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001481 RID: 5249
		ImpactType impactType { get; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001482 RID: 5250
		IHitOwner hitter { get; }

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001483 RID: 5251
		IHitOwner receiver { get; }
	}
}
