// Decompile from assembly: Assembly-CSharp.dll

using System;

public class KeyBindingManager : IKeyBindingManager
{
	public static string UPDATED = "KeyBindingManager.UPDATED";

	private bool isBindingKey;

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

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	public bool IsBindingKey
	{
		get
		{
			return this.isBindingKey;
		}
	}

	public void Begin()
	{
		this.timer.CancelTimeout(new Action(this.end));
		if (!this.isBindingKey)
		{
			this.isBindingKey = true;
			this.signalBus.Dispatch(KeyBindingManager.UPDATED);
		}
	}

	public void End()
	{
		this.timer.CancelTimeout(new Action(this.end));
		this.timer.SetTimeout(1, new Action(this.end));
	}

	private void end()
	{
		if (this.isBindingKey)
		{
			this.isBindingKey = false;
			this.signalBus.Dispatch(KeyBindingManager.UPDATED);
		}
	}
}
