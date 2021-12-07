// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

public class BakedAnimationDataLoader
{
	private struct LoadBakedDataJobContext
	{
		public string characterName;

		public IBakedAnimationDataManager bakedAnimationDataManager;

		public byte[] data;
	}

	private class WaitForHandles<THandle> : CustomYieldInstruction where THandle : WaitHandle
	{
		private List<THandle> handles;

		public override bool keepWaiting
		{
			get
			{
				foreach (THandle current in this.handles)
				{
					if (!current.WaitOne(0))
					{
						return true;
					}
				}
				return false;
			}
		}

		public WaitForHandles(List<THandle> handles)
		{
			this.handles = handles;
		}
	}

	private sealed class _asyncLoadBakedData_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		private sealed class _asyncLoadBakedData_c__AnonStorey1
		{
			internal StringBuilder error;

			internal BakedAnimationDataLoader._asyncLoadBakedData_c__Iterator0 __f__ref_0;
		}

		private sealed class _asyncLoadBakedData_c__AnonStorey2
		{
			internal string characterName;

			internal ManualResetEvent handle;

			internal BakedAnimationDataLoader._asyncLoadBakedData_c__Iterator0 __f__ref_0;

			internal BakedAnimationDataLoader._asyncLoadBakedData_c__Iterator0._asyncLoadBakedData_c__AnonStorey1 __f__ref_1;

			internal void __m__0(object ctx)
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
					object error = this.__f__ref_1.error;
					lock (error)
					{
						this.__f__ref_1.error.AppendFormat("Failed to deserialize baked animation data for {0}\n", this.characterName);
					}
				}
				finally
				{
					this.handle.Set();
				}
			}
		}

		internal Stopwatch _watch___0;

		internal List<ManualResetEvent> _handles___0;

		internal Dictionary<string, byte[]>.Enumerator _locvar0;

		internal IBakedAnimationDataManager bakedAnimationDataManager;

		internal Action<string> onFailure;

		internal Action onDone;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		private BakedAnimationDataLoader._asyncLoadBakedData_c__Iterator0._asyncLoadBakedData_c__AnonStorey1 _locvar1;

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

		public _asyncLoadBakedData_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._locvar1 = new BakedAnimationDataLoader._asyncLoadBakedData_c__Iterator0._asyncLoadBakedData_c__AnonStorey1();
				this._locvar1.__f__ref_0 = this;
				this._watch___0 = new Stopwatch();
				this._watch___0.Start();
				this._handles___0 = new List<ManualResetEvent>();
				this._locvar1.error = new StringBuilder();
				this._locvar0 = BakedAnimationDataLoader.bakedAnimDataAssets.GetEnumerator();
				try
				{
					while (this._locvar0.MoveNext())
					{
						KeyValuePair<string, byte[]> current = this._locvar0.Current;
						BakedAnimationDataLoader._asyncLoadBakedData_c__Iterator0._asyncLoadBakedData_c__AnonStorey2 _asyncLoadBakedData_c__AnonStorey = new BakedAnimationDataLoader._asyncLoadBakedData_c__Iterator0._asyncLoadBakedData_c__AnonStorey2();
						_asyncLoadBakedData_c__AnonStorey.__f__ref_0 = this;
						_asyncLoadBakedData_c__AnonStorey.__f__ref_1 = this._locvar1;
						_asyncLoadBakedData_c__AnonStorey.characterName = current.Key;
						byte[] value = current.Value;
						BakedAnimationDataLoader.LoadBakedDataJobContext loadBakedDataJobContext = new BakedAnimationDataLoader.LoadBakedDataJobContext
						{
							characterName = _asyncLoadBakedData_c__AnonStorey.characterName,
							bakedAnimationDataManager = this.bakedAnimationDataManager,
							data = value
						};
						_asyncLoadBakedData_c__AnonStorey.handle = new ManualResetEvent(false);
						this._handles___0.Add(_asyncLoadBakedData_c__AnonStorey.handle);
						ThreadPool.QueueUserWorkItem(new WaitCallback(_asyncLoadBakedData_c__AnonStorey.__m__0), loadBakedDataJobContext);
					}
				}
				finally
				{
					((IDisposable)this._locvar0).Dispose();
				}
				this._current = new BakedAnimationDataLoader.WaitForHandles<ManualResetEvent>(this._handles___0);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._locvar1.error.Length > 0 && this.onFailure != null)
				{
					this.onFailure(this._locvar1.error.ToString());
				}
				if (this.onDone != null)
				{
					this.onDone();
				}
				this._watch___0.Stop();
				UnityEngine.Debug.LogFormat("Completed deserialization of {0} baked animation data.  Took {1} seconds.", new object[]
				{
					BakedAnimationDataLoader.bakedAnimDataAssets.Count,
					(float)this._watch___0.ElapsedMilliseconds / 1000f
				});
				this._PC = -1;
				break;
			}
			return false;
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

	private static volatile bool isApplicationShutDown = false;

	private static Dictionary<string, byte[]> bakedAnimDataAssets = new Dictionary<string, byte[]>();

	public static void LoadResourcesSync(GameEnvironmentData data)
	{
		BakedAnimationDataLoader.bakedAnimDataAssets.Clear();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (CharacterDefinition current in data.characters)
		{
			if (!current.isRandom)
			{
				BakedAnimationDataLoader.syncLoadRequest(current, stringBuilder);
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

	public static void LoadBakedData(MonoBehaviour host, IBakedAnimationDataManager bakedAnimationDataManager, Action onDone, Action<string> onFailure)
	{
		host.StartCoroutine(BakedAnimationDataLoader.asyncLoadBakedData(bakedAnimationDataManager, onDone, onFailure));
	}

	private static IEnumerator asyncLoadBakedData(IBakedAnimationDataManager bakedAnimationDataManager, Action onDone, Action<string> onFailure)
	{
		BakedAnimationDataLoader._asyncLoadBakedData_c__Iterator0 _asyncLoadBakedData_c__Iterator = new BakedAnimationDataLoader._asyncLoadBakedData_c__Iterator0();
		_asyncLoadBakedData_c__Iterator.bakedAnimationDataManager = bakedAnimationDataManager;
		_asyncLoadBakedData_c__Iterator.onFailure = onFailure;
		_asyncLoadBakedData_c__Iterator.onDone = onDone;
		return _asyncLoadBakedData_c__Iterator;
	}

	public static void OnApplicationQuit(IBakedAnimationDataManager bakedAnimationDataManager)
	{
		BakedAnimationDataLoader.isApplicationShutDown = true;
		bakedAnimationDataManager.OnApplicationQuit();
	}

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

	private static void deserializeBakedAnimDataJob(object data)
	{
		BakedAnimationDataLoader.LoadBakedDataJobContext loadBakedDataJobContext = (BakedAnimationDataLoader.LoadBakedDataJobContext)data;
		BakedAnimationData data2 = BakedAnimationSerializer.Deserialize(new MemoryStream(loadBakedDataJobContext.data));
		loadBakedDataJobContext.bakedAnimationDataManager.Set(loadBakedDataJobContext.characterName, data2);
	}
}
