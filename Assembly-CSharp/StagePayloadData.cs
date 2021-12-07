using System;
using System.Collections.Generic;

// Token: 0x02000AC1 RID: 2753
[Serializable]
public class StagePayloadData
{
	// Token: 0x040033D6 RID: 13270
	public List<StageID> legalStages = new List<StageID>();

	// Token: 0x040033D7 RID: 13271
	public Dictionary<StageID, StageState> stageStates = new Dictionary<StageID, StageState>();

	// Token: 0x040033D8 RID: 13272
	public StageID lastRandomStage;
}
