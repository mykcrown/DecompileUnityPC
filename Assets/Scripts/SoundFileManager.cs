// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SoundFileManager : ISoundFileManager
{
	private Dictionary<string, AssetBundle> assetBundleCache = new Dictionary<string, AssetBundle>();

	private Dictionary<SoundKey, SoundFileData> cache = new Dictionary<SoundKey, SoundFileData>();

	private SoundMap map = new SoundMap();

	private StringBuilder stringBuilder = new StringBuilder();

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	public SoundFileData GetSound(SoundKey key)
	{
		if (!this.cache.ContainsKey(key))
		{
			this.loadSound(key);
		}
		return this.cache[key];
	}

	public AudioData GetSoundAsAudioData(SoundKey key)
	{
		SoundFileData sound = this.GetSound(key);
		return new AudioData
		{
			sound = sound.sound,
			volume = sound.volume,
			syncMode = sound.syncMode
		};
	}

	public void PreloadSound(SoundKey key)
	{
		if (!this.cache.ContainsKey(key))
		{
			this.loadSound(key);
		}
	}

	public void PreloadBundle(SoundBundleKey key, bool preloadIndividualSounds)
	{
		this.getAssetBundle(this.map.GetBundleName(key));
		if (preloadIndividualSounds)
		{
			SoundKey[] bundleSounds = this.map.GetBundleSounds(key);
			for (int i = 0; i < bundleSounds.Length; i++)
			{
				SoundKey key2 = bundleSounds[i];
				this.GetSound(key2);
			}
		}
	}

	private void loadSound(SoundKey key)
	{
		if (this.gameController.MatchIsRunning)
		{
			UnityEngine.Debug.LogError("LOADING SOUND KEY " + key + " DURING GAMEPLAY, please make sure any sounds needed are preloaded!!");
		}
		string fileName = this.map.GetFileName(key);
		if (string.IsNullOrEmpty(fileName))
		{
			UnityEngine.Debug.LogError("ERROR null bundleFileName from key " + key + ", this shouldn't be possible!");
			this.cache[key] = null;
		}
		else
		{
			AssetBundle assetBundle = this.getAssetBundle(fileName);
			if (assetBundle == null)
			{
				UnityEngine.Debug.LogError("BUNDLE NOT FOUND " + fileName);
				this.cache[key] = null;
			}
			else
			{
				ProfilingUtil.BeginTimer();
				string assetName = this.map.GetAssetName(key);
				this.cache[key] = assetBundle.LoadAsset<SoundFileData>(assetName);
				ProfilingUtil.EndTimer("Sound " + assetName);
			}
		}
	}

	private AssetBundle getAssetBundle(string bundleFileName)
	{
		if (!this.assetBundleCache.ContainsKey(bundleFileName))
		{
			this.loadAssetBundle(bundleFileName);
		}
		return this.assetBundleCache[bundleFileName];
	}

	private void loadAssetBundle(string bundleFileName)
	{
		if (this.gameController.MatchIsRunning)
		{
			UnityEngine.Debug.LogError("LOADING SOUND BUNDLE " + bundleFileName + " DURING GAMEPLAY, please make sure any sounds needed are preloaded!!");
		}
		this.stringBuilder.Remove(0, this.stringBuilder.Length);
		this.stringBuilder.Append(Application.streamingAssetsPath);
		this.stringBuilder.Append("/AssetBundles/Sounds/");
		this.stringBuilder.Append(bundleFileName);
		string text = this.stringBuilder.ToString();
		ProfilingUtil.BeginTimer();
		this.assetBundleCache[bundleFileName] = AssetBundle.LoadFromFile(text);
		if (this.assetBundleCache[bundleFileName] == null)
		{
			UnityEngine.Debug.LogError(string.Concat(new string[]
			{
				"Loading sound bundle ",
				bundleFileName,
				" at ",
				text,
				" failed, this shouldn't happen!"
			}));
		}
		ProfilingUtil.EndTimer("Load sound bundle -  " + bundleFileName);
	}
}
