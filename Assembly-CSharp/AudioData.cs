using System;
using UnityEngine;

// Token: 0x020002A3 RID: 675
[Serializable]
public struct AudioData
{
	// Token: 0x06000E77 RID: 3703 RVA: 0x00059AE8 File Offset: 0x00057EE8
	public AudioData(AudioClip sound, float volume = 1f, AudioSyncMode syncMode = AudioSyncMode.Synchronized)
	{
		this.sound = sound;
		this.volume = volume;
		this.syncMode = syncMode;
		this.isVoice = false;
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x00059B08 File Offset: 0x00057F08
	public AudioData MultiplyVolume(float volumeMult)
	{
		AudioData result = new AudioData(this.sound, this.volume * volumeMult, this.syncMode);
		return result;
	}

	// Token: 0x0400086D RID: 2157
	public AudioClip sound;

	// Token: 0x0400086E RID: 2158
	public float volume;

	// Token: 0x0400086F RID: 2159
	public AudioSyncMode syncMode;

	// Token: 0x04000870 RID: 2160
	public bool isVoice;

	// Token: 0x04000871 RID: 2161
	public static AudioData Empty = new AudioData(null, 1f, AudioSyncMode.Synchronized);
}
