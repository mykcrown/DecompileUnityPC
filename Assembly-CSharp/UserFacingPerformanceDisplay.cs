using System;
using IconsServer;
using network;
using TMPro;
using UnityEngine;

// Token: 0x02000A80 RID: 2688
public class UserFacingPerformanceDisplay : BaseGamewideOverlay
{
	// Token: 0x17001294 RID: 4756
	// (get) Token: 0x06004E7D RID: 20093 RVA: 0x00149A93 File Offset: 0x00147E93
	// (set) Token: 0x06004E7E RID: 20094 RVA: 0x00149A9B File Offset: 0x00147E9B
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17001295 RID: 4757
	// (get) Token: 0x06004E7F RID: 20095 RVA: 0x00149AA4 File Offset: 0x00147EA4
	// (set) Token: 0x06004E80 RID: 20096 RVA: 0x00149AAC File Offset: 0x00147EAC
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x17001296 RID: 4758
	// (get) Token: 0x06004E81 RID: 20097 RVA: 0x00149AB5 File Offset: 0x00147EB5
	// (set) Token: 0x06004E82 RID: 20098 RVA: 0x00149ABD File Offset: 0x00147EBD
	[Inject]
	public IIconsServerAPI serverAPI { get; set; }

	// Token: 0x17001297 RID: 4759
	// (get) Token: 0x06004E83 RID: 20099 RVA: 0x00149AC6 File Offset: 0x00147EC6
	// (set) Token: 0x06004E84 RID: 20100 RVA: 0x00149ACE File Offset: 0x00147ECE
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x17001298 RID: 4760
	// (get) Token: 0x06004E85 RID: 20101 RVA: 0x00149AD7 File Offset: 0x00147ED7
	// (set) Token: 0x06004E86 RID: 20102 RVA: 0x00149ADF File Offset: 0x00147EDF
	[Inject]
	public P2PServerMgr p2pServerMgr { get; set; }

	// Token: 0x17001299 RID: 4761
	// (get) Token: 0x06004E87 RID: 20103 RVA: 0x00149AE8 File Offset: 0x00147EE8
	// (set) Token: 0x06004E88 RID: 20104 RVA: 0x00149AF0 File Offset: 0x00147EF0
	[Inject]
	public IPingManager pingManager { private get; set; }

	// Token: 0x06004E89 RID: 20105 RVA: 0x00149AF9 File Offset: 0x00147EF9
	[PostConstruct]
	public void Init()
	{
		base.signalBus.AddListener(UserVideoSettingsModel.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	// Token: 0x06004E8A RID: 20106 RVA: 0x00149B1D File Offset: 0x00147F1D
	private void onUpdate()
	{
		base.gameObject.SetActive(this.userVideoSettingsModel.ShowPerformance);
	}

	// Token: 0x1700129A RID: 4762
	// (set) Token: 0x06004E8B RID: 20107 RVA: 0x00149B35 File Offset: 0x00147F35
	public FPSCounter DisplayFPS
	{
		set
		{
			this.displayFPS = value;
			FPSCounter fpscounter = this.displayFPS;
			fpscounter.OnRecord = (Action)Delegate.Combine(fpscounter.OnRecord, new Action(this.onFpsUpdate));
		}
	}

	// Token: 0x1700129B RID: 4763
	// (set) Token: 0x06004E8C RID: 20108 RVA: 0x00149B65 File Offset: 0x00147F65
	public FPSCounter GameTickFPS
	{
		set
		{
			this.gameTickFPS = value;
			FPSCounter fpscounter = this.gameTickFPS;
			fpscounter.OnRecord = (Action)Delegate.Combine(fpscounter.OnRecord, new Action(this.onFpsUpdate));
		}
	}

	// Token: 0x06004E8D RID: 20109 RVA: 0x00149B95 File Offset: 0x00147F95
	private int boundFPS(float fps)
	{
		return this.boundFPS((int)fps);
	}

	// Token: 0x06004E8E RID: 20110 RVA: 0x00149B9F File Offset: 0x00147F9F
	private int boundFPS(ulong fps)
	{
		return this.boundFPS((int)fps);
	}

	// Token: 0x06004E8F RID: 20111 RVA: 0x00149BAC File Offset: 0x00147FAC
	private int boundFPS(int fps)
	{
		int num = (int)Mathf.Round((float)fps);
		if (num > 999)
		{
			num = 999;
		}
		else if (num < -999)
		{
			num = -999;
		}
		return num;
	}

	// Token: 0x06004E90 RID: 20112 RVA: 0x00149BEC File Offset: 0x00147FEC
	private void updatePingDisplay()
	{
		Color color = this.PingText.color;
		float a = color.a;
		if (this.customLobby.IsInLobby)
		{
			if (!this.reportedLatencyThisFrame)
			{
				this.latency = (float)this.pingManager.LatencyMs;
			}
			color = Color.white;
			if (this.latency >= 115f)
			{
				color = Color.yellow;
			}
			else if (this.latency >= 200f)
			{
				color = Color.red;
			}
			this.displayString.Reset();
			this.displayString.AppendInt0to999(this.boundFPS(this.latency));
			this.PingText.SetCharArray(this.displayString.arr, 0, this.displayString.len);
		}
		else if (this.PingText != null)
		{
			this.PingText.text = this.BLANK;
			color = Color.white;
		}
		color.a = a;
		this.PingText.color = color;
	}

	// Token: 0x06004E91 RID: 20113 RVA: 0x00149CF8 File Offset: 0x001480F8
	private void onFpsUpdate()
	{
		this.displayString.Reset();
		this.displayString.AppendInt0to999(this.boundFPS(this.displayFPS.FPS));
		this.FPSText.SetCharArray(this.displayString.arr, 0, this.displayString.len);
		this.displayString.Reset();
		this.displayString.AppendInt0to999(this.boundFPS(this.gameTickFPS.FPS));
		this.GameTickFPSText.SetCharArray(this.displayString.arr, 0, this.displayString.len);
		float percent = Mathf.Max(0f, (this.displayFPS.FPS - 20f) / 40f);
		this.GameTickFPSText.color = this.createColor(percent, this.GameTickFPSText.color);
		this.updatePingDisplay();
		this.reportedLatencyThisFrame = false;
	}

	// Token: 0x06004E92 RID: 20114 RVA: 0x00149DE3 File Offset: 0x001481E3
	private Color createColor(float percent, Color color)
	{
		color.r = 1f;
		color.g = percent;
		color.b = percent;
		return color;
	}

	// Token: 0x06004E93 RID: 20115 RVA: 0x00149E02 File Offset: 0x00148202
	public void ReportNetHealth(NetworkHealthReport report)
	{
		this.latency = (float)report.calculatedLatencyMs;
		if (this.latency != 0f)
		{
			this.reportedLatencyThisFrame = true;
		}
	}

	// Token: 0x04003336 RID: 13110
	private string BLANK = "0";

	// Token: 0x04003337 RID: 13111
	public TextMeshProUGUI FPSText;

	// Token: 0x04003338 RID: 13112
	public TextMeshProUGUI GameTickFPSText;

	// Token: 0x04003339 RID: 13113
	public TextMeshProUGUI PingText;

	// Token: 0x0400333A RID: 13114
	private FPSCounter displayFPS;

	// Token: 0x0400333B RID: 13115
	private FPSCounter gameTickFPS;

	// Token: 0x0400333C RID: 13116
	private float latency;

	// Token: 0x0400333D RID: 13117
	private StaticString displayString = new StaticString(8);

	// Token: 0x04003344 RID: 13124
	private bool reportedLatencyThisFrame;
}
