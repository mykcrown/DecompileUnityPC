using System;
using FixedPoint;
using UnityEngine.UI;

// Token: 0x02000A3A RID: 2618
public class DamageWidget : TrainingWidget
{
	// Token: 0x06004CB0 RID: 19632 RVA: 0x001451E0 File Offset: 0x001435E0
	public override void OnLeft()
	{
		this.UpdateDamage(-1);
	}

	// Token: 0x06004CB1 RID: 19633 RVA: 0x001451EE File Offset: 0x001435EE
	public override void OnRight()
	{
		this.UpdateDamage(1);
	}

	// Token: 0x06004CB2 RID: 19634 RVA: 0x001451FC File Offset: 0x001435FC
	public void UpdateDamage(Fixed amount)
	{
		try
		{
			base.events.Broadcast(new UpdateDamageCommand(this.damage + amount, this.PlayerNumber));
		}
		catch (ArgumentException)
		{
			return;
		}
		this.damage += amount;
		this.damageText.text = (int)this.damage + "%";
	}

	// Token: 0x04003251 RID: 12881
	public Text damageText;

	// Token: 0x04003252 RID: 12882
	public PlayerNum PlayerNumber;

	// Token: 0x04003253 RID: 12883
	private Fixed damage = 0;
}
