// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class EnterGameRequest : GameEvent, IUIRequest
{
	public StageID stageID
	{
		get;
		private set;
	}

	public bool confirmed
	{
		get;
		private set;
	}

	public EnterGameRequest(StageID stageID, bool confirmed)
	{
		this.stageID = stageID;
		this.confirmed = confirmed;
	}
}
