using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class FlockScare : MonoBehaviour
{
	// Token: 0x06000039 RID: 57 RVA: 0x0000410C File Offset: 0x0000250C
	private void CheckProximityToLandingSpots()
	{
		this.IterateLandingSpots();
		if (this.currentController._activeLandingSpots > 0 && this.CheckDistanceToLandingSpot(this.landingSpotControllers[this.lsc]))
		{
			this.landingSpotControllers[this.lsc].ScareAll();
		}
		base.Invoke("CheckProximityToLandingSpots", this.scareInterval);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x0000416C File Offset: 0x0000256C
	private void IterateLandingSpots()
	{
		this.ls += this.checkEveryNthLandingSpot;
		this.currentController = this.landingSpotControllers[this.lsc];
		int childCount = this.currentController.transform.childCount;
		if (this.ls > childCount - 1)
		{
			this.ls -= childCount;
			if (this.lsc < this.landingSpotControllers.Length - 1)
			{
				this.lsc++;
			}
			else
			{
				this.lsc = 0;
			}
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000041FC File Offset: 0x000025FC
	private bool CheckDistanceToLandingSpot(LandingSpotController lc)
	{
		Transform transform = lc.transform;
		Transform child = transform.GetChild(this.ls);
		LandingSpot component = child.GetComponent<LandingSpot>();
		if (component.landingChild != null)
		{
			float sqrMagnitude = (child.position - base.transform.position).sqrMagnitude;
			if (sqrMagnitude < this.distanceToScare * this.distanceToScare)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x0000426C File Offset: 0x0000266C
	private void Invoker()
	{
		for (int i = 0; i < this.InvokeAmounts; i++)
		{
			float num = this.scareInterval / (float)this.InvokeAmounts * (float)i;
			base.Invoke("CheckProximityToLandingSpots", this.scareInterval + num);
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000042B5 File Offset: 0x000026B5
	private void OnEnable()
	{
		base.CancelInvoke("CheckProximityToLandingSpots");
		if (this.landingSpotControllers.Length > 0)
		{
			this.Invoker();
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000042D6 File Offset: 0x000026D6
	private void OnDisable()
	{
		base.CancelInvoke("CheckProximityToLandingSpots");
	}

	// Token: 0x04000077 RID: 119
	public LandingSpotController[] landingSpotControllers;

	// Token: 0x04000078 RID: 120
	public float scareInterval = 0.1f;

	// Token: 0x04000079 RID: 121
	public float distanceToScare = 2f;

	// Token: 0x0400007A RID: 122
	public int checkEveryNthLandingSpot = 1;

	// Token: 0x0400007B RID: 123
	public int InvokeAmounts = 1;

	// Token: 0x0400007C RID: 124
	private int lsc;

	// Token: 0x0400007D RID: 125
	private int ls;

	// Token: 0x0400007E RID: 126
	private LandingSpotController currentController;
}
