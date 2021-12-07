using System;
using UnityEngine;

// Token: 0x020002B6 RID: 694
[Serializable]
public class SoundDataConfig : ScriptableObject
{
	// Token: 0x040008B5 RID: 2229
	public bool editorTestingMode;

	// Token: 0x040008B6 RID: 2230
	public SoundDataConfig.GenericSounds generic = new SoundDataConfig.GenericSounds();

	// Token: 0x040008B7 RID: 2231
	public SoundDataConfig.WavedashArena wavedashArena = new SoundDataConfig.WavedashArena();

	// Token: 0x040008B8 RID: 2232
	public SoundDataConfig.Cryostation cryoStation = new SoundDataConfig.Cryostation();

	// Token: 0x040008B9 RID: 2233
	public SoundDataConfig.CombatLab combatLab = new SoundDataConfig.CombatLab();

	// Token: 0x040008BA RID: 2234
	public SoundDataConfig.CombatLabAlt combatLabAlt = new SoundDataConfig.CombatLabAlt();

	// Token: 0x040008BB RID: 2235
	public SoundDataConfig.Malumalu maluMalu = new SoundDataConfig.Malumalu();

	// Token: 0x040008BC RID: 2236
	public SoundDataConfig.MalumaluAlt maluMaluAlt = new SoundDataConfig.MalumaluAlt();

	// Token: 0x040008BD RID: 2237
	public SoundDataConfig.ForbiddenShrine forbiddenShrine = new SoundDataConfig.ForbiddenShrine();

	// Token: 0x040008BE RID: 2238
	public SoundDataConfig.ForbiddenShrineAlt forbiddenShrineAlt = new SoundDataConfig.ForbiddenShrineAlt();

	// Token: 0x040008BF RID: 2239
	public SoundDataConfig.Shadowbriar shadowbriar = new SoundDataConfig.Shadowbriar();

	// Token: 0x040008C0 RID: 2240
	public SoundDataConfig.ShadowbriarAlt shadowbriarAlt = new SoundDataConfig.ShadowbriarAlt();

	// Token: 0x040008C1 RID: 2241
	public SoundDataConfig.Cargo cargo = new SoundDataConfig.Cargo();

	// Token: 0x040008C2 RID: 2242
	public SoundDataConfig.AshaniPostGame ashaniPostGame = new SoundDataConfig.AshaniPostGame();

	// Token: 0x040008C3 RID: 2243
	public SoundDataConfig.KiddPostGame kiddPostGame = new SoundDataConfig.KiddPostGame();

	// Token: 0x040008C4 RID: 2244
	public SoundDataConfig.XanaPostGame xanaPostGame = new SoundDataConfig.XanaPostGame();

	// Token: 0x040008C5 RID: 2245
	public SoundDataConfig.RaymerPostGame raymerPostGame = new SoundDataConfig.RaymerPostGame();

	// Token: 0x040008C6 RID: 2246
	public SoundDataConfig.ZhurongPostGame zhurongPostGame = new SoundDataConfig.ZhurongPostGame();

	// Token: 0x040008C7 RID: 2247
	public SoundDataConfig.AfiPostGame afiPostGame = new SoundDataConfig.AfiPostGame();

	// Token: 0x040008C8 RID: 2248
	public SoundDataConfig.WeishanPostGame weishanPostGame = new SoundDataConfig.WeishanPostGame();

	// Token: 0x040008C9 RID: 2249
	public SoundDataConfig.AshaniStore ashaniStore = new SoundDataConfig.AshaniStore();

	// Token: 0x040008CA RID: 2250
	public SoundDataConfig.KiddStore kiddStore = new SoundDataConfig.KiddStore();

	// Token: 0x040008CB RID: 2251
	public SoundDataConfig.XanaStore xanaStore = new SoundDataConfig.XanaStore();

	// Token: 0x040008CC RID: 2252
	public SoundDataConfig.RaymerStore raymerStore = new SoundDataConfig.RaymerStore();

	// Token: 0x040008CD RID: 2253
	public SoundDataConfig.ZhurongStore zhurongStore = new SoundDataConfig.ZhurongStore();

	// Token: 0x040008CE RID: 2254
	public SoundDataConfig.AfiStore afiStore = new SoundDataConfig.AfiStore();

