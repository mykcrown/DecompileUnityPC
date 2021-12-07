using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class LandingSpotController : MonoBehaviour
{
	// Token: 0x0600004E RID: 78 RVA: 0x0000557C File Offset: 0x0000397C
	public void Start()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._flock == null)
		{
			this._flock = (FlockController)UnityEngine.Object.FindObjectOfType(typeof(FlockController));
			Debug.Log(this + " has no assigned FlockController, a random FlockController has been assigned");
		}
		if (this._landOnStart)
		{
			base.StartCoroutine(this.InstantLandOnStart(0.1f));
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000055FE File Offset: 0x000039FE
	public void ScareAll()
	{
		this.ScareAll(0f, 1f);
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00005610 File Offset: 0x00003A10
	public void ScareAll(float minDelay, float maxDelay)
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				LandingSpot component = this._thisT.GetChild(i).GetComponent<LandingSpot>();
				component.Invoke("ReleaseFlockChild", UnityEngine.Random.Range(minDelay, maxDelay));
			}
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x0000567C File Offset: 0x00003A7C
	public void LandAll()
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				LandingSpot component = this._thisT.GetChild(i).GetComponent<LandingSpot>();
				base.StartCoroutine(component.GetFlockChild(0f, 2f));
			}
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000056EC File Offset: 0x00003AEC
	public IEnumerator InstantLandOnStart(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				LandingSpot component = this._thisT.GetChild(i).GetComponent<LandingSpot>();
				component.InstantLand();
			}
		}
		yield break;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00005710 File Offset: 0x00003B10
	public IEnumerator InstantLand(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				LandingSpot component = this._thisT.GetChild(i).GetComponent<LandingSpot>();
				component.InstantLand();
			}
		}
		yield break;
	}

	// Token: 0x0400008B RID: 139
	public bool _randomRotate = true;

	// Token: 0x0400008C RID: 140
	public Vector2 _autoCatchDelay = new Vector2(10f, 20f);

	// Token: 0x0400008D RID: 141
	public Vector2 _autoDismountDelay = new Vector2(10f, 20f);

	// Token: 0x0400008E RID: 142
	public float _maxBirdDistance = 20f;

	// Token: 0x0400008F RID: 143
	public float _minBirdDistance = 5f;

	// Token: 0x04000090 RID: 144
	public bool _takeClosest;

	// Token: 0x04000091 RID: 145
	public FlockController _flock;

	// Token: 0x04000092 RID: 146
	public bool _landOnStart;

	// Token: 0x04000093 RID: 147
	public bool _soarLand = true;

	// Token: 0x04000094 RID: 148
	public bool _onlyBirdsAbove;

	// Token: 0x04000095 RID: 149
	public float _landingSpeedModifier = 0.5f;

	// Token: 0x04000096 RID: 150
	public float _landingTurnSpeedModifier = 5f;

	// Token: 0x04000097 RID: 151
	public Transform _featherPS;

	// Token: 0x04000098 RID: 152
	public Transform _thisT;

	// Token: 0x04000099 RID: 153
	public int _activeLandingSpots;

	// Token: 0x0400009A RID: 154
	public float _snapLandDistance = 0.1f;

	// Token: 0x0400009B RID: 155
	public float _landedRotateSpeed = 0.01f;

	// Token: 0x0400009C RID: 156
	public float _gizmoSize = 0.2f;
}
