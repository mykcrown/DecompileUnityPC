using System;
using System.Collections.Generic;
using strange.extensions.signal.impl;

// Token: 0x0200020F RID: 527
public class SignalBus : ISignalBus
{
	// Token: 0x060009FB RID: 2555 RVA: 0x00051327 File Offset: 0x0004F727
	public void AddListener(string signalName, Action theFunction)
	{
		this.getBasicSignal(signalName).AddListener(theFunction);
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00051336 File Offset: 0x0004F736
	public void AddOnce(string signalName, Action theFunction)
	{
		this.getBasicSignal(signalName).AddOnce(theFunction);
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x00051345 File Offset: 0x0004F745
	public void RemoveListener(string signalName, Action theFunction)
	{
		this.getBasicSignal(signalName).RemoveListener(theFunction);
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x00051354 File Offset: 0x0004F754
	public void Dispatch(string signalName)
	{
		this.getBasicSignal(signalName).Dispatch();
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00051362 File Offset: 0x0004F762
	private Signal getBasicSignal(string signalName)
	{
		if (!this.mapBasic.ContainsKey(signalName))
		{
			this.mapBasic[signalName] = new Signal();
		}
		return this.mapBasic[signalName];
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x00051394 File Offset: 0x0004F794
	public T GetSignal<T>() where T : new()
	{
		Type typeFromHandle = typeof(T);
		if (!this.mapByType.ContainsKey(typeFromHandle))
		{
			this.mapByType[typeFromHandle] = Activator.CreateInstance<T>();
		}
		return (T)((object)this.mapByType[typeFromHandle]);
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x000513E4 File Offset: 0x0004F7E4
	public T GetSignal<T>(string name) where T : new()
	{
		if (!this.mapByName.ContainsKey(name))
		{
			this.mapByName[name] = Activator.CreateInstance<T>();
		}
		return (T)((object)this.mapByName[name]);
	}

	// Token: 0x040006F7 RID: 1783
	private Dictionary<string, Signal> mapBasic = new Dictionary<string, Signal>();

	// Token: 0x040006F8 RID: 1784
	private Dictionary<string, object> mapByName = new Dictionary<string, object>();

	// Token: 0x040006F9 RID: 1785
	private Dictionary<Type, object> mapByType = new Dictionary<Type, object>();
}
