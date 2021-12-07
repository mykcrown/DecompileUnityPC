// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class FPSCounter : MonoBehaviour, ITickable
{
	private int arrayPosition;

	private int[] timeRecord = new int[300];

	private float lastTime;

	private float lastRecordedTime;

	public bool RecordOnUpdate;

	public Action OnRecord;

	public float FPS
	{
		get;
		private set;
	}

	private void Awake()
	{
		for (int i = 0; i < this.timeRecord.Length; i++)
		{
			this.timeRecord[i] = 0;
		}
	}

	private void Update()
	{
		if (this.RecordOnUpdate)
		{
			this.onNewFrame();
		}
	}

	public void TickFrame()
	{
		if (!this.RecordOnUpdate && GameClient.IsCurrentFrame)
		{
			this.onNewFrame();
		}
	}

	private void onNewFrame()
	{
		float num = Time.realtimeSinceStartup - this.lastTime;
		this.lastTime = Time.realtimeSinceStartup;
		this.timeRecord[this.arrayPosition] = (int)(num * 1000000f);
		this.arrayPosition++;
		if (this.arrayPosition >= this.timeRecord.Length)
		{
			this.arrayPosition = 0;
		}
		if (Time.realtimeSinceStartup >= this.lastRecordedTime + 1f)
		{
			this.recordTime();
		}
	}

	private void recordTime()
	{
		this.lastRecordedTime = Time.realtimeSinceStartup;
		float worst = this.getWorst(0, 20);
		float worst2 = this.getWorst(20, 20);
		float worst3 = this.getWorst(40, 20);
		float num = (worst + worst2 + worst3) / 3f;
		if (num == 0f)
		{
			this.FPS = 60f;
		}
		else
		{
			this.FPS = 1000000f / num;
		}
		if (this.OnRecord != null)
		{
			this.OnRecord();
		}
	}

	private float getWorst(int segmentStart, int segmentSize)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < segmentSize; i++)
		{
			int num3 = this.arrayPosition - i - segmentStart;
			if (num3 < 0)
			{
				num3 += this.timeRecord.Length;
			}
			num2 += this.timeRecord[num3];
			if (this.timeRecord[num3] > num)
			{
				num = this.timeRecord[num3];
			}
		}
		return (float)(num2 / segmentSize);
	}
}
