using System;
using System.Collections.Generic;
using InControl;

// Token: 0x020008F1 RID: 2289
public class PlayerJoinGameController : IPlayerJoinGameController
{
	// Token: 0x17000E18 RID: 3608
	// (get) Token: 0x06003ABC RID: 15036 RVA: 0x00112B33 File Offset: 0x00110F33
	// (set) Token: 0x06003ABD RID: 15037 RVA: 0x00112B3B File Offset: 0x00110F3B
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000E19 RID: 3609
	// (get) Token: 0x06003ABE RID: 15038 RVA: 0x00112B44 File Offset: 0x00110F44
	// (set) Token: 0x06003ABF RID: 15039 RVA: 0x00112B4C File Offset: 0x00110F4C
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000E1A RID: 3610
	// (get) Token: 0x06003AC0 RID: 15040 RVA: 0x00112B55 File Offset: 0x00110F55
	// (set) Token: 0x06003AC1 RID: 15041 RVA: 0x00112B5D File Offset: 0x00110F5D
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000E1B RID: 3611
	// (get) Token: 0x06003AC2 RID: 15042 RVA: 0x00112B66 File Offset: 0x00110F66
	// (set) Token: 0x06003AC3 RID: 15043 RVA: 0x00112B6E File Offset: 0x00110F6E
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000E1C RID: 3612
	// (get) Token: 0x06003AC4 RID: 15044 RVA: 0x00112B77 File Offset: 0x00110F77
	// (set) Token: 0x06003AC5 RID: 15045 RVA: 0x00112B7F File Offset: 0x00110F7F
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17000E1D RID: 3613
	// (get) Token: 0x06003AC6 RID: 15046 RVA: 0x00112B88 File Offset: 0x00110F88
	// (set) Token: 0x06003AC7 RID: 15047 RVA: 0x00112B90 File Offset: 0x00110F90
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x06003AC8 RID: 15048 RVA: 0x00112B99 File Offset: 0x00110F99
	[PostConstruct]
	public void Init()
	{
		this.signalBus.GetSignal<SetPlayerDeviceSignal>().AddListener(new Action<int, IInputDevice>(this.onSetPlayerDevice));
	}

	// Token: 0x06003AC9 RID: 15049 RVA: 0x00112BB7 File Offset: 0x00110FB7
	private void onSetPlayerDevice(int portId, IInputDevice device)
	{
		if (this.shouldAssignPlayers)
		{
			this.tryAssignDevice(portId, device);
		}
	}

	// Token: 0x06003ACA RID: 15050 RVA: 0x00112BCC File Offset: 0x00110FCC
	public void DoUpdate()
	{
		if (this.shouldAssignPlayers)
		{
			HashSet<int> hashSet = new HashSet<int>(this.userInputManager.UnassignedPortIds);
			foreach (int portId in hashSet)
			{
				PlayerInputPort portWithId = this.userInputManager.GetPortWithId(portId);
				IInputDevice deviceWithPort = this.userInputManager.GetDeviceWithPort(portWithId);
				if (InputUtils.IsDeviceActivity(deviceWithPort))
				{
					this.tryAssignDevice(portId, deviceWithPort);
				}
			}
		}
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x00112C68 File Offset: 0x00111068
	public void Activate()
	{
		this.isActive = true;
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x00112C71 File Offset: 0x00111071
	public void Deactivate()
	{
		this.isActive = false;
	}

	// Token: 0x06003ACD RID: 15053 RVA: 0x00112C7C File Offset: 0x0011107C
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

	// Token: 0x06003ACE RID: 15054 RVA: 0x00112CE8 File Offset: 0x001110E8
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

	// Token: 0x06003ACF RID: 15055 RVA: 0x00112D48 File Offset: 0x00111148
	private bool canAssignDevice(PlayerSelectionInfo info)
	{
		return this.userInputManager.GetDeviceWithPlayerNum(info.playerNum) == null && info.type != PlayerType.CPU && (!this.battleServerAPI.IsConnected || this.battleServerAPI.IsLocalPlayer(info.playerNum));
	}

	// Token: 0x17000E1E RID: 3614
	// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x00112DA4 File Offset: 0x001111A4
	private bool shouldAssignPlayers
	{
		get
		{
			return this.isActive && (!this.battleServerAPI.IsOnlineMatchReady || !this.battleServerAPI.IsSinglePlayerNetworkGame) && this.userInputManager.UnassignedPortIds.Count > 0 && this.uiManager.CurrentInputModule is CursorInputModule;
		}
	}

	// Token: 0x04002879 RID: 10361
	private bool isActive;
}
