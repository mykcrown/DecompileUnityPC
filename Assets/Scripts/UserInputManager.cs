// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using P2P;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserInputManager : IUserInputManager
{
	private Mapping<int, PlayerInputPort> ports = new Mapping<int, PlayerInputPort>();

	private List<int> activePorts = new List<int>();

	private HashSet<int> unassignedActivePorts = new HashSet<int>();

	private Mapping<PlayerInputPort, IUser> userMapping = new Mapping<PlayerInputPort, IUser>();

	private Mapping<PlayerInputPort, IInputDevice> deviceMapping = new Mapping<PlayerInputPort, IInputDevice>();

	private Mapping<PlayerInputPort, PlayerNum> playerMapping = new Mapping<PlayerInputPort, PlayerNum>();

	private GameObject userManagerObject;

	[Inject]
	public IUserManager userManager
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
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
	public ISignalBus signalBus
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
	public IInputSettingsSaveManager settingsSaveManager
	{
		get;
		set;
	}

	public int LocalUserCount
	{
		get
		{
			return this.userMapping.Count;
		}
	}

	public PlayerNum PrimaryUserPlayerNum
	{
		get
		{
			return this.GetPlayerNum(this.userManager.PrimaryUser);
		}
	}

	public Mapping<int, PlayerInputPort> Ports
	{
		get
		{
			return this.ports;
		}
	}

	public HashSet<int> UnassignedPortIds
	{
		get
		{
			return this.unassignedActivePorts;
		}
	}

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
		IBattleServerAPI expr_7A = this.battleServerAPI;
		expr_7A.OnMatchReady = (Action<SP2PMatchBasicPlayerDesc[]>)Delegate.Combine(expr_7A.OnMatchReady, new Action<SP2PMatchBasicPlayerDesc[]>(this.onMatchReady));
	}

	private void onMatchReady(SP2PMatchBasicPlayerDesc[] players)
	{
		this.AssignLocalPlayerNums(this.battleServerAPI.LocalPlayerNumIDs.ToArray());
	}

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
		foreach (PlayerInputPort current in this.deviceMapping)
		{
			IInputDevice inputDevice = this.deviceMapping[current];
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

	public void UnbindAll()
	{
		this.userMapping.Clear();
		this.deviceMapping.Clear();
		this.playerMapping.Clear();
		this.activePorts.Clear();
		this.unassignedActivePorts.Clear();
	}

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

	public void Unbind(PlayerNum playerNum)
	{
		if (this.playerMapping.ContainsSecond(playerNum))
		{
			this.Unbind(this.playerMapping[playerNum]);
		}
	}

	public PlayerInputPort GetPortWithId(int portId)
	{
		if (this.ports.ContainsFirst(portId))
		{
			return this.ports[portId];
		}
		return null;
	}

	public PlayerInputPort GetPortWithDevice(IInputDevice device)
	{
		if (device != InputDevice.Null && this.deviceMapping.ContainsSecond(device))
		{
			return this.deviceMapping.GetFirst(device);
		}
		return null;
	}

	public IUser GetUserWithDevice(IInputDevice device)
	{
		PlayerInputPort portWithDevice = this.GetPortWithDevice(device);
		if (portWithDevice != null && this.userMapping.ContainsFirst(portWithDevice))
		{
			return this.userMapping.GetSecond(portWithDevice);
		}
		return null;
	}

	public PlayerInputPort GetPortWithUser(IUser user)
	{
		if (this.userMapping.ContainsSecond(user))
		{
			return this.userMapping.GetFirst(user);
		}
		return null;
	}

	public IInputDevice GetDeviceWithUser(IUser user)
	{
		PlayerInputPort portWithUser = this.GetPortWithUser(user);
		if (portWithUser != null && this.deviceMapping.ContainsFirst(portWithUser))
		{
			return this.deviceMapping.GetSecond(this.GetPortWithUser(user));
		}
		return InputDevice.Null;
	}

	public IInputDevice GetDeviceWithPort(PlayerInputPort port)
	{
		if (this.deviceMapping.ContainsFirst(port))
		{
			return this.deviceMapping.GetSecond(port);
		}
		return null;
	}

	public IUser GetUserWithPort(PlayerInputPort port)
	{
		if (this.userMapping.ContainsFirst(port))
		{
			return this.userMapping.GetSecond(port);
		}
		return null;
	}

	public PlayerInputPort GetLocalPlayer(int index)
	{
		return (index >= this.activePorts.Count) ? null : this.ports[this.activePorts[index]];
	}

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

	public PlayerInputPort GetPortWithPlayerNum(PlayerNum playerNum)
	{
		if (this.playerMapping.ContainsSecond(playerNum))
		{
			return this.playerMapping[playerNum];
		}
		return null;
	}

	public IInputDevice GetDeviceWithPlayerNum(PlayerNum playerNum)
	{
		PlayerInputPort portWithPlayerNum = this.GetPortWithPlayerNum(playerNum);
		if (portWithPlayerNum == null)
		{
			return null;
		}
		return this.GetDeviceWithPort(portWithPlayerNum);
	}

	public IUser GetUserWithPlayerNum(PlayerNum playerNum)
	{
		return this.GetUserWithPort(this.GetPortWithPlayerNum(playerNum));
	}

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

	public PlayerNum GetPlayerNum(IUser user)
	{
		PlayerInputPort portWithUser = this.GetPortWithUser(user);
		if (portWithUser != null && this.playerMapping.ContainsFirst(portWithUser))
		{
			return this.playerMapping.GetSecond(portWithUser);
		}
		return PlayerNum.None;
	}

	public PlayerNum GetPlayerNum(IInputDevice device)
	{
		PlayerInputPort portWithDevice = this.GetPortWithDevice(device);
		if (portWithDevice != null && this.playerMapping.ContainsFirst(portWithDevice))
		{
			return this.playerMapping.GetSecond(portWithDevice);
		}
		return PlayerNum.None;
	}

	public PlayerNum GetPlayerNum(PlayerInputPort port)
	{
		if (this.playerMapping.ContainsFirst(port))
		{
			return this.playerMapping[port];
		}
		return PlayerNum.None;
	}

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

	public PlayerInputPort GetFirstPortWithNoUser()
	{
		foreach (int current in this.ports)
		{
			PlayerInputPort playerInputPort = this.ports[current];
			if (this.GetUserWithPort(playerInputPort) == null)
			{
				return playerInputPort;
			}
		}
		return null;
	}

	public PlayerInputPort GetFirstPortWithNoDevice()
	{
		foreach (int current in this.ports)
		{
			PlayerInputPort playerInputPort = this.ports[current];
			if (this.GetDeviceWithPort(playerInputPort) == null)
			{
				return playerInputPort;
			}
		}
		return null;
	}

	public PlayerInputPort GetFirstPortWithNoPlayer()
	{
		foreach (int current in this.ports)
		{
			PlayerInputPort playerInputPort = this.ports[current];
			PlayerNum playerNum = this.GetPlayerNum(playerInputPort);
			if (playerNum == PlayerNum.None)
			{
				return playerInputPort;
			}
		}
		return null;
	}

	public void ResetPlayerMapping()
	{
		this.playerMapping.Clear();
		foreach (int current in this.activePorts)
		{
			this.unassignedActivePorts.Add(current);
		}
	}

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

	public bool IsPlayerNumInUse(PlayerNum playerNum)
	{
		return this.playerMapping.ContainsSecond(playerNum);
	}

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
}
