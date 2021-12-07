using System;

// Token: 0x020005F6 RID: 1526
public interface IPlayerState
{
	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x06002494 RID: 9364
	// (set) Token: 0x06002495 RID: 9365
	MetaState MetaState { get; set; }

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x06002496 RID: 9366
	// (set) Token: 0x06002497 RID: 9367
	ActionState ActionState { get; set; }

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x06002498 RID: 9368
	// (set) Token: 0x06002499 RID: 9369
	SubStates SubState { get; set; }

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x0600249A RID: 9370
	bool CanEndCrouch { get; }

	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x0600249B RID: 9371
	bool CanDropThroughPlatform { get; }

	// Token: 0x0600249C RID: 9372
	bool CanBeginDashing(HorizontalDirection direction);

	// Token: 0x0600249D RID: 9373
	bool CanBeginRunPivot(HorizontalDirection direction);

	// Token: 0x0600249E RID: 9374
	bool CanBeginPivot(HorizontalDirection direction);

	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x0600249F RID: 9375
	bool CanBeginWalking { get; }

	// Token: 0x170008C0 RID: 2240
	// (get) Token: 0x060024A0 RID: 9376
	bool CanBeginIdling { get; }

	// Token: 0x170008C1 RID: 2241
	// (get) Token: 0x060024A1 RID: 9377
	bool CanBeginCrouching { get; }

	// Token: 0x170008C2 RID: 2242
	// (get) Token: 0x060024A2 RID: 9378
	bool CanBeginBraking { get; }

	// Token: 0x170008C3 RID: 2243
	// (get) Token: 0x060024A3 RID: 9379
	bool CanBeginTeetering { get; }

	// Token: 0x170008C4 RID: 2244
	// (get) Token: 0x060024A4 RID: 9380
	bool CanBeginFalling { get; }

	// Token: 0x170008C5 RID: 2245
	// (get) Token: 0x060024A5 RID: 9381
	bool CanResumeGrabbing { get; }

	// Token: 0x170008C6 RID: 2246
	// (get) Token: 0x060024A6 RID: 9382
	bool CanReleaseShieldVoluntarily { get; }

	// Token: 0x170008C7 RID: 2247
	// (get) Token: 0x060024A7 RID: 9383
	bool CanMaintainShield { get; }

	// Token: 0x170008C8 RID: 2248
	// (get) Token: 0x060024A8 RID: 9384
	bool CanBeginShield { get; }

	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x060024A9 RID: 9385
	bool CanMove { get; }

	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x060024AA RID: 9386
	bool CanUseMoves { get; }

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x060024AB RID: 9387
	bool CanUseEmotes { get; }

	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x060024AC RID: 9388
	bool CanJump { get; }

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x060024AD RID: 9389
	bool CanReleaseLedge { get; }

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x060024AE RID: 9390
	bool CanGrabLedge { get; }

	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x060024AF RID: 9391
	bool CanFallThroughPlatforms { get; }

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x060024B0 RID: 9392
	bool CanDieOffTop { get; }

	// Token: 0x060024B1 RID: 9393
	bool CanWallJump(HorizontalDirection wallJumpDirection);

	// Token: 0x060024B2 RID: 9394
	bool CanTech(SurfaceType surfaceType);

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x060024B3 RID: 9395
	bool IsInUntechableStun { get; }

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x060024B4 RID: 9396
	bool IsTechOffCooldown { get; }

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x060024B5 RID: 9397
	bool IsTechableMode { get; }

	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x060024B6 RID: 9398
	bool IsDownState { get; }

	// Token: 0x170008D5 RID: 2261
	// (get) Token: 0x060024B7 RID: 9399
	bool IsGrabbedState { get; }

	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x060024B8 RID: 9400
	bool IsStandardGrabbingState { get; }

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x060024B9 RID: 9401
	bool IsShieldingState { get; }

	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x060024BA RID: 9402
	bool IsStandingState { get; }

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x060024BB RID: 9403
	bool IsJumpingState { get; }

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x060024BC RID: 9404
	bool IsLedgeHangingState { get; }

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x060024BD RID: 9405
	bool IsDashing { get; }

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x060024BE RID: 9406
	bool IsDashPivoting { get; }

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x060024BF RID: 9407
	bool IsTumbling { get; }

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x060024C0 RID: 9408
	bool IsLedgeGrabbing { get; }

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x060024C1 RID: 9409
	bool IsBraking { get; }

