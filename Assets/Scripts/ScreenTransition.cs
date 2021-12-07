// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ScreenTransition
{
	public ScreenTransitionType type;

	public float time = 0.15f;

	public float delay;

	public ScreenTransition(ScreenTransitionType type)
	{
		this.type = type;
	}

	public ScreenTransition SetDelay(float delay)
	{
		this.delay = delay;
		return this;
	}

	public ScreenTransition SetTime(float time)
	{
		this.time = time;
		return this;
	}
}
