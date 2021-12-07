using System;
using FixedPoint;

// Token: 0x02000590 RID: 1424
public interface ICharacterPhysicsData
{
	// Token: 0x170006FC RID: 1788
	// (get) Token: 0x0600200F RID: 8207
	Fixed SlowWalkMaxSpeed { get; }

	// Token: 0x170006FD RID: 1789
	// (get) Token: 0x06002010 RID: 8208
	Fixed MediumWalkMaxSpeed { get; }

	// Token: 0x170006FE RID: 1790
	// (get) Token: 0x06002011 RID: 8209
	Fixed FastWalkMaxSpeed { get; }

	// Token: 0x170006FF RID: 1791
	// (get) Token: 0x06002012 RID: 8210
	Fixed RunMaxSpeed { get; }

	// Token: 0x17000700 RID: 1792
	// (get) Token: 0x06002013 RID: 8211
	Fixed GroundToAirMaxSpeed { get; }

	// Token: 0x17000701 RID: 1793
	// (get) Token: 0x06002014 RID: 8212
	Fixed WalkAcceleration { get; }

	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x06002015 RID: 8213
	Fixed RunPivotAcceleration { get; }

	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x06002016 RID: 8214
	Fixed Friction { get; }

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x06002017 RID: 8215
	Fixed HighSpeedFriction { get; }

	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x06002018 RID: 8216
	Fixed DashStartSpeed { get; }

	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x06002019 RID: 8217
	Fixed DashMaxSpeed { get; }

	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x0600201A RID: 8218
	Fixed DashAcceleration { get; }

	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x0600201B RID: 8219
	Fixed AirMaxSpeed { get; }

	// Token: 0x17000709 RID: 1801
	// (get) Token: 0x0600201C RID: 8220
	Fixed AirAcceleration { get; }

	// Token: 0x1700070A RID: 1802
	// (get) Token: 0x0600201D RID: 8221
	Fixed AirFriction { get; }

	// Token: 0x1700070B RID: 1803
	// (get) Token: 0x0600201E RID: 8222
	Fixed Gravity { get; }

	// Token: 0x1700070C RID: 1804
	// (get) Token: 0x0600201F RID: 8223
	Fixed MaxFallSpeed { get; }

	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x06002020 RID: 8224
	Fixed HelplessAirSpeedMultiplier { get; }

	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x06002021 RID: 8225
	Fixed HelplessAirAccelerationMultiplier { get; }

	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x06002022 RID: 8226
	Fixed JumpSpeed { get; }

	// Token: 0x17000710 RID: 1808
	// (get) Token: 0x06002023 RID: 8227
	Fixed SecondaryJumpSpeed { get; }

	// Token: 0x17000711 RID: 1809
	// (get) Token: 0x06002024 RID: 8228
	Fixed ShortJumpSpeed { get; }

	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06002025 RID: 8229
	int JumpCount { get; }

	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06002026 RID: 8230
	Fixed Weight { get; }

	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06002027 RID: 8231
	Fixed FastFallSpeed { get; }

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06002028 RID: 8232
	Fixed ShieldBreakSpeed { get; }

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x06002029 RID: 8233
	bool IgnorePlatforms { get; }
}
