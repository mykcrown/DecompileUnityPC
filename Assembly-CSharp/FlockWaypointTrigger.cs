using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class FlockWaypointTrigger : MonoBehaviour
{
	// Token: 0x06000040 RID: 64 RVA: 0x000042F8 File Offset: 0x000026F8
	public void Start()
	{
		if (this._flockChild == null)
		{
			this._flockChild = base.transform.parent.GetComponent<FlockChild>();
		}
		float num = UnityEngine.Random.Range(this._timer, this._timer * 3f);
		base.InvokeRepeating("Trigger", num, num);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00004351 File Offset: 0x00002751
	public void Trigger()
	{
		this._flockChild.Wander(0f);
	}

	// Token: 0x0400007F RID: 127
	public float _timer = 1f;

	// Token: 0x04000080 RID: 128
	public FlockChild _flockChild;
}
