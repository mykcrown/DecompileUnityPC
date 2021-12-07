// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

public class MoveAudioHandle : AudioHandle
{
	private MoveEffectCancelCondition cancelCondition;

	private int noInterruptGameFrame;

	private float softKillTime;

	public MoveAudioHandle(AudioManager manager, SoundEffect sound, MoveModel move, IAudioOwner owner)
	{
		this.manager = manager;
		this.cancelCondition = ((sound.cancelCondition != MoveEffectCancelCondition.Default) ? this.cancelCondition : MoveEffectCancelCondition.Never);
		this.softKillTime = sound.softKillTime;
		int num = sound.noInterruptFrame;
		if (num == 0)
		{
			bool flag = false;
			HitData[] hitData = move.data.hitData;
			for (int i = 0; i < hitData.Length; i++)
			{
				HitData hitData2 = hitData[i];
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
		this.channelId = manager.PlayGameSound(new AudioRequest(sound.GetRandomSound(), owner, new Action<AudioReference, bool>(this._MoveAudioHandle_m__0)));
	}

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

	private void _MoveAudioHandle_m__0(AudioReference channel, bool completed)
	{
		this.channelId = new AudioReference(null, -1);
	}
}