	// Token: 0x040008CF RID: 2255
	public SoundDataConfig.WeishanStore weishanStore = new SoundDataConfig.WeishanStore();

	// Token: 0x040008D0 RID: 2256
	public SoundDataConfig.LoginSounds login = new SoundDataConfig.LoginSounds();

	// Token: 0x040008D1 RID: 2257
	public SoundDataConfig.MainMenuSounds mainMenu = new SoundDataConfig.MainMenuSounds();

	// Token: 0x040008D2 RID: 2258
	public SoundDataConfig.CharacterSelectSounds characterSelect = new SoundDataConfig.CharacterSelectSounds();

	// Token: 0x040008D3 RID: 2259
	public SoundDataConfig.StageSelectSounds stageSelect = new SoundDataConfig.StageSelectSounds();

	// Token: 0x040008D4 RID: 2260
	public SoundDataConfig.LoadingBattleSounds loadingBattle = new SoundDataConfig.LoadingBattleSounds();

	// Token: 0x040008D5 RID: 2261
	public SoundDataConfig.InGameSounds inGame = new SoundDataConfig.InGameSounds();

	// Token: 0x040008D6 RID: 2262
	public SoundDataConfig.VictoryScreenSounds victoryScreen = new SoundDataConfig.VictoryScreenSounds();

	// Token: 0x040008D7 RID: 2263
	public SoundDataConfig.PlayerProgressionSounds playerProgression = new SoundDataConfig.PlayerProgressionSounds();

	// Token: 0x040008D8 RID: 2264
	public SoundDataConfig.CustomLobbySounds customLobby = new SoundDataConfig.CustomLobbySounds();

	// Token: 0x040008D9 RID: 2265
	public SoundDataConfig.SettingsSounds settings = new SoundDataConfig.SettingsSounds();

	// Token: 0x040008DA RID: 2266
	public SoundDataConfig.StoreSounds store = new SoundDataConfig.StoreSounds();

	// Token: 0x040008DB RID: 2267
	public SoundDataConfig.UnboxingSounds unboxing = new SoundDataConfig.UnboxingSounds();

	// Token: 0x040008DC RID: 2268
	public SoundDataConfig.CreditsScreenSounds credits = new SoundDataConfig.CreditsScreenSounds();

	// Token: 0x020002B7 RID: 695
	[Serializable]
	public class WavedashArena
	{
		// Token: 0x040008DD RID: 2269
		public AudioData music;
	}

	// Token: 0x020002B8 RID: 696
	[Serializable]
	public class Cryostation
	{
		// Token: 0x040008DE RID: 2270
		public AudioData music;
	}

	// Token: 0x020002B9 RID: 697
	[Serializable]
	public class CombatLab
	{
		// Token: 0x040008DF RID: 2271
		public AudioData music;
	}

	// Token: 0x020002BA RID: 698
	[Serializable]
	public class CombatLabAlt
	{
		// Token: 0x040008E0 RID: 2272
		public AudioData music;
	}

	// Token: 0x020002BB RID: 699
	[Serializable]
	public class Malumalu
	{
		// Token: 0x040008E1 RID: 2273
		public AudioData music;
	}

	// Token: 0x020002BC RID: 700
	[Serializable]
	public class MalumaluAlt
	{
		// Token: 0x040008E2 RID: 2274
		public AudioData music;
	}

	// Token: 0x020002BD RID: 701
	[Serializable]
	public class ForbiddenShrine
	{
		// Token: 0x040008E3 RID: 2275
		public AudioData music;
	}

	// Token: 0x020002BE RID: 702
	[Serializable]
	public class ForbiddenShrineAlt
	{
		// Token: 0x040008E4 RID: 2276
		public AudioData music;
	}

	// Token: 0x020002BF RID: 703
	[Serializable]
	public class Shadowbriar
	{
		// Token: 0x040008E5 RID: 2277
		public AudioData music;
	}

	// Token: 0x020002C0 RID: 704
	[Serializable]
	public class ShadowbriarAlt
	{
		// Token: 0x040008E6 RID: 2278
		public AudioData music;
	}

