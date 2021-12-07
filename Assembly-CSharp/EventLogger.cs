using System;
using System.Collections.Generic;

// Token: 0x02000B17 RID: 2839
public class EventLogger
{
	// Token: 0x1700130A RID: 4874
	// (get) Token: 0x0600516B RID: 20843 RVA: 0x001523F8 File Offset: 0x001507F8
	// (set) Token: 0x0600516C RID: 20844 RVA: 0x00152400 File Offset: 0x00150800
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x1700130B RID: 4875
	// (get) Token: 0x0600516D RID: 20845 RVA: 0x00152409 File Offset: 0x00150809
	// (set) Token: 0x0600516E RID: 20846 RVA: 0x00152411 File Offset: 0x00150811
	[Inject]
	public ILogger logger { get; set; }

	// Token: 0x0600516F RID: 20847 RVA: 0x0015241C File Offset: 0x0015081C
	public void Init()
	{
		foreach (KeyValuePair<Type, LogLevel> keyValuePair in EventLogger.eventLogLevels)
		{
			this.events.Subscribe(keyValuePair.Key, new Events.EventHandler(this.onGameEvent));
		}
	}

	// Token: 0x06005170 RID: 20848 RVA: 0x00152490 File Offset: 0x00150890
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

	// Token: 0x0400346D RID: 13421
	private static Dictionary<Type, LogLevel> eventLogLevels = new Dictionary<Type, LogLevel>
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
