// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Threading;
using UnityEngine;

public class HologramAnimationEvents : MonoBehaviour
{
	public event HologramAnimationEventHandler OnSlideAway;

	public event HologramAnimationEventHandler OnReleaseCamera;

	public void OnSlideAwayComplete()
	{
		this.OnSlideAway();
	}

	public void OnReleaseCameraEvent()
	{
		this.OnReleaseCamera();
	}
}
