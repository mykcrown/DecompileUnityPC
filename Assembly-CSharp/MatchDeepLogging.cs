using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006CF RID: 1743
public class MatchDeepLogging : IMatchDeepLogging
{
	// Token: 0x17000ABD RID: 2749
	// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x000E3B10 File Offset: 0x000E1F10
	// (set) Token: 0x06002BAA RID: 11178 RVA: 0x000E3B18 File Offset: 0x000E1F18
	[Inject]
	public ISaveFileData saveFileData { get; set; }

	// Token: 0x17000ABE RID: 2750
	// (get) Token: 0x06002BAB RID: 11179 RVA: 0x000E3B21 File Offset: 0x000E1F21
	// (set) Token: 0x06002BAC RID: 11180 RVA: 0x000E3B29 File Offset: 0x000E1F29
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000ABF RID: 2751
	// (get) Token: 0x06002BAD RID: 11181 RVA: 0x000E3B32 File Offset: 0x000E1F32
	// (set) Token: 0x06002BAE RID: 11182 RVA: 0x000E3B3A File Offset: 0x000E1F3A
	[Inject]
	public IServerConnectionManager serverConnectionManager { get; set; }

	// Token: 0x17000AC0 RID: 2752
	// (get) Token: 0x06002BAF RID: 11183 RVA: 0x000E3B43 File Offset: 0x000E1F43
	// (set) Token: 0x06002BB0 RID: 11184 RVA: 0x000E3B4B File Offset: 0x000E1F4B
	[Inject]
	public IRollbackStatus status { get; set; }

	// Token: 0x06002BB1 RID: 11185 RVA: 0x000E3B54 File Offset: 0x000E1F54
	public void BeginMatch(string matchId, string opponentName)
	{
		this.currentMatch = new MatchDeepLogging.Match();
		this.currentMatch.matchId = matchId;
		this.currentMatch.opponentName = opponentName;
		this.currentMatch.needLogResults = true;
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x000E3B85 File Offset: 0x000E1F85
	public void EndMatch()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		if (this.currentMatch.needLogResults)
		{
			this.saveMatchLog();
		}
		this.currentMatch = null;
	}

