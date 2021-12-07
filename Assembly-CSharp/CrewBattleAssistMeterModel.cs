using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x0200049B RID: 1179
public class CrewBattleAssistMeterModel
{
	// Token: 0x060019F3 RID: 6643 RVA: 0x00085E79 File Offset: 0x00084279
	public CrewBattleAssistMeterModel()
	{
		this.amounts[TeamNum.Team1] = 0;
		this.amounts[TeamNum.Team2] = 0;
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x00085EB0 File Offset: 0x000842B0
	public Dictionary<TeamNum, Fixed> GetAmounts()
	{
		return this.amounts;
	}

	// Token: 0x060019F5 RID: 6645 RVA: 0x00085EB8 File Offset: 0x000842B8
	public void LoadAmounts(Dictionary<TeamNum, Fixed> amounts)
	{
		this.amounts = amounts;
	}

	// Token: 0x060019F6 RID: 6646 RVA: 0x00085EC4 File Offset: 0x000842C4
	public void Increment(TeamNum team, Fixed amount)
	{
		if (!this.amounts.ContainsKey(team))
		{
			this.amounts[team] = amount;
		}
		else
		{
			Dictionary<TeamNum, Fixed> dictionary;
			(dictionary = this.amounts)[team] = dictionary[team] + amount;
		}
		if (amount > 100)
		{
			amount = 100;
		}
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x00085F27 File Offset: 0x00084327
	public void SetMeter(TeamNum team, Fixed amount)
	{
		this.amounts[team] = amount;
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x00085F38 File Offset: 0x00084338
	public CrewBattleAssistMeterModel Clone()
	{
		CrewBattleAssistMeterModel crewBattleAssistMeterModel = new CrewBattleAssistMeterModel();
		crewBattleAssistMeterModel.LoadAmounts(new Dictionary<TeamNum, Fixed>(this.amounts));
		return crewBattleAssistMeterModel;
	}

	// Token: 0x04001352 RID: 4946
	private Dictionary<TeamNum, Fixed> amounts = new Dictionary<TeamNum, Fixed>();
}
