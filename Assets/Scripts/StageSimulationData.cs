// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StageSimulationData : MonoBehaviour
{
	public BoundsRect cameraBoundsRect;

	public BoundsRect blastZoneBoundsRect;

	public StageSpawnModeData spawnData;

	public Vector3 cameraOffset;

	public Color offscreenUIColor = default(Color);

	public List<AnimationClip> overrideVictoryPoseCameraAnimations = new List<AnimationClip>();

	public List<bool> shouldOverrideVictoryPosePositions = new List<bool>();

	public List<Vector3ListWrapper> overrideVictoryPosePositions = new List<Vector3ListWrapper>();

	[HideInInspector]
	public List<StageTrigger> triggers = new List<StageTrigger>();

	[HideInInspector]
	public List<StageBehaviourGroup> behaviourGroups = new List<StageBehaviourGroup>();

	public Rect CameraBounds
	{
		get
		{
			return (!(this.cameraBoundsRect == null)) ? this.cameraBoundsRect.bounds : default(Rect);
		}
	}

	public Rect BlastZoneBounds
	{
		get
		{
			return (!(this.blastZoneBoundsRect == null)) ? this.blastZoneBoundsRect.bounds : default(Rect);
		}
	}

	public void SetCameraBounds(Vector2 topLeft, Vector2 bottomRight)
	{
		if (this.cameraBoundsRect != null)
		{
			this.cameraBoundsRect.transform.position = topLeft;
			this.cameraBoundsRect.dimensions = new Vector2(bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
		}
	}

	public void SetBlastZoneBounds(Vector2 topLeft, Vector2 bottomRight)
	{
		if (this.blastZoneBoundsRect != null)
		{
			this.blastZoneBoundsRect.transform.position = topLeft;
			this.blastZoneBoundsRect.dimensions = new Vector2(bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
		}
	}
}
