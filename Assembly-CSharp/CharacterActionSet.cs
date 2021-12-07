using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020004C1 RID: 1217
[Serializable]
public class CharacterActionSet : IEnumerable
{
	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x000899B7 File Offset: 0x00087DB7
	public CharacterActionData idle
	{
		get
		{
			return this.GetAction(ActionState.Idle, true);
		}
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06001AC6 RID: 6854 RVA: 0x000899C1 File Offset: 0x00087DC1
	public CharacterActionData walkFast
	{
		get
		{
			return this.GetAction(ActionState.WalkFast, true);
		}
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x000899CB File Offset: 0x00087DCB
	public CharacterActionData walkMedium
	{
		get
		{
			return this.GetAction(ActionState.WalkMedium, true);
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06001AC8 RID: 6856 RVA: 0x000899D6 File Offset: 0x00087DD6
	public CharacterActionData walkSlow
	{
		get
		{
			return this.GetAction(ActionState.WalkSlow, true);
		}
	}

	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x000899E1 File Offset: 0x00087DE1
	public CharacterActionData wavedash
	{
		get
		{
			return this.GetAction(ActionState.Wavedash, true);
		}
	}

	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x06001ACA RID: 6858 RVA: 0x000899EC File Offset: 0x00087DEC
	public CharacterActionData run
	{
		get
		{
			return this.GetAction(ActionState.Run, true);
		}
	}

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x06001ACB RID: 6859 RVA: 0x000899F7 File Offset: 0x00087DF7
	public CharacterActionData takeOff
	{
		get
		{
			return this.GetAction(ActionState.TakeOff, true);
		}
	}

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06001ACC RID: 6860 RVA: 0x00089A01 File Offset: 0x00087E01
	public CharacterActionData jumpStraight
	{
		get
		{
			return this.GetAction(ActionState.JumpStraight, true);
		}
	}

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06001ACD RID: 6861 RVA: 0x00089A0B File Offset: 0x00087E0B
	public CharacterActionData jumpBack
	{
		get
		{
			return this.GetAction(ActionState.JumpBack, true);
		}
	}

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06001ACE RID: 6862 RVA: 0x00089A15 File Offset: 0x00087E15
	public CharacterActionData jumpForward
	{
		get
		{
			return this.GetAction(ActionState.JumpForward, true);
		}
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x06001ACF RID: 6863 RVA: 0x00089A1F File Offset: 0x00087E1F
	public CharacterActionData airJump
	{
		get
		{
			return this.GetAction(ActionState.AirJump, true);
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x06001AD0 RID: 6864 RVA: 0x00089A2A File Offset: 0x00087E2A
	public CharacterActionData fallStraight
	{
		get
		{
			return this.GetAction(ActionState.FallStraight, true);
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x00089A34 File Offset: 0x00087E34
	public CharacterActionData fallBack
	{
		get
		{
			return this.GetAction(ActionState.FallBack, true);
		}
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06001AD2 RID: 6866 RVA: 0x00089A3E File Offset: 0x00087E3E
	public CharacterActionData fallForward
	{
		get
		{
			return this.GetAction(ActionState.FallForward, true);
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x00089A49 File Offset: 0x00087E49
	public CharacterActionData fallHelpless
	{
		get
		{
			return this.GetAction(ActionState.FallHelpless, true);
		}
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x00089A54 File Offset: 0x00087E54
	public CharacterActionData landing
	{
		get
		{
			return this.GetAction(ActionState.Landing, true);
		}
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x00089A5F File Offset: 0x00087E5F
	public CharacterActionData crouching
	{
		get
		{
			return this.GetAction(ActionState.Crouching, true);
		}
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x00089A6A File Offset: 0x00087E6A
	public CharacterActionData crouchBegin
	{
		get
		{
			return this.GetAction(ActionState.CrouchBegin, true);
		}
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x00089A75 File Offset: 0x00087E75
	public CharacterActionData crouchEnd
	{
		get
		{
			return this.GetAction(ActionState.CrouchEnd, true);
		}
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x00089A80 File Offset: 0x00087E80
	public CharacterActionData platformDrop
	{
		get
		{
			return this.GetAction(ActionState.PlatformDrop, true);
		}
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x00089A8B File Offset: 0x00087E8B
	public CharacterActionData shieldLoop
	{
		get
		{
			return this.GetAction(ActionState.ShieldLoop, true);
		}
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06001ADA RID: 6874 RVA: 0x00089A96 File Offset: 0x00087E96
	public CharacterActionData shieldBegin
	{
		get
		{
			return this.GetAction(ActionState.ShieldBegin, true);
		}
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06001ADB RID: 6875 RVA: 0x00089AA1 File Offset: 0x00087EA1
	public CharacterActionData shieldEnd
	{
		get
		{
			return this.GetAction(ActionState.ShieldEnd, true);
		}
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x06001ADC RID: 6876 RVA: 0x00089AAC File Offset: 0x00087EAC
	public CharacterActionData fallDown
	{
		get
		{
			return this.GetAction(ActionState.FallDown, true);
		}
	}

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06001ADD RID: 6877 RVA: 0x00089AB7 File Offset: 0x00087EB7
	public CharacterActionData downedLoop
	{
		get
		{
			return this.GetAction(ActionState.DownedLoop, true);
		}
	}

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x06001ADE RID: 6878 RVA: 0x00089AC2 File Offset: 0x00087EC2
	public CharacterActionData recoil
	{
		get
		{
			return this.GetAction(ActionState.Recoil, true);
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x06001ADF RID: 6879 RVA: 0x00089ACD File Offset: 0x00087ECD
	public CharacterActionData death
	{
		get
		{
			return this.GetAction(ActionState.Death, true);
		}
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x06001AE0 RID: 6880 RVA: 0x00089AD8 File Offset: 0x00087ED8
	public CharacterActionData tumble
	{
		get
		{
			return this.GetAction(ActionState.Tumble, true);
		}
	}

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x00089AE3 File Offset: 0x00087EE3
	public CharacterActionData dazedBegin
	{
		get
		{
			return this.GetAction(ActionState.DazedBegin, true);
		}
	}

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x00089AEE File Offset: 0x00087EEE
	public CharacterActionData dazedLoop
	{
		get
		{
			return this.GetAction(ActionState.DazedLoop, true);
		}
	}

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x00089AF9 File Offset: 0x00087EF9
	public CharacterActionData dazedEnd
	{
		get
		{
			return this.GetAction(ActionState.DazedEnd, true);
		}
	}

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x00089B04 File Offset: 0x00087F04
	public CharacterActionData edgeGrab
	{
		get
		{
			return this.GetAction(ActionState.EdgeGrab, true);
		}
	}

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x00089B0F File Offset: 0x00087F0F
	public CharacterActionData edgeHang
	{
		get
		{
			return this.GetAction(ActionState.EdgeHang, true);
		}
	}

	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x00089B1A File Offset: 0x00087F1A
	public CharacterActionData teeterLoop
	{
		get
		{
			return this.GetAction(ActionState.TeeterLoop, true);
		}
	}

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x00089B25 File Offset: 0x00087F25
	public CharacterActionData teeterBegin
	{
		get
		{
			return this.GetAction(ActionState.TeeterBegin, true);
		}
	}

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x00089B30 File Offset: 0x00087F30
	public CharacterActionData grabbing
	{
		get
		{
			return this.GetAction(ActionState.Grabbing, true);
		}
	}

	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x00089B3B File Offset: 0x00087F3B
	public CharacterActionData grabbedBegin
	{
		get
		{
			return this.GetAction(ActionState.GrabbedBegin, true);
		}
	}

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06001AEA RID: 6890 RVA: 0x00089B46 File Offset: 0x00087F46
	public CharacterActionData grabbedLoop
	{
		get
		{
			return this.GetAction(ActionState.GrabbedLoop, true);
		}
	}

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06001AEB RID: 6891 RVA: 0x00089B51 File Offset: 0x00087F51
	public CharacterActionData grabbedPummelled
	{
		get
		{
			return this.GetAction(ActionState.GrabbedPummelled, true);
		}
	}

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06001AEC RID: 6892 RVA: 0x00089B5C File Offset: 0x00087F5C
	public CharacterActionData grabEscapeGround
	{
		get
		{
			return this.GetAction(ActionState.GrabEscapeGround, true);
		}
	}

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x06001AED RID: 6893 RVA: 0x00089B67 File Offset: 0x00087F67
	public CharacterActionData grabEscapeAir
	{
		get
		{
			return this.GetAction(ActionState.GrabEscapeAir, true);
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x06001AEE RID: 6894 RVA: 0x00089B72 File Offset: 0x00087F72
	public CharacterActionData grabRelease
	{
		get
		{
			return this.GetAction(ActionState.GrabRelease, true);
		}
	}

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06001AEF RID: 6895 RVA: 0x00089B7D File Offset: 0x00087F7D
	public CharacterActionData thrown
	{
		get
		{
			return this.GetAction(ActionState.Thrown, true);
		}
	}

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x00089B88 File Offset: 0x00087F88
	public CharacterActionData dash
	{
		get
		{
			return this.GetAction(ActionState.Dash, true);
		}
	}

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x00089B93 File Offset: 0x00087F93
	public CharacterActionData dashPivot
	{
		get
		{
			return this.GetAction(ActionState.DashPivot, true);
		}
	}

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x00089B9E File Offset: 0x00087F9E
	public CharacterActionData dashBrake
	{
		get
		{
			return this.GetAction(ActionState.DashBrake, true);
		}
	}

	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00089BA9 File Offset: 0x00087FA9
	public CharacterActionData brake
	{
		get
		{
			return this.GetAction(ActionState.Brake, true);
		}
	}

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x00089BB4 File Offset: 0x00087FB4
	public CharacterActionData runPivot
	{
		get
		{
			return this.GetAction(ActionState.RunPivot, true);
		}
	}

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x00089BBF File Offset: 0x00087FBF
	public CharacterActionData runPivotBrake
	{
		get
		{
			return this.GetAction(ActionState.RunPivotBrake, true);
		}
	}

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x00089BCA File Offset: 0x00087FCA
	public CharacterActionData pivot
	{
		get
		{
			return this.GetAction(ActionState.Pivot, true);
		}
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x00089BD5 File Offset: 0x00087FD5
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.characterActions.GetEnumerator();
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x00089BE7 File Offset: 0x00087FE7
	public static bool isOptionalState(ActionState state)
	{
		return state == ActionState.JumpForward || state == ActionState.JumpBack;
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x00089BF8 File Offset: 0x00087FF8
	public CharacterActionData GetAction(ActionState characterActionState, bool createIfNull = false)
	{
		if (this.lookup.ContainsKey(characterActionState))
		{
			return this.lookup[characterActionState];
		}
		foreach (CharacterActionData characterActionData in this.characterActions)
		{
			if (characterActionData.characterActionState == characterActionState)
			{
				this.lookup[characterActionState] = characterActionData;
				return characterActionData;
			}
		}
		if (createIfNull)
		{
			CharacterActionData characterActionData2 = new CharacterActionData(characterActionState, characterActionState.ToString());
			this.characterActions.Add(characterActionData2);
			this.lookup[characterActionState] = characterActionData2;
		}
		else
		{
			this.lookup[characterActionState] = null;
		}
		return this.lookup[characterActionState];
	}

	// Token: 0x04001445 RID: 5189
	private Dictionary<ActionState, CharacterActionData> lookup = new Dictionary<ActionState, CharacterActionData>(default(ActionStateComparer));

	// Token: 0x04001446 RID: 5190
	public List<CharacterActionData> characterActions = new List<CharacterActionData>();
}
