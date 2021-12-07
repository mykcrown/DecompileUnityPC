// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;

internal class TextRedrawTracker
{
	private int maxMonitoringFrames = 10;

	public TextMeshProUGUI textField;

	public float prevWidth;

	private int monitorFrameCount;

	public Action onResizeCallback;

	public bool monitor
	{
		get;
		private set;
	}

	public void Release()
	{
		this.onResizeCallback = null;
		this.textField = null;
	}

	public void BeginMonitoring()
	{
		this.monitor = true;
		this.monitorFrameCount = 0;
	}

	public void TickMonitoring()
	{
		this.monitorFrameCount++;
		if (this.monitorFrameCount >= this.maxMonitoringFrames)
		{
			this.endMonitoring();
		}
	}

	private void endMonitoring()
	{
		this.monitor = false;
	}
}
