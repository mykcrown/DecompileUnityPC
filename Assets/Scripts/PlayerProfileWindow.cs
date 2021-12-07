// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

public class PlayerProfileWindow : ClientBehavior
{
	public WavedashTMProInput InputField;

	public WavedashTextEntry TextEntry;

	public Action OnCloseRequest;

	private PlayerNum playerNum;

	[Inject]
	public UIManager uiManager
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

	public void Initialize(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
	}

	public override void Awake()
	{
		base.Awake();
		this.InputField.EnterCallback = new Action(this.enterName);
	}

	public void OnOpened()
	{
		this.timer.SetTimeout(1, new Action(this._OnOpened_m__0));
	}

	public void enterName()
	{
		base.signalBus.GetSignal<SetPlayerProfileNameSignal>().Dispatch(this.playerNum, this.InputField.text);
		this.OnCloseRequest();
	}

	private void _OnOpened_m__0()
	{
		(this.uiManager.CurrentInputModule as CursorInputModule).SetSelectedInputField(this.InputField);
	}
}
