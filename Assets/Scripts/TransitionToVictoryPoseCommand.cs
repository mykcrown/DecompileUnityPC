// Decompile from assembly: Assembly-CSharp.dll

using System;

public class TransitionToVictoryPoseCommand : GameEvent
{
	public VictoryScreenPayload Payload;

	public TransitionToVictoryPoseCommand(VictoryScreenPayload payload)
	{
		this.Payload = payload;
	}
}
