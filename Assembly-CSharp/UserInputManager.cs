using System;
using System.Collections.Generic;
using InControl;
using P2P;
using UnityEngine;

// Token: 0x02000A92 RID: 2706
public class UserInputManager : IUserInputManager
{
	// Token: 0x170012C9 RID: 4809
	// (get) Token: 0x06004F18 RID: 20248 RVA: 0x0014B295 File Offset: 0x00149695
	// (set) Token: 0x06004F19 RID: 20249 RVA: 0x0014B29D File Offset: 0x0014969D
	[Inject]
	public IUserManager userManager { get; set; }

	// Token: 0x170012CA RID: 4810
	// (get) Token: 0x06004F1A RID: 20250 RVA: 0x0014B2A6 File Offset: 0x001496A6
	// (set) Token: 0x06004F1B RID: 20251 RVA: 0x0014B2AE File Offset: 0x001496AE
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x170012CB RID: 4811
	// (get) Token: 0x06004F1C RID: 20252 RVA: 0x0014B2B7 File Offset: 0x001496B7
	// (set) Token: 0x06004F1D RID: 20253 RVA: 0x0014B2BF File Offset: 0x001496BF
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170012CC RID: 4812
	// (get) Token: 0x06004F1E RID: 20254 RVA: 0x0014B2C8 File Offset: 0x001496C8
	// (set) Token: 0x06004F1F RID: 20255 RVA: 0x0014B2D0 File Offset: 0x001496D0
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170012CD RID: 4813
	// (get) Token: 0x06004F20 RID: 20256 RVA: 0x0014B2D9 File Offset: 0x001496D9
	// (set) Token: 0x06004F21 RID: 20257 RVA: 0x0014B2E1 File Offset: 0x001496E1
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x170012CE RID: 4814
	// (get) Token: 0x06004F22 RID: 20258 RVA: 0x0014B2EA File Offset: 0x001496EA
	// (set) Token: 0x06004F23 RID: 20259 RVA: 0x0014B2F2 File Offset: 0x001496F2
	[Inject]
	public IInputSettingsSaveManager settingsSaveManager { get; set; }

	// Token: 0x06004F24 RID: 20260 RVA: 0x0014B2FC File Offset: 0x001496FC
	public void Init(ConfigData config)
	{
		this.userManagerObject = new GameObject("__USER_INPUT_MANAGER");
		UnityEngine.Object.DontDestroyOnLoad(this.userManagerObject);
		for (int i = 0; i < config.maxPlayers; i++)
		{
			PlayerInputPort playerInputPort = this.userManagerObject.AddComponent<PlayerInputPort>();
			this.injector.Inject(playerInputPort);
			playerInputPort.Id = 100 + i;
			playerInputPort.Initialize(config.inputConfig, this);
			this.ports.Add(playerInputPort.Id, playerInputPort);
		}
		IBattleServerAPI battleServerAPI = this.battleServerAPI;
		battleServerAPI.OnMatchReady = (Action<SP2PMatchBasicPlayerDesc[]>)Delegate.Combine(battleServerAPI.OnMatchReady, new Action<SP2PMatchBasicPlayerDesc[]>(this.onMatchReady));
	}

	// Token: 0x06004F25 RID: 20261 RVA: 0x0014B3A4 File Offset: 0x001497A4
	private void onMatchReady(SP2PMatchBasicPlayerDesc[] players)
	{
		this.AssignLocalPlayerNums(this.battleServerAPI.LocalPlayerNumIDs.ToArray());
	}

	// Token: 0x06004F26 RID: 20262 RVA: 0x0014B3C0 File Offset: 0x001497C0
	public bool BindToAvailableUser(PlayerInputPort port, IInputDevice device)
	{
		if (port == null || device == null)
		{
			return false;
		}
		IUser user = null;
		IUser[] users = this.userManager.Users;
		for (int i = 0; i < users.Length; i++)
		{
			if (users[i] != null && this.GetDeviceWithUser(users[i]) == InputDevice.Null)
			{
				user = users[i];
				break;
			}
		}
		if (user == null)
		{
			user = this.userManager.AddGuest();
		}
		return this.Bind(port, device, user);
	}

