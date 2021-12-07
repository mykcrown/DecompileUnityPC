// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using network;
using System;
using TMPro;
using UnityEngine;

public class UserFacingPerformanceDisplay : BaseGamewideOverlay
{
	private string BLANK = "0";

	public TextMeshProUGUI FPSText;

	public TextMeshProUGUI GameTickFPSText;

	public TextMeshProUGUI PingText;

	private FPSCounter displayFPS;

	private FPSCounter gameTickFPS;

	private float latency;

	private StaticString displayString = new StaticString(8);

	private bool reportedLatencyThisFrame;

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel
	{
		get;
		set;
	}

	[Inject]
	public IIconsServerAPI serverAPI
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyController customLobby
	{
		get;
		set;
	}

	[Inject]
	public P2PServerMgr p2pServerMgr
	{
		get;
		set;
	}

	[Inject]
	public IPingManager pingManager
	{
		private get;
		set;
	}

	public FPSCounter DisplayFPS
	{
		set
		{
			this.displayFPS = value;
			FPSCounter expr_0D = this.displayFPS;
			expr_0D.OnRecord = (Action)Delegate.Combine(expr_0D.OnRecord, new Action(this.onFpsUpdate));
		}
	}

	public FPSCounter GameTickFPS
	{
		set
		{
			this.gameTickFPS = value;
			FPSCounter expr_0D = this.gameTickFPS;
			expr_0D.OnRecord = (Action)Delegate.Combine(expr_0D.OnRecord, new Action(this.onFpsUpdate));
		}
	}

	[PostConstruct]
	public void Init()
	{
		base.signalBus.AddListener(UserVideoSettingsModel.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	private void onUpdate()
	{
		base.gameObject.SetActive(this.userVideoSettingsModel.ShowPerformance);
	}

	private int boundFPS(float fps)
	{
		return this.boundFPS((int)fps);
	}

	private int boundFPS(ulong fps)
	{
		return this.boundFPS((int)fps);
	}

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

	private Color createColor(float percent, Color color)
	{
		color.r = 1f;
		color.g = percent;
		color.b = percent;
		return color;
	}

	public void ReportNetHealth(NetworkHealthReport report)
	{
		this.latency = (float)report.calculatedLatencyMs;
		if (this.latency != 0f)
		{
			this.reportedLatencyThisFrame = true;
		}
	}
}
