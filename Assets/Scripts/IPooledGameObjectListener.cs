// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPooledGameObjectListener
{
	void OnAcquired();

	void OnReleased();

	void OnCooledOff();
}
