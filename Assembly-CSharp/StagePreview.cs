using System;
using UnityEngine;

// Token: 0x02000646 RID: 1606
public class StagePreview : MonoBehaviour
{
	// Token: 0x0600275A RID: 10074 RVA: 0x000BFDE0 File Offset: 0x000BE1E0
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

	// Token: 0x04001CD5 RID: 7381
	public bool ShowVictoryPose;
}
