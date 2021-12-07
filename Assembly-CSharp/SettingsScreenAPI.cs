using System;
using InControl;

// Token: 0x02000980 RID: 2432
public class SettingsScreenAPI : ISettingsScreenAPI
{
	// Token: 0x17000F84 RID: 3972
	// (get) Token: 0x060041D0 RID: 16848 RVA: 0x00126F85 File Offset: 0x00125385
	// (set) Token: 0x060041D1 RID: 16849 RVA: 0x00126F8D File Offset: 0x0012538D
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000F85 RID: 3973
	// (get) Token: 0x060041D2 RID: 16850 RVA: 0x00126F96 File Offset: 0x00125396
	// (set) Token: 0x060041D3 RID: 16851 RVA: 0x00126F9E File Offset: 0x0012539E
	[Inject]
	public PlayerInputLocator playerInput { get; set; }

	// Token: 0x17000F86 RID: 3974
	// (get) Token: 0x060041D4 RID: 16852 RVA: 0x00126FA7 File Offset: 0x001253A7
	// (set) Token: 0x060041D5 RID: 16853 RVA: 0x00126FAF File Offset: 0x001253AF
	public PlayerInputPort InputPort { get; private set; }

	// Token: 0x060041D6 RID: 16854 RVA: 0x00126FB8 File Offset: 0x001253B8
	public void SetPlayer(int portId)
	{
		this.InputPort = this.userInputManager.GetPortWithId(portId);
		if (this.InputPort == null)
		{
			this.InputPort = this.userInputManager.GetLocalPlayer(0);
		}
		if (this.InputPort.Device == InputDevice.Null)
		{
			this.playerInput.Input.AssignFirstAvailableDevice(this.InputPort, DevicePreference.Any);
		}
	}
}
