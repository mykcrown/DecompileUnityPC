// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class StagePayloadData
{
	public List<StageID> legalStages = new List<StageID>();

	public Dictionary<StageID, StageState> stageStates = new Dictionary<StageID, StageState>();

	public StageID lastRandomStage;
}
