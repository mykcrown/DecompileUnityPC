// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CameraRenderPerformance : MonoBehaviour
{
	private float fullFrameTime;

	private int fullFrameCount;

	private float fullFrameSum;

	private void FixedUpdate()
	{
		this.fullFrameTime = Time.realtimeSinceStartup;
	}

	private void OnGUI()
	{
		float num = (Time.realtimeSinceStartup - this.fullFrameTime) * 1000f;
		this.fullFrameCount++;
		this.fullFrameSum += num;
		if (this.fullFrameCount == 300)
		{
			float num2 = this.fullFrameSum / (float)this.fullFrameCount;
			UnityEngine.Debug.Log("Frame time " + num2);
			this.fullFrameCount = 0;
			this.fullFrameSum = 0f;
		}
	}
}