	// Token: 0x06004F27 RID: 20263 RVA: 0x0014B444 File Offset: 0x00149844
	public bool BindWithoutDeviceToAvailableUser(PlayerInputPort port)
	{
		if (port == null)
		{
			return false;
		}
		IUser user = null;
		IUser[] users = this.userManager.Users;
		for (int i = 0; i < users.Length; i++)
		{
			if (users[i] != null && this.GetPortWithUser(users[i]) == null)
			{
				user = users[i];
				break;
			}
		}
		if (user == null)
		{
			user = this.userManager.AddGuest();
		}
		return this.BindWithoutDevice(port, user);
	}

	// Token: 0x06004F28 RID: 20264 RVA: 0x0014B4C0 File Offset: 0x001498C0
	public bool Bind(PlayerInputPort port, IInputDevice device, IUser user)
	{
		if (port == null || device == null || user == null)
		{
			return false;
		}
		int id = port.Id;
		if (this.userMapping.AreEitherInMapping(port, user) && !this.userMapping.AreMappedTogether(port, user))
		{
			return false;
		}
		if (this.deviceMapping.AreEitherInMapping(port, device) && !this.deviceMapping.AreMappedTogether(port, device))
		{
			return false;
		}
		if (!this.activePorts.Contains(id))
		{
			this.activePorts.Add(id);
		}
		if (!this.playerMapping.ContainsFirst(port))
		{
			this.unassignedActivePorts.Add(id);
		}
		this.userMapping.Add(port, user);
		bool flag = false;
		foreach (PlayerInputPort key in this.deviceMapping)
		{
			IInputDevice inputDevice = this.deviceMapping[key];
			if (inputDevice != null && inputDevice != device && InputUtils.GetDeviceType(device) == InputUtils.GetDeviceType(inputDevice))
			{
				flag = true;
				break;
			}
		}
		port.SetDevice(device, !flag);
		InputSettingsData settings = this.settingsSaveManager.LoadInputSettings(port.Device, false, port.ShouldPersistSettings);
		port.LoadSettings(settings);
		this.deviceMapping.Add(port, device);
		return true;
	}

	// Token: 0x06004F29 RID: 20265 RVA: 0x0014B63C File Offset: 0x00149A3C
	public bool BindWithoutDevice(PlayerInputPort port, IUser user)
	{
		if (port == null || user == null)
		{
			return false;
		}
		int id = port.Id;
		if (this.userMapping.AreEitherInMapping(port, user))
		{
			return false;
		}
		if (!this.activePorts.Contains(id))
		{
			this.activePorts.Add(id);
		}
		if (!this.playerMapping.ContainsFirst(port))
		{
			this.unassignedActivePorts.Add(id);
		}
		this.userMapping.Add(port, user);
		return true;
	}

	// Token: 0x06004F2A RID: 20266 RVA: 0x0014B6C2 File Offset: 0x00149AC2
	public void UnbindAll()
	{
		this.userMapping.Clear();
		this.deviceMapping.Clear();
		this.playerMapping.Clear();
		this.activePorts.Clear();
		this.unassignedActivePorts.Clear();
	}

	// Token: 0x06004F2B RID: 20267 RVA: 0x0014B6FC File Offset: 0x00149AFC
	public void Unbind(PlayerInputPort port)
	{
		if (this.userMapping.ContainsFirst(port))
		{
			this.userMapping.RemoveByFirst(port);
		}
		if (this.deviceMapping.ContainsFirst(port))
		{
			this.deviceMapping.RemoveByFirst(port);
		}
		PlayerNum playerNum = PlayerNum.None;
		if (this.playerMapping.ContainsFirst(port))
		{
			playerNum = this.playerMapping.GetSecond(port);
			this.playerMapping.RemoveByFirst(port);
		}
		int id = port.Id;
		if (this.activePorts.Contains(id))
		{
			this.activePorts.Remove(id);
		}
		port.SetDevice(InputDevice.Null, false);
		this.unassignedActivePorts.Remove(id);
		if (playerNum != PlayerNum.None)
		{
			this.events.Broadcast(new PortUnassignedPlayerEvent(id, playerNum));
		}
	}

	// Token: 0x06004F2C RID: 20268 RVA: 0x0014B7CB File Offset: 0x00149BCB
	public void Unbind(PlayerNum playerNum)
	{
		if (this.playerMapping.ContainsSecond(playerNum))
		{
			this.Unbind(this.playerMapping[playerNum]);
		}
	}

