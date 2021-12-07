// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine.UI;

public class ClientSelectable : Selectable
{
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

	protected override void Awake()
	{
		base.Awake();
		this.inject();
	}

	private void inject()
	{
		if (this.injector == null)
		{
			StaticInject.Inject(this);
		}
	}
}
