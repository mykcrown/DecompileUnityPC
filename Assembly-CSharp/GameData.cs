using System;

// Token: 0x02000414 RID: 1044
public class GameData : IGameData
{
	// Token: 0x060015C3 RID: 5571 RVA: 0x0007763C File Offset: 0x00075A3C
	public GameData(ConfigData data, GameEnvironmentData environment, ILocalization localization = null)
	{
		this.StageData = new StageDataStore(localization);
		this.GameModeData = new GameModeDataStore(localization);
		this.ConfigData = data;
		this.StageData.Load(environment.stages);
		this.GameModeData.Load(environment.gameModes);
		this.featureToggles = environment.toggles;
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x060015C4 RID: 5572 RVA: 0x0007769C File Offset: 0x00075A9C
	// (set) Token: 0x060015C5 RID: 5573 RVA: 0x000776A4 File Offset: 0x00075AA4
	public StageDataStore StageData { get; private set; }

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x060015C6 RID: 5574 RVA: 0x000776AD File Offset: 0x00075AAD
	// (set) Token: 0x060015C7 RID: 5575 RVA: 0x000776B5 File Offset: 0x00075AB5
	public GameModeDataStore GameModeData { get; private set; }

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x060015C8 RID: 5576 RVA: 0x000776BE File Offset: 0x00075ABE
	// (set) Token: 0x060015C9 RID: 5577 RVA: 0x000776C6 File Offset: 0x00075AC6
	public ConfigData ConfigData { get; private set; }

	// Token: 0x060015CA RID: 5578 RVA: 0x000776CF File Offset: 0x00075ACF
	public bool IsFeatureEnabled(FeatureID feature)
	{
		return this.featureToggles.GetToggle(feature);
	}

	// Token: 0x040010AF RID: 4271
	private FeatureToggles featureToggles;
}