	// Token: 0x06002BB3 RID: 11187 RVA: 0x000E3BB0 File Offset: 0x000E1FB0
	private void saveMatchLog()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		List<string> list = new List<string>();
		list.Add(string.Join(",", new List<string>
		{
			"TICK AT FRAME",
			"Problem?",
			"Update begin time",
			"Tick begin time",
			"Processing end time",
			"Render time",
			"Tick end time",
			"Prerender time",
			"Postrender time",
			"Frame delta",
			"Total memory",
			"remoteInputFrames",
			"Rollback frames",
			"Rollback duration",
			"Broadcast local inputs",
			"calculatedLatencyMs",
			"fullyConfirmedFrame",
			"initialTimestepDelta",
			"inputDelayFrames",
			"msSinceStartCompensated",
			"renderDelay?",
			"syncDelay?",
			"renderWaitError?",
			"frameWaitError?"
		}.ToArray()));
		for (int i = 0; i < this.currentMatch.currentIndex; i++)
		{
			MatchDeepLogging.Match.Record record = this.currentMatch.list[i];
			List<string> list2 = new List<string>();
			list2.Add(record.currentFrame + string.Empty);
			list2.Add((!record.isProblem) ? string.Empty : "TRUE");
			list2.Add(record.updateBeginTime + string.Empty);
			list2.Add(record.tickBeginTime + string.Empty);
			list2.Add(record.processingEndTime + string.Empty);
			list2.Add(record.renderFrameTime + string.Empty);
			list2.Add(record.tickEndTime + string.Empty);
			list2.Add(record.preRenderTime + string.Empty);
			list2.Add(record.postRenderTime + string.Empty);
			list2.Add(record.frameDelta + string.Empty);
			list2.Add(record.totalMemory + string.Empty);
			string text = string.Empty;
			foreach (MatchDeepLogging.Match.Record.RemoteInputFrame remoteInputFrame in record.remoteInputFrame)
			{
				text = text + " player: " + remoteInputFrame.playerID;
				text = text + " frame: " + remoteInputFrame.frame;
				text = text + " time: " + remoteInputFrame.time;
				text = text + " numInputs: " + remoteInputFrame.numInputs;
			}
			list2.Add(text);
			list2.Add(record.rollbackFramesTicked + string.Empty);
			list2.Add(record.rollbackDurationMs + string.Empty);
			list2.Add(record.broadcastInputs + string.Empty);
			list2.Add(record.calculatedLatencyMs + string.Empty);
			list2.Add(record.fullyConfirmedFrame + string.Empty);
			list2.Add(record.initialTimestepDelta + string.Empty);
			list2.Add(record.inputDelayFrames + string.Empty);
			list2.Add(record.msSinceStartCompensated + string.Empty);
			list2.Add((!record.renderDelay) ? string.Empty : "TRUE");
			list2.Add((!record.syncDelay) ? string.Empty : "TRUE");
			list2.Add((!record.renderWaitError) ? string.Empty : "TRUE");
			list2.Add((!record.frameWaitError) ? string.Empty : "TRUE");
			string item = string.Join(",", list2.ToArray());
			list.Add(item);
		}
		string text2 = "\r\n" + string.Join("\r\n", list.ToArray());
		string str = string.Concat(new string[]
		{
			"deeplog_vs",
			this.currentMatch.opponentName.ToUpper(),
			"_",
			this.currentMatch.matchId,
			".txt"
		});
		Debug.Log("Save log with file name " + str);
		float num = Time.realtimeSinceStartup - realtimeSinceStartup;
		Debug.Log("Deep log save time: " + num);
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x000E4150 File Offset: 0x000E2550
	public void UpdateLoopBegin()
	{
		if (this.gameController.currentGame == null || this.currentMatch == null || this.gameController.currentGame.IsPaused)
		{
			return;
		}
		this.currentMatch.currentIndex++;
		if (this.currentMatch.currentIndex >= this.currentMatch.list.Count)
		{
			this.currentMatch.currentIndex = 0;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.updateBeginTime = WTime.precisionTimeSinceStartup;
		record.totalMemory = GC.GetTotalMemory(false);
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x000E4206 File Offset: 0x000E2606
	public void UpdateLoopEnd()
	{
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x000E4208 File Offset: 0x000E2608
	public void RecordTickStart(int frameDelta, int currentFrame)
	{
		if (this.currentMatch == null)
		{
			return;
		}
		if (this.gameController.currentGame.IsPaused)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.tickBeginTime = WTime.precisionTimeSinceStartup;
		record.frameDelta = frameDelta;
		record.currentFrame = currentFrame;
		if (frameDelta > 1)
		{
			record.isProblem = true;
			this.currentMatch.needLogResults = true;
		}
	}

	// Token: 0x06002BB7 RID: 11191 RVA: 0x000E4288 File Offset: 0x000E2688
	public void RecordRollbackStatus()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.calculatedLatencyMs = this.status.CalculatedLatencyMs;
		record.fullyConfirmedFrame = this.status.FullyConfirmedFrame;
		record.initialTimestepDelta = this.status.InitialTimestepDelta;
		record.inputDelayFrames = this.status.InputDelayFrames;
		record.msSinceStartCompensated = this.status.MsSinceStart;
	}

	// Token: 0x06002BB8 RID: 11192 RVA: 0x000E4314 File Offset: 0x000E2714
	public void RecordRemoteInput(int playerID, int startFrame, int numInputs)
	{
		if (this.currentMatch == null)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		MatchDeepLogging.Match.Record.RemoteInputFrame remoteInputFrame = new MatchDeepLogging.Match.Record.RemoteInputFrame();
		remoteInputFrame.frame = startFrame;
		remoteInputFrame.time = WTime.precisionTimeSinceStartup;
		remoteInputFrame.playerID = playerID;
		remoteInputFrame.numInputs = numInputs;
		record.remoteInputFrame.Add(remoteInputFrame);
	}

	// Token: 0x06002BB9 RID: 11193 RVA: 0x000E437C File Offset: 0x000E277C
	public void RecordRollback(int framesTicked, double rollbackDurationMs)
	{
		if (this.currentMatch == null)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.rollbackFramesTicked = framesTicked;
		record.rollbackDurationMs = rollbackDurationMs;
		if (framesTicked >= 3)
		{
			record.isProblem = true;
			this.currentMatch.needLogResults = true;
		}
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x000E43DC File Offset: 0x000E27DC
	public void RecordBroadcastLocalInputs(int localPlayerID, int playerID, int beginFrame)
	{
		if (this.currentMatch == null)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		MatchDeepLogging.Match.Record record2 = record;
		string broadcastInputs = record2.broadcastInputs;
		record2.broadcastInputs = string.Concat(new object[]
		{
			broadcastInputs,
			"|from ",
			localPlayerID,
			" to ",
			playerID,
			":",
			beginFrame
		});
	}

	// Token: 0x06002BBB RID: 11195 RVA: 0x000E4460 File Offset: 0x000E2860
	public void RecordProcessingEnd()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		if (this.gameController.currentGame.IsPaused)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.processingEndTime = WTime.precisionTimeSinceStartup;
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x000E44B8 File Offset: 0x000E28B8
	public void RecordTickEnd()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		if (this.gameController.currentGame.IsPaused)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.tickEndTime = WTime.precisionTimeSinceStartup;
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x000E4510 File Offset: 0x000E2910
	public void RecordRenderFrame()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		if (this.gameController.currentGame == null || this.gameController.currentGame.IsPaused)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.renderFrameTime = WTime.precisionTimeSinceStartup;
	}

	// Token: 0x06002BBE RID: 11198 RVA: 0x000E457C File Offset: 0x000E297C
	public void RecordPreRender()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		if (this.gameController.currentGame == null || this.gameController.currentGame.IsPaused)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.preRenderTime = WTime.precisionTimeSinceStartup;
	}

	// Token: 0x06002BBF RID: 11199 RVA: 0x000E45E8 File Offset: 0x000E29E8
	public void RecordPostRender()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		if (this.gameController.currentGame == null || this.gameController.currentGame.IsPaused)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.postRenderTime = WTime.precisionTimeSinceStartup;
	}

