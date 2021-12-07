// Decompile from assembly: Assembly-CSharp.dll

using System;

public class InputBlocker : IInputBlocker
{
	private int _lock;

	private InputLockUpdateSignal signal;

	[Inject]
	public UIManager uiManager
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signal = this.signalBus.GetSignal<InputLockUpdateSignal>();
	}

	public bool IsLocked()
	{
		return this._lock > 0;
	}

	public InputBlock Request()
	{
		this._lock++;
		if (this._lock == 1)
		{
			this.doLock();
		}
		return new InputBlock();
	}

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

	private void doLock()
	{
		this.signal.Dispatch();
	}

	private void undoLock()
	{
		this.signal.Dispatch();
	}
}
