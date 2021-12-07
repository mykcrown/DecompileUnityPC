using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B7B RID: 2939
public class NetGraphVisualizer : ITickable
{
	// Token: 0x1700138C RID: 5004
	// (get) Token: 0x060054D3 RID: 21715 RVA: 0x001B358C File Offset: 0x001B198C
	// (set) Token: 0x060054D4 RID: 21716 RVA: 0x001B3594 File Offset: 0x001B1994
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x1700138D RID: 5005
	// (get) Token: 0x060054D5 RID: 21717 RVA: 0x001B359D File Offset: 0x001B199D
	// (set) Token: 0x060054D6 RID: 21718 RVA: 0x001B35A5 File Offset: 0x001B19A5
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700138E RID: 5006
	// (get) Token: 0x060054D7 RID: 21719 RVA: 0x001B35AE File Offset: 0x001B19AE
	// (set) Token: 0x060054D8 RID: 21720 RVA: 0x001B35B5 File Offset: 0x001B19B5
	public static float MasterGraphWidth { get; private set; }

	// Token: 0x1700138F RID: 5007
	// (get) Token: 0x060054D9 RID: 21721 RVA: 0x001B35BD File Offset: 0x001B19BD
	// (set) Token: 0x060054DA RID: 21722 RVA: 0x001B35C4 File Offset: 0x001B19C4
	public static float MasterGraphHeight { get; private set; }

	// Token: 0x060054DB RID: 21723 RVA: 0x001B35CC File Offset: 0x001B19CC
	public void Init()
	{
		GameObject gameObject = new GameObject("NetGraphVisualizer");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		this.devConsole.AddConsoleVariable<NetGraphMode>("net_graph", "mode", "Net Graph Mode", "Net Graph render mode.", () => this.graphMode, delegate(NetGraphMode value)
		{
			this.SetGraphMode(value);
		});
		this.devConsole.AddConsoleVariable<float>("net_graph", "width", "Net Graph Width", "Width of the net graph on screen.", () => this.fpsGraph.config.width, new Action<float>(this.setGraphWidth));
		this.devConsole.AddConsoleVariable<float>("net_graph", "height", "Net Graph Height", "Height of the net graph on screen.", () => this.fpsGraph.config.height, new Action<float>(this.setGraphHeight));
		this.devConsole.AddConsoleVariable<int>("net_graph", "tracked_frames", "Net Graph Tracked Frames", "Number of frames the graph tracks.", () => this.trackedFrames, new Action<int>(this.setGraphTrackedFrames));
		this.graphsForModes = new Dictionary<NetGraphMode, HashSet<DebugGraph>>();
		foreach (NetGraphMode key in EnumUtil.GetValues<NetGraphMode>())
		{
			this.graphsForModes.Add(key, new HashSet<DebugGraph>());
		}
		NetGraphVisualizer.MasterGraphWidth = 600f;
		NetGraphVisualizer.MasterGraphHeight = 400f;
		this.fpsGraph = this.addGraph(new DebugGraph("FPS", this.trackedFrames, new DebugGraphConfig
		{
			screenOffset = Vector2.zero,
			width = 600f,
			height = 400f,
			color = Color.green,
			requiredViewSize = 30f,
			viewStyle = DebugGraphViewStyle.DynamicRecent,
			graphStyle = DebugGraphStyle.Normal,
			parent = gameObject.transform
		}), NetGraphMode.CPU);
		this.latencyGraph = this.addGraph(new DebugGraph("Latency", this.trackedFrames, new DebugGraphConfig
		{
			screenOffset = Vector2.zero,
			width = 600f,
			height = 400f,
			color = new Color(1f, 0.5f, 0f),
			requiredViewSize = 30f,
			viewStyle = DebugGraphViewStyle.Fixed,
			graphStyle = DebugGraphStyle.Normal,
			minValue = 0f,
			maxValue = 200f,
			parent = gameObject.transform
		}), NetGraphMode.Network);
		this.frameDelayGraph = this.addGraph(new DebugGraph("Frame Delay", this.trackedFrames, new DebugGraphConfig
		{
			screenOffset = Vector2.zero,
			width = 600f,
			height = 400f,
			color = Color.blue,
			viewStyle = DebugGraphViewStyle.Fixed,
			graphStyle = DebugGraphStyle.Normal,
			minValue = (float)(-(float)RollbackStatePoolContainer.ROLLBACK_FRAMES),
			maxValue = (float)RollbackStatePoolContainer.ROLLBACK_FRAMES,
			parent = gameObject.transform
		}), NetGraphMode.Network);
		this.memoryGraph = this.addGraph(new DebugGraph("Memory", this.trackedFrames, new DebugGraphConfig
		{
			screenOffset = Vector2.zero,
			width = 600f,
			height = 400f,
			color = WColor.FromBytes(139, 69, 19, byte.MaxValue),
			viewStyle = DebugGraphViewStyle.DynamicPersistent,
			graphStyle = DebugGraphStyle.Value,
			parent = gameObject.transform
		}), NetGraphMode.Memory);
		this.gcCollectGraph = this.addGraph(new DebugGraph("Collections", this.trackedFrames, new DebugGraphConfig
		{
			screenOffset = Vector2.zero,
			width = 600f,
			height = 400f,
			color = Color.red,
			viewStyle = DebugGraphViewStyle.Fixed,
			minValue = 0.1f,
			maxValue = 1f,
			graphStyle = DebugGraphStyle.Value,
			parent = gameObject.transform
		}), NetGraphMode.Memory);
	}

