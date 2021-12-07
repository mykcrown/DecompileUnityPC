using System;

// Token: 0x02000696 RID: 1686
public interface IControllerInputDevice : IInputDevice
{
	// Token: 0x17000A30 RID: 2608
	// (get) Token: 0x06002993 RID: 10643
	// (set) Token: 0x06002994 RID: 10644
	float LeftStickLowerDeadZone { get; set; }

	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x06002995 RID: 10645
	// (set) Token: 0x06002996 RID: 10646
	float RightStickLowerDeadZone { get; set; }

	// Token: 0x17000A32 RID: 2610
	// (get) Token: 0x06002997 RID: 10647
	// (set) Token: 0x06002998 RID: 10648
	float LeftTriggerDeadZone { get; set; }

	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x06002999 RID: 10649
	// (set) Token: 0x0600299A RID: 10650
	float RightTriggerDeadZone { get; set; }

	// Token: 0x17000A34 RID: 2612
	// (get) Token: 0x0600299B RID: 10651
	float DefaultLeftStickLowerDeadZone { get; }

	// Token: 0x17000A35 RID: 2613
	// (get) Token: 0x0600299C RID: 10652
	float DefaultRightStickLowerDeadZone { get; }

	// Token: 0x17000A36 RID: 2614
	// (get) Token: 0x0600299D RID: 10653
	float DefaultLeftTriggerDeadZone { get; }

	// Token: 0x17000A37 RID: 2615
	// (get) Token: 0x0600299E RID: 10654
	float DefaultRightTriggerDeadZone { get; }
}
