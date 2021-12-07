using System;

// Token: 0x020002A4 RID: 676
public class AudioHandle
{
	// Token: 0x06000E7A RID: 3706 RVA: 0x00059B44 File Offset: 0x00057F44
	public AudioHandle(AudioManager manager, SoundEffect sound, IAudioOwner owner)
	{
		this.manager = manager;
		this.channelId = manager.PlayGameSound(new AudioRequest(sound.GetRandomSound(), owner, delegate(AudioReference channel, bool completed)
		{
			this.channelId = new AudioReference(null, -1);
		}));
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x00059B77 File Offset: 0x00057F77
	protected AudioHandle()
	{
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00059B7F File Offset: 0x00057F7F
	public void Stop()
	{
		if (this.channelId.sourceId >= 0)
		{
			this.manager.StopSound(this.channelId, 0f);
		}
	}

	// Token: 0x04000872 RID: 2162
	protected AudioManager manager;

	// Token: 0x04000873 RID: 2163
	protected AudioReference channelId;
}
