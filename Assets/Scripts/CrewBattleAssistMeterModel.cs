// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class CrewBattleAssistMeterModel
{
	private Dictionary<TeamNum, Fixed> amounts = new Dictionary<TeamNum, Fixed>();

	public CrewBattleAssistMeterModel()
	{
		this.amounts[TeamNum.Team1] = 0;
		this.amounts[TeamNum.Team2] = 0;
	}

	public Dictionary<TeamNum, Fixed> GetAmounts()
	{
		return this.amounts;
	}

	public void LoadAmounts(Dictionary<TeamNum, Fixed> amounts)
	{
		this.amounts = amounts;
	}

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

	public void SetMeter(TeamNum team, Fixed amount)
	{
		this.amounts[team] = amount;
	}

	public CrewBattleAssistMeterModel Clone()
	{
		CrewBattleAssistMeterModel crewBattleAssistMeterModel = new CrewBattleAssistMeterModel();
		crewBattleAssistMeterModel.LoadAmounts(new Dictionary<TeamNum, Fixed>(this.amounts));
		return crewBattleAssistMeterModel;
	}
}
