using System;

// Token: 0x02000416 RID: 1046
public class GameDataManager : IGameData
{
	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x060015D0 RID: 5584 RVA: 0x000776E5 File Offset: 0x00075AE5
	// (set) Token: 0x060015D1 RID: 5585 RVA: 0x000776ED File Offset: 0x00075AED
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x060015D2 RID: 5586 RVA: 0x000776F6 File Offset: 0x00075AF6
	// (set) Token: 0x060015D3 RID: 5587 RVA: 0x000776FE File Offset: 0x00075AFE
	public IGameData GameData { get; private set; }

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00077707 File Offset: 0x00075B07
	public StageDataStore StageData
	{
		get
		{
			return this.GameData.StageData;
		}
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x060015D5 RID: 5589 RVA: 0x00077714 File Offset: 0x00075B14
	public GameModeDataStore GameModeData
	{
		get
		{
			return this.GameData.GameModeData;
		}
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x060015D6 RID: 5590 RVA: 0x00077721 File Offset: 0x00075B21
	public ConfigData ConfigData
	{
		get
		{
			return this.GameData.ConfigData;
		}
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x0007772E File Offset: 0x00075B2E
	public bool IsFeatureEnabled(FeatureID feature)
	{
		return this.GameData.IsFeatureEnabled(feature);
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x0007773C File Offset: 0x00075B3C
	public void Initialize(ConfigData data, GameEnvironmentData environmentData)
	{
		this.GameData = new GameData(data, environmentData, this.localization);
	}
}
