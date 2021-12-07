using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200064C RID: 1612
public class StageSimulationData : MonoBehaviour
{
	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x06002782 RID: 10114 RVA: 0x000C0B84 File Offset: 0x000BEF84
	public Rect CameraBounds
	{
		get
		{
			return (!(this.cameraBoundsRect == null)) ? this.cameraBoundsRect.bounds : default(Rect);
		}
	}

	// Token: 0x170009B6 RID: 2486
	// (get) Token: 0x06002783 RID: 10115 RVA: 0x000C0BBC File Offset: 0x000BEFBC
	public Rect BlastZoneBounds
	{
		get
		{
			return (!(this.blastZoneBoundsRect == null)) ? this.blastZoneBoundsRect.bounds : default(Rect);
		}
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x000C0BF4 File Offset: 0x000BEFF4
	public void SetCameraBounds(Vector2 topLeft, Vector2 bottomRight)
	{
		if (this.cameraBoundsRect != null)
		{
			this.cameraBoundsRect.transform.position = topLeft;
			this.cameraBoundsRect.dimensions = new Vector2(bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
		}
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x000C0C58 File Offset: 0x000BF058
	public void SetBlastZoneBounds(Vector2 topLeft, Vector2 bottomRight)
	{
		if (this.blastZoneBoundsRect != null)
		{
			this.blastZoneBoundsRect.transform.position = topLeft;
			this.blastZoneBoundsRect.dimensions = new Vector2(bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
		}
	}

	// Token: 0x04001CE9 RID: 7401
	public BoundsRect cameraBoundsRect;

	// Token: 0x04001CEA RID: 7402
	public BoundsRect blastZoneBoundsRect;

	// Token: 0x04001CEB RID: 7403
	public StageSpawnModeData spawnData;

	// Token: 0x04001CEC RID: 7404
	public Vector3 cameraOffset;

	// Token: 0x04001CED RID: 7405
	public Color offscreenUIColor = default(Color);

	// Token: 0x04001CEE RID: 7406
	public List<AnimationClip> overrideVictoryPoseCameraAnimations = new List<AnimationClip>();

	// Token: 0x04001CEF RID: 7407
	public List<bool> shouldOverrideVictoryPosePositions = new List<bool>();

	// Token: 0x04001CF0 RID: 7408
	public List<Vector3ListWrapper> overrideVictoryPosePositions = new List<Vector3ListWrapper>();

	// Token: 0x04001CF1 RID: 7409
	[HideInInspector]
	public List<StageTrigger> triggers = new List<StageTrigger>();

	// Token: 0x04001CF2 RID: 7410
	[HideInInspector]
	public List<StageBehaviourGroup> behaviourGroups = new List<StageBehaviourGroup>();
}
