// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPoolListener
{
	void OnAcquired();

	void OnReleased();

	void OnCooledOff();
}