	// Token: 0x020002C1 RID: 705
	[Serializable]
	public class Cargo
	{
		// Token: 0x040008E7 RID: 2279
		public AudioData music;
	}

	// Token: 0x020002C2 RID: 706
	[Serializable]
	public class AshaniPostGame
	{
		// Token: 0x040008E8 RID: 2280
		public AudioData levelup;
	}

	// Token: 0x020002C3 RID: 707
	[Serializable]
	public class KiddPostGame
	{
		// Token: 0x040008E9 RID: 2281
		public AudioData levelup;
	}

	// Token: 0x020002C4 RID: 708
	[Serializable]
	public class XanaPostGame
	{
		// Token: 0x040008EA RID: 2282
		public AudioData levelup;
	}

	// Token: 0x020002C5 RID: 709
	[Serializable]
	public class RaymerPostGame
	{
		// Token: 0x040008EB RID: 2283
		public AudioData levelup;
	}

	// Token: 0x020002C6 RID: 710
	[Serializable]
	public class ZhurongPostGame
	{
		// Token: 0x040008EC RID: 2284
		public AudioData levelup;
	}

	// Token: 0x020002C7 RID: 711
	[Serializable]
	public class AfiPostGame
	{
		// Token: 0x040008ED RID: 2285
		public AudioData levelup;
	}

	// Token: 0x020002C8 RID: 712
	[Serializable]
	public class WeishanPostGame
	{
		// Token: 0x040008EE RID: 2286
		public AudioData levelup;
	}

	// Token: 0x020002C9 RID: 713
	[Serializable]
	public class AshaniStore
	{
		// Token: 0x040008EF RID: 2287
		public AudioData purchaseItem;
	}

	// Token: 0x020002CA RID: 714
	[Serializable]
	public class KiddStore
	{
		// Token: 0x040008F0 RID: 2288
		public AudioData purchaseItem;
	}

	// Token: 0x020002CB RID: 715
	[Serializable]
	public class XanaStore
	{
		// Token: 0x040008F1 RID: 2289
		public AudioData purchaseItem;
	}

	// Token: 0x020002CC RID: 716
	[Serializable]
	public class RaymerStore
	{
		// Token: 0x040008F2 RID: 2290
		public AudioData purchaseItem;
	}

	// Token: 0x020002CD RID: 717
	[Serializable]
	public class ZhurongStore
	{
		// Token: 0x040008F3 RID: 2291
		public AudioData purchaseItem;
	}

	// Token: 0x020002CE RID: 718
	[Serializable]
	public class AfiStore
	{
		// Token: 0x040008F4 RID: 2292
		public AudioData purchaseItem;
	}

	// Token: 0x020002CF RID: 719
	[Serializable]
	public class WeishanStore
	{
		// Token: 0x040008F5 RID: 2293
		public AudioData purchaseItem;
	}

	// Token: 0x020002D0 RID: 720
	[Serializable]
	public class GenericSounds
	{
		// Token: 0x040008F6 RID: 2294
		public AudioData buttonHighlight;

		// Token: 0x040008F7 RID: 2295
		public AudioData dialogOpen;

		// Token: 0x040008F8 RID: 2296
		public AudioData dialogClose;

		// Token: 0x040008F9 RID: 2297
		public AudioData arrowClickLeft;

		// Token: 0x040008FA RID: 2298
		public AudioData arrowClickRight;

		// Token: 0x040008FB RID: 2299
		public AudioData dropdownOpen;

		// Token: 0x040008FC RID: 2300
		public AudioData dropdownClose;

		// Token: 0x040008FD RID: 2301
		public AudioData toggleOn;

		// Token: 0x040008FE RID: 2302
		public AudioData toggleOff;

		// Token: 0x040008FF RID: 2303
		public AudioData typingCharacter;

		// Token: 0x04000900 RID: 2304
		public AudioData typingBackspace;

		// Token: 0x04000901 RID: 2305
		public AudioData screenForwardTransition;

		// Token: 0x04000902 RID: 2306
		public AudioData screenBackTransition;

		// Token: 0x04000903 RID: 2307
		public AudioData dynamicMoveActivate;

		// Token: 0x04000904 RID: 2308
		public AudioData powerMoveActivate;

		// Token: 0x04000905 RID: 2309
		public AudioData dynamicMoveDenied;