	// Token: 0x060054DC RID: 21724 RVA: 0x001B39D8 File Offset: 0x001B1DD8
	public void TickFrame()
	{
		if (this.graphMode == NetGraphMode.Off)
		{
			return;
		}
		this.prevMemAllocated = this.memAllocated;
		this.memAllocated = GC.GetTotalMemory(false);
		this.memoryGraph.ReportValue((float)this.memAllocated);
		this.gcCollectGraph.ReportValue((this.prevMemAllocated <= this.memAllocated) ? 0f : 1f);
		foreach (DebugGraph debugGraph in this.debugGraphs)
		{
			debugGraph.TickFrame();
		}
		foreach (KeyValuePair<NetGraphMode, HashSet<DebugGraph>> keyValuePair in this.graphsForModes)
		{
			if (BitField.CountBitFlags((uint)keyValuePair.Key) == 1 && (this.graphMode & keyValuePair.Key) != NetGraphMode.Off)
			{
				foreach (DebugGraph debugGraph2 in keyValuePair.Value)
				{
					if (!debugGraph2.IsShown)
					{
						debugGraph2.Show();
					}
					debugGraph2.Draw();
				}
			}
		}
	}

	// Token: 0x060054DD RID: 21725 RVA: 0x001B3B60 File Offset: 0x001B1F60
	public void ReportHealth(NetworkHealthReport report)
	{
		if (this.graphMode == NetGraphMode.Off)
		{
			return;
		}
		this.latencyGraph.ReportValue((float)report.calculatedLatencyMs);
		this.frameDelayGraph.ReportValue((float)report.messageDelay);
	}

	// Token: 0x060054DE RID: 21726 RVA: 0x001B3B92 File Offset: 0x001B1F92
	public void ReportFPS(float fps)
	{
		if (this.graphMode == NetGraphMode.Off)
		{
			return;
		}
		this.fpsGraph.ReportValue(fps);
	}

	// Token: 0x060054DF RID: 21727 RVA: 0x001B3BAC File Offset: 0x001B1FAC
	public void SetGraphMode(NetGraphMode mode)
	{
		this.graphMode = mode;
		foreach (DebugGraph debugGraph in this.debugGraphs)
		{
			debugGraph.Hide();
		}
		foreach (NetGraphMode netGraphMode in EnumUtil.GetValues<NetGraphMode>())
		{
			if ((netGraphMode & this.graphMode) != NetGraphMode.Off)
			{
				foreach (DebugGraph debugGraph2 in this.graphsForModes[netGraphMode])
				{
					debugGraph2.Show();
				}
			}
		}
	}

	// Token: 0x060054E0 RID: 21728 RVA: 0x001B3C94 File Offset: 0x001B2094
	public DebugGraph AddGraph(string name, DebugGraphConfig config, NetGraphMode graphMode)
	{
		if (this.namesToGraphs.ContainsKey(name))
		{
			return this.namesToGraphs[name];
		}
		return this.addGraph(new DebugGraph(name, this.trackedFrames, config), graphMode);
	}

	// Token: 0x060054E1 RID: 21729 RVA: 0x001B3CD5 File Offset: 0x001B20D5
	private DebugGraph addGraph(DebugGraph graph, NetGraphMode mode)
	{
		this.debugGraphs.Add(graph);
		this.namesToGraphs[graph.Name] = graph;
		this.graphsForModes[mode].Add(graph);
		return graph;
	}

	// Token: 0x060054E2 RID: 21730 RVA: 0x001B3D0C File Offset: 0x001B210C
	private void setGraphWidth(float width)
	{
		NetGraphVisualizer.MasterGraphWidth = width;
		foreach (DebugGraph debugGraph in this.debugGraphs)
		{
			debugGraph.config.width = width;
		}
	}

	// Token: 0x060054E3 RID: 21731 RVA: 0x001B3D74 File Offset: 0x001B2174
	private void setGraphHeight(float height)
	{
		NetGraphVisualizer.MasterGraphHeight = height;
		foreach (DebugGraph debugGraph in this.debugGraphs)
		{
			debugGraph.config.height = height;
		}
	}

	// Token: 0x060054E4 RID: 21732 RVA: 0x001B3DDC File Offset: 0x001B21DC
	private void setGraphTrackedFrames(int trackedFrames)
	{
		this.trackedFrames = trackedFrames;
		foreach (DebugGraph debugGraph in this.debugGraphs)
		{
			debugGraph.SetFrameCount(this.trackedFrames);
		}
	}

	// Token: 0x040035C5 RID: 13765
	private NetGraphMode graphMode;

	// Token: 0x040035C6 RID: 13766
	private List<DebugGraph> debugGraphs = new List<DebugGraph>();

	// Token: 0x040035C7 RID: 13767
	private Dictionary<string, DebugGraph> namesToGraphs = new Dictionary<string, DebugGraph>();

	// Token: 0x040035C8 RID: 13768
	private Dictionary<NetGraphMode, HashSet<DebugGraph>> graphsForModes;

	// Token: 0x040035C9 RID: 13769
	private DebugGraph fpsGraph;

	// Token: 0x040035CA RID: 13770
	private DebugGraph latencyGraph;

	// Token: 0x040035CB RID: 13771
	private DebugGraph frameDelayGraph;

	// Token: 0x040035CC RID: 13772
	private DebugGraph memoryGraph;

	// Token: 0x040035CD RID: 13773
	private DebugGraph gcCollectGraph;

	// Token: 0x040035CE RID: 13774
	private int trackedFrames = 240;

	// Token: 0x040035CF RID: 13775
	private long prevMemAllocated;

	// Token: 0x040035D0 RID: 13776
	private long memAllocated;

	// Token: 0x040035D1 RID: 13777
	public const int DEFAULT_GRAPH_WIDTH = 600;

	// Token: 0x040035D2 RID: 13778
	public const int DEFAULT_GRAPH_HEIGHT = 400;
}
