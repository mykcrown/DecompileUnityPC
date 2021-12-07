// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public abstract class UIScreenController
{
	private Dictionary<Type, Action<GameEvent>> requestHandlers = new Dictionary<Type, Action<GameEvent>>();

	public Payload payload;

	protected IUIAdapter adapter;

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
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

	public void Register(IUIAdapter adapter)
	{
		this.adapter = adapter;
		this.setupListeners();
	}

	public void Unregister()
	{
		this.removeListeners();
	}

	public void Initialize()
	{
		this.setup();
	}

	protected abstract void setupListeners();

	protected virtual void removeListeners()
	{
	}

	protected virtual void setup()
	{
	}

	protected void subscribe(Type type, Action<GameEvent> callback)
	{
		this.requestHandlers.Add(type, callback);
		this.adapter.Subscribe(type);
	}

	protected void sendUpdate(GameEvent message)
	{
		this.adapter.SendUpdate(message);
	}

	public void HandleRequest(GameEvent message)
	{
		Type type = message.GetType();
		if (this.requestHandlers.ContainsKey(type))
		{
			this.requestHandlers[type](message);
		}
	}
}