		// Token: 0x04000906 RID: 2310
		public AudioData powerMoveDenied;
	}

	// Token: 0x020002D1 RID: 721
	[Serializable]
	public class LoginSounds
	{
		// Token: 0x04000907 RID: 2311
		public AudioData accountCreationConfirmed;

		// Token: 0x04000908 RID: 2312
		public AudioData accountCreationError;
	}

	// Token: 0x020002D2 RID: 722
	[Serializable]
	public class MainMenuSounds
	{
		// Token: 0x04000909 RID: 2313
		public AudioData music;

		// Token: 0x0400090A RID: 2314
		public AudioData mainMenuFoldoutOpen;

		// Token: 0x0400090B RID: 2315
		public AudioData mainMenuFoldoutClose;

		// Token: 0x0400090C RID: 2316
		public AudioData queueStart;

		// Token: 0x0400090D RID: 2317
		public AudioData queueStop;
	}

	// Token: 0x020002D3 RID: 723
	[Serializable]
	public class CharacterSelectSounds
	{
		// Token: 0x0400090E RID: 2318
		public AudioData characterSelectScreenOpen;

		// Token: 0x0400090F RID: 2319
		public AudioData onlineBlindPickScreenOpen;

		// Token: 0x04000910 RID: 2320
		public AudioData characterHighlight;

		// Token: 0x04000911 RID: 2321
		public AudioData characterSelect;

		// Token: 0x04000912 RID: 2322
		public AudioData characterDeselect;

		// Token: 0x04000913 RID: 2323
		public AudioData randomHighlight;

		// Token: 0x04000914 RID: 2324
		public AudioData randomSelect;

		// Token: 0x04000915 RID: 2325
		public AudioData randomDeselect;

		// Token: 0x04000916 RID: 2326
		public AudioData skinCycle;

		// Token: 0x04000917 RID: 2327
		public AudioData teamCycle;

		// Token: 0x04000918 RID: 2328
		public AudioData totemPartnerCycle;

		// Token: 0x04000919 RID: 2329
		public AudioData ruleSetDelete;

		// Token: 0x0400091A RID: 2330
		public AudioData ruleSetAdd;

		// Token: 0x0400091B RID: 2331
		public AudioData ruleSetSelect;
	}

	// Token: 0x020002D4 RID: 724
	[Serializable]
	public class StageSelectSounds
	{
		// Token: 0x0400091C RID: 2332
		public AudioData stageSelectScreenOpen;

		// Token: 0x0400091D RID: 2333
		public AudioData stageStrike;

		// Token: 0x0400091E RID: 2334
		public AudioData stageUnstrike;

		// Token: 0x0400091F RID: 2335
		public AudioData stageStikesReset;
	}

	// Token: 0x020002D5 RID: 725
	[Serializable]
	public class LoadingBattleSounds
	{
		// Token: 0x04000920 RID: 2336
		public AudioData loadingBattleScreenOpen;
	}

	// Token: 0x020002D6 RID: 726
	[Serializable]
	public class InGameSounds
	{
		// Token: 0x04000921 RID: 2337
		public AudioData exitGame;

		// Token: 0x04000922 RID: 2338
		public AudioData endGame;

		// Token: 0x04000923 RID: 2339
		public AudioData pause;

		// Token: 0x04000924 RID: 2340
		public AudioData unpause;

		// Token: 0x04000925 RID: 2341
		public AudioData flyout;

		// Token: 0x04000926 RID: 2342
		public AudioData flyout_small;

		// Token: 0x04000927 RID: 2343
		public AudioData tumbleHitGround;
	}

	// Token: 0x020002D7 RID: 727
	[Serializable]
	public class VictoryScreenSounds
	{
		// Token: 0x04000928 RID: 2344
		public AudioData victoryScreenOpen;
	}

	// Token: 0x020002D8 RID: 728
	[Serializable]
	public class PlayerProgressionSounds
	{
		// Token: 0x04000929 RID: 2345
		public AudioData playerProgressionScreenOpen;

		// Token: 0x0400092A RID: 2346
		public AudioData experienceTick;

		// Token: 0x0400092B RID: 2347
		public AudioData levelUp;
	}

