using System;

// Token: 0x020002A5 RID: 677
public class MoveAudioHandle : AudioHandle
{
	// Token: 0x06000E7E RID: 3710 RVA: 0x00059BB8 File Offset: 0x00057FB8
	public MoveAudioHandle(AudioManager manager, SoundEffect sound, MoveModel move, IAudioOwner owner)
	{
		this.manager = manager;
		this.cancelCondition = ((sound.cancelCondition != MoveEffectCancelCondition.Default) ? this.cancelCondition : MoveEffectCancelCondition.Never);
		this.softKillTime = sound.softKillTime;
		int num = sound.noInterruptFrame;
		if (num == 0)
		{
			bool flag = false;
			foreach (HitData hitData2 in move.data.hitData)
			{
				if (!flag || hitData2.startFrame < num)
				{
					flag = true;
					num = hitData2.startFrame;
					if (num == 0)
					{
						break;
					}
				}
			}
		}
		this.noInterruptGameFrame = move.gameFrame + num;
		this.channelId = manager.PlayGameSound(new AudioRequest(sound.GetRandomSound(), owner, delegate(AudioReference channel, bool completed)
		{
			this.channelId = new AudioReference(null, -1);
		}));
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00059C8C File Offset: 0x0005808C
	public bool TryToStop(MoveEndType moveEndType, bool transitioningToContinuingMove, int gameFrame)
	{
		if (this.channelId.sourceId < 0)
		{
			return true;
		}
		if (gameFrame < this.noInterruptGameFrame && MoveModel.EffectHandle.StopConditionMatches(this.cancelCondition, moveEndType, transitioningToContinuingMove))
		{
			this.manager.StopSound(this.channelId, this.softKillTime);
			return true;
		}
		return false;
	}

	// Token: 0x04000874 RID: 2164
	private MoveEffectCancelCondition cancelCondition;

	// Token: 0x04000875 RID: 2165
	private int noInterruptGameFrame;

	// Token: 0x04000876 RID: 2166
	private float softKillTime;
}
