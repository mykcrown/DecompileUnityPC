using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x0200057B RID: 1403
public class AnimationTimeline
{
	// Token: 0x06001F7A RID: 8058 RVA: 0x000A0C7C File Offset: 0x0009F07C
	public AnimationTimeline(Fixed baseSpeed, int animFrames, List<AnimationSpeedMultiplier> multipliers = null, int skipFrames = 0)
	{
		this.baseSpeed = baseSpeed;
		this.animFrames = animFrames;
		this.skipFrames = skipFrames;
		this.multipliers = multipliers;
		this.gameFrameToAnimFrameMap = AnimationTimeline.generateGameFrameMap(multipliers, animFrames, skipFrames);
		this.trueFramelength = this.gameFrameToAnimFrameMap.Count;
	}

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06001F7B RID: 8059 RVA: 0x000A0CCC File Offset: 0x0009F0CC
	// (set) Token: 0x06001F7C RID: 8060 RVA: 0x000A0CD4 File Offset: 0x0009F0D4
	public Fixed baseSpeed { get; private set; }

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06001F7D RID: 8061 RVA: 0x000A0CDD File Offset: 0x0009F0DD
	// (set) Token: 0x06001F7E RID: 8062 RVA: 0x000A0CE5 File Offset: 0x0009F0E5
	public int animFrames { get; private set; }

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x06001F7F RID: 8063 RVA: 0x000A0CEE File Offset: 0x0009F0EE
	// (set) Token: 0x06001F80 RID: 8064 RVA: 0x000A0CF6 File Offset: 0x0009F0F6
	public int trueFramelength { get; private set; }

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x06001F81 RID: 8065 RVA: 0x000A0CFF File Offset: 0x0009F0FF
	// (set) Token: 0x06001F82 RID: 8066 RVA: 0x000A0D07 File Offset: 0x0009F107
	public int skipFrames { get; private set; }

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x06001F83 RID: 8067 RVA: 0x000A0D10 File Offset: 0x0009F110
	// (set) Token: 0x06001F84 RID: 8068 RVA: 0x000A0D18 File Offset: 0x0009F118
	private List<AnimationSpeedMultiplier> multipliers { get; set; }

	// Token: 0x170006DD RID: 1757
	// (get) Token: 0x06001F85 RID: 8069 RVA: 0x000A0D21 File Offset: 0x0009F121
	// (set) Token: 0x06001F86 RID: 8070 RVA: 0x000A0D29 File Offset: 0x0009F129
	public Dictionary<int, Fixed> gameFrameToAnimFrameMap { get; private set; }

	// Token: 0x06001F87 RID: 8071 RVA: 0x000A0D34 File Offset: 0x0009F134
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

	// Token: 0x06001F88 RID: 8072 RVA: 0x000A0D8D File Offset: 0x0009F18D
	public void AddSpeedMultiplier(AnimationSpeedMultiplier multiplier)
	{
		this.multipliers.Add(multiplier);
		this.gameFrameToAnimFrameMap = AnimationTimeline.generateGameFrameMap(this.multipliers, this.animFrames, this.skipFrames);
		this.trueFramelength = this.gameFrameToAnimFrameMap.Count;
	}

	// Token: 0x06001F89 RID: 8073 RVA: 0x000A0DCC File Offset: 0x0009F1CC
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
				foreach (AnimationSpeedMultiplier animationSpeedMultiplier in multipliers)
				{
					if (fixed2 >= animationSpeedMultiplier.startFrame && fixed2 <= animationSpeedMultiplier.endFrame)
					{
						@fixed *= (Fixed)((double)animationSpeedMultiplier.speedMultiplier);
					}
				}
			}
			@fixed = FixedMath.Max(AnimationTimeline.minSpeed, @fixed);
			fixed2 += @fixed;
			num++;
		}
		return dictionary;
	}

	// Token: 0x040018EE RID: 6382
	private static Fixed minSpeed = (Fixed)0.001;
}
