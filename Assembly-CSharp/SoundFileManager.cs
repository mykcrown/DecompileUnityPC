using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020002E0 RID: 736
public class SoundFileManager : ISoundFileManager
{
	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0005C07E File Offset: 0x0005A47E
	// (set) Token: 0x06000F54 RID: 3924 RVA: 0x0005C086 File Offset: 0x0005A486
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x06000F55 RID: 3925 RVA: 0x0005C08F File Offset: 0x0005A48F
	public SoundFileData GetSound(SoundKey key)
	{
		if (!this.cache.ContainsKey(key))
		{
			this.loadSound(key);
		}
		return this.cache[key];
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x0005C0B8 File Offset: 0x0005A4B8
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

	// Token: 0x06000F57 RID: 3927 RVA: 0x0005C0FD File Offset: 0x0005A4FD
	public void PreloadSound(SoundKey key)
	{
		if (!this.cache.ContainsKey(key))
		{
			this.loadSound(key);
		}
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x0005C118 File Offset: 0x0005A518
	public void PreloadBundle(SoundBundleKey key, bool preloadIndividualSounds)
	{
		this.getAssetBundle(this.map.GetBundleName(key));
		if (preloadIndividualSounds)
		{
			foreach (SoundKey key2 in this.map.GetBundleSounds(key))
			{
				this.GetSound(key2);
			}
		}
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x0005C16C File Offset: 0x0005A56C
	private void loadSound(SoundKey key)
	{
		if (this.gameController.MatchIsRunning)
		{
			Debug.LogError("LOADING SOUND KEY " + key + " DURING GAMEPLAY, please make sure any sounds needed are preloaded!!");
		}
		string fileName = this.map.GetFileName(key);
		if (string.IsNullOrEmpty(fileName))
		{
			Debug.LogError("ERROR null bundleFileName from key " + key + ", this shouldn't be possible!");
			this.cache[key] = null;
		}
		else
		{
			AssetBundle assetBundle = this.getAssetBundle(fileName);
			if (assetBundle == null)
			{
				Debug.LogError("BUNDLE NOT FOUND " + fileName);
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

	// Token: 0x06000F5A RID: 3930 RVA: 0x0005C252 File Offset: 0x0005A652
	private AssetBundle getAssetBundle(string bundleFileName)
	{
		if (!this.assetBundleCache.ContainsKey(bundleFileName))
		{
			this.loadAssetBundle(bundleFileName);
		}
		return this.assetBundleCache[bundleFileName];
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x0005C278 File Offset: 0x0005A678
	private void loadAssetBundle(string bundleFileName)
	{
		if (this.gameController.MatchIsRunning)
		{
			Debug.LogError("LOADING SOUND BUNDLE " + bundleFileName + " DURING GAMEPLAY, please make sure any sounds needed are preloaded!!");
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
			Debug.LogError(string.Concat(new string[]
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

	// Token: 0x04000963 RID: 2403
	private Dictionary<string, AssetBundle> assetBundleCache = new Dictionary<string, AssetBundle>();

	// Token: 0x04000964 RID: 2404
	private Dictionary<SoundKey, SoundFileData> cache = new Dictionary<SoundKey, SoundFileData>();

	// Token: 0x04000965 RID: 2405
	private SoundMap map = new SoundMap();

	// Token: 0x04000966 RID: 2406
	private StringBuilder stringBuilder = new StringBuilder();
}