	// Token: 0x170008E0 RID: 2272
	// (get) Token: 0x060024C2 RID: 9410
	bool IsDashBraking { get; }

	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x060024C3 RID: 9411
	bool IsPivoting { get; }

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x060024C4 RID: 9412
	bool IsTakingOff { get; }

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x060024C5 RID: 9413
	bool IsLanding { get; }

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x060024C6 RID: 9414
	bool IsHelpless { get; }

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x060024C7 RID: 9415
	bool IsJumpingUp { get; }

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x060024C8 RID: 9416
	bool IsAirJumping { get; }

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x060024C9 RID: 9417
	bool IsCrouching { get; }

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x060024CA RID: 9418
	bool IsRunning { get; }

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x060024CB RID: 9419
	bool IsTeetering { get; }

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x060024CC RID: 9420
	bool IsIdling { get; }

	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x060024CD RID: 9421
	bool IsWalking { get; }

	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x060024CE RID: 9422
	bool IsGrabReleasing { get; }

	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x060024CF RID: 9423
	bool IsGrabEscaping { get; }

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x060024D0 RID: 9424
	bool IsDazed { get; }

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x060024D1 RID: 9425
	bool IsRunPivoting { get; }

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x060024D2 RID: 9426
	bool IsDownedLooping { get; }

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x060024D3 RID: 9427
	bool IsHitStunned { get; }

	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x060024D4 RID: 9428
	bool IsThrown { get; }

	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x060024D5 RID: 9429
	bool IsPlatformDropping { get; }

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x060024D6 RID: 9430
	bool IsRespawning { get; }

	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x060024D7 RID: 9431
	bool IsBusyRespawning { get; }

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x060024D8 RID: 9432
	bool IsHitLagPaused { get; }

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x060024D9 RID: 9433
	bool IsUnderChainGrabPrevention { get; }

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x060024DA RID: 9434
	bool IsCameraFlourishMode { get; }

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x060024DB RID: 9435
	bool IsCameraZoomMode { get; }

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x060024DC RID: 9436
	bool IsBusyWithMove { get; }

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x060024DD RID: 9437
	bool IsBlockMovement { get; }

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x060024DE RID: 9438
	bool IsBlockFastFall { get; }

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x060024DF RID: 9439
	bool IsBusyWithAction { get; }

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x060024E0 RID: 9440
	bool IsMoveActive { get; }

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x060024E1 RID: 9441
	bool IsFalling { get; }

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x060024E2 RID: 9442
	bool IsInControl { get; }

	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x060024E3 RID: 9443
	bool IsRecovered { get; }

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x060024E4 RID: 9444
	bool IsGrounded { get; }

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x060024E5 RID: 9445
	bool IsStunned { get; }

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x060024E6 RID: 9446
	bool IsJumpStunned { get; }

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x060024E7 RID: 9447
	bool IsShieldBroken { get; }

	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x060024E8 RID: 9448
	bool IsDead { get; }

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x060024E9 RID: 9449
	bool IsImmuneToBlastZone { get; }

	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x060024EA RID: 9450
	bool IsAffectedByUnflinchingKnockback { get; }

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x060024EB RID: 9451
	bool IsLedgeRecovering { get; }

	// Token: 0x1700090A RID: 2314
	// (get) Token: 0x060024EC RID: 9452
	bool IsLedgeHanging { get; }

	// Token: 0x1700090B RID: 2315
	// (get) Token: 0x060024ED RID: 9453
	bool IsWallJumping { get; }

	// Token: 0x1700090C RID: 2316
	// (get) Token: 0x060024EE RID: 9454
	bool ShouldIgnoreForces { get; }

	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x060024EF RID: 9455
	bool ShouldPlayFallOrLandAction { get; }

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x060024F0 RID: 9456
	bool ShouldUseRecoveryJump { get; }

	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x060024F1 RID: 9457
	int ActionStateFrame { get; }
}
