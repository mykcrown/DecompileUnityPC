using System;

// Token: 0x020006B4 RID: 1716
public interface IButtonInputDelegate
{
	// Token: 0x06002ABF RID: 10943
	void OnSubmitPressed();

	// Token: 0x06002AC0 RID: 10944
	void OnCancelPressed();

	// Token: 0x06002AC1 RID: 10945
	void OnRightTriggerPressed();

	// Token: 0x06002AC2 RID: 10946
	void OnLeftTriggerPressed();

	// Token: 0x06002AC3 RID: 10947
	void OnLeftBumperPressed();

	// Token: 0x06002AC4 RID: 10948
	void OnZPressed();

	// Token: 0x06002AC5 RID: 10949
	void OnRightStickRight();

	// Token: 0x06002AC6 RID: 10950
	void OnRightStickLeft();

	// Token: 0x06002AC7 RID: 10951
	void OnRightStickUp();

	// Token: 0x06002AC8 RID: 10952
	void OnRightStickDown();

	// Token: 0x06002AC9 RID: 10953
	void UpdateRightStick(float x, float y);

	// Token: 0x06002ACA RID: 10954
	void OnLeft();

	// Token: 0x06002ACB RID: 10955
	void OnRight();

	// Token: 0x06002ACC RID: 10956
	void OnUp();

	// Token: 0x06002ACD RID: 10957
	void OnDown();

	// Token: 0x06002ACE RID: 10958
	void OnDPadLeft();

	// Token: 0x06002ACF RID: 10959
	void OnDPadRight();

	// Token: 0x06002AD0 RID: 10960
	void OnDPadUp();

	// Token: 0x06002AD1 RID: 10961
	void OnDPadDown();

	// Token: 0x06002AD2 RID: 10962
	void OnYButtonPressed();

	// Token: 0x06002AD3 RID: 10963
	void OnXButtonPressed();

	// Token: 0x06002AD4 RID: 10964
	void OnAnythingPressed();

	// Token: 0x06002AD5 RID: 10965
	void OnAnyMouseEvent();

	// Token: 0x06002AD6 RID: 10966
	void OnAnyNavigationButtonPressed();
}