	// Token: 0x06004F2D RID: 20269 RVA: 0x0014B7F0 File Offset: 0x00149BF0
	public PlayerInputPort GetPortWithId(int portId)
	{
		if (this.ports.ContainsFirst(portId))
		{
			return this.ports[portId];
		}
		return null;
	}

	// Token: 0x06004F2E RID: 20270 RVA: 0x0014B811 File Offset: 0x00149C11
	public PlayerInputPort GetPortWithDevice(IInputDevice device)
	{
		if (device != InputDevice.Null && this.deviceMapping.ContainsSecond(device))
		{
			return this.deviceMapping.GetFirst(device);
		}
		return null;
	}

	// Token: 0x06004F2F RID: 20271 RVA: 0x0014B840 File Offset: 0x00149C40
	public IUser GetUserWithDevice(IInputDevice device)
	{
		PlayerInputPort portWithDevice = this.GetPortWithDevice(device);
		if (portWithDevice != null && this.userMapping.ContainsFirst(portWithDevice))
		{
			return this.userMapping.GetSecond(portWithDevice);
		}
		return null;
	}

	// Token: 0x06004F30 RID: 20272 RVA: 0x0014B880 File Offset: 0x00149C80
	public PlayerInputPort GetPortWithUser(IUser user)
	{
		if (this.userMapping.ContainsSecond(user))
		{
			return this.userMapping.GetFirst(user);
		}
		return null;
	}

	// Token: 0x06004F31 RID: 20273 RVA: 0x0014B8A4 File Offset: 0x00149CA4
	public IInputDevice GetDeviceWithUser(IUser user)
	{
		PlayerInputPort portWithUser = this.GetPortWithUser(user);
		if (portWithUser != null && this.deviceMapping.ContainsFirst(portWithUser))
		{
			return this.deviceMapping.GetSecond(this.GetPortWithUser(user));
		}
		return InputDevice.Null;
	}

	// Token: 0x06004F32 RID: 20274 RVA: 0x0014B8EE File Offset: 0x00149CEE
	public IInputDevice GetDeviceWithPort(PlayerInputPort port)
	{
		if (this.deviceMapping.ContainsFirst(port))
		{
			return this.deviceMapping.GetSecond(port);
		}
		return null;
	}

	// Token: 0x06004F33 RID: 20275 RVA: 0x0014B90F File Offset: 0x00149D0F
	public IUser GetUserWithPort(PlayerInputPort port)
	{
		if (this.userMapping.ContainsFirst(port))
		{
			return this.userMapping.GetSecond(port);
		}
		return null;
	}

	// Token: 0x06004F34 RID: 20276 RVA: 0x0014B930 File Offset: 0x00149D30
	public PlayerInputPort GetLocalPlayer(int index)
	{
		return (index >= this.activePorts.Count) ? null : this.ports[this.activePorts[index]];
	}

	// Token: 0x06004F35 RID: 20277 RVA: 0x0014B960 File Offset: 0x00149D60
	public int GetBestPortId(PlayerNum playerNum)
	{
		int result = 100;
		PlayerInputPort portWithPlayerNum = this.GetPortWithPlayerNum(playerNum);
		if (portWithPlayerNum != null)
		{
			result = portWithPlayerNum.Id;
		}
		return result;
	}

	// Token: 0x06004F36 RID: 20278 RVA: 0x0014B98C File Offset: 0x00149D8C
	public PlayerInputPort GetPortWithPlayerNum(PlayerNum playerNum)
	{
		if (this.playerMapping.ContainsSecond(playerNum))
		{
			return this.playerMapping[playerNum];
		}
		return null;
	}

	// Token: 0x06004F37 RID: 20279 RVA: 0x0014B9B0 File Offset: 0x00149DB0
	public IInputDevice GetDeviceWithPlayerNum(PlayerNum playerNum)
	{
		PlayerInputPort portWithPlayerNum = this.GetPortWithPlayerNum(playerNum);
		if (portWithPlayerNum == null)
		{
			return null;
		}
		return this.GetDeviceWithPort(portWithPlayerNum);
	}

	// Token: 0x06004F38 RID: 20280 RVA: 0x0014B9DA File Offset: 0x00149DDA
	public IUser GetUserWithPlayerNum(PlayerNum playerNum)
	{
		return this.GetUserWithPort(this.GetPortWithPlayerNum(playerNum));
	}

