using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Text;
using BattleServer;
using IconsServer;
using UnityEngine;

// Token: 0x02000883 RID: 2179
public class RollbackStatePoolContainer
{
	// Token: 0x17000D4B RID: 3403
	// (get) Token: 0x060036C0 RID: 14016 RVA: 0x000FFA7A File Offset: 0x000FDE7A
	// (set) Token: 0x060036C1 RID: 14017 RVA: 0x000FFA82 File Offset: 0x000FDE82
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000D4C RID: 3404
	// (get) Token: 0x060036C2 RID: 14018 RVA: 0x000FFA8B File Offset: 0x000FDE8B
	// (set) Token: 0x060036C3 RID: 14019 RVA: 0x000FFA93 File Offset: 0x000FDE93
	[Inject]
	public IBattleServerAPI battleServer { get; set; }

	// Token: 0x17000D4D RID: 3405
	// (get) Token: 0x060036C4 RID: 14020 RVA: 0x000FFA9C File Offset: 0x000FDE9C
	// (set) Token: 0x060036C5 RID: 14021 RVA: 0x000FFAA4 File Offset: 0x000FDEA4
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000D4E RID: 3406
	// (get) Token: 0x060036C6 RID: 14022 RVA: 0x000FFAAD File Offset: 0x000FDEAD
	// (set) Token: 0x060036C7 RID: 14023 RVA: 0x000FFAB5 File Offset: 0x000FDEB5
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000D4F RID: 3407
	// (get) Token: 0x060036C8 RID: 14024 RVA: 0x000FFABE File Offset: 0x000FDEBE
	// (set) Token: 0x060036C9 RID: 14025 RVA: 0x000FFAC6 File Offset: 0x000FDEC6
	public bool HasDesynced { get; private set; }

	// Token: 0x060036CA RID: 14026 RVA: 0x000FFAD0 File Offset: 0x000FDED0
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

	// Token: 0x060036CB RID: 14027 RVA: 0x000FFB74 File Offset: 0x000FDF74
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

