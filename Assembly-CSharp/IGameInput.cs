using System;
using FixedPoint;

// Token: 0x0200068B RID: 1675
public interface IGameInput
{
	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x0600296C RID: 10604
	Fixed HorizontalAxisValue { get; }

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x0600296D RID: 10605
	Fixed VerticalAxisValue { get; }

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x0600296E RID: 10606
	bool IsTapJumpInputPressed { get; }

	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x0600296F RID: 10607
	bool IsJumpInputPressed { get; }

	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x06002970 RID: 10608
	bool IsCrouchingInputPressed { get; }

	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x06002971 RID: 10609
	bool IsShieldInputPressed { get; }

	// Token: 0x06002972 RID: 10610
	bool IsHorizontalDirectionHeld(HorizontalDirection dir);

	// Token: 0x06002973 RID: 10611
	bool GetButton(ButtonPress press);

	// Token: 0x06002974 RID: 10612
	void Vibrate(float leftMotor, float rightMotor, float duration);
}
