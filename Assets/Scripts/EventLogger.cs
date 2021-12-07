// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class EventLogger
{
	private static Dictionary<Type, LogLevel> eventLogLevels;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public ILogger logger
	{
		get;
		set;
	}

	static EventLogger()
	{
		EventLogger.eventLogLevels = new Dictionary<Type, LogLevel>
		{
			{
				typeof(GameStartEvent),
				LogLevel.InfoBasic
			},
			{
				typeof(GameEndEvent),
				LogLevel.InfoBasic
			},
			{
				typeof(CharacterDeathEvent),
				LogLevel.InfoBasic
			}
		};
	}

	public void Init()
	{
		foreach (KeyValuePair<Type, LogLevel> current in EventLogger.eventLogLevels)
		{
			this.events.Subscribe(current.Key, new Events.EventHandler(this.onGameEvent));
		}
	}

	private void onGameEvent(GameEvent message)
	{
		LogLevel logLevel = LogLevel.None;
		Type type = message.GetType();
		if (EventLogger.eventLogLevels.ContainsKey(type))
		{
			logLevel = EventLogger.eventLogLevels[type];
		}
		this.logger.LogFormat(logLevel, string.Format("Game Event: {0}", type.ToString()), Array.Empty<object>());
	}
}
