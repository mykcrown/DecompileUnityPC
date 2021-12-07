// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientBehavior : MonoBehaviour
{
	private List<SignalListenerRecord> signalListeners = new List<SignalListenerRecord>();

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
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
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
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
	public IUIAdapter uiAdapter
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	protected bool isConnected
	{
		get
		{
			return this.battleServerAPI != null && this.battleServerAPI.IsConnected;
		}
	}

	public virtual void Awake()
	{
		this.checkInject();
	}

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

	private void inject()
	{
		StaticInject.Inject(this);
	}

	protected void listen(string signalName, Action theFunction)
	{
		this.signalBus.AddListener(signalName, theFunction);
		this.signalListeners.Add(new SignalListenerRecord(signalName, theFunction));
	}

	protected void dispatch(string signalName)
	{
		this.signalBus.Dispatch(signalName);
	}

	protected virtual void removeAllListeners()
	{
		for (int i = this.signalListeners.Count - 1; i >= 0; i--)
		{
			this.signalBus.RemoveListener(this.signalListeners[i].name, this.signalListeners[i].theFunction);
		}
		this.signalListeners.Clear();
	}

	public virtual void OnDestroy()
	{
		this.removeAllListeners();
	}
}
