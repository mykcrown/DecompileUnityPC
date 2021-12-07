using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000391 RID: 913
public class ClientBehavior : MonoBehaviour
{
	// Token: 0x17000393 RID: 915
	// (get) Token: 0x0600138A RID: 5002 RVA: 0x0006F1C2 File Offset: 0x0006D5C2
	// (set) Token: 0x0600138B RID: 5003 RVA: 0x0006F1CA File Offset: 0x0006D5CA
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x0600138C RID: 5004 RVA: 0x0006F1D3 File Offset: 0x0006D5D3
	// (set) Token: 0x0600138D RID: 5005 RVA: 0x0006F1DB File Offset: 0x0006D5DB
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x0600138E RID: 5006 RVA: 0x0006F1E4 File Offset: 0x0006D5E4
	// (set) Token: 0x0600138F RID: 5007 RVA: 0x0006F1EC File Offset: 0x0006D5EC
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06001390 RID: 5008 RVA: 0x0006F1F5 File Offset: 0x0006D5F5
	// (set) Token: 0x06001391 RID: 5009 RVA: 0x0006F1FD File Offset: 0x0006D5FD
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06001392 RID: 5010 RVA: 0x0006F206 File Offset: 0x0006D606
	// (set) Token: 0x06001393 RID: 5011 RVA: 0x0006F20E File Offset: 0x0006D60E
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06001394 RID: 5012 RVA: 0x0006F217 File Offset: 0x0006D617
	// (set) Token: 0x06001395 RID: 5013 RVA: 0x0006F21F File Offset: 0x0006D61F
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06001396 RID: 5014 RVA: 0x0006F228 File Offset: 0x0006D628
	// (set) Token: 0x06001397 RID: 5015 RVA: 0x0006F230 File Offset: 0x0006D630
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06001398 RID: 5016 RVA: 0x0006F239 File Offset: 0x0006D639
	// (set) Token: 0x06001399 RID: 5017 RVA: 0x0006F241 File Offset: 0x0006D641
	[Inject]
	public IUIAdapter uiAdapter { get; set; }

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x0600139A RID: 5018 RVA: 0x0006F24A File Offset: 0x0006D64A
	// (set) Token: 0x0600139B RID: 5019 RVA: 0x0006F252 File Offset: 0x0006D652
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x0600139C RID: 5020 RVA: 0x0006F25B File Offset: 0x0006D65B
	protected bool isConnected
	{
		get
		{
			return this.battleServerAPI != null && this.battleServerAPI.IsConnected;
		}
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x0006F276 File Offset: 0x0006D676
	public virtual void Awake()
	{
		this.checkInject();
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x0006F27E File Offset: 0x0006D67E
	private void checkInject()
	{
		if (this.injector == null)
		{
			if (StaticInject.readyToInject)
			{
				this.inject();
			}
			else
			{
				SystemBoot.AddStartupCallback(new Action(this.inject));
			}
		}
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x0006F2B1 File Offset: 0x0006D6B1
	private void inject()
	{
		StaticInject.Inject(this);
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x0006F2B9 File Offset: 0x0006D6B9
	protected void listen(string signalName, Action theFunction)
	{
		this.signalBus.AddListener(signalName, theFunction);
		this.signalListeners.Add(new SignalListenerRecord(signalName, theFunction));
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x0006F2DA File Offset: 0x0006D6DA
	protected void dispatch(string signalName)
	{
		this.signalBus.Dispatch(signalName);
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x0006F2E8 File Offset: 0x0006D6E8
	protected virtual void removeAllListeners()
	{
		for (int i = this.signalListeners.Count - 1; i >= 0; i--)
		{
			this.signalBus.RemoveListener(this.signalListeners[i].name, this.signalListeners[i].theFunction);
		}
		this.signalListeners.Clear();
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x0006F34B File Offset: 0x0006D74B
	public virtual void OnDestroy()
	{
		this.removeAllListeners();
	}

	// Token: 0x04000CFC RID: 3324
	private List<SignalListenerRecord> signalListeners = new List<SignalListenerRecord>();
}