	// Token: 0x170012CF RID: 4815
	// (get) Token: 0x06004F39 RID: 20281 RVA: 0x0014B9E9 File Offset: 0x00149DE9
	public int LocalUserCount
	{
		get
		{
			return this.userMapping.Count;
		}
	}

	// Token: 0x06004F3A RID: 20282 RVA: 0x0014B9F8 File Offset: 0x00149DF8
	public bool AssignLocalPlayerNums(PlayerNum[] playerNums)
	{
		if (this.activePorts.Count < playerNums.Length)
		{
			return false;
		}
		this.ResetPlayerMapping();
		for (int i = 0; i < playerNums.Length; i++)
		{
			PlayerNum playerNum = playerNums[i];
			this.AssignPlayerNum(this.activePorts[i], playerNum);
		}
		for (PlayerNum playerNum2 = PlayerNum.Player1; playerNum2 < PlayerNum.All; playerNum2++)
		{
			this.signalBus.GetSignal<PlayerAssignedSignal>().Dispatch(playerNum2);
		}
		return true;
	}

	// Token: 0x06004F3B RID: 20283 RVA: 0x0014BA70 File Offset: 0x00149E70
	public bool AssignPlayerNum(int portId, PlayerNum playerNum)
	{
		PlayerInputPort portWithId = this.GetPortWithId(portId);
		if (this.playerMapping.AreEitherInMapping(portWithId, playerNum))
		{
			return false;
		}
		this.playerMapping.Add(portWithId, playerNum);
		this.unassignedActivePorts.Remove(portId);
		portWithId.OnAssignedToPlayer();
		return true;
	}

	// Token: 0x06004F3C RID: 20284 RVA: 0x0014BABC File Offset: 0x00149EBC
	public PlayerNum GetPlayerNum(IUser user)
	{
		PlayerInputPort portWithUser = this.GetPortWithUser(user);
		if (portWithUser != null && this.playerMapping.ContainsFirst(portWithUser))
		{
			return this.playerMapping.GetSecond(portWithUser);
		}
		return PlayerNum.None;
	}

	// Token: 0x06004F3D RID: 20285 RVA: 0x0014BB00 File Offset: 0x00149F00
	public PlayerNum GetPlayerNum(IInputDevice device)
	{
		PlayerInputPort portWithDevice = this.GetPortWithDevice(device);
		if (portWithDevice != null && this.playerMapping.ContainsFirst(portWithDevice))
		{
			return this.playerMapping.GetSecond(portWithDevice);
		}
		return PlayerNum.None;
	}

	// Token: 0x06004F3E RID: 20286 RVA: 0x0014BB41 File Offset: 0x00149F41
	public PlayerNum GetPlayerNum(PlayerInputPort port)
	{
		if (this.playerMapping.ContainsFirst(port))
		{
			return this.playerMapping[port];
		}
		return PlayerNum.None;
	}

	// Token: 0x170012D0 RID: 4816
	// (get) Token: 0x06004F3F RID: 20287 RVA: 0x0014BB63 File Offset: 0x00149F63
	public PlayerNum PrimaryUserPlayerNum
	{
		get
		{
			return this.GetPlayerNum(this.userManager.PrimaryUser);
		}
	}

	// Token: 0x06004F40 RID: 20288 RVA: 0x0014BB78 File Offset: 0x00149F78
	public PlayerNum GetLocalPlayerNum(int portId)
	{
		for (int i = 0; i < this.activePorts.Count; i++)
		{
			if (this.activePorts[i] == portId)
			{
				return (PlayerNum)i;
			}
		}
		return PlayerNum.None;
	}

	// Token: 0x06004F41 RID: 20289 RVA: 0x0014BBB8 File Offset: 0x00149FB8
	public PlayerInputPort GetFirstPortWithNoUser()
	{
		foreach (int key in this.ports)
		{
			PlayerInputPort playerInputPort = this.ports[key];
			if (this.GetUserWithPort(playerInputPort) == null)
			{
				return playerInputPort;
			}
		}
		return null;
	}

	// Token: 0x06004F42 RID: 20290 RVA: 0x0014BC34 File Offset: 0x0014A034
	public PlayerInputPort GetFirstPortWithNoDevice()
	{
		foreach (int key in this.ports)
		{
			PlayerInputPort playerInputPort = this.ports[key];
			if (this.GetDeviceWithPort(playerInputPort) == null)
			{
				return playerInputPort;
			}
		}
		return null;
	}

