using System;

// Token: 0x02000AB7 RID: 2743
public interface IEvents
{
	// Token: 0x0600506E RID: 20590
	void Broadcast(Type messageType);

	// Token: 0x0600506F RID: 20591
	void Broadcast(GameEvent message);

	// Token: 0x06005070 RID: 20592
	void Unsubscribe(Type messageType, Events.EventHandler callback);

	// Token: 0x06005071 RID: 20593
	void Subscribe(Type messageType, Events.EventHandler callback);
}