	// Token: 0x06002BC0 RID: 11200 RVA: 0x000E4654 File Offset: 0x000E2A54
	public void RecordRenderWaitError()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.renderWaitError = true;
		record.isProblem = true;
		this.currentMatch.needLogResults = true;
	}

	// Token: 0x06002BC1 RID: 11201 RVA: 0x000E46A4 File Offset: 0x000E2AA4
	public void RecordSyncWait()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.syncDelay = true;
		this.currentMatch.needLogResults = true;
	}

	// Token: 0x06002BC2 RID: 11202 RVA: 0x000E46EC File Offset: 0x000E2AEC
	public void RecordRenderDelay()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.renderDelay = true;
		record.isProblem = true;
		this.currentMatch.needLogResults = true;
	}

	// Token: 0x06002BC3 RID: 11203 RVA: 0x000E473C File Offset: 0x000E2B3C
	public void RecordFrameWaitError()
	{
		if (this.currentMatch == null)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		record.frameWaitError = true;
		record.isProblem = true;
		this.currentMatch.needLogResults = true;
	}

	// Token: 0x04001F29 RID: 7977
	private MatchDeepLogging.Match currentMatch;

	// Token: 0x020006D0 RID: 1744
	public class Match
	{
		// Token: 0x06002BC4 RID: 11204 RVA: 0x000E478C File Offset: 0x000E2B8C
		public Match()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			long totalMemory = GC.GetTotalMemory(false);
			this.list = new List<MatchDeepLogging.Match.Record>(32400);
			for (int i = 0; i < this.list.Capacity; i++)
			{
				this.list.Add(new MatchDeepLogging.Match.Record());
			}
			float num = Time.realtimeSinceStartup - realtimeSinceStartup;
			float num2 = (float)(GC.GetTotalMemory(false) - totalMemory) / 1024f / 1024f;
			Debug.Log("Logger setup time: " + num + "s");
			Debug.Log("Logger memory reserve: " + num2 + "MB");
		}

		// Token: 0x04001F2A RID: 7978
		public List<MatchDeepLogging.Match.Record> list;

		// Token: 0x04001F2B RID: 7979
		public int currentIndex;

		// Token: 0x04001F2C RID: 7980
		public string matchId;

		// Token: 0x04001F2D RID: 7981
		public string opponentName;

		// Token: 0x04001F2E RID: 7982
		public bool needLogResults;

		// Token: 0x020006D1 RID: 1745
		public class Record
		{
			// Token: 0x04001F2F RID: 7983
			public int frameDelta;

			// Token: 0x04001F30 RID: 7984
			public int currentFrame;

			// Token: 0x04001F31 RID: 7985
			public long totalMemory;

			// Token: 0x04001F32 RID: 7986
			public double tickBeginTime;

			// Token: 0x04001F33 RID: 7987
			public double tickEndTime;

			// Token: 0x04001F34 RID: 7988
			public double processingEndTime;

			// Token: 0x04001F35 RID: 7989
			public double renderFrameTime;

			// Token: 0x04001F36 RID: 7990
			public double updateBeginTime;

			// Token: 0x04001F37 RID: 7991
			public double preRenderTime;

			// Token: 0x04001F38 RID: 7992
			public double postRenderTime;

			// Token: 0x04001F39 RID: 7993
			public List<MatchDeepLogging.Match.Record.RemoteInputFrame> remoteInputFrame = new List<MatchDeepLogging.Match.Record.RemoteInputFrame>(3);

			// Token: 0x04001F3A RID: 7994
			public int rollbackFramesTicked;

			// Token: 0x04001F3B RID: 7995
			public double rollbackDurationMs;

			// Token: 0x04001F3C RID: 7996
			public string broadcastInputs = string.Empty;

			// Token: 0x04001F3D RID: 7997
			public int calculatedLatencyMs;

			// Token: 0x04001F3E RID: 7998
			public int fullyConfirmedFrame;

			// Token: 0x04001F3F RID: 7999
			public int initialTimestepDelta;

			// Token: 0x04001F40 RID: 8000
			public int inputDelayFrames;

			// Token: 0x04001F41 RID: 8001
			public long msSinceStartCompensated;

			// Token: 0x04001F42 RID: 8002
			public bool frameWaitError;

			// Token: 0x04001F43 RID: 8003
			public bool renderWaitError;

			// Token: 0x04001F44 RID: 8004
			public bool renderDelay;

			// Token: 0x04001F45 RID: 8005
			public bool syncDelay;

			// Token: 0x04001F46 RID: 8006
			public bool isProblem;

			// Token: 0x020006D2 RID: 1746
			public class RemoteInputFrame
			{
				// Token: 0x04001F47 RID: 8007
				public int frame;

				// Token: 0x04001F48 RID: 8008
				public double time;

				// Token: 0x04001F49 RID: 8009
				public int playerID;

				// Token: 0x04001F4A RID: 8010
				public int numInputs;
			}
		}
	}
}
