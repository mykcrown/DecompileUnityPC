using System;
using InControl;

// Token: 0x0200097F RID: 2431
public interface IInputSettingsScreenAPI
{
	// Token: 0x060041AD RID: 16813
	void Initialize();

	// Token: 0x060041AE RID: 16814
	void SetBinding(ButtonPress buttonPress, BindingSource bindingSource, int buttonIndex);

	// Token: 0x060041AF RID: 16815
	BindingSource GetBindingSource(ButtonPress buttonPress, int buttonIndex);

	// Token: 0x060041B0 RID: 16816
	void ListenForBindingSource(ButtonPress buttonPress, int buttonIndex);

	// Token: 0x060041B1 RID: 16817
	void RemoveBinding(ButtonPress buttonPress, int buttonIndex);

	// Token: 0x060041B2 RID: 16818
	void CancelListenForBinding();

	// Token: 0x17000F75 RID: 3957
	// (get) Token: 0x060041B3 RID: 16819
	bool IsListeningForBinding { get; }

	// Token: 0x17000F76 RID: 3958
	// (get) Token: 0x060041B4 RID: 16820
	ButtonPress ListeningButtonPress { get; }

	// Token: 0x17000F77 RID: 3959
	// (get) Token: 0x060041B5 RID: 16821
	int ListeningButtonIndex { get; }

	// Token: 0x17000F78 RID: 3960
	// (get) Token: 0x060041B6 RID: 16822
	// (set) Token: 0x060041B7 RID: 16823
	ButtonPress LastUnboundButtonPress { get; set; }

	// Token: 0x17000F79 RID: 3961
	// (get) Token: 0x060041B8 RID: 16824
	// (set) Token: 0x060041B9 RID: 16825
	BindingSource LastUnboundBinding { get; set; }

	// Token: 0x060041BA RID: 16826
	void SaveControls();

	// Token: 0x060041BB RID: 16827
	void ResetControls();

	// Token: 0x060041BC RID: 16828
	void OnExitScreen();

	// Token: 0x17000F7A RID: 3962
	// (get) Token: 0x060041BD RID: 16829
	// (set) Token: 0x060041BE RID: 16830
	bool isTapJump { get; set; }

	// Token: 0x17000F7B RID: 3963
	// (get) Token: 0x060041BF RID: 16831
	// (set) Token: 0x060041C0 RID: 16832
	bool isTapStrike { get; set; }

	// Token: 0x17000F7C RID: 3964
	// (get) Token: 0x060041C1 RID: 16833
	// (set) Token: 0x060041C2 RID: 16834
	bool isRecoveryJump { get; set; }

	// Token: 0x17000F7D RID: 3965
	// (get) Token: 0x060041C3 RID: 16835
	// (set) Token: 0x060041C4 RID: 16836
	bool isDoubleTapToRun { get; set; }

	// Token: 0x17000F7E RID: 3966
	// (get) Token: 0x060041C5 RID: 16837
	// (set) Token: 0x060041C6 RID: 16838
	float LeftStickDeadZone { get; set; }

	// Token: 0x17000F7F RID: 3967
	// (get) Token: 0x060041C7 RID: 16839
	// (set) Token: 0x060041C8 RID: 16840
	float RightStickDeadZone { get; set; }

	// Token: 0x17000F80 RID: 3968
	// (get) Token: 0x060041C9 RID: 16841
	// (set) Token: 0x060041CA RID: 16842
	float LeftTriggerDeadZone { get; set; }

	// Token: 0x17000F81 RID: 3969
	// (get) Token: 0x060041CB RID: 16843
	// (set) Token: 0x060041CC RID: 16844
	float RightTriggerDeadZone { get; set; }

	// Token: 0x17000F82 RID: 3970
	// (get) Token: 0x060041CD RID: 16845
	bool SettingsChanged { get; }

	// Token: 0x17000F83 RID: 3971
	// (get) Token: 0x060041CE RID: 16846
	bool IsMovementUnbound { get; }
}
