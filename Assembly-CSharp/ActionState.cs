using System;
using Beebyte.Obfuscator;

// Token: 0x020004EE RID: 1262
[Skip]
public enum ActionState
{
	// Token: 0x040014D0 RID: 5328
	Idle,
	// Token: 0x040014D1 RID: 5329
	WalkFast,
	// Token: 0x040014D2 RID: 5330
	UNUSED_1,
	// Token: 0x040014D3 RID: 5331
	TakeOff,
	// Token: 0x040014D4 RID: 5332
	JumpStraight,
	// Token: 0x040014D5 RID: 5333
	JumpBack,
	// Token: 0x040014D6 RID: 5334
	JumpForward,
	// Token: 0x040014D7 RID: 5335
	FallStraight,
	// Token: 0x040014D8 RID: 5336
	FallBack,
	// Token: 0x040014D9 RID: 5337
	FallForward,
	// Token: 0x040014DA RID: 5338
	FallHelpless,
	// Token: 0x040014DB RID: 5339
	Landing,
	// Token: 0x040014DC RID: 5340
	Crouching,
	// Token: 0x040014DD RID: 5341
	CrouchBegin,
	// Token: 0x040014DE RID: 5342
	CrouchEnd,
	// Token: 0x040014DF RID: 5343
	ShieldLoop,
	// Token: 0x040014E0 RID: 5344
	DownedLoop,
	// Token: 0x040014E1 RID: 5345
	GrabbedLoop,
	// Token: 0x040014E2 RID: 5346
	GrabbedPummelled,
	// Token: 0x040014E3 RID: 5347
	GrabEscapeAir,
	// Token: 0x040014E4 RID: 5348
	RunPivotBrake,
	// Token: 0x040014E5 RID: 5349
	DazedLoop,
	// Token: 0x040014E6 RID: 5350
	DazedEnd,
	// Token: 0x040014E7 RID: 5351
	WalkSlow,
	// Token: 0x040014E8 RID: 5352
	WalkMedium,
	// Token: 0x040014E9 RID: 5353
	TeeterBegin,
	// Token: 0x040014EA RID: 5354
	FallDown,
	// Token: 0x040014EB RID: 5355
	UNUSED_10,
	// Token: 0x040014EC RID: 5356
	UNUSED_11,
	// Token: 0x040014ED RID: 5357
	UNUSED_12,
	// Token: 0x040014EE RID: 5358
	Tumble,
	// Token: 0x040014EF RID: 5359
	UNUSED_13,
	// Token: 0x040014F0 RID: 5360
	EdgeGrab,
	// Token: 0x040014F1 RID: 5361
	EdgeHang,
	// Token: 0x040014F2 RID: 5362
	Run,
	// Token: 0x040014F3 RID: 5363
	Grabbing,
	// Token: 0x040014F4 RID: 5364
	GrabbedBegin,
	// Token: 0x040014F5 RID: 5365
	Dash,
	// Token: 0x040014F6 RID: 5366
	Brake,
	// Token: 0x040014F7 RID: 5367
	RunPivot,
	// Token: 0x040014F8 RID: 5368
	DazedBegin,
	// Token: 0x040014F9 RID: 5369
	Pivot,
	// Token: 0x040014FA RID: 5370
	Recoil,
	// Token: 0x040014FB RID: 5371
	ShieldEnd,
	// Token: 0x040014FC RID: 5372
	ShieldBegin,
	// Token: 0x040014FD RID: 5373
	GrabRelease,
	// Token: 0x040014FE RID: 5374
	GrabEscapeGround,
	// Token: 0x040014FF RID: 5375
	Thrown,
	// Token: 0x04001500 RID: 5376
	TeeterLoop,
	// Token: 0x04001501 RID: 5377
	AirJump,
	// Token: 0x04001502 RID: 5378
	DashBrake,
	// Token: 0x04001503 RID: 5379
	DashPivot,
	// Token: 0x04001504 RID: 5380
	Death,
	// Token: 0x04001505 RID: 5381
	UsingMove,
	// Token: 0x04001506 RID: 5382
	None,
	// Token: 0x04001507 RID: 5383
	HitStunAirS,
	// Token: 0x04001508 RID: 5384
	HitStunAirM,
	// Token: 0x04001509 RID: 5385
	HitStunAirL,
	// Token: 0x0400150A RID: 5386
	HitStunGroundS,
	// Token: 0x0400150B RID: 5387
	HitStunGroundM,
	// Token: 0x0400150C RID: 5388
	HitStunGroundL,
	// Token: 0x0400150D RID: 5389
	HitStunMeteorS,
	// Token: 0x0400150E RID: 5390
	HitStunMeteorM,
	// Token: 0x0400150F RID: 5391
	HitStunMeteorL,
	// Token: 0x04001510 RID: 5392
	HitTumbleHigh,
	// Token: 0x04001511 RID: 5393
	HitTumbleLow,
	// Token: 0x04001512 RID: 5394
	HitTumbleNeutral,
	// Token: 0x04001513 RID: 5395
	HitTumbleSpin,
	// Token: 0x04001514 RID: 5396
	HitTumbleTop,
	// Token: 0x04001515 RID: 5397
	HitStunForcedGetUp,
	// Token: 0x04001516 RID: 5398
	PlatformDrop,
	// Token: 0x04001517 RID: 5399
	Wavedash
}
