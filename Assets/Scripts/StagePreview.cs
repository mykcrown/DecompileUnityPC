// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class StagePreview : MonoBehaviour
{
	public bool ShowVictoryPose;

	private void Awake()
	{
		if (GameClient.IsCreated)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		SystemBoot.Mode mode = (!this.ShowVictoryPose) ? SystemBoot.Mode.StagePreview : SystemBoot.Mode.VictoryPosePreview;
		SystemBoot.Startup(mode, null);
	}
}
