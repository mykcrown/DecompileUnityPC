using System;
using FixedPoint;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003C8 RID: 968
[Serializable]
public class ConfigData : ScriptableObject, IPreloadedGameAsset
{
	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06001537 RID: 5431 RVA: 0x000753ED File Offset: 0x000737ED
	public CharacterColorConfig characterColorConfig
	{
		get
		{
			return this.characterColorConfigFile.obj;
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06001538 RID: 5432 RVA: 0x000753FA File Offset: 0x000737FA
	public UserProfileSettings userProfileSettings
	{
		get
		{
			return this.userProfileSettingsFile.obj;
		}
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06001539 RID: 5433 RVA: 0x00075407 File Offset: 0x00073807
	public bool IsAutoStart
	{
		get
		{
			return Application.isEditor && Debug.isDebugBuild && this.DebugConfig.testGameSettings.enableAutoStartTestGame;
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x0600153A RID: 5434 RVA: 0x00075430 File Offset: 0x00073830
	public DebugConfig DebugConfig
	{
		get
		{
			if (this.debugConfig == null)
			{
				this.debugConfig = DebugConfig.LoadConfig();
			}
			return this.debugConfig;
		}
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x00075454 File Offset: 0x00073854
	public void Rescale(Fixed rescale)
	{
		this.cameraConfig.Rescale(rescale);
		this.knockbackConfig.Rescale(rescale);
		this.shieldConfig.Rescale(rescale);
		this.comboEscapeConfig.Rescale(rescale);
		this.lagConfig.Rescale(rescale);
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x00075494 File Offset: 0x00073894
	public void RegisterPreload(PreloadContext context)
	{
		this.defaultCharacterEffects.RegisterPreload(context);
		this.hitConfig.RegisterPreload(context);
		this.priorityConfig.RegisterPreload(context);
		this.respawnConfig.RegisterPreload(context);
		context.Register(new PreloadDef(this.moveData.WeaponTrailPrefab, PreloadType.WEAPON_TRAIL), 16);
		if (this.defaultCharacterEffects.trailData != null)
		{
			this.defaultCharacterEffects.trailData.RegisterPreload(context);
		}
	}

	// Token: 0x04000DEB RID: 3563
	public static readonly string CONFIG_DATA_PATH = "Assets/Wavedash/Resources/Config/Config.asset";

	// Token: 0x04000DEC RID: 3564
	public string gameName;

	// Token: 0x04000DED RID: 3565
	[FormerlySerializedAs("gameGUI")]
	public UIConfig uiConfig;

	// Token: 0x04000DEE RID: 3566
	public int fps = 60;

	// Token: 0x04000DEF RID: 3567
	public float gameSpeed = 1f;

	// Token: 0x04000DF0 RID: 3568
	public int maxPlayers = 4;

	// Token: 0x04000DF1 RID: 3569
	public Fixed attackAssistOffstageFallSpeedMultiplier = (Fixed)0.15000000596046448;

	// Token: 0x04000DF2 RID: 3570
	public FrameSyncMode frameSyncMode = FrameSyncMode.UPDATE_FORCE;

	// Token: 0x04000DF3 RID: 3571
	public bool warmAllShaders = true;

	// Token: 0x04000DF4 RID: 3572
	public AutoQualityDetectionConfig autoQualityDetectionConfig;

	// Token: 0x04000DF5 RID: 3573
	public DefaultQualityConfig defaultQualityConfig;

	// Token: 0x04000DF6 RID: 3574
	[FormerlySerializedAs("cameraOptions")]
	public CameraConfig cameraConfig;

	// Token: 0x04000DF7 RID: 3575
	public GrabConfig grabConfig;

	// Token: 0x04000DF8 RID: 3576
	public FlourishConfig flourishConfig;

	// Token: 0x04000DF9 RID: 3577
	public LagConfig lagConfig;

	// Token: 0x04000DFA RID: 3578
	public ChargeConfig chargeConfig;

	// Token: 0x04000DFB RID: 3579
	public ShieldConfig shieldConfig;

	// Token: 0x04000DFC RID: 3580
	public LedgeConfig ledgeConfig;

	// Token: 0x04000DFD RID: 3581
	public HitParticleConfig hitConfig;

	// Token: 0x04000DFE RID: 3582
	public RespawnConfig respawnConfig;

	// Token: 0x04000DFF RID: 3583
	public CharacterEffectConfig defaultCharacterEffects;

	// Token: 0x04000E00 RID: 3584
	public CharacterColorConfigFile characterColorConfigFile = new CharacterColorConfigFile();

	// Token: 0x04000E01 RID: 3585
	public PrioritiyConfig priorityConfig;

	// Token: 0x04000E02 RID: 3586
	public ComboEscapeConfig comboEscapeConfig;

	// Token: 0x04000E03 RID: 3587
	public SpikeConfig spikeConfig;

	// Token: 0x04000E04 RID: 3588
	public StaleMoveQueueConfig staleMoves;

	// Token: 0x04000E05 RID: 3589
	public GlobalMoveData moveData;

	// Token: 0x04000E06 RID: 3590
	public SoundSettings soundSettings;

	// Token: 0x04000E07 RID: 3591
	public ReplaySettings replaySettings;

	// Token: 0x04000E08 RID: 3592
	public UIUXSettings uiuxSettings;

	// Token: 0x04000E09 RID: 3593
	public UserProfileSettingsFile userProfileSettingsFile = new UserProfileSettingsFile();

	// Token: 0x04000E0A RID: 3594
	public LobbySettings lobbySettings;

	// Token: 0x04000E0B RID: 3595
	public GlobalMoves globalMoves;

	// Token: 0x04000E0C RID: 3596
	public StageSettings stageSettings;

	// Token: 0x04000E0D RID: 3597
	public NetworkSettings networkSettings;

	// Token: 0x04000E0E RID: 3598
	public IdentitySettings identitySettings;

	// Token: 0x04000E0F RID: 3599
	public HologramSettings hologramSettings;

	// Token: 0x04000E10 RID: 3600
	public StoreSettings storeSettings;

	// Token: 0x04000E11 RID: 3601
	public RollbackDebugSettings rollbackDebugSettings;

	// Token: 0x04000E12 RID: 3602
	public TauntSettings tauntSettings;

	// Token: 0x04000E13 RID: 3603
	public CharacterSelectSettings characterSelectSettings;

	// Token: 0x04000E14 RID: 3604
	public NuxSettings nuxSettings;

	// Token: 0x04000E15 RID: 3605
	public InputConfigData inputConfig;

	// Token: 0x04000E16 RID: 3606
	[FormerlySerializedAs("knockbackOptions")]
	public KnockbackConfig knockbackConfig = new KnockbackConfig();

	// Token: 0x04000E17 RID: 3607
	public WallJumpConfig wallJumpConfig = new WallJumpConfig();

	// Token: 0x04000E18 RID: 3608
	public AnimationConfig animationConfig = new AnimationConfig();

	// Token: 0x04000E19 RID: 3609
	public AnnouncementConfigData announcements;

	// Token: 0x04000E1A RID: 3610
	public UILocalizationData uiLocalizationData = new UILocalizationData();

	// Token: 0x04000E1B RID: 3611
	public LinkTableDictionary uiLinkTable = new LinkTableDictionary
	{
		{
			"terms_of_service",
			"https://icons.gg/terms-of-service/"
		},
		{
			"privacy_policy",
			"https://icons.gg/privacy-policy/"
		},
		{
			"faq",
			"https://wavedash.zendesk.com/hc/en-us"
		},
		{
			"faq_vjoy",
			"https://wavedash.zendesk.com/hc/en-us/articles/360006959191-Input-Errors-Detected-Removing-VJoy-"
		},
		{
			"discord",
			"https://discord.gg/xqcANv"
		}
	};

	// Token: 0x04000E1C RID: 3612
	public PlayerNetsukeDisplay netsukeDisplayPrefab;

	// Token: 0x04000E1D RID: 3613
	public PlayerNetsukeDisplay netsukeStorePrefab;

	// Token: 0x04000E1E RID: 3614
	public bool detect3D_Hits;

	// Token: 0x04000E1F RID: 3615
	public bool interpolateHitboxes;

	// Token: 0x04000E20 RID: 3616
	public string buildPlayerTimestamp;

	// Token: 0x04000E21 RID: 3617
	public bool music = true;

	// Token: 0x04000E22 RID: 3618
	public float musicVolume = 1f;

	// Token: 0x04000E23 RID: 3619
	public float pausedMusicVolume = 0.5f;

	// Token: 0x04000E24 RID: 3620
	public float musicFadeTime = 0.25f;

	// Token: 0x04000E25 RID: 3621
	public float soundfxVolume = 1f;

	// Token: 0x04000E26 RID: 3622
	private DebugConfig debugConfig;
}
