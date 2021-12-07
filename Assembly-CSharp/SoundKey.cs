using System;

// Token: 0x020002E2 RID: 738
public enum SoundKey
{
	// Token: 0x04000968 RID: 2408
	empty,
	// Token: 0x04000969 RID: 2409
	generic_buttonHighlight,
	// Token: 0x0400096A RID: 2410
	generic_dialogOpen,
	// Token: 0x0400096B RID: 2411
	generic_dialogClose,
	// Token: 0x0400096C RID: 2412
	generic_arrowClickLeft,
	// Token: 0x0400096D RID: 2413
	generic_arrowClickRight,
	// Token: 0x0400096E RID: 2414
	generic_dropdownOpen,
	// Token: 0x0400096F RID: 2415
	generic_dropdownClose,
	// Token: 0x04000970 RID: 2416
	generic_toggleOn,
	// Token: 0x04000971 RID: 2417
	generic_toggleOff,
	// Token: 0x04000972 RID: 2418
	generic_typingCharacter,
	// Token: 0x04000973 RID: 2419
	generic_typingBackspace,
	// Token: 0x04000974 RID: 2420
	generic_screenForwardTransition,
	// Token: 0x04000975 RID: 2421
	generic_screenBackTransition,
	// Token: 0x04000976 RID: 2422
	generic_dynamicMoveActivate,
	// Token: 0x04000977 RID: 2423
	generic_powerMoveActivate,
	// Token: 0x04000978 RID: 2424
	generic_dynamicMoveDenied,
	// Token: 0x04000979 RID: 2425
	generic_powerMoveDenied,
	// Token: 0x0400097A RID: 2426
	wavedashArena_music,
	// Token: 0x0400097B RID: 2427
	cryoStation_music,
	// Token: 0x0400097C RID: 2428
	combatLab_music,
	// Token: 0x0400097D RID: 2429
	combatLabAlt_music,
	// Token: 0x0400097E RID: 2430
	maluMalu_music,
	// Token: 0x0400097F RID: 2431
	maluMaluAlt_music,
	// Token: 0x04000980 RID: 2432
	forbiddenShrine_music,
	// Token: 0x04000981 RID: 2433
	forbiddenShrineAlt_music,
	// Token: 0x04000982 RID: 2434
	shadowbriar_music,
	// Token: 0x04000983 RID: 2435
	shadowbriarAlt_music,
	// Token: 0x04000984 RID: 2436
	cargo_music,
	// Token: 0x04000985 RID: 2437
	ashaniPostGame_levelup,
	// Token: 0x04000986 RID: 2438
	kiddPostGame_levelup,
	// Token: 0x04000987 RID: 2439
	xanaPostGame_levelup,
	// Token: 0x04000988 RID: 2440
	raymerPostGame_levelup,
	// Token: 0x04000989 RID: 2441
	zhurongPostGame_levelup,
	// Token: 0x0400098A RID: 2442
	afiPostGame_levelup,
	// Token: 0x0400098B RID: 2443
	weishanPostGame_levelup,
	// Token: 0x0400098C RID: 2444
	ashaniStore_purchaseItem,
	// Token: 0x0400098D RID: 2445
	kiddStore_purchaseItem,
	// Token: 0x0400098E RID: 2446
	xanaStore_purchaseItem,
	// Token: 0x0400098F RID: 2447
	raymerStore_purchaseItem,
	// Token: 0x04000990 RID: 2448
	zhurongStore_purchaseItem,
	// Token: 0x04000991 RID: 2449
	afiStore_purchaseItem,
	// Token: 0x04000992 RID: 2450
	weishanStore_purchaseItem,
	// Token: 0x04000993 RID: 2451
	login_accountCreationConfirmed,
	// Token: 0x04000994 RID: 2452
	login_accountCreationError,
	// Token: 0x04000995 RID: 2453
	mainMenu_music,
	// Token: 0x04000996 RID: 2454
	mainMenu_mainMenuFoldoutOpen,
	// Token: 0x04000997 RID: 2455
	mainMenu_mainMenuFoldoutClose,
	// Token: 0x04000998 RID: 2456
	mainMenu_queueStart,
	// Token: 0x04000999 RID: 2457
	mainMenu_queueStop,
	// Token: 0x0400099A RID: 2458
	characterSelect_characterSelectScreenOpen,
	// Token: 0x0400099B RID: 2459
	characterSelect_onlineBlindPickScreenOpen,
	// Token: 0x0400099C RID: 2460
	characterSelect_characterHighlight,
	// Token: 0x0400099D RID: 2461
	characterSelect_characterSelect,
	// Token: 0x0400099E RID: 2462
	characterSelect_characterDeselect,
	// Token: 0x0400099F RID: 2463
	characterSelect_randomHighlight,
	// Token: 0x040009A0 RID: 2464
	characterSelect_randomSelect,
	// Token: 0x040009A1 RID: 2465
	characterSelect_randomDeselect,
	// Token: 0x040009A2 RID: 2466
	characterSelect_skinCycle,
	// Token: 0x040009A3 RID: 2467
	characterSelect_teamCycle,
	// Token: 0x040009A4 RID: 2468
	characterSelect_totemPartnerCycle,
	// Token: 0x040009A5 RID: 2469
	characterSelect_ruleSetDelete,
	// Token: 0x040009A6 RID: 2470
	characterSelect_ruleSetAdd,
	// Token: 0x040009A7 RID: 2471
	characterSelect_ruleSetSelect,
	// Token: 0x040009A8 RID: 2472
	stageSelect_stageSelectScreenOpen,
	// Token: 0x040009A9 RID: 2473
	stageSelect_stageStrike,
	// Token: 0x040009AA RID: 2474
	stageSelect_stageUnstrike,
	// Token: 0x040009AB RID: 2475
	stageSelect_stageStikesReset,
	// Token: 0x040009AC RID: 2476
	loadingBattle_loadingBattleScreenOpen,
	// Token: 0x040009AD RID: 2477
	inGame_exitGame,
	// Token: 0x040009AE RID: 2478
	inGame_endGame,
	// Token: 0x040009AF RID: 2479
	inGame_pause,
	// Token: 0x040009B0 RID: 2480
	inGame_unpause,
	// Token: 0x040009B1 RID: 2481
	inGame_flyout,
	// Token: 0x040009B2 RID: 2482
	inGame_flyout_small,
	// Token: 0x040009B3 RID: 2483
	inGame_tumbleHitGround,
	// Token: 0x040009B4 RID: 2484
	victoryScreen_victoryScreenOpen,
	// Token: 0x040009B5 RID: 2485
	playerProgression_playerProgressionScreenOpen,
	// Token: 0x040009B6 RID: 2486
	playerProgression_experienceTick,
	// Token: 0x040009B7 RID: 2487
	playerProgression_levelUp,
	// Token: 0x040009B8 RID: 2488
	customLobby_customLobbyScreenOpen,
	// Token: 0x040009B9 RID: 2489
	customLobby_customLobbyPlayerJoined,
	// Token: 0x040009BA RID: 2490
	customLobby_customLobbyPlayerLeft,
	// Token: 0x040009BB RID: 2491
	settings_settingsScreenOpen,
	// Token: 0x040009BC RID: 2492
	settings_controlBindingSelect,
	// Token: 0x040009BD RID: 2493
	settings_controlBindingSet,
	// Token: 0x040009BE RID: 2494
	settings_settingsReset,
	// Token: 0x040009BF RID: 2495
	settings_settingsTabChange,
	// Token: 0x040009C0 RID: 2496
	settings_soundEffectSliderSet,
	// Token: 0x040009C1 RID: 2497
	store_music,
	// Token: 0x040009C2 RID: 2498
	store_storeScreenOpen,
	// Token: 0x040009C3 RID: 2499
	store_characterEquipViewOpen,
	// Token: 0x040009C4 RID: 2500
	store_characterEquipViewClosed,
	// Token: 0x040009C5 RID: 2501
	store_collectiblesEquipViewOpen,
	// Token: 0x040009C6 RID: 2502
	store_collectiblesEquipViewClosed,
	// Token: 0x040009C7 RID: 2503
	store_miniCharacterSelected,
	// Token: 0x040009C8 RID: 2504
	store_categorySelected,
	// Token: 0x040009C9 RID: 2505
	store_equipmentSelected,
	// Token: 0x040009CA RID: 2506
	store_itemEquip,
	// Token: 0x040009CB RID: 2507
	store_storeTabChange,
	// Token: 0x040009CC RID: 2508
	store_purchaseDialogOpened,
	// Token: 0x040009CD RID: 2509
	store_purchaseConfirmed,
	// Token: 0x040009CE RID: 2510
	unboxing_music,
	// Token: 0x040009CF RID: 2511
	unboxing_openUnboxingScene,
	// Token: 0x040009D0 RID: 2512
	unboxing_exitUnboxingScene,
	// Token: 0x040009D1 RID: 2513
	unboxing_clickOpenBox,
	// Token: 0x040009D2 RID: 2514
	unboxing_clickOpenBoxButton,
	// Token: 0x040009D3 RID: 2515
	unboxing_crystalDrop,
	// Token: 0x040009D4 RID: 2516
	unboxing_portalAppear,
	// Token: 0x040009D5 RID: 2517
	unboxing_blueFlashSide,
	// Token: 0x040009D6 RID: 2518
	unboxing_blueFlashCenter,
	// Token: 0x040009D7 RID: 2519
	unboxing_highlight,
	// Token: 0x040009D8 RID: 2520
	unboxing_currency,
	// Token: 0x040009D9 RID: 2521
	unboxing_itemSideCommon,
	// Token: 0x040009DA RID: 2522
	unboxing_itemSideUncommon,
	// Token: 0x040009DB RID: 2523
	unboxing_itemSideRare,
	// Token: 0x040009DC RID: 2524
	unboxing_itemSideLegendary,
	// Token: 0x040009DD RID: 2525
	unboxing_itemCenterCommon,
	// Token: 0x040009DE RID: 2526
	unboxing_itemCenterUncommon,
	// Token: 0x040009DF RID: 2527
	unboxing_itemCenterRare,
	// Token: 0x040009E0 RID: 2528
	unboxing_itemCenterLegendary,
	// Token: 0x040009E1 RID: 2529
	credits_music
}
