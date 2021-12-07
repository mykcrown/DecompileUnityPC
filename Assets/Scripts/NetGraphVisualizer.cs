// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NetGraphVisualizer : ITickable
{
	private NetGraphMode graphMode;

	private List<DebugGraph> debugGraphs = new List<DebugGraph>();

	private Dictionary<string, DebugGraph> namesToGraphs = new Dictionary<string, DebugGraph>();

	private Dictionary<NetGraphMode, HashSet<DebugGraph>> graphsForModes;

	private DebugGraph fpsGraph;

	private DebugGraph latencyGraph;

	private DebugGraph frameDelayGraph;

	private DebugGraph memoryGraph;

	private DebugGraph gcCollectGraph;

	private int trackedFrames = 240;

	private long prevMemAllocated;

	private long memAllocated;

	public const int DEFAULT_GRAPH_WIDTH = 600;

	public const int DEFAULT_GRAPH_HEIGHT = 400;

	[Inject]
	public IDevConsole devConsole
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

	public static float MasterGraphWidth
	{
		get;
		private set;
	}

	public static float MasterGraphHeight
	{
		get;
		private set;
	}

	public void Init()
	{
		GameObject gameObject = new GameObject("NetGraphVisualizer");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		this.devConsole.AddConsoleVariable<NetGraphMode>("net_graph", "mode", "Net Graph Mode", "Net Graph render mode.", new Func<NetGraphMode>(this._Init_m__0), new Action<NetGraphMode>(this._Init_m__1));
		this.devConsole.AddConsoleVariable<float>("net_graph", "width", "Net Graph Width", "Width of the net graph on screen.", new Func<float>(this._Init_m__2), new Action<float>(this.setGraphWidth));
		this.devConsole.AddConsoleVariable<float>("net_graph", "height", "Net Graph Height", "Height of the net graph on screen.", new Func<float>(this._Init_m__3), new Action<float>(this.setGraphHeight));
		this.devConsole.AddConsoleVariable<int>("net_graph", "tracked_frames", "Net Graph Tracked Frames", "Number of frames the graph tracks.", new Func<int>(this._Init_m__4), new Action<int>(this.setGraphTrackedFrames));
		this.graphsForModes = new Dictionary<NetGraphMode, HashSet<DebugGraph>>();
		NetGraphMode[] values = EnumUtil.GetValues<NetGraphMode>();
		for (int i = 0; i < values.Length; i++)
		{
			NetGraphMode key = values[i];
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
			color = WColor.FromBytes(139, 69, 19, 255),
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
		foreach (DebugGraph current in this.debugGraphs)
		{
			current.TickFrame();
		}
		foreach (KeyValuePair<NetGraphMode, HashSet<DebugGraph>> current2 in this.graphsForModes)
		{
			if (BitField.CountBitFlags((uint)current2.Key) == 1 && (this.graphMode & current2.Key) != NetGraphMode.Off)
			{
				foreach (DebugGraph current3 in current2.Value)
				{
					if (!current3.IsShown)
					{
						current3.Show();
					}
					current3.Draw();
				}
			}
		}
	}

	public void ReportHealth(NetworkHealthReport report)
	{
		if (this.graphMode == NetGraphMode.Off)
		{
			return;
		}
		this.latencyGraph.ReportValue((float)report.calculatedLatencyMs);
		this.frameDelayGraph.ReportValue((float)report.messageDelay);
	}

	public void ReportFPS(float fps)
	{
		if (this.graphMode == NetGraphMode.Off)
		{
			return;
		}
		this.fpsGraph.ReportValue(fps);
	}

	public void SetGraphMode(NetGraphMode mode)
	{
		this.graphMode = mode;
		foreach (DebugGraph current in this.debugGraphs)
		{
			current.Hide();
		}
		NetGraphMode[] values = EnumUtil.GetValues<NetGraphMode>();
		for (int i = 0; i < values.Length; i++)
		{
			NetGraphMode netGraphMode = values[i];
			if ((netGraphMode & this.graphMode) != NetGraphMode.Off)
			{
				foreach (DebugGraph current2 in this.graphsForModes[netGraphMode])
				{
					current2.Show();
				}
			}
		}
	}

	public DebugGraph AddGraph(string name, DebugGraphConfig config, NetGraphMode graphMode)
	{
		if (this.namesToGraphs.ContainsKey(name))
		{
			return this.namesToGraphs[name];
		}
		return this.addGraph(new DebugGraph(name, this.trackedFrames, config), graphMode);
	}

	private DebugGraph addGraph(DebugGraph graph, NetGraphMode mode)
	{
		this.debugGraphs.Add(graph);
		this.namesToGraphs[graph.Name] = graph;
		this.graphsForModes[mode].Add(graph);
		return graph;
	}

	private void setGraphWidth(float width)
	{
		NetGraphVisualizer.MasterGraphWidth = width;
		foreach (DebugGraph current in this.debugGraphs)
		{
			current.config.width = width;
		}
	}

	private void setGraphHeight(float height)
	{
		NetGraphVisualizer.MasterGraphHeight = height;
		foreach (DebugGraph current in this.debugGraphs)
		{
			current.config.height = height;
		}
	}

	private void setGraphTrackedFrames(int trackedFrames)
	{
		this.trackedFrames = trackedFrames;
		foreach (DebugGraph current in this.debugGraphs)
		{
			current.SetFrameCount(this.trackedFrames);
		}
	}

	private NetGraphMode _Init_m__0()
	{
		return this.graphMode;
	}

	private void _Init_m__1(NetGraphMode value)
	{
		this.SetGraphMode(value);
	}

	private float _Init_m__2()
	{
		return this.fpsGraph.config.width;
	}

	private float _Init_m__3()
	{
		return this.fpsGraph.config.height;
	}

	private int _Init_m__4()
	{
		return this.trackedFrames;
	}
}
