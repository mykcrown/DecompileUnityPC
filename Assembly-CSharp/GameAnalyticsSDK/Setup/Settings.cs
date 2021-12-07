using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GameAnalyticsSDK.Setup
{
	// Token: 0x02000031 RID: 49
	public class Settings : ScriptableObject
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x0000DD64 File Offset: 0x0000C164
		public void SetCustomUserID(string customID)
		{
			if (customID != string.Empty)
			{
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000DD78 File Offset: 0x0000C178
		public void RemovePlatformAtIndex(int index)
		{
			if (index >= 0 && index < this.Platforms.Count)
			{
				this.gameKey.RemoveAt(index);
				this.secretKey.RemoveAt(index);
				this.Build.RemoveAt(index);
				this.SelectedPlatformStudio.RemoveAt(index);
				this.SelectedPlatformGame.RemoveAt(index);
				this.SelectedPlatformGameID.RemoveAt(index);
				this.SelectedStudio.RemoveAt(index);
				this.SelectedGame.RemoveAt(index);
				this.PlatformFoldOut.RemoveAt(index);
				this.Platforms.RemoveAt(index);
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000DE18 File Offset: 0x0000C218
		public void AddPlatform(RuntimePlatform platform)
		{
			this.gameKey.Add(string.Empty);
			this.secretKey.Add(string.Empty);
			this.Build.Add("0.1");
			this.SelectedPlatformStudio.Add(string.Empty);
			this.SelectedPlatformGame.Add(string.Empty);
			this.SelectedPlatformGameID.Add(-1);
			this.SelectedStudio.Add(0);
			this.SelectedGame.Add(0);
			this.PlatformFoldOut.Add(true);
			this.Platforms.Add(platform);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000DEB4 File Offset: 0x0000C2B4
		public string[] GetAvailablePlatforms()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < Settings.AvailablePlatforms.Length; i++)
			{
				RuntimePlatform runtimePlatform = Settings.AvailablePlatforms[i];
				if (runtimePlatform == RuntimePlatform.IPhonePlayer)
				{
					if (!this.Platforms.Contains(RuntimePlatform.tvOS) && !this.Platforms.Contains(runtimePlatform))
					{
						list.Add(runtimePlatform.ToString());
					}
					else if (!this.Platforms.Contains(runtimePlatform))
					{
						list.Add(runtimePlatform.ToString());
					}
				}
				else if (runtimePlatform == RuntimePlatform.tvOS)
				{
					if (!this.Platforms.Contains(RuntimePlatform.IPhonePlayer) && !this.Platforms.Contains(runtimePlatform))
					{
						list.Add(runtimePlatform.ToString());
					}
					else if (!this.Platforms.Contains(runtimePlatform))
					{
						list.Add(runtimePlatform.ToString());
					}
				}
				else if (runtimePlatform == RuntimePlatform.MetroPlayerARM)
				{
					if (!this.Platforms.Contains(runtimePlatform))
					{
						list.Add("WSA");
					}
				}
				else if (!this.Platforms.Contains(runtimePlatform))
				{
					list.Add(runtimePlatform.ToString());
				}
			}
			return list.ToArray();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000E010 File Offset: 0x0000C410
		public bool IsGameKeyValid(int index, string value)
		{
			bool result = true;
			for (int i = 0; i < this.Platforms.Count; i++)
			{
				if (index != i && value.Equals(this.gameKey[i]))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000E064 File Offset: 0x0000C464
		public bool IsSecretKeyValid(int index, string value)
		{
			bool result = true;
			for (int i = 0; i < this.Platforms.Count; i++)
			{
				if (index != i && value.Equals(this.secretKey[i]))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000E0B8 File Offset: 0x0000C4B8
		public void UpdateGameKey(int index, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				bool flag = this.IsGameKeyValid(index, value);
				if (flag)
				{
					this.gameKey[index] = value;
				}
				else if (this.gameKey[index].Equals(value))
				{
					this.gameKey[index] = string.Empty;
				}
			}
			else
			{
				this.gameKey[index] = value;
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000E12C File Offset: 0x0000C52C
		public void UpdateSecretKey(int index, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				bool flag = this.IsSecretKeyValid(index, value);
				if (flag)
				{
					this.secretKey[index] = value;
				}
				else if (this.secretKey[index].Equals(value))
				{
					this.secretKey[index] = string.Empty;
				}
			}
			else
			{
				this.secretKey[index] = value;
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000E19F File Offset: 0x0000C59F
		public string GetGameKey(int index)
		{
			return this.gameKey[index];
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000E1AD File Offset: 0x0000C5AD
		public string GetSecretKey(int index)
		{
			return this.secretKey[index];
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000E1BB File Offset: 0x0000C5BB
		public void SetCustomArea(string customArea)
		{
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000E1BD File Offset: 0x0000C5BD
		public void SetKeys(string gamekey, string secretkey)
		{
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000E1BF File Offset: 0x0000C5BF
		// Note: this type is marked as 'beforefieldinit'.
		static Settings()
		{
			RuntimePlatform[] array = new RuntimePlatform[8];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.$field-44781D9C27CA776121BB96E77181191FBF3039DA).FieldHandle);
			Settings.AvailablePlatforms = array;
		}

		// Token: 0x0400012E RID: 302
		[HideInInspector]
		public static string VERSION = "5.1.11";

		// Token: 0x0400012F RID: 303
		[HideInInspector]
		public static bool CheckingForUpdates = false;

		// Token: 0x04000130 RID: 304
		public int TotalMessagesSubmitted;

		// Token: 0x04000131 RID: 305
		public int TotalMessagesFailed;

		// Token: 0x04000132 RID: 306
		public int DesignMessagesSubmitted;

		// Token: 0x04000133 RID: 307
		public int DesignMessagesFailed;

		// Token: 0x04000134 RID: 308
		public int QualityMessagesSubmitted;

		// Token: 0x04000135 RID: 309
		public int QualityMessagesFailed;

		// Token: 0x04000136 RID: 310
		public int ErrorMessagesSubmitted;

		// Token: 0x04000137 RID: 311
		public int ErrorMessagesFailed;

		// Token: 0x04000138 RID: 312
		public int BusinessMessagesSubmitted;

		// Token: 0x04000139 RID: 313
		public int BusinessMessagesFailed;

		// Token: 0x0400013A RID: 314
		public int UserMessagesSubmitted;

		// Token: 0x0400013B RID: 315
		public int UserMessagesFailed;

		// Token: 0x0400013C RID: 316
		public string CustomArea = string.Empty;

		// Token: 0x0400013D RID: 317
		[SerializeField]
		private List<string> gameKey = new List<string>();

		// Token: 0x0400013E RID: 318
		[SerializeField]
		private List<string> secretKey = new List<string>();

		// Token: 0x0400013F RID: 319
		[SerializeField]
		public List<string> Build = new List<string>();

		// Token: 0x04000140 RID: 320
		[SerializeField]
		public List<string> SelectedPlatformStudio = new List<string>();

		// Token: 0x04000141 RID: 321
		[SerializeField]
		public List<string> SelectedPlatformGame = new List<string>();

		// Token: 0x04000142 RID: 322
		[SerializeField]
		public List<int> SelectedPlatformGameID = new List<int>();

		// Token: 0x04000143 RID: 323
		[SerializeField]
		public List<int> SelectedStudio = new List<int>();

		// Token: 0x04000144 RID: 324
		[SerializeField]
		public List<int> SelectedGame = new List<int>();

		// Token: 0x04000145 RID: 325
		public string NewVersion = string.Empty;

		// Token: 0x04000146 RID: 326
		public string Changes = string.Empty;

		// Token: 0x04000147 RID: 327
		public bool SignUpOpen = true;

		// Token: 0x04000148 RID: 328
		public string StudioName = string.Empty;

		// Token: 0x04000149 RID: 329
		public string GameName = string.Empty;

		// Token: 0x0400014A RID: 330
		public string EmailGA = string.Empty;

		// Token: 0x0400014B RID: 331
		[NonSerialized]
		public string PasswordGA = string.Empty;

		// Token: 0x0400014C RID: 332
		[NonSerialized]
		public string TokenGA = string.Empty;

		// Token: 0x0400014D RID: 333
		[NonSerialized]
		public string ExpireTime = string.Empty;

		// Token: 0x0400014E RID: 334
		[NonSerialized]
		public string LoginStatus = "Not logged in.";

		// Token: 0x0400014F RID: 335
		[NonSerialized]
		public bool JustSignedUp;

		// Token: 0x04000150 RID: 336
		[NonSerialized]
		public bool HideSignupWarning;

		// Token: 0x04000151 RID: 337
		public bool IntroScreen = true;

		// Token: 0x04000152 RID: 338
		[NonSerialized]
		public List<Studio> Studios;

		// Token: 0x04000153 RID: 339
		public bool InfoLogEditor = true;

		// Token: 0x04000154 RID: 340
		public bool InfoLogBuild = true;

		// Token: 0x04000155 RID: 341
		public bool VerboseLogBuild;

		// Token: 0x04000156 RID: 342
		public bool UseManualSessionHandling;

		// Token: 0x04000157 RID: 343
		public bool SendExampleGameDataToMyGame;

		// Token: 0x04000158 RID: 344
		public bool InternetConnectivity;

		// Token: 0x04000159 RID: 345
		public List<string> CustomDimensions01 = new List<string>();

		// Token: 0x0400015A RID: 346
		public List<string> CustomDimensions02 = new List<string>();

		// Token: 0x0400015B RID: 347
		public List<string> CustomDimensions03 = new List<string>();

		// Token: 0x0400015C RID: 348
		public List<string> ResourceItemTypes = new List<string>();

		// Token: 0x0400015D RID: 349
		public List<string> ResourceCurrencies = new List<string>();

		// Token: 0x0400015E RID: 350
		public RuntimePlatform LastCreatedGamePlatform;

		// Token: 0x0400015F RID: 351
		public List<RuntimePlatform> Platforms = new List<RuntimePlatform>();

		// Token: 0x04000160 RID: 352
		public Settings.InspectorStates CurrentInspectorState;

		// Token: 0x04000161 RID: 353
		public List<Settings.HelpTypes> ClosedHints = new List<Settings.HelpTypes>();

		// Token: 0x04000162 RID: 354
		public bool DisplayHints;

		// Token: 0x04000163 RID: 355
		public Vector2 DisplayHintsScrollState;

		// Token: 0x04000164 RID: 356
		public Texture2D Logo;

		// Token: 0x04000165 RID: 357
		public Texture2D UpdateIcon;

		// Token: 0x04000166 RID: 358
		public Texture2D InfoIcon;

		// Token: 0x04000167 RID: 359
		public Texture2D DeleteIcon;

		// Token: 0x04000168 RID: 360
		public Texture2D GameIcon;

		// Token: 0x04000169 RID: 361
		public Texture2D HomeIcon;

		// Token: 0x0400016A RID: 362
		public Texture2D InstrumentIcon;

		// Token: 0x0400016B RID: 363
		public Texture2D QuestionIcon;

		// Token: 0x0400016C RID: 364
		public Texture2D UserIcon;

		// Token: 0x0400016D RID: 365
		public Texture2D AmazonIcon;

		// Token: 0x0400016E RID: 366
		public Texture2D GooglePlayIcon;

		// Token: 0x0400016F RID: 367
		public Texture2D iosIcon;

		// Token: 0x04000170 RID: 368
		public Texture2D macIcon;

		// Token: 0x04000171 RID: 369
		public Texture2D windowsPhoneIcon;

		// Token: 0x04000172 RID: 370
		[NonSerialized]
		public GUIStyle SignupButton;

		// Token: 0x04000173 RID: 371
		public bool UsePlayerSettingsBuildNumber;

		// Token: 0x04000174 RID: 372
		public bool SubmitErrors = true;

		// Token: 0x04000175 RID: 373
		public int MaxErrorCount = 10;

		// Token: 0x04000176 RID: 374
		public bool SubmitFpsAverage = true;

		// Token: 0x04000177 RID: 375
		public bool SubmitFpsCritical = true;

		// Token: 0x04000178 RID: 376
		public bool IncludeGooglePlay = true;

		// Token: 0x04000179 RID: 377
		public int FpsCriticalThreshold = 20;

		// Token: 0x0400017A RID: 378
		public int FpsCirticalSubmitInterval = 1;

		// Token: 0x0400017B RID: 379
		public List<bool> PlatformFoldOut = new List<bool>();

		// Token: 0x0400017C RID: 380
		public bool CustomDimensions01FoldOut;

		// Token: 0x0400017D RID: 381
		public bool CustomDimensions02FoldOut;

		// Token: 0x0400017E RID: 382
		public bool CustomDimensions03FoldOut;

		// Token: 0x0400017F RID: 383
		public bool ResourceItemTypesFoldOut;

		// Token: 0x04000180 RID: 384
		public bool ResourceCurrenciesFoldOut;

		// Token: 0x04000181 RID: 385
		public static readonly RuntimePlatform[] AvailablePlatforms;

		// Token: 0x02000032 RID: 50
		public enum HelpTypes
		{
			// Token: 0x04000183 RID: 387
			None,
			// Token: 0x04000184 RID: 388
			IncludeSystemSpecsHelp,
			// Token: 0x04000185 RID: 389
			ProvideCustomUserID
		}

		// Token: 0x02000033 RID: 51
		public enum MessageTypes
		{
			// Token: 0x04000187 RID: 391
			None,
			// Token: 0x04000188 RID: 392
			Error,
			// Token: 0x04000189 RID: 393
			Info,
			// Token: 0x0400018A RID: 394
			Warning
		}

		// Token: 0x02000034 RID: 52
		public struct HelpInfo
		{
			// Token: 0x0400018B RID: 395
			public string Message;

			// Token: 0x0400018C RID: 396
			public Settings.MessageTypes MsgType;

			// Token: 0x0400018D RID: 397
			public Settings.HelpTypes HelpType;
		}

		// Token: 0x02000035 RID: 53
		public enum InspectorStates
		{
			// Token: 0x0400018F RID: 399
			Account,
			// Token: 0x04000190 RID: 400
			Basic,
			// Token: 0x04000191 RID: 401
			Debugging,
			// Token: 0x04000192 RID: 402
			Pref
		}
	}
}
