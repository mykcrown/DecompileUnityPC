using System;
using System.Collections.Generic;

// Token: 0x02000A94 RID: 2708
public interface IUserInputManager
{
	// Token: 0x06004F4C RID: 20300
	void Init(ConfigData config);

	// Token: 0x06004F4D RID: 20301
	bool BindToAvailableUser(PlayerInputPort port, IInputDevice device);

	// Token: 0x06004F4E RID: 20302
	bool BindWithoutDeviceToAvailableUser(PlayerInputPort port);

	// Token: 0x06004F4F RID: 20303
	bool Bind(PlayerInputPort port, IInputDevice device, IUser user);

	// Token: 0x06004F50 RID: 20304
	void Unbind(PlayerInputPort port);

	// Token: 0x06004F51 RID: 20305
	void UnbindAll();

	// Token: 0x06004F52 RID: 20306
	void Unbind(PlayerNum playerNum);

	// Token: 0x06004F53 RID: 20307
	PlayerInputPort GetPortWithId(int portId);

	// Token: 0x06004F54 RID: 20308
	PlayerInputPort GetPortWithDevice(IInputDevice device);

	// Token: 0x06004F55 RID: 20309
	PlayerInputPort GetPortWithUser(IUser user);

	// Token: 0x06004F56 RID: 20310
	PlayerInputPort GetPortWithPlayerNum(PlayerNum playerNum);

	// Token: 0x06004F57 RID: 20311
	PlayerInputPort GetLocalPlayer(int index);

	// Token: 0x06004F58 RID: 20312
	int GetBestPortId(PlayerNum playerNum);

	// Token: 0x06004F59 RID: 20313
	IInputDevice GetDeviceWithPort(PlayerInputPort port);

	// Token: 0x06004F5A RID: 20314
	IInputDevice GetDeviceWithUser(IUser user);

	// Token: 0x06004F5B RID: 20315
	IInputDevice GetDeviceWithPlayerNum(PlayerNum playerNum);

	// Token: 0x06004F5C RID: 20316
	IUser GetUserWithPort(PlayerInputPort port);

	// Token: 0x06004F5D RID: 20317
	IUser GetUserWithDevice(IInputDevice device);

	// Token: 0x06004F5E RID: 20318
	IUser GetUserWithPlayerNum(PlayerNum playerNum);

	// Token: 0x170012D3 RID: 4819
	// (get) Token: 0x06004F5F RID: 20319
	int LocalUserCount { get; }

	// Token: 0x06004F60 RID: 20320
	bool AssignLocalPlayerNums(PlayerNum[] playerNums);

	// Token: 0x06004F61 RID: 20321
	bool AssignPlayerNum(int portId, PlayerNum playerNum);

	// Token: 0x06004F62 RID: 20322
	PlayerNum GetPlayerNum(IUser user);

	// Token: 0x06004F63 RID: 20323
	PlayerNum GetPlayerNum(IInputDevice device);

	// Token: 0x06004F64 RID: 20324
	PlayerNum GetPlayerNum(PlayerInputPort port);

	// Token: 0x170012D4 RID: 4820
	// (get) Token: 0x06004F65 RID: 20325
	PlayerNum PrimaryUserPlayerNum { get; }

	// Token: 0x06004F66 RID: 20326
	PlayerNum GetLocalPlayerNum(int portId);

	// Token: 0x06004F67 RID: 20327
	PlayerInputPort GetFirstPortWithNoUser();

	// Token: 0x06004F68 RID: 20328
	PlayerInputPort GetFirstPortWithNoDevice();

	// Token: 0x06004F69 RID: 20329
	PlayerInputPort GetFirstPortWithNoPlayer();

	// Token: 0x170012D5 RID: 4821
	// (get) Token: 0x06004F6A RID: 20330
	Mapping<int, PlayerInputPort> Ports { get; }

	// Token: 0x170012D6 RID: 4822
	// (get) Token: 0x06004F6B RID: 20331
	HashSet<int> UnassignedPortIds { get; }

	// Token: 0x06004F6C RID: 20332
	void ResetPlayerMapping();

	// Token: 0x06004F6D RID: 20333
	void ResetPlayerMappingForPort(PlayerInputPort port);

	// Token: 0x06004F6E RID: 20334
	bool IsPlayerNumInUse(PlayerNum playerNum);

	// Token: 0x06004F6F RID: 20335
	bool ForceBindAvailablePortToPlayer(PlayerNum playerNum);

	// Token: 0x06004F70 RID: 20336
	bool ForceBindAvailablePortToPlayerNoUser(PlayerNum playerNum);
}
