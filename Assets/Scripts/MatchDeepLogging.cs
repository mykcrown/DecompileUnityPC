// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchDeepLogging : IMatchDeepLogging
{
	public class Match
	{
		public class Record
		{
			public class RemoteInputFrame
			{
				public int frame;

				public double time;

				public int playerID;

				public int numInputs;
			}

			public int frameDelta;

			public int currentFrame;

			public long totalMemory;

			public double tickBeginTime;

			public double tickEndTime;

			public double processingEndTime;

			public double renderFrameTime;

			public double updateBeginTime;

			public double preRenderTime;

			public double postRenderTime;

			public List<MatchDeepLogging.Match.Record.RemoteInputFrame> remoteInputFrame = new List<MatchDeepLogging.Match.Record.RemoteInputFrame>(3);

			public int rollbackFramesTicked;

			public double rollbackDurationMs;

			public string broadcastInputs = string.Empty;

			public int calculatedLatencyMs;

			public int fullyConfirmedFrame;

			public int initialTimestepDelta;

			public int inputDelayFrames;

			public long msSinceStartCompensated;

			public bool frameWaitError;

			public bool renderWaitError;

			public bool renderDelay;

			public bool syncDelay;

			public bool isProblem;
		}

		public List<MatchDeepLogging.Match.Record> list;

		public int currentIndex;

		public string matchId;

		public string opponentName;

		public bool needLogResults;

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
			UnityEngine.Debug.Log("Logger setup time: " + num + "s");
			UnityEngine.Debug.Log("Logger memory reserve: " + num2 + "MB");
		}
	}

	private MatchDeepLogging.Match currentMatch;

	[Inject]
	public ISaveFileData saveFileData
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

	[Inject]
	public IServerConnectionManager serverConnectionManager
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatus status
	{
		get;
		set;
	}

	public void BeginMatch(string matchId, string opponentName)
	{
		this.currentMatch = new MatchDeepLogging.Match();
		this.currentMatch.matchId = matchId;
		this.currentMatch.opponentName = opponentName;
		this.currentMatch.needLogResults = true;
	}

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
			foreach (MatchDeepLogging.Match.Record.RemoteInputFrame current in record.remoteInputFrame)
			{
				text = text + " player: " + current.playerID;
				text = text + " frame: " + current.frame;
				text = text + " time: " + current.time;
				text = text + " numInputs: " + current.numInputs;
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
		UnityEngine.Debug.Log("Save log with file name " + str);
		float num = Time.realtimeSinceStartup - realtimeSinceStartup;
		UnityEngine.Debug.Log("Deep log save time: " + num);
	}

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

	public void UpdateLoopEnd()
	{
	}

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

	public void RecordBroadcastLocalInputs(int localPlayerID, int playerID, int beginFrame)
	{
		if (this.currentMatch == null)
		{
			return;
		}
		MatchDeepLogging.Match.Record record = this.currentMatch.list[this.currentMatch.currentIndex];
		MatchDeepLogging.Match.Record expr_29 = record;
		string broadcastInputs = expr_29.broadcastInputs;
		expr_29.broadcastInputs = string.Concat(new object[]
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
}
