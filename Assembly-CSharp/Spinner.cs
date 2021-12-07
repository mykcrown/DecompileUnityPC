using System;
using UnityEngine;

// Token: 0x02000B84 RID: 2948
public class Spinner : VFXBehavior
{
	// Token: 0x06005523 RID: 21795 RVA: 0x001B48CF File Offset: 0x001B2CCF
	public override void OnVFXInit()
	{
	}

	// Token: 0x06005524 RID: 21796 RVA: 0x001B48D1 File Offset: 0x001B2CD1
	public override void OnVFXStart()
	{
		this.mTime = 0f;
	}

	// Token: 0x06005525 RID: 21797 RVA: 0x001B48E0 File Offset: 0x001B2CE0
	private void Update()
	{
		this.mTime += WTime.deltaTime;
		float num = this.Speed * WTime.deltaTime;
		if (this.SpeedTimeCurve.keys.Length != 0)
		{
			num *= this.SpeedTimeCurve.Evaluate(this.mTime);
		}
		base.transform.Rotate(this.SpinAxis, num, this.RotateSpace);
	}

	// Token: 0x04003615 RID: 13845
	public Vector3 SpinAxis;

	// Token: 0x04003616 RID: 13846
	public AnimationCurve SpeedTimeCurve;

	// Token: 0x04003617 RID: 13847
	public float Speed;

	// Token: 0x04003618 RID: 13848
	public Space RotateSpace;

	// Token: 0x04003619 RID: 13849
	private float mTime;
}
