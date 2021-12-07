using System;
using System.Collections.Generic;

// Token: 0x02000677 RID: 1655
[Serializable]
public class StatTrackerModel : RollbackStateTyped<StatTrackerModel>
{
	// Token: 0x060028EE RID: 10478 RVA: 0x000C5EB4 File Offset: 0x000C42B4
	public override void CopyTo(StatTrackerModel targetIn)
	{
		if (targetIn.PlayerStats.Count != this.PlayerStats.Count)
		{
			targetIn.PlayerStats.Clear();
			foreach (PlayerStats playerStats in this.PlayerStats)
			{
				targetIn.PlayerStats.Add(new PlayerStats());
			}
		}
		for (int i = 0; i < this.PlayerStats.Count; i++)
		{
			this.PlayerStats[i].CopyTo(targetIn.PlayerStats[i]);
		}
	}

	// Token: 0x060028EF RID: 10479 RVA: 0x000C5F78 File Offset: 0x000C4378
	public override object Clone()
	{
		StatTrackerModel statTrackerModel = new StatTrackerModel();
		this.CopyTo(statTrackerModel);
		return statTrackerModel;
	}

	// Token: 0x04001DA0 RID: 7584
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<PlayerStats> PlayerStats = new List<PlayerStats>(8);
}
