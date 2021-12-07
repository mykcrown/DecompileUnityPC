// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ConfigData : ScriptableObject, IPreloadedGameAsset
{
	public static readonly string CONFIG_DATA_PATH = "Assets/Wavedash/Resources/Config/Config.asset";

	public string gameName;

	[FormerlySerializedAs("gameGUI")]
	public UIConfig uiConfig;

	public int fps = 60;

	public float gameSpeed = 1f;

	public int maxPlayers = 4;

	public Fixed attackAssistOffstageFallSpeedMultiplier = (Fixed)0.15000000596046448;

	public FrameSyncMode frameSyncMode = FrameSyncMode.UPDATE_FORCE;

	public bool warmAllShaders = true;

	public AutoQualityDetectionConfig autoQualityDetectionConfig;

	public DefaultQualityConfig defaultQualityConfig;

	[FormerlySerializedAs("cameraOptions")]
	public CameraConfig cameraConfig;

	public GrabConfig grabConfig;

	public FlourishConfig flourishConfig;

	public LagConfig lagConfig;

	public ChargeConfig chargeConfig;

	public ShieldConfig shieldConfig;

	public LedgeConfig ledgeConfig;

	public HitParticleConfig hitConfig;

	public RespawnConfig respawnConfig;

	public CharacterEffectConfig defaultCharacterEffects;

	public CharacterColorConfigFile characterColorConfigFile = new CharacterColorConfigFile();

	public PrioritiyConfig priorityConfig;

	public ComboEscapeConfig comboEscapeConfig;

	public SpikeConfig spikeConfig;

	public StaleMoveQueueConfig staleMoves;

	public GlobalMoveData moveData;

	public SoundSettings soundSettings;

	public ReplaySettings replaySettings;

	public UIUXSettings uiuxSettings;

	public UserProfileSettingsFile userProfileSettingsFile = new UserProfileSettingsFile();

	public LobbySettings lobbySettings;

	public GlobalMoves globalMoves;

	public StageSettings stageSettings;

	public NetworkSettings networkSettings;

	public IdentitySettings identitySettings;

	public HologramSettings hologramSettings;

	public StoreSettings storeSettings;

	public RollbackDebugSettings rollbackDebugSettings;

	public TauntSettings tauntSettings;

	public CharacterSelectSettings characterSelectSettings;

	public NuxSettings nuxSettings;

	public InputConfigData inputConfig;

	[FormerlySerializedAs("knockbackOptions")]
	public KnockbackConfig knockbackConfig = new KnockbackConfig();

	public WallJumpConfig wallJumpConfig = new WallJumpConfig();

	public AnimationConfig animationConfig = new AnimationConfig();

	public AnnouncementConfigData announcements;

	public UILocalizationData uiLocalizationData = new UILocalizationData();

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

	public PlayerNetsukeDisplay netsukeDisplayPrefab;

	public PlayerNetsukeDisplay netsukeStorePrefab;

	public bool detect3D_Hits;

	public bool interpolateHitboxes;

	public string buildPlayerTimestamp;

	public bool music = true;

	public float musicVolume = 1f;

	public float pausedMusicVolume = 0.5f;

	public float musicFadeTime = 0.25f;

	public float soundfxVolume = 1f;

	private DebugConfig debugConfig;

	public CharacterColorConfig characterColorConfig
	{
		get
		{
			return this.characterColorConfigFile.obj;
		}
	}

	public UserProfileSettings userProfileSettings
	{
		get
		{
			return this.userProfileSettingsFile.obj;
		}
	}

	public bool IsAutoStart
	{
		get
		{
			return Application.isEditor && Debug.isDebugBuild && this.DebugConfig.testGameSettings.enableAutoStartTestGame;
		}
	}

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

	public void Rescale(Fixed rescale)
	{
		this.cameraConfig.Rescale(rescale);
		this.knockbackConfig.Rescale(rescale);
		this.shieldConfig.Rescale(rescale);
		this.comboEscapeConfig.Rescale(rescale);
		this.lagConfig.Rescale(rescale);
	}

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
}
