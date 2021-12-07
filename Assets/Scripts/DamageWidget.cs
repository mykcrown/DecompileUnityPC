// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine.UI;

public class DamageWidget : TrainingWidget
{
	public Text damageText;

	public PlayerNum PlayerNumber;

	private Fixed damage = 0;

	public override void OnLeft()
	{
		this.UpdateDamage(-1);
	}

	public override void OnRight()
	{
		this.UpdateDamage(1);
	}

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
}
