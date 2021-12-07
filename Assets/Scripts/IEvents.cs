// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IEvents
{
	void Broadcast(Type messageType);

	void Broadcast(GameEvent message);

	void Unsubscribe(Type messageType, Events.EventHandler callback);

	void Subscribe(Type messageType, Events.EventHandler callback);
}
