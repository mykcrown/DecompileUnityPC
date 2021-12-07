using System;

// Token: 0x02000917 RID: 2327
public interface ICustomInputModule
{
	// Token: 0x06003C6D RID: 15469
	bool ShouldActivateModule();

	// Token: 0x06003C6E RID: 15470
	void UpdateModule();

	// Token: 0x17000E69 RID: 3689
	// (set) Token: 0x06003C6F RID: 15471
	UIManager uiManager { set; }

	// Token: 0x17000E6A RID: 3690
	// (get) Token: 0x06003C70 RID: 15472
	// (set) Token: 0x06003C71 RID: 15473
	bool enabled { get; set; }

	// Token: 0x06003C72 RID: 15474
	void InitControlMode(ControlMode controlMode);

	// Token: 0x17000E6B RID: 3691
	// (get) Token: 0x06003C73 RID: 15475
	ControlMode CurrentMode { get; }

	// Token: 0x17000E6C RID: 3692
	// (get) Token: 0x06003C74 RID: 15476
	bool IsMouseMode { get; }

	// Token: 0x17000E6D RID: 3693
	// (get) Token: 0x06003C75 RID: 15477
	WavedashTMProInput CurrentInputField { get; }

	// Token: 0x06003C76 RID: 15478
	void SetSelectedInputField(WavedashTMProInput inputField);

	// Token: 0x06003C77 RID: 15479
	void SetPauseMode(bool isPause);
}
