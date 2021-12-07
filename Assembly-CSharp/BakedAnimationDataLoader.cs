using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

// Token: 0x02000344 RID: 836
public class BakedAnimationDataLoader
{
	// Token: 0x060011B6 RID: 4534 RVA: 0x000662C8 File Offset: 0x000646C8
	public static void LoadResourcesSync(GameEnvironmentData data)
	{
		BakedAnimationDataLoader.bakedAnimDataAssets.Clear();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (CharacterDefinition characterDefinition in data.characters)
		{
			if (!characterDefinition.isRandom)
			{
				BakedAnimationDataLoader.syncLoadRequest(characterDefinition, stringBuilder);
			}
		}
		if (stringBuilder.Length > 0)
		{
			UnityEngine.Debug.LogErrorFormat("Errors loading baked animation data resources: {0}", new object[]
			{
				stringBuilder.ToString()
			});
		}
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x00066364 File Offset: 0x00064764
	public static void LoadBakedData(MonoBehaviour host, IBakedAnimationDataManager bakedAnimationDataManager, Action onDone, Action<string> onFailure)
	{
		host.StartCoroutine(BakedAnimationDataLoader.asyncLoadBakedData(bakedAnimationDataManager, onDone, onFailure));
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x00066378 File Offset: 0x00064778
	private static IEnumerator asyncLoadBakedData(IBakedAnimationDataManager bakedAnimationDataManager, Action onDone, Action<string> onFailure)
	{
		Stopwatch watch = new Stopwatch();
		watch.Start();
		List<ManualResetEvent> handles = new List<ManualResetEvent>();
		StringBuilder error = new StringBuilder();
		foreach (KeyValuePair<string, byte[]> keyValuePair in BakedAnimationDataLoader.bakedAnimDataAssets)
		{
			string characterName = keyValuePair.Key;
			byte[] value = keyValuePair.Value;
			BakedAnimationDataLoader.LoadBakedDataJobContext loadBakedDataJobContext = new BakedAnimationDataLoader.LoadBakedDataJobContext
			{
				characterName = characterName,
				bakedAnimationDataManager = bakedAnimationDataManager,
				data = value
			};
			ManualResetEvent handle = new ManualResetEvent(false);
			handles.Add(handle);
			ThreadPool.QueueUserWorkItem(delegate(object ctx)
			{
				if (BakedAnimationDataLoader.isApplicationShutDown)
				{
					return;
				}
				try
				{
					BakedAnimationDataLoader.deserializeBakedAnimDataJob(ctx);
				}
				catch
				{
					object error = error;
					lock (error)
					{
						error.AppendFormat("Failed to deserialize baked animation data for {0}\n", characterName);
					}
				}
				finally
				{
					handle.Set();
				}
			}, loadBakedDataJobContext);
		}
		yield return new BakedAnimationDataLoader.WaitForHandles<ManualResetEvent>(handles);
		if (error.Length > 0 && onFailure != null)
		{
			onFailure(error.ToString());
		}
		if (onDone != null)
		{
			onDone();
		}
		watch.Stop();
		UnityEngine.Debug.LogFormat("Completed deserialization of {0} baked animation data.  Took {1} seconds.", new object[]
		{
			BakedAnimationDataLoader.bakedAnimDataAssets.Count,
			(float)watch.ElapsedMilliseconds / 1000f
		});
		yield break;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x000663A1 File Offset: 0x000647A1
	public static void OnApplicationQuit(IBakedAnimationDataManager bakedAnimationDataManager)
	{
		BakedAnimationDataLoader.isApplicationShutDown = true;
		bakedAnimationDataManager.OnApplicationQuit();
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x000663B4 File Offset: 0x000647B4
	private static void syncLoadRequest(CharacterDefinition character, StringBuilder error)
	{
		string characterBakedDataAssetPath = BakedAnimationSerializer.GetCharacterBakedDataAssetPath(character.characterName, true);
		TextAsset textAsset = Resources.Load<TextAsset>(characterBakedDataAssetPath);
		if (textAsset == null)
		{
			lock (error)
			{
				error.AppendFormat("Failed to load baked animation data {0}\n", characterBakedDataAssetPath);
			}
		}
		else
		{
			object obj = BakedAnimationDataLoader.bakedAnimDataAssets;
			lock (obj)
			{
				byte[] bytes = textAsset.bytes;
				BakedAnimationDataLoader.bakedAnimDataAssets.Add(character.characterName, bytes);
			}
		}
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x00066468 File Offset: 0x00064868
	private static void deserializeBakedAnimDataJob(object data)
	{
		BakedAnimationDataLoader.LoadBakedDataJobContext loadBakedDataJobContext = (BakedAnimationDataLoader.LoadBakedDataJobContext)data;
		BakedAnimationData data2 = BakedAnimationSerializer.Deserialize(new MemoryStream(loadBakedDataJobContext.data));
		loadBakedDataJobContext.bakedAnimationDataManager.Set(loadBakedDataJobContext.characterName, data2);
	}

	// Token: 0x04000B4B RID: 2891
	private static volatile bool isApplicationShutDown = false;

	// Token: 0x04000B4C RID: 2892
	private static Dictionary<string, byte[]> bakedAnimDataAssets = new Dictionary<string, byte[]>();

	// Token: 0x02000345 RID: 837
	private struct LoadBakedDataJobContext
	{
		// Token: 0x04000B4D RID: 2893
		public string characterName;

		// Token: 0x04000B4E RID: 2894
		public IBakedAnimationDataManager bakedAnimationDataManager;

		// Token: 0x04000B4F RID: 2895
		public byte[] data;
	}

	// Token: 0x02000346 RID: 838
	private class WaitForHandles<THandle> : CustomYieldInstruction where THandle : WaitHandle
	{
		// Token: 0x060011BD RID: 4541 RVA: 0x000664B6 File Offset: 0x000648B6
		public WaitForHandles(List<THandle> handles)
		{
			this.handles = handles;
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060011BE RID: 4542 RVA: 0x000664C8 File Offset: 0x000648C8
		public override bool keepWaiting
		{
			get
			{
				foreach (THandle thandle in this.handles)
				{
					if (!thandle.WaitOne(0))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x04000B50 RID: 2896
		private List<THandle> handles;
	}
}