	// Token: 0x06004F43 RID: 20291 RVA: 0x0014BCB0 File Offset: 0x0014A0B0
	public PlayerInputPort GetFirstPortWithNoPlayer()
	{
		foreach (int key in this.ports)
		{
			PlayerInputPort playerInputPort = this.ports[key];
			PlayerNum playerNum = this.GetPlayerNum(playerInputPort);
			if (playerNum == PlayerNum.None)
			{
				return playerInputPort;
			}
		}
		return null;
	}

	// Token: 0x170012D1 RID: 4817
	// (get) Token: 0x06004F44 RID: 20292 RVA: 0x0014BD2C File Offset: 0x0014A12C
	public Mapping<int, PlayerInputPort> Ports
	{
		get
		{
			return this.ports;
		}
	}

	// Token: 0x170012D2 RID: 4818
	// (get) Token: 0x06004F45 RID: 20293 RVA: 0x0014BD34 File Offset: 0x0014A134
	public HashSet<int> UnassignedPortIds
	{
		get
		{
			return this.unassignedActivePorts;
		}
	}

	// Token: 0x06004F46 RID: 20294 RVA: 0x0014BD3C File Offset: 0x0014A13C
	public void ResetPlayerMapping()
	{
		this.playerMapping.Clear();
		foreach (int item in this.activePorts)
		{
			this.unassignedActivePorts.Add(item);
		}
	}

	// Token: 0x06004F47 RID: 20295 RVA: 0x0014BDAC File Offset: 0x0014A1AC
	public void ResetPlayerMappingForPort(PlayerInputPort port)
	{
		PlayerNum second = this.playerMapping.GetSecond(port);
		this.playerMapping.RemoveByFirst(port);
		if (second != PlayerNum.None)
		{
			this.events.Broadcast(new PortUnassignedPlayerEvent(port.Id, second));
		}
		this.unassignedActivePorts.Add(port.Id);
	}

	// Token: 0x06004F48 RID: 20296 RVA: 0x0014BE04 File Offset: 0x0014A204
	public bool IsPlayerNumInUse(PlayerNum playerNum)
	{
		return this.playerMapping.ContainsSecond(playerNum);
	}

	// Token: 0x06004F49 RID: 20297 RVA: 0x0014BE14 File Offset: 0x0014A214
	public bool ForceBindAvailablePortToPlayer(PlayerNum playerNum)
	{
		PlayerInputPort portWithPlayerNum = this.GetPortWithPlayerNum(playerNum);
		if (portWithPlayerNum == null)
		{
			PlayerInputPort firstPortWithNoUser = this.GetFirstPortWithNoUser();
			int id = firstPortWithNoUser.Id;
			this.BindWithoutDeviceToAvailableUser(firstPortWithNoUser);
			return this.AssignPlayerNum(id, playerNum);
		}
		return false;
	}

	// Token: 0x06004F4A RID: 20298 RVA: 0x0014BE58 File Offset: 0x0014A258
	public bool ForceBindAvailablePortToPlayerNoUser(PlayerNum playerNum)
	{
		PlayerInputPort portWithPlayerNum = this.GetPortWithPlayerNum(playerNum);
		if (portWithPlayerNum == null)
		{
			PlayerInputPort firstPortWithNoPlayer = this.GetFirstPortWithNoPlayer();
			int id = firstPortWithNoPlayer.Id;
			return this.AssignPlayerNum(id, playerNum);
		}
		return false;
	}

	// Token: 0x0400338E RID: 13198
	private Mapping<int, PlayerInputPort> ports = new Mapping<int, PlayerInputPort>();

	// Token: 0x0400338F RID: 13199
	private List<int> activePorts = new List<int>();

	// Token: 0x04003390 RID: 13200
	private HashSet<int> unassignedActivePorts = new HashSet<int>();

	// Token: 0x04003391 RID: 13201
	private Mapping<PlayerInputPort, IUser> userMapping = new Mapping<PlayerInputPort, IUser>();

	// Token: 0x04003392 RID: 13202
	private Mapping<PlayerInputPort, IInputDevice> deviceMapping = new Mapping<PlayerInputPort, IInputDevice>();

	// Token: 0x04003393 RID: 13203
	private Mapping<PlayerInputPort, PlayerNum> playerMapping = new Mapping<PlayerInputPort, PlayerNum>();

	// Token: 0x0400339A RID: 13210
	private GameObject userManagerObject;
}
