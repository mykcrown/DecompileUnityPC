// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;

public class SignalBus : ISignalBus
{
	private Dictionary<string, Signal> mapBasic = new Dictionary<string, Signal>();

	private Dictionary<string, object> mapByName = new Dictionary<string, object>();

	private Dictionary<Type, object> mapByType = new Dictionary<Type, object>();

	public void AddListener(string signalName, Action theFunction)
	{
		this.getBasicSignal(signalName).AddListener(theFunction);
	}

	public void AddOnce(string signalName, Action theFunction)
	{
		this.getBasicSignal(signalName).AddOnce(theFunction);
	}

	public void RemoveListener(string signalName, Action theFunction)
	{
		this.getBasicSignal(signalName).RemoveListener(theFunction);
	}

	public void Dispatch(string signalName)
	{
		this.getBasicSignal(signalName).Dispatch();
	}

	private Signal getBasicSignal(string signalName)
	{
		if (!this.mapBasic.ContainsKey(signalName))
		{
			this.mapBasic[signalName] = new Signal();
		}
		return this.mapBasic[signalName];
	}

	public T GetSignal<T>() where T : new()
	{
		Type typeFromHandle = typeof(T);
		if (!this.mapByType.ContainsKey(typeFromHandle))
		{
			this.mapByType[typeFromHandle] = Activator.CreateInstance<T>();
		}
		return (T)((object)this.mapByType[typeFromHandle]);
	}

	public T GetSignal<T>(string name) where T : new()
	{
		if (!this.mapByName.ContainsKey(name))
		{
			this.mapByName[name] = Activator.CreateInstance<T>();
		}
		return (T)((object)this.mapByName[name]);
	}
}
