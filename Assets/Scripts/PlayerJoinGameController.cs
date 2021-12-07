// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;

public class PlayerJoinGameController : IPlayerJoinGameController
{
	private bool isActive;

	[Inject]
	public IUserInputManager userInputManager
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
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	[Inject]
	public UIManager uiManager
	{
		get;
		set;
	}

	private bool shouldAssignPlayers
	{
		get
		{
			return this.isActive && (!this.battleServerAPI.IsOnlineMatchReady || !this.battleServerAPI.IsSinglePlayerNetworkGame) && this.userInputManager.UnassignedPortIds.Count > 0 && this.uiManager.CurrentInputModule is CursorInputModule;
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.GetSignal<SetPlayerDeviceSignal>().AddListener(new Action<int, IInputDevice>(this.onSetPlayerDevice));
	}

	private void onSetPlayerDevice(int portId, IInputDevice device)
	{
		if (this.shouldAssignPlayers)
		{
			this.tryAssignDevice(portId, device);
		}
	}

	public void DoUpdate()
	{
		if (this.shouldAssignPlayers)
		{
			HashSet<int> hashSet = new HashSet<int>(this.userInputManager.UnassignedPortIds);
			foreach (int current in hashSet)
			{
				PlayerInputPort portWithId = this.userInputManager.GetPortWithId(current);
				IInputDevice deviceWithPort = this.userInputManager.GetDeviceWithPort(portWithId);
				if (InputUtils.IsDeviceActivity(deviceWithPort))
				{
					this.tryAssignDevice(current, deviceWithPort);
				}
			}
		}
	}

	public void Activate()
	{
		this.isActive = true;
	}

	public void Deactivate()
	{
		this.isActive = false;
	}

	private void tryAssignDevice(int portId, IInputDevice device)
	{
		if (device != null && device != InputDevice.Null)
		{
			PlayerNum playerNum = this.findFirstOpenPlayerSlot();
			if (playerNum != PlayerNum.None && this.userInputManager.AssignPlayerNum(portId, playerNum))
			{
				this.events.Broadcast(new SetPlayerTypeRequest(playerNum, PlayerType.Human, false));
				this.signalBus.GetSignal<PlayerAssignedSignal>().Dispatch(playerNum);
			}
		}
	}

	private PlayerNum findFirstOpenPlayerSlot()
	{
		for (int i = 0; i < this.enterNewGame.GamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.enterNewGame.GamePayload.players[i];
			if (this.canAssignDevice(playerSelectionInfo))
			{
				return playerSelectionInfo.playerNum;
			}
		}
		return PlayerNum.None;
	}

	private bool canAssignDevice(PlayerSelectionInfo info)
	{
		return this.userInputManager.GetDeviceWithPlayerNum(info.playerNum) == null && info.type != PlayerType.CPU && (!this.battleServerAPI.IsConnected || this.battleServerAPI.IsLocalPlayer(info.playerNum));
	}
}