	// Token: 0x020002D9 RID: 729
	[Serializable]
	public class CustomLobbySounds
	{
		// Token: 0x0400092C RID: 2348
		public AudioData customLobbyScreenOpen;

		// Token: 0x0400092D RID: 2349
		public AudioData customLobbyPlayerJoined;

		// Token: 0x0400092E RID: 2350
		public AudioData customLobbyPlayerLeft;
	}

	// Token: 0x020002DA RID: 730
	[Serializable]
	public class SettingsSounds
	{
		// Token: 0x0400092F RID: 2351
		public AudioData settingsScreenOpen;

		// Token: 0x04000930 RID: 2352
		public AudioData controlBindingSelect;

		// Token: 0x04000931 RID: 2353
		public AudioData controlBindingSet;

		// Token: 0x04000932 RID: 2354
		public AudioData settingsReset;

		// Token: 0x04000933 RID: 2355
		public AudioData settingsTabChange;

		// Token: 0x04000934 RID: 2356
		public AudioData soundEffectSliderSet;
	}

	// Token: 0x020002DB RID: 731
	[Serializable]
	public class StoreSounds
	{
		// Token: 0x04000935 RID: 2357
		public AudioData music;

		// Token: 0x04000936 RID: 2358
		public AudioData storeScreenOpen;

		// Token: 0x04000937 RID: 2359
		public AudioData characterEquipViewOpen;

		// Token: 0x04000938 RID: 2360
		public AudioData characterEquipViewClosed;

		// Token: 0x04000939 RID: 2361
		public AudioData collectiblesEquipViewOpen;

		// Token: 0x0400093A RID: 2362
		public AudioData collectiblesEquipViewClosed;

		// Token: 0x0400093B RID: 2363
		public AudioData miniCharacterSelected;

		// Token: 0x0400093C RID: 2364
		public AudioData categorySelected;

		// Token: 0x0400093D RID: 2365
		public AudioData equipmentSelected;

		// Token: 0x0400093E RID: 2366
		public AudioData itemEquip;

		// Token: 0x0400093F RID: 2367
		public AudioData storeTabChange;

		// Token: 0x04000940 RID: 2368
		public AudioData purchaseDialogOpened;

		// Token: 0x04000941 RID: 2369
		public AudioData purchaseConfirmed;
	}

	// Token: 0x020002DC RID: 732
	[Serializable]
	public class UnboxingSounds
	{
		// Token: 0x04000942 RID: 2370
		public AudioData music;

		// Token: 0x04000943 RID: 2371
		public AudioData openUnboxingScene;

		// Token: 0x04000944 RID: 2372
		public AudioData exitUnboxingScene;

		// Token: 0x04000945 RID: 2373
		public AudioData clickOpenBox;

		// Token: 0x04000946 RID: 2374
		public AudioData clickOpenBoxButton;

		// Token: 0x04000947 RID: 2375
		public AudioData crystalDrop;

		// Token: 0x04000948 RID: 2376
		public AudioData portalAppear;

		// Token: 0x04000949 RID: 2377
		public AudioData blueFlashSide;

		// Token: 0x0400094A RID: 2378
		public AudioData blueFlashCenter;

		// Token: 0x0400094B RID: 2379
		public AudioData highlight;

		// Token: 0x0400094C RID: 2380
		public AudioData currency;

		// Token: 0x0400094D RID: 2381
		public AudioData itemSideCommon;

		// Token: 0x0400094E RID: 2382
		public AudioData itemSideUncommon;

		// Token: 0x0400094F RID: 2383
		public AudioData itemSideRare;

		// Token: 0x04000950 RID: 2384
		public AudioData itemSideLegendary;

		// Token: 0x04000951 RID: 2385
		public AudioData itemCenterCommon;

		// Token: 0x04000952 RID: 2386
		public AudioData itemCenterUncommon;

		// Token: 0x04000953 RID: 2387
		public AudioData itemCenterRare;

		// Token: 0x04000954 RID: 2388
		public AudioData itemCenterLegendary;
	}

	// Token: 0x020002DD RID: 733
	[Serializable]
	public class CreditsScreenSounds
	{
		// Token: 0x04000955 RID: 2389
		public AudioData music;
	}
}
