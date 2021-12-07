// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IUserInputManager
{
	int LocalUserCount
	{
		get;
	}

	PlayerNum PrimaryUserPlayerNum
	{
		get;
	}

	Mapping<int, PlayerInputPort> Ports
	{
		get;
	}

	HashSet<int> UnassignedPortIds
	{
		get;
	}

	void Init(ConfigData config);

	bool BindToAvailableUser(PlayerInputPort port, IInputDevice device);

	bool BindWithoutDeviceToAvailableUser(PlayerInputPort port);

	bool Bind(PlayerInputPort port, IInputDevice device, IUser user);

	void Unbind(PlayerInputPort port);

	void UnbindAll();

	void Unbind(PlayerNum playerNum);

	PlayerInputPort GetPortWithId(int portId);

	PlayerInputPort GetPortWithDevice(IInputDevice device);

	PlayerInputPort GetPortWithUser(IUser user);

	PlayerInputPort GetPortWithPlayerNum(PlayerNum playerNum);

	PlayerInputPort GetLocalPlayer(int index);

	int GetBestPortId(PlayerNum playerNum);

	IInputDevice GetDeviceWithPort(PlayerInputPort port);

	IInputDevice GetDeviceWithUser(IUser user);

	IInputDevice GetDeviceWithPlayerNum(PlayerNum playerNum);

	IUser GetUserWithPort(PlayerInputPort port);

	IUser GetUserWithDevice(IInputDevice device);

	IUser GetUserWithPlayerNum(PlayerNum playerNum);

	bool AssignLocalPlayerNums(PlayerNum[] playerNums);

	bool AssignPlayerNum(int portId, PlayerNum playerNum);

	PlayerNum GetPlayerNum(IUser user);

	PlayerNum GetPlayerNum(IInputDevice device);

	PlayerNum GetPlayerNum(PlayerInputPort port);

	PlayerNum GetLocalPlayerNum(int portId);

	PlayerInputPort GetFirstPortWithNoUser();

	PlayerInputPort GetFirstPortWithNoDevice();

	PlayerInputPort GetFirstPortWithNoPlayer();

	void ResetPlayerMapping();

	void ResetPlayerMappingForPort(PlayerInputPort port);

	bool IsPlayerNumInUse(PlayerNum playerNum);

	bool ForceBindAvailablePortToPlayer(PlayerNum playerNum);

	bool ForceBindAvailablePortToPlayerNoUser(PlayerNum playerNum);
}
