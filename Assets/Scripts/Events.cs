// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class Events : IEvents
{
	public delegate void EventHandler(GameEvent message);

	private Dictionary<Type, Events.EventHandler> eventMap = new Dictionary<Type, Events.EventHandler>();

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

	public void Broadcast(GameEvent message)
	{
		Type type = message.GetType();
		if (!this.eventMap.ContainsKey(type))
		{
			return;
		}
		this.eventMap[type](message);
	}

	public void Broadcast(Type messageType)
	{
		if (!this.eventMap.ContainsKey(messageType))
		{
			return;
		}
		this.eventMap[messageType](null);
	}

	public string GenerateDebugString(bool verbose = false)
	{
		int num = 0;
		int num2 = 0;
		string text = string.Empty;
		foreach (Type current in this.eventMap.Keys)
		{
			num2++;
			int num3 = this.eventMap[current].GetInvocationList().Length;
			if (verbose)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n",
					current.ToString(),
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
}
