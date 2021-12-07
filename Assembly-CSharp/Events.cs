using System;
using System.Collections.Generic;

// Token: 0x02000AB5 RID: 2741
public class Events : IEvents
{
	// Token: 0x06005065 RID: 20581 RVA: 0x0014F730 File Offset: 0x0014DB30
	public void Subscribe(Type eventType, Events.EventHandler callback)
	{
		if (!this.eventMap.ContainsKey(eventType))
		{
			this.eventMap.Add(eventType, callback);
		}
		else
		{
			Dictionary<Type, Events.EventHandler> dictionary;
			(dictionary = this.eventMap)[eventType] = (Events.EventHandler)Delegate.Combine(dictionary[eventType], callback);
		}
	}

	// Token: 0x06005066 RID: 20582 RVA: 0x0014F784 File Offset: 0x0014DB84
	public void Unsubscribe(Type eventType, Events.EventHandler callback)
	{
		if (!this.eventMap.ContainsKey(eventType))
		{
			return;
		}
		Dictionary<Type, Events.EventHandler> dictionary;
		(dictionary = this.eventMap)[eventType] = (Events.EventHandler)Delegate.Remove(dictionary[eventType], callback);
		if (this.eventMap[eventType] == null)
		{
			this.eventMap.Remove(eventType);
		}
	}

	// Token: 0x06005067 RID: 20583 RVA: 0x0014F7E4 File Offset: 0x0014DBE4
	public void Broadcast(GameEvent message)
	{
		Type type = message.GetType();
		if (!this.eventMap.ContainsKey(type))
		{
			return;
		}
		this.eventMap[type](message);
	}

	// Token: 0x06005068 RID: 20584 RVA: 0x0014F81C File Offset: 0x0014DC1C
	public void Broadcast(Type messageType)
	{
		if (!this.eventMap.ContainsKey(messageType))
		{
			return;
		}
		this.eventMap[messageType](null);
	}

	// Token: 0x06005069 RID: 20585 RVA: 0x0014F844 File Offset: 0x0014DC44
	public string GenerateDebugString(bool verbose = false)
	{
		int num = 0;
		int num2 = 0;
		string text = string.Empty;
		foreach (Type type in this.eventMap.Keys)
		{
			num2++;
			int num3 = this.eventMap[type].GetInvocationList().Length;
			if (verbose)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n",
					type.ToString(),
					": ",
					num3
				});
			}
			num += num3;
		}
		text = string.Concat(new object[]
		{
			"Total Events: ",
			num,
			", Total Types: ",
			num2,
			text
		});
		return text;
	}

	// Token: 0x040033BC RID: 13244
	private Dictionary<Type, Events.EventHandler> eventMap = new Dictionary<Type, Events.EventHandler>();

	// Token: 0x02000AB6 RID: 2742
	// (Invoke) Token: 0x0600506B RID: 20587
	public delegate void EventHandler(GameEvent message);
}
