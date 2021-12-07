// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class SetStageStateRequest : UIEvent, IUIRequest
{
	public StageID stageID
	{
		get;
		private set;
	}

	public StageState state
	{
		get;
		private set;
	}

	public SetStageStateRequest(StageID stageID, StageState state)
	{
		this.stageID = stageID;
		this.state = state;
	}
}