	// Token: 0x060036CC RID: 14028 RVA: 0x000FFC3C File Offset: 0x000FE03C
	protected int getBufferIndexForFrame(int frame)
	{
		return frame % RollbackStatePoolContainer.ROLLBACK_STATE_BUFFER_LENGTH;
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x000FFC45 File Offset: 0x000FE045
	public RollbackStateContainer GetRollbackState(int frame)
	{
		return this.stateBuffer[this.getBufferIndexForFrame(frame)];
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x000FFC55 File Offset: 0x000FE055
	public void GetRollbackState(int frame, ref RollbackStateContainer rollbackStateContainer)
	{
		rollbackStateContainer = this.stateBuffer[this.getBufferIndexForFrame(frame)];
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x000FFC68 File Offset: 0x000FE068
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
			foreach (RollbackInput rollbackInput in rollbackInputFrame.inputs)
			{
				rollbackInput.playerID = 255;
			}
		}
		return rollbackInputFrame;
	}

	// Token: 0x060036D0 RID: 14032 RVA: 0x000FFCE8 File Offset: 0x000FE0E8
	public void ResetQueuedInputs(int frame)
	{
		foreach (RollbackInputFrame rollbackInputFrame in this.queuedInputArr)
		{
			if (rollbackInputFrame.hasInputsSet && rollbackInputFrame.frame >= frame)
			{
				rollbackInputFrame.hasInputsSet = false;
			}
		}
	}

	// Token: 0x060036D1 RID: 14033 RVA: 0x000FFD34 File Offset: 0x000FE134
	private void onDesync(ServerEvent message)
	{
		int desyncFrame = (message as DesyncEvent).desyncFrame;
		this.onDesyncFrame(desyncFrame);
	}

	// Token: 0x060036D2 RID: 14034 RVA: 0x000FFD54 File Offset: 0x000FE154
	private void onDesyncFrame(int frame)
	{
		Debug.LogError("ON DESYNC FRAME " + frame);
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
				foreach (RollbackInput rollbackInput in queuedInputsForFrame.inputs)
				{
					string text = string.Empty;
					IEnumerator enumerator = Enum.GetValues(typeof(ButtonPress)).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							if (rollbackInput.values.GetButton((ButtonPress)obj))
							{
								if (text.Length > 0)
								{
									text += ", ";
								}
								text += obj.ToString();
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

	// Token: 0x060036D3 RID: 14035 RVA: 0x00100004 File Offset: 0x000FE404
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
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
				{
					gzipStream.Write(bytes, 0, bytes.Length);
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

	// Token: 0x060036D4 RID: 14036 RVA: 0x00100158 File Offset: 0x000FE558
	private void chunkifyDesync(byte[] data, string matchString, int chunkIndex, int byteIndex)
	{
		int count = Math.Min(data.Length - byteIndex, this.desyncChunkSize);
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
			{
				gzipStream.Write(data, byteIndex, count);
			}
		}
		byteIndex += this.desyncChunkSize;
		if (byteIndex < data.Length)
		{
			this.timer.SetTimeout(25, delegate
			{
				this.chunkifyDesync(data, matchString, chunkIndex + 1, byteIndex);
			});
		}
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x0010024C File Offset: 0x000FE64C
	public void RecordRollback(int rollbackToFrame, int currentFrame)
	{
		int num = this.rollbackDebugInfoMod % this.recentRollbackInfo.Length;
		this.recentRollbackInfo[num].rollbackToFrame = rollbackToFrame;
		this.recentRollbackInfo[num].currentFrame = currentFrame;
		this.rollbackDebugInfoMod++;
	}

	// Token: 0x060036D6 RID: 14038 RVA: 0x00100294 File Offset: 0x000FE694
	public void SendHashCode(int playerId, int frame, short hashCode)
	{
		int bufferIndexForFrame = this.getBufferIndexForFrame(frame);
		this.hashCodeEventBuffer[bufferIndexForFrame].senderId = (byte)playerId;
		this.hashCodeEventBuffer[bufferIndexForFrame].frame = frame;
		this.hashCodeEventBuffer[bufferIndexForFrame].hashCode = hashCode;
		this.battleServer.QueueUnreliableMessage(this.hashCodeEventBuffer[bufferIndexForFrame]);
	}

	// Token: 0x0400253B RID: 9531
	public static readonly string DESYNCED = "RollbackStatePoolContainer.Desynced";

	// Token: 0x0400253C RID: 9532
	public static readonly int ROLLBACK_FRAMES = 120;

	// Token: 0x0400253D RID: 9533
	public static readonly int INPUT_FRAMES_HISTORY = 1024;

	// Token: 0x0400253E RID: 9534
	public const int SNAPSHOT_GRANULARITY = 1;

	// Token: 0x0400253F RID: 9535
	public static readonly int ROLLBACK_STATE_BUFFER_LENGTH = RollbackStatePoolContainer.ROLLBACK_FRAMES * 2;

	// Token: 0x04002545 RID: 9541
	private RollbackStateContainer[] stateBuffer = new RollbackStateContainer[RollbackStatePoolContainer.ROLLBACK_STATE_BUFFER_LENGTH];

	// Token: 0x04002546 RID: 9542
	private HashCodeEvent[] hashCodeEventBuffer = new HashCodeEvent[RollbackStatePoolContainer.ROLLBACK_STATE_BUFFER_LENGTH];

	// Token: 0x04002547 RID: 9543
	private RollbackInputFrame[] queuedInputArr = new RollbackInputFrame[RollbackStatePoolContainer.INPUT_FRAMES_HISTORY];

	// Token: 0x04002548 RID: 9544
	private int queuedInputLen;

	// Token: 0x04002549 RID: 9545
	private RollbackStatePoolContainer.RecentRollbackInfo[] recentRollbackInfo = new RollbackStatePoolContainer.RecentRollbackInfo[5];

	// Token: 0x0400254A RID: 9546
	private int rollbackDebugInfoMod;

	// Token: 0x0400254B RID: 9547
	public int CachedFrameWithAllInputs;

	// Token: 0x0400254C RID: 9548
	public int CachedCurrentFrame;

	// Token: 0x0400254D RID: 9549
	public int CachedFullySyncronizedFrame;

	// Token: 0x0400254E RID: 9550
	private int desyncChunkSize = 30000;

	// Token: 0x02000884 RID: 2180
	private class RecentRollbackInfo
	{
		// Token: 0x0400254F RID: 9551
		public int rollbackToFrame;

		// Token: 0x04002550 RID: 9552
		public int currentFrame;
	}
}
