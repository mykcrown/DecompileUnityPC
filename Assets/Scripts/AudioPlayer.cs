// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioPlayer : IAudioPlayer
{
	public class PooledAudioSource
	{
		public int Id;

		public AudioSource Source;

		public float Time;

		public bool Active;

		public float FadeStartVolume;

		public float FadeStartTime;

		public Action<AudioReference, bool> OnFinish;

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

		public void InitializeFadeOut(float fadeTime)
		{
			this.Time = Mathf.Min(fadeTime, this.Time);
			this.FadeStartVolume = this.Source.volume;
			this.FadeStartTime = this.Time;
		}
	}

	private class AudioPlayerUpdater : MonoBehaviour
	{
	}

	private struct SourceVolumePair
	{
		public AudioSource source;

		public float volume;
	}

	private sealed class _Init_c__AnonStorey1
	{
		internal GameObject parentObject;

		internal AudioPlayer _this;

		internal AudioPlayer.PooledAudioSource __m__0()
		{
			AudioSource source = this.parentObject.AddComponent<AudioSource>();
			int num = this._this.audioSourceID++;
			AudioPlayer.PooledAudioSource pooledAudioSource = new AudioPlayer.PooledAudioSource
			{
				Source = source,
				Active = true,
				Id = num,
				FadeStartTime = 0f,
				FadeStartVolume = 0f
			};
			this._this.audioSourceByID[num] = pooledAudioSource;
			return pooledAudioSource;
		}
	}

	private sealed class _TickAudioPlayer_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal WaitForEndOfFrame _waitForEndOfFrame___0;

		internal AudioPlayer player;

		internal Dictionary<int, AudioPlayer.PooledAudioSource>.Enumerator _locvar0;

		internal List<IAudioOwner>.Enumerator _locvar1;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _TickAudioPlayer_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._waitForEndOfFrame___0 = new WaitForEndOfFrame();
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._locvar0 = this.player.audioSourceByID.GetEnumerator();
			try
			{
				while (this._locvar0.MoveNext())
				{
					KeyValuePair<int, AudioPlayer.PooledAudioSource> current = this._locvar0.Current;
					if (current.Value.Active && !current.Value.Source.loop && current.Value.TickTimeDelta(Time.deltaTime))
					{
						this.player.StopPooledSound(current.Value, current.Value.FadeStartTime == 0f);
					}
				}
			}
			finally
			{
				((IDisposable)this._locvar0).Dispose();
			}
			this._locvar1 = this.player.currentOwners.GetEnumerator();
			try
			{
				while (this._locvar1.MoveNext())
				{
					IAudioOwner current2 = this._locvar1.Current;
					current2.TickTimeDelta(Time.deltaTime);
				}
			}
			finally
			{
				((IDisposable)this._locvar1).Dispose();
			}
			this._current = this._waitForEndOfFrame___0;
			if (!this._disposing)
			{
				this._PC = 1;
			}
			return true;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private const int MENU_SOURCE_BUFFER = 64;

	private const int MUSIC_PLAYER_BUFFER = 4;

	private List<AudioPlayer.SourceVolumePair> menuSources = new List<AudioPlayer.SourceVolumePair>();

	private MusicPlayer primaryMusic;

	private List<MusicPlayer> musicPlayers = new List<MusicPlayer>();

	public List<IAudioOwner> currentOwners = new List<IAudioOwner>();

	private GenericObjectPool<AudioPlayer.PooledAudioSource> audioSourcePool;

	private Dictionary<int, AudioPlayer.PooledAudioSource> audioSourceByID = new Dictionary<int, AudioPlayer.PooledAudioSource>();

	private int audioSourceID;

	private static Action<AudioPlayer.PooledAudioSource> __f__am_cache0;

	[Inject]
	public UserAudioSettings userAudioSettings
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public IExceptionParser exceptionParser
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	public float SfxVolume
	{
		get
		{
			return this.config.soundfxVolume * this.userAudioSettings.GetSoundEffectsVolume();
		}
	}

	public void Init(GameObject parentObject)
	{
		AudioPlayer._Init_c__AnonStorey1 _Init_c__AnonStorey = new AudioPlayer._Init_c__AnonStorey1();
		_Init_c__AnonStorey.parentObject = parentObject;
		_Init_c__AnonStorey._this = this;
		int arg_40_0 = 0;
		GenericObjectPool<AudioPlayer.PooledAudioSource>.NewCallback arg_40_1 = new GenericObjectPool<AudioPlayer.PooledAudioSource>.NewCallback(_Init_c__AnonStorey.__m__0);
		if (AudioPlayer.__f__am_cache0 == null)
		{
			AudioPlayer.__f__am_cache0 = new Action<AudioPlayer.PooledAudioSource>(AudioPlayer._Init_m__0);
		}
		this.audioSourcePool = new GenericObjectPool<AudioPlayer.PooledAudioSource>(arg_40_0, arg_40_1, AudioPlayer.__f__am_cache0, null);
		for (int i = 0; i < 64; i++)
		{
			AudioSource audioSource = _Init_c__AnonStorey.parentObject.AddComponent<AudioSource>();
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
			AudioSource audioSource2 = _Init_c__AnonStorey.parentObject.AddComponent<AudioSource>();
			audioSource2.mute = !this.config.music;
			instance.Init(audioSource2);
			this.musicPlayers.Add(instance);
		}
		UserAudioSettings expr_112 = this.userAudioSettings;
		expr_112.OnMusicVolumeUpdate = (Action)Delegate.Combine(expr_112.OnMusicVolumeUpdate, new Action(this.onMusicVolumeUpdate));
		this.UpdateVolume();
		GameObject gameObject = new GameObject("AudioPlayerUpdater");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		gameObject.AddComponent<AudioPlayer.AudioPlayerUpdater>().StartCoroutine(AudioPlayer.TickAudioPlayer(this));
	}

	public void Register(IAudioOwner owner)
	{
		if (!this.currentOwners.Contains(owner))
		{
			this.currentOwners.Add(owner);
		}
	}

	public void Unregister(IAudioOwner owner)
	{
		if (this.currentOwners.Contains(owner))
		{
			this.currentOwners.Remove(owner);
		}
	}

	public void UpdateVolume()
	{
		this.onMusicVolumeUpdate();
		this.onSfxVolumeUpdate();
	}

	public void Destroy()
	{
		UserAudioSettings expr_06 = this.userAudioSettings;
		expr_06.OnMusicVolumeUpdate = (Action)Delegate.Remove(expr_06.OnMusicVolumeUpdate, new Action(this.onMusicVolumeUpdate));
	}

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

	private bool isMusicPlaying(AudioClip music)
	{
		return this.primaryMusic != null && this.primaryMusic.Clip == music;
	}

	private MusicPlayer getAvailableMusicPlayer()
	{
		foreach (MusicPlayer current in this.musicPlayers)
		{
			if (current.IsAvailable())
			{
				return current;
			}
		}
		return this.musicPlayers[0];
	}

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
			UnityEngine.Debug.LogError(ex.Message);
		}
	}

	private string getMatchId()
	{
		return (!(this.gameController.currentGame != null)) ? "Null" : this.gameController.currentGame.MatchID;
	}

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
			UnityEngine.Debug.LogError("Not enough menu sources in buffer. Consider increasing the size of the buffer " + this.menuSources.Count);
		}
	}

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

	void IAudioPlayer.StopSound(AudioReference audioRef, float fadeTime)
	{
		if (audioRef.owner == null)
		{
			AudioPlayer.PooledAudioSource pooledAudioSource;
			if (!this.audioSourceByID.TryGetValue(audioRef.sourceId, out pooledAudioSource))
			{
				UnityEngine.Debug.LogWarning("Failed to find audio source with id " + audioRef.sourceId);
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

	void IAudioPlayer.PauseSounds(SoundType type, bool paused)
	{
		if (type != SoundType.SFX)
		{
			UnityEngine.Debug.LogWarning("Pausing soundtype " + type + " is not currently supported.");
		}
		else if (paused)
		{
			foreach (AudioPlayer.PooledAudioSource current in this.audioSourceByID.Values)
			{
				current.Source.Pause();
			}
		}
		else
		{
			foreach (AudioPlayer.PooledAudioSource current2 in this.audioSourceByID.Values)
			{
				current2.Source.UnPause();
			}
		}
	}

	private void onMusicVolumeUpdate()
	{
		foreach (MusicPlayer current in this.musicPlayers)
		{
			current.UpdateVolume();
		}
	}

	private void onSfxVolumeUpdate()
	{
		foreach (AudioPlayer.SourceVolumePair current in this.menuSources)
		{
			current.source.volume = current.volume * this.SfxVolume;
		}
	}

	public static IEnumerator TickAudioPlayer(AudioPlayer player)
	{
		AudioPlayer._TickAudioPlayer_c__Iterator0 _TickAudioPlayer_c__Iterator = new AudioPlayer._TickAudioPlayer_c__Iterator0();
		_TickAudioPlayer_c__Iterator.player = player;
		return _TickAudioPlayer_c__Iterator;
	}

	private static void _Init_m__0(AudioPlayer.PooledAudioSource source)
	{
		source.Source.Stop();
		source.Source.clip = null;
		source.Active = true;
		source.FadeStartTime = 0f;
		source.FadeStartVolume = 0f;
	}
}
