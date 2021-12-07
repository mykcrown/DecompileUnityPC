using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class AudioPlayer : IAudioPlayer
{
	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x0005A648 File Offset: 0x00058A48
	// (set) Token: 0x06000EC5 RID: 3781 RVA: 0x0005A650 File Offset: 0x00058A50
	[Inject]
	public UserAudioSettings userAudioSettings { get; set; }

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x0005A659 File Offset: 0x00058A59
	// (set) Token: 0x06000EC7 RID: 3783 RVA: 0x0005A661 File Offset: 0x00058A61
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x0005A66A File Offset: 0x00058A6A
	// (set) Token: 0x06000EC9 RID: 3785 RVA: 0x0005A672 File Offset: 0x00058A72
	[Inject]
	public IExceptionParser exceptionParser { get; set; }

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000ECA RID: 3786 RVA: 0x0005A67B File Offset: 0x00058A7B
	// (set) Token: 0x06000ECB RID: 3787 RVA: 0x0005A683 File Offset: 0x00058A83
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000ECC RID: 3788 RVA: 0x0005A68C File Offset: 0x00058A8C
	// (set) Token: 0x06000ECD RID: 3789 RVA: 0x0005A694 File Offset: 0x00058A94
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0005A69D File Offset: 0x00058A9D
	public float SfxVolume
	{
		get
		{
			return this.config.soundfxVolume * this.userAudioSettings.GetSoundEffectsVolume();
		}
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0005A6B8 File Offset: 0x00058AB8
	public void Init(GameObject parentObject)
	{
		this.audioSourcePool = new GenericObjectPool<AudioPlayer.PooledAudioSource>(0, delegate()
		{
			AudioSource source = parentObject.AddComponent<AudioSource>();
			int num = this.audioSourceID++;
			AudioPlayer.PooledAudioSource pooledAudioSource = new AudioPlayer.PooledAudioSource
			{
				Source = source,
				Active = true,
				Id = num,
				FadeStartTime = 0f,
				FadeStartVolume = 0f
			};
			this.audioSourceByID[num] = pooledAudioSource;
			return pooledAudioSource;
		}, delegate(AudioPlayer.PooledAudioSource source)
		{
			source.Source.Stop();
			source.Source.clip = null;
			source.Active = true;
			source.FadeStartTime = 0f;
			source.FadeStartVolume = 0f;
		}, null);
		for (int i = 0; i < 64; i++)
		{
			AudioSource audioSource = parentObject.AddComponent<AudioSource>();
			audioSource.loop = false;
			audioSource.playOnAwake = false;
			audioSource.ignoreListenerVolume = true;
			audioSource.priority = 256;
			AudioPlayer.SourceVolumePair item = default(AudioPlayer.SourceVolumePair);
			item.source = audioSource;
			item.volume = 1f;
			this.menuSources.Add(item);
		}
		for (int j = 0; j < 4; j++)
		{
			MusicPlayer instance = this.injector.GetInstance<MusicPlayer>();
			AudioSource audioSource2 = parentObject.AddComponent<AudioSource>();
			audioSource2.mute = !this.config.music;
			instance.Init(audioSource2);
			this.musicPlayers.Add(instance);
		}
		UserAudioSettings userAudioSettings = this.userAudioSettings;
		userAudioSettings.OnMusicVolumeUpdate = (Action)Delegate.Combine(userAudioSettings.OnMusicVolumeUpdate, new Action(this.onMusicVolumeUpdate));
		this.UpdateVolume();
		GameObject gameObject = new GameObject("AudioPlayerUpdater");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		gameObject.AddComponent<AudioPlayer.AudioPlayerUpdater>().StartCoroutine(AudioPlayer.TickAudioPlayer(this));
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0005A824 File Offset: 0x00058C24
	public void Register(IAudioOwner owner)
	{
		if (!this.currentOwners.Contains(owner))
		{
			this.currentOwners.Add(owner);
		}
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x0005A843 File Offset: 0x00058C43
	public void Unregister(IAudioOwner owner)
	{
		if (this.currentOwners.Contains(owner))
		{
			this.currentOwners.Remove(owner);
		}
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x0005A863 File Offset: 0x00058C63
	public void UpdateVolume()
	{
		this.onMusicVolumeUpdate();
		this.onSfxVolumeUpdate();
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x0005A871 File Offset: 0x00058C71
	public void Destroy()
	{
		UserAudioSettings userAudioSettings = this.userAudioSettings;
		userAudioSettings.OnMusicVolumeUpdate = (Action)Delegate.Remove(userAudioSettings.OnMusicVolumeUpdate, new Action(this.onMusicVolumeUpdate));
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0005A89C File Offset: 0x00058C9C
	public void PlayMusic(AudioClip music, float volume)
	{
		if (this.isMusicPlaying(music))
		{
			return;
		}
		if (this.primaryMusic != null)
		{
			this.primaryMusic.FadeOutAndCancel(-1f, null);
		}
		if (music != null)
		{
			this.primaryMusic = this.getAvailableMusicPlayer();
			this.primaryMusic.Play(music, volume);
		}
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x0005A8F7 File Offset: 0x00058CF7
	private bool isMusicPlaying(AudioClip music)
	{
		return this.primaryMusic != null && this.primaryMusic.Clip == music;
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0005A918 File Offset: 0x00058D18
	private MusicPlayer getAvailableMusicPlayer()
	{
		foreach (MusicPlayer musicPlayer in this.musicPlayers)
		{
			if (musicPlayer.IsAvailable())
			{
				return musicPlayer;
			}
		}
		return this.musicPlayers[0];
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0005A990 File Offset: 0x00058D90
	public void StopMusic(Action callback, float fadeTime = -1f)
	{
		if (this.primaryMusic == null)
		{
			if (callback != null)
			{
				callback();
			}
		}
		else
		{
			this.primaryMusic.FadeOutAndCancel(fadeTime, callback);
		}
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0005A9BC File Offset: 0x00058DBC
	public void PlayGameSound(AudioRequest request, int sourceId, IAudioOwner attachTo)
	{
		try
		{
			if (attachTo != null)
			{
				AudioPlayer.PooledAudioSource source = attachTo.GetSource(sourceId);
				source.Source.spatialBlend = this.config.soundSettings.spatialBlend;
				source.Source.minDistance = this.config.soundSettings.spatialBlendMinDist;
				source.Source.maxDistance = this.config.soundSettings.spatialBlendMaxDist;
				source.Source.dopplerLevel = 0f;
				source.Source.loop = false;
				source.Source.volume = this.SfxVolume * request.volume;
				source.Source.clip = request.sound;
				source.Source.pitch = request.pitch;
				source.Source.Play();
				source.Time = request.sound.length;
				source.OnFinish = request.onFinish;
			}
			else
			{
				AudioPlayer.PooledAudioSource pooledAudioSource = this.audioSourceByID[sourceId];
				pooledAudioSource.Source.dopplerLevel = 0f;
				pooledAudioSource.Source.loop = false;
				pooledAudioSource.Source.volume = this.SfxVolume * request.volume;
				pooledAudioSource.Source.clip = request.sound;
				pooledAudioSource.Source.pitch = request.pitch;
				pooledAudioSource.Source.Play();
				pooledAudioSource.Time = request.sound.length;
				pooledAudioSource.OnFinish = request.onFinish;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0005AB74 File Offset: 0x00058F74
	private string getMatchId()
	{
		return (!(this.gameController.currentGame != null)) ? "Null" : this.gameController.currentGame.MatchID;
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0005ABA8 File Offset: 0x00058FA8
	public void PlayMenuSound(AudioClip sound, float volume = 1f, float pitch = 1f, float delay = 0f)
	{
		if (sound != null)
		{
			this.onSfxVolumeUpdate();
			for (int i = 0; i < this.menuSources.Count; i++)
			{
				AudioPlayer.SourceVolumePair value = this.menuSources[i];
				if (!this.menuSources[i].source.isPlaying)
				{
					value.source.volume = volume * this.SfxVolume;
					value.source.clip = sound;
					value.source.pitch = pitch;
					value.source.PlayScheduled(AudioSettings.dspTime + (double)delay);
					value.volume = volume;
					this.menuSources[i] = value;
					return;
				}
			}
			Debug.LogError("Not enough menu sources in buffer. Consider increasing the size of the buffer " + this.menuSources.Count);
		}
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0005AC88 File Offset: 0x00059088
	public int GetAudioSourceId(IAudioOwner owner)
	{
		if (owner == null)
		{
			AudioPlayer.PooledAudioSource pooledAudioSource = this.audioSourcePool.New();
			return pooledAudioSource.Id;
		}
		AudioPlayer.PooledAudioSource pooledAudioSource2 = owner.New();
		return pooledAudioSource2.Id;
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0005ACBC File Offset: 0x000590BC
	public void PlayLoopingSound(AudioRequest request, int sourceId)
	{
		if (request.attachTo == null)
		{
			AudioPlayer.PooledAudioSource pooledAudioSource = this.audioSourceByID[sourceId];
			pooledAudioSource.Source.loop = true;
			pooledAudioSource.Source.volume = this.SfxVolume * request.volume;
			pooledAudioSource.Source.clip = request.sound;
			pooledAudioSource.Source.pitch = request.pitch;
			pooledAudioSource.Source.Play();
			pooledAudioSource.OnFinish = request.onFinish;
		}
		else
		{
			AudioPlayer.PooledAudioSource source = request.attachTo.GetSource(sourceId);
			source.Source.loop = true;
			source.Source.volume = this.SfxVolume * request.volume;
			source.Source.clip = request.sound;
			source.Source.pitch = request.pitch;
			source.Source.Play();
			source.OnFinish = request.onFinish;
		}
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0005ADB8 File Offset: 0x000591B8
	private void StopPooledSound(AudioPlayer.PooledAudioSource source, bool clipCompleted)
	{
		if (source.Source != null && !source.Source.Equals(null))
		{
			try
			{
				source.Source.Stop();
				if (source.OnFinish != null)
				{
					source.OnFinish(new AudioReference(null, source.Id), clipCompleted);
					source.OnFinish = null;
				}
			}
			finally
			{
				this.audioSourcePool.Store(source);
				source.Active = false;
			}
		}
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0005AE48 File Offset: 0x00059248
	void IAudioPlayer.StopSound(AudioReference audioRef, float fadeTime)
	{
		if (audioRef.owner == null)
		{
			AudioPlayer.PooledAudioSource pooledAudioSource;
			if (!this.audioSourceByID.TryGetValue(audioRef.sourceId, out pooledAudioSource))
			{
				Debug.LogWarning("Failed to find audio source with id " + audioRef.sourceId);
				return;
			}
			if (fadeTime <= 0f)
			{
				this.StopPooledSound(pooledAudioSource, false);
			}
			else
			{
				pooledAudioSource.InitializeFadeOut(fadeTime);
			}
		}
		else
		{
			audioRef.owner.StopSound(audioRef.sourceId, fadeTime);
		}
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0005AED0 File Offset: 0x000592D0
	void IAudioPlayer.PauseSounds(SoundType type, bool paused)
	{
		if (type != SoundType.SFX)
		{
			Debug.LogWarning("Pausing soundtype " + type + " is not currently supported.");
		}
		else if (paused)
		{
			foreach (AudioPlayer.PooledAudioSource pooledAudioSource in this.audioSourceByID.Values)
			{
				pooledAudioSource.Source.Pause();
			}
		}
		else
		{
			foreach (AudioPlayer.PooledAudioSource pooledAudioSource2 in this.audioSourceByID.Values)
			{
				pooledAudioSource2.Source.UnPause();
			}
		}
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x0005AFC4 File Offset: 0x000593C4
	private void onMusicVolumeUpdate()
	{
		foreach (MusicPlayer musicPlayer in this.musicPlayers)
		{
			musicPlayer.UpdateVolume();
		}
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0005B020 File Offset: 0x00059420
	private void onSfxVolumeUpdate()
	{
		foreach (AudioPlayer.SourceVolumePair sourceVolumePair in this.menuSources)
		{
			sourceVolumePair.source.volume = sourceVolumePair.volume * this.SfxVolume;
		}
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0005B090 File Offset: 0x00059490
	public static IEnumerator TickAudioPlayer(AudioPlayer player)
	{
		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		for (;;)
		{
			foreach (KeyValuePair<int, AudioPlayer.PooledAudioSource> keyValuePair in player.audioSourceByID)
			{
				if (keyValuePair.Value.Active && !keyValuePair.Value.Source.loop && keyValuePair.Value.TickTimeDelta(Time.deltaTime))
				{
					player.StopPooledSound(keyValuePair.Value, keyValuePair.Value.FadeStartTime == 0f);
				}
			}
			foreach (IAudioOwner audioOwner in player.currentOwners)
			{
				audioOwner.TickTimeDelta(Time.deltaTime);
			}
			yield return waitForEndOfFrame;
		}
		yield break;
	}

	// Token: 0x04000884 RID: 2180
	private const int MENU_SOURCE_BUFFER = 64;

	// Token: 0x04000885 RID: 2181
	private const int MUSIC_PLAYER_BUFFER = 4;

	// Token: 0x0400088B RID: 2187
	private List<AudioPlayer.SourceVolumePair> menuSources = new List<AudioPlayer.SourceVolumePair>();

	// Token: 0x0400088C RID: 2188
	private MusicPlayer primaryMusic;

	// Token: 0x0400088D RID: 2189
	private List<MusicPlayer> musicPlayers = new List<MusicPlayer>();

	// Token: 0x0400088E RID: 2190
	public List<IAudioOwner> currentOwners = new List<IAudioOwner>();

	// Token: 0x0400088F RID: 2191
	private GenericObjectPool<AudioPlayer.PooledAudioSource> audioSourcePool;

	// Token: 0x04000890 RID: 2192
	private Dictionary<int, AudioPlayer.PooledAudioSource> audioSourceByID = new Dictionary<int, AudioPlayer.PooledAudioSource>();

	// Token: 0x04000891 RID: 2193
	private int audioSourceID;

	// Token: 0x020002AB RID: 683
	public class PooledAudioSource
	{
		// Token: 0x06000EE5 RID: 3813 RVA: 0x0005B0EC File Offset: 0x000594EC
		public bool TickTimeDelta(float dt)
		{
			if (this.Time == 0f)
			{
				return false;
			}
			this.Time = Mathf.Max(0f, this.Time - dt);
			if (this.FadeStartTime > 0f)
			{
				this.Source.volume = MathUtil.Mix(this.FadeStartVolume, 0f, this.Time, 2f);
			}
			return this.Time == 0f;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0005B166 File Offset: 0x00059566
		public void InitializeFadeOut(float fadeTime)
		{
			this.Time = Mathf.Min(fadeTime, this.Time);
			this.FadeStartVolume = this.Source.volume;
			this.FadeStartTime = this.Time;
		}

		// Token: 0x04000893 RID: 2195
		public int Id;

		// Token: 0x04000894 RID: 2196
		public AudioSource Source;

		// Token: 0x04000895 RID: 2197
		public float Time;

		// Token: 0x04000896 RID: 2198
		public bool Active;

		// Token: 0x04000897 RID: 2199
		public float FadeStartVolume;

		// Token: 0x04000898 RID: 2200
		public float FadeStartTime;

		// Token: 0x04000899 RID: 2201
		public Action<AudioReference, bool> OnFinish;
	}

	// Token: 0x020002AC RID: 684
	private class AudioPlayerUpdater : MonoBehaviour
	{
	}

	// Token: 0x020002AD RID: 685
	private struct SourceVolumePair
	{
		// Token: 0x0400089A RID: 2202
		public AudioSource source;

		// Token: 0x0400089B RID: 2203
		public float volume;
	}
}
