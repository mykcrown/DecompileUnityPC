// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ISignalBus
{
	void AddListener(string signalName, Action theFunction);

	void AddOnce(string signalName, Action theFunction);

	void RemoveListener(string signalName, Action theFunction);

	T GetSignal<T>(string name) where T : new();

	T GetSignal<T>() where T : new();

	void Dispatch(string signalName);
}
