using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class LandingButtons : MonoBehaviour
{
	// Token: 0x06000043 RID: 67 RVA: 0x00004378 File Offset: 0x00002778
	public void OnGUI()
	{
		GUI.Label(new Rect(20f, 20f, 125f, 18f), "Landing Spots: " + this._landingSpotController.transform.childCount);
		if (GUI.Button(new Rect(20f, 40f, 125f, 18f), "Scare All"))
		{
			this._landingSpotController.ScareAll();
		}
		if (GUI.Button(new Rect(20f, 60f, 125f, 18f), "Land In Reach"))
		{
			this._landingSpotController.LandAll();
		}
		if (GUI.Button(new Rect(20f, 80f, 125f, 18f), "Land Instant"))
		{
			base.StartCoroutine(this._landingSpotController.InstantLand(0.01f));
		}
		if (GUI.Button(new Rect(20f, 100f, 125f, 18f), "Destroy"))
		{
			this._flockController.destroyBirds();
		}
		GUI.Label(new Rect(20f, 120f, 125f, 18f), "Bird Amount: " + this._flockController._childAmount);
		this._flockController._childAmount = (int)GUI.HorizontalSlider(new Rect(20f, 140f, 125f, 18f), (float)this._flockController._childAmount, 0f, 250f);
	}

	// Token: 0x04000081 RID: 129
	public LandingSpotController _landingSpotController;

	// Token: 0x04000082 RID: 130
	public FlockController _flockController;

	// Token: 0x04000083 RID: 131
	public float hSliderValue = 250f;
}
