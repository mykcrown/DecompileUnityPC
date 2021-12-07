using System;

// Token: 0x02000941 RID: 2369
public class InputBlocker : IInputBlocker
{
	// Token: 0x17000EDD RID: 3805
	// (get) Token: 0x06003E96 RID: 16022 RVA: 0x0011D22A File Offset: 0x0011B62A
	// (set) Token: 0x06003E97 RID: 16023 RVA: 0x0011D232 File Offset: 0x0011B632
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x17000EDE RID: 3806
	// (get) Token: 0x06003E98 RID: 16024 RVA: 0x0011D23B File Offset: 0x0011B63B
	// (set) Token: 0x06003E99 RID: 16025 RVA: 0x0011D243 File Offset: 0x0011B643
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06003E9A RID: 16026 RVA: 0x0011D24C File Offset: 0x0011B64C
	[PostConstruct]
	public void Init()
	{
		this.signal = this.signalBus.GetSignal<InputLockUpdateSignal>();
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x0011D25F File Offset: 0x0011B65F
	public bool IsLocked()
	{
		return this._lock > 0;
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0011D26A File Offset: 0x0011B66A
	public InputBlock Request()
	{
		this._lock++;
		if (this._lock == 1)
		{
			this.doLock();
		}
		return new InputBlock();
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x0011D291 File Offset: 0x0011B691
	public void Release(InputBlock block)
	{
		if (block.alreadyUsed)
		{
			return;
		}
		block.alreadyUsed = true;
		this._lock--;
		if (this._lock == 0)
		{
			this.undoLock();
		}
	}

	// Token: 0x06003E9E RID: 16030 RVA: 0x0011D2C5 File Offset: 0x0011B6C5
	private void doLock()
	{
		this.signal.Dispatch();
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x0011D2D2 File Offset: 0x0011B6D2
	private void undoLock()
	{
		this.signal.Dispatch();
	}

	// Token: 0x04002A78 RID: 10872
	private int _lock;

	// Token: 0x04002A79 RID: 10873
	private InputLockUpdateSignal signal;
}
