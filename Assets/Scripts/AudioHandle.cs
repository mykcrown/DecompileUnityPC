// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

public class AudioHandle
{
	protected AudioManager manager;

	protected AudioReference channelId;

	public AudioHandle(AudioManager manager, SoundEffect sound, IAudioOwner owner)
	{
		this.manager = manager;
		this.channelId = manager.PlayGameSound(new AudioRequest(sound.GetRandomSound(), owner, new Action<AudioReference, bool>(this._AudioHandle_m__0)));
	}

	protected AudioHandle()
	{
	}

	public void Stop()
	{
		if (this.channelId.sourceId >= 0)
		{
			this.manager.StopSound(this.channelId, 0f);
		}
	}

	private void _AudioHandle_m__0(AudioReference channel, bool completed)
	{
		this.channelId = new AudioReference(null, -1);
	}
}
