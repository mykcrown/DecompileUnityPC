// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class StatTrackerModel : RollbackStateTyped<StatTrackerModel>
{
	[IgnoreCopyValidation, IsClonedManually]
	public List<PlayerStats> PlayerStats = new List<PlayerStats>(8);

	public override void CopyTo(StatTrackerModel targetIn)
	{
		if (targetIn.PlayerStats.Count != this.PlayerStats.Count)
		{
			targetIn.PlayerStats.Clear();
			foreach (PlayerStats current in this.PlayerStats)
			{
				targetIn.PlayerStats.Add(new PlayerStats());
			}
		}
		for (int i = 0; i < this.PlayerStats.Count; i++)
		{
			this.PlayerStats[i].CopyTo(targetIn.PlayerStats[i]);
		}
	}

	public override object Clone()
	{
		StatTrackerModel statTrackerModel = new StatTrackerModel();
		this.CopyTo(statTrackerModel);
		return statTrackerModel;
	}
}
