using System;
using System.Collections.Generic;

// Token: 0x02000A78 RID: 2680
public abstract class UIScreenController
{
	// Token: 0x1700128E RID: 4750
	// (get) Token: 0x06004E57 RID: 20055 RVA: 0x0010F3D7 File Offset: 0x0010D7D7
	// (set) Token: 0x06004E58 RID: 20056 RVA: 0x0010F3DF File Offset: 0x0010D7DF
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x1700128F RID: 4751
	// (get) Token: 0x06004E59 RID: 20057 RVA: 0x0010F3E8 File Offset: 0x0010D7E8
	// (set) Token: 0x06004E5A RID: 20058 RVA: 0x0010F3F0 File Offset: 0x0010D7F0
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17001290 RID: 4752
	// (get) Token: 0x06004E5B RID: 20059 RVA: 0x0010F3F9 File Offset: 0x0010D7F9
	// (set) Token: 0x06004E5C RID: 20060 RVA: 0x0010F401 File Offset: 0x0010D801
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06004E5D RID: 20061 RVA: 0x0010F40A File Offset: 0x0010D80A
	public void Register(IUIAdapter adapter)
	{
		this.adapter = adapter;
		this.setupListeners();
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x0010F419 File Offset: 0x0010D819
	public void Unregister()
	{
		this.removeListeners();
	}

	// Token: 0x06004E5F RID: 20063 RVA: 0x0010F421 File Offset: 0x0010D821
	public void Initialize()
	{
		this.setup();
	}

	// Token: 0x06004E60 RID: 20064
	protected abstract void setupListeners();

	// Token: 0x06004E61 RID: 20065 RVA: 0x0010F429 File Offset: 0x0010D829
	protected virtual void removeListeners()
	{
	}

	// Token: 0x06004E62 RID: 20066 RVA: 0x0010F42B File Offset: 0x0010D82B
	protected virtual void setup()
	{
	}

	// Token: 0x06004E63 RID: 20067 RVA: 0x0010F42D File Offset: 0x0010D82D
	protected void subscribe(Type type, Action<GameEvent> callback)
	{
		this.requestHandlers.Add(type, callback);
		this.adapter.Subscribe(type);
	}

	// Token: 0x06004E64 RID: 20068 RVA: 0x0010F448 File Offset: 0x0010D848
	protected void sendUpdate(GameEvent message)
	{
		this.adapter.SendUpdate(message);
	}

	// Token: 0x06004E65 RID: 20069 RVA: 0x0010F458 File Offset: 0x0010D858
	public void HandleRequest(GameEvent message)
	{
		Type type = message.GetType();
		if (this.requestHandlers.ContainsKey(type))
		{
			this.requestHandlers[type](message);
		}
	}

	// Token: 0x0400332F RID: 13103
	private Dictionary<Type, Action<GameEvent>> requestHandlers = new Dictionary<Type, Action<GameEvent>>();

	// Token: 0x04003330 RID: 13104
	public Payload payload;

	// Token: 0x04003331 RID: 13105
	protected IUIAdapter adapter;
}
