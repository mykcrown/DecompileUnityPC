// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using IconsServer;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class RollbackStatePoolContainer
{
	private class RecentRollbackInfo
	{
		public int rollbackToFrame;

		public int currentFrame;
	}

	private sealed class _chunkifyDesync_c__AnonStorey0
	{
		internal byte[] data;

		internal string matchString;

		internal int chunkIndex;

		internal int byteIndex;

		internal RollbackStatePoolContainer _this;

		internal void __m__0()
		{
			this._this.chunkifyDesync(this.data, this.matchString, this.chunkIndex + 1, this.byteIndex);
		}
	}

	public static readonly string DESYNCED = "RollbackStatePoolContainer.Desynced";

	public static readonly int ROLLBACK_FRAMES = 120;

	public static readonly int INPUT_FRAMES_HISTORY = 1024;

	public const int SNAPSHOT_GRANULARITY = 1;

	public static readonly int ROLLBACK_STATE_BUFFER_LENGTH = RollbackStatePoolContainer.ROLLBACK_FRAMES * 2;

	private RollbackStateContainer[] stateBuffer = new RollbackStateContainer[RollbackStatePoolContainer.ROLLBACK_STATE_BUFFER_LENGTH];

	private HashCodeEvent[] hashCodeEventBuffer = new HashCodeEvent[RollbackStatePoolContainer.ROLLBACK_STATE_BUFFER_LENGTH];

	private RollbackInputFrame[] queuedInputArr = new RollbackInputFrame[RollbackStatePoolContainer.INPUT_FRAMES_HISTORY];

	private int queuedInputLen;

	private RollbackStatePoolContainer.RecentRollbackInfo[] recentRollbackInfo = new RollbackStatePoolContainer.RecentRollbackInfo[5];

	private int rollbackDebugInfoMod;

	public int CachedFrameWithAllInputs;

	public int CachedCurrentFrame;

	public int CachedFullySyncronizedFrame;

	private int desyncChunkSize = 30000;

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServer
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	public bool HasDesynced
	{
		get;
		private set;
	}

	[PostConstruct]
	public void Init()
	{
		this.iconsServerAPI.ListenForServerEvents<DesyncEvent>(new Action<ServerEvent>(this.onDesync));
		this.queuedInputLen = this.queuedInputArr.Length;
		for (int i = 0; i < this.queuedInputArr.Length; i++)
		{
			this.queuedInputArr[i] = new RollbackInputFrame();
		}
		if (RollbackStatePoolContainer.ROLLBACK_FRAMES != InputMsg.MaxInputArraySize)
		{
			throw new Exception("ROLLBACK buffer overflow.");
		}
		for (int j = 0; j < RollbackStatePoolContainer.ROLLBACK_STATE_BUFFER_LENGTH; j++)
		{
			this.hashCodeEventBuffer[j] = new HashCodeEvent();
			this.stateBuffer[j] = new RollbackStateContainer(false);
		}
	}

	public void InitiailizeForMatch(int playerCount)
	{
		for (int i = 0; i < this.queuedInputArr.Length; i++)
		{
			this.queuedInputArr[i].frame = -1;
			this.queuedInputArr[i].hasInputsSet = false;
			this.queuedInputArr[i].inputs = new RollbackInput[playerCount];
			for (int j = 0; j < this.queuedInputArr[i].inputs.Length; j++)
			{
				this.queuedInputArr[i].inputs[j] = new RollbackInput();
				this.queuedInputArr[i].inputs[j].playerID = 255;
			}
		}
		for (int k = 0; k < this.recentRollbackInfo.Length; k++)
		{
			this.recentRollbackInfo[k] = new RollbackStatePoolContainer.RecentRollbackInfo();
		}
	}

	protected int getBufferIndexForFrame(int frame)
	{
		return frame % RollbackStatePoolContainer.ROLLBACK_STATE_BUFFER_LENGTH;
	}

	public RollbackStateContainer GetRollbackState(int frame)
	{
		return this.stateBuffer[this.getBufferIndexForFrame(frame)];
	}

	public void GetRollbackState(int frame, ref RollbackStateContainer rollbackStateContainer)
	{
		rollbackStateContainer = this.stateBuffer[this.getBufferIndexForFrame(frame)];
	}

	public RollbackInputFrame GetQueuedInputsForFrame(int frame)
	{
		int num = frame % this.queuedInputLen;
		RollbackInputFrame rollbackInputFrame = this.queuedInputArr[num];
		if (rollbackInputFrame.frame != frame)
		{
			if (frame < rollbackInputFrame.frame)
			{
				throw new Exception("RollbackBufferTooSmall");
			}
			rollbackInputFrame.frame = frame;
			rollbackInputFrame.hasInputsSet = false;
			RollbackInput[] inputs = rollbackInputFrame.inputs;
			for (int i = 0; i < inputs.Length; i++)
			{
				RollbackInput rollbackInput = inputs[i];
				rollbackInput.playerID = 255;
			}
		}
		return rollbackInputFrame;
	}

	public void ResetQueuedInputs(int frame)
	{
		RollbackInputFrame[] array = this.queuedInputArr;
		for (int i = 0; i < array.Length; i++)
		{
			RollbackInputFrame rollbackInputFrame = array[i];
			if (rollbackInputFrame.hasInputsSet && rollbackInputFrame.frame >= frame)
			{
				rollbackInputFrame.hasInputsSet = false;
			}
		}
	}

	private void onDesync(ServerEvent message)
	{
		int desyncFrame = (message as DesyncEvent).desyncFrame;
		this.onDesyncFrame(desyncFrame);
	}

	private void onDesyncFrame(int frame)
	{
		UnityEngine.Debug.LogError("ON DESYNC FRAME " + frame);
		int bufferIndexForFrame = this.getBufferIndexForFrame(frame);
		RollbackStateContainer rollbackStateContainer = this.stateBuffer[bufferIndexForFrame];
		StringBuilder stringBuilder = new StringBuilder(50000);
		stringBuilder.AppendFormat("ClientID has current frame {0} and inputs up to {1}\n", this.CachedCurrentFrame, this.CachedFrameWithAllInputs);
		int num = Math.Min(RollbackStatePoolContainer.ROLLBACK_FRAMES, 15);
		for (int i = frame - num; i <= frame; i++)
		{
			stringBuilder.AppendFormat("Frame:{0}", i);
			RollbackInputFrame queuedInputsForFrame = this.GetQueuedInputsForFrame(i);
			if (queuedInputsForFrame.hasInputsSet)
			{
				RollbackInput[] inputs = queuedInputsForFrame.inputs;
				for (int j = 0; j < inputs.Length; j++)
				{
					RollbackInput rollbackInput = inputs[j];
					string text = string.Empty;
					IEnumerator enumerator = Enum.GetValues(typeof(ButtonPress)).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object current = enumerator.Current;
							if (rollbackInput.values.GetButton((ButtonPress)current))
							{
								if (text.Length > 0)
								{
									text += ", ";
								}
								text += current.ToString();
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					stringBuilder.AppendFormat("[{0}:{1},{2}:{3}:{4}]", new object[]
					{
						text,
						rollbackInput.values.GetAxis(InputType.HorizontalAxis) / IntegerAxis.MAX_VALUE,
						rollbackInput.values.GetAxis(InputType.VerticalAxis) / IntegerAxis.MAX_VALUE,
						rollbackInput.values.inputFlags,
						rollbackInput.usedPreviousFrame
					});
				}
			}
			else
			{
				stringBuilder.AppendFormat("No Inputs Set", Array.Empty<object>());
			}
			stringBuilder.Append("\n");
		}
		for (int k = 0; k < this.recentRollbackInfo.Length; k++)
		{
			int num2 = (this.rollbackDebugInfoMod + this.recentRollbackInfo.Length - (k + 1)) % this.recentRollbackInfo.Length;
			RollbackStatePoolContainer.RecentRollbackInfo recentRollbackInfo = this.recentRollbackInfo[num2];
			stringBuilder.AppendFormat("Rolled back from {0} to {1}\n", recentRollbackInfo.currentFrame, recentRollbackInfo.rollbackToFrame);
		}
		this.recordDesync(rollbackStateContainer.LogOutHashes(stringBuilder));
		this.HasDesynced = true;
		this.signalBus.Dispatch(RollbackStatePoolContainer.DESYNCED);
	}

	private void recordDesync(string desyncText)
	{
		bool flag = true;
		byte[] bytes = Encoding.ASCII.GetBytes(desyncText);
		if (!Directory.Exists("Desyncs"))
		{
			Directory.CreateDirectory("Desyncs");
		}
		string text = this.battleServer.MatchID.ToString();
		string path = string.Format("Desyncs/Desync-{0}-{1}.wdd", text, this.iconsServerAPI.Username);
		if (flag)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}
		else
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
				{
					gZipStream.Write(bytes, 0, bytes.Length);
				}
				using (FileStream fileStream2 = new FileStream(path, FileMode.Create))
				{
					byte[] array = memoryStream.ToArray();
					fileStream2.Write(array, 0, array.Length);
				}
			}
			this.chunkifyDesync(bytes, text, 0, 0);
		}
	}

	private void chunkifyDesync(byte[] data, string matchString, int chunkIndex, int byteIndex)
	{
		RollbackStatePoolContainer._chunkifyDesync_c__AnonStorey0 _chunkifyDesync_c__AnonStorey = new RollbackStatePoolContainer._chunkifyDesync_c__AnonStorey0();
		_chunkifyDesync_c__AnonStorey.data = data;
		_chunkifyDesync_c__AnonStorey.matchString = matchString;
		_chunkifyDesync_c__AnonStorey.chunkIndex = chunkIndex;
		_chunkifyDesync_c__AnonStorey.byteIndex = byteIndex;
		_chunkifyDesync_c__AnonStorey._this = this;
		int count = Math.Min(_chunkifyDesync_c__AnonStorey.data.Length - _chunkifyDesync_c__AnonStorey.byteIndex, this.desyncChunkSize);
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
			{
				gZipStream.Write(_chunkifyDesync_c__AnonStorey.data, _chunkifyDesync_c__AnonStorey.byteIndex, count);
			}
		}
		_chunkifyDesync_c__AnonStorey.byteIndex += this.desyncChunkSize;
		if (_chunkifyDesync_c__AnonStorey.byteIndex < _chunkifyDesync_c__AnonStorey.data.Length)
		{
			this.timer.SetTimeout(25, new Action(_chunkifyDesync_c__AnonStorey.__m__0));
		}
	}

	public void RecordRollback(int rollbackToFrame, int currentFrame)
	{
		int num = this.rollbackDebugInfoMod % this.recentRollbackInfo.Length;
		this.recentRollbackInfo[num].rollbackToFrame = rollbackToFrame;
		this.recentRollbackInfo[num].currentFrame = currentFrame;
		this.rollbackDebugInfoMod++;
	}

	public void SendHashCode(int playerId, int frame, short hashCode)
	{
		int bufferIndexForFrame = this.getBufferIndexForFrame(frame);
		this.hashCodeEventBuffer[bufferIndexForFrame].senderId = (byte)playerId;
		this.hashCodeEventBuffer[bufferIndexForFrame].frame = frame;
		this.hashCodeEventBuffer[bufferIndexForFrame].hashCode = hashCode;
		this.battleServer.QueueUnreliableMessage(this.hashCodeEventBuffer[bufferIndexForFrame]);
	}
}
