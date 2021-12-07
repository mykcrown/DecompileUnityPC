// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class AnimationTimeline
{
	private static Fixed minSpeed = (Fixed)0.001;

	public Fixed baseSpeed
	{
		get;
		private set;
	}

	public int animFrames
	{
		get;
		private set;
	}

	public int trueFramelength
	{
		get;
		private set;
	}

	public int skipFrames
	{
		get;
		private set;
	}

	private List<AnimationSpeedMultiplier> multipliers
	{
		get;
		set;
	}

	public Dictionary<int, Fixed> gameFrameToAnimFrameMap
	{
		get;
		private set;
	}

	public AnimationTimeline(Fixed baseSpeed, int animFrames, List<AnimationSpeedMultiplier> multipliers = null, int skipFrames = 0)
	{
		this.baseSpeed = baseSpeed;
		this.animFrames = animFrames;
		this.skipFrames = skipFrames;
		this.multipliers = multipliers;
		this.gameFrameToAnimFrameMap = AnimationTimeline.generateGameFrameMap(multipliers, animFrames, skipFrames);
		this.trueFramelength = this.gameFrameToAnimFrameMap.Count;
	}

	public int CalculateGameFrameCount(int startAnimFrame, int endAnimFrame)
	{
		int num = -1;
		for (int i = 0; i < this.trueFramelength; i++)
		{
			Fixed one = this.gameFrameToAnimFrameMap[i];
			if (one >= startAnimFrame && num == -1)
			{
				num = i;
			}
			if (one >= endAnimFrame)
			{
				return i - num;
			}
		}
		return 0;
	}

	public void AddSpeedMultiplier(AnimationSpeedMultiplier multiplier)
	{
		this.multipliers.Add(multiplier);
		this.gameFrameToAnimFrameMap = AnimationTimeline.generateGameFrameMap(this.multipliers, this.animFrames, this.skipFrames);
		this.trueFramelength = this.gameFrameToAnimFrameMap.Count;
	}

	private static Dictionary<int, Fixed> generateGameFrameMap(List<AnimationSpeedMultiplier> multipliers, int animFrames, int skipFrames)
	{
		Dictionary<int, Fixed> dictionary = new Dictionary<int, Fixed>();
		Fixed @fixed = 1;
		int num = 0;
		Fixed fixed2 = skipFrames;
		while (fixed2 < animFrames)
		{
			dictionary[num] = fixed2;
			@fixed = 1;
			if (multipliers != null)
			{
				foreach (AnimationSpeedMultiplier current in multipliers)
				{
					if (fixed2 >= current.startFrame && fixed2 <= current.endFrame)
					{
						@fixed *= (Fixed)((double)current.speedMultiplier);
					}
				}
			}
			@fixed = FixedMath.Max(AnimationTimeline.minSpeed, @fixed);
			fixed2 += @fixed;
			num++;
		}
		return dictionary;
	}
}
