using System;
using UnityEngine;

// Token: 0x02000293 RID: 659
public class stageParticleManager : MonoBehaviour
{
	// Token: 0x06000DD2 RID: 3538 RVA: 0x000577E8 File Offset: 0x00055BE8
	public void start_ps()
	{
		if (this.ps != null)
		{
			this.ps.playOnAwake = true;
			this.ps.enableEmission = true;
			this.ps.Play();
			if (this.debug)
			{
				Debug.Log("started " + this.ps.name);
			}
		}
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x00057850 File Offset: 0x00055C50
	public void stop_ps()
	{
		if (this.ps != null)
		{
			this.ps.playOnAwake = false;
			this.ps.enableEmission = false;
			this.ps.Stop();
			if (this.debug)
			{
				Debug.Log("stopped " + this.ps.name);
			}
		}
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x000578B6 File Offset: 0x00055CB6
	private void Start()
	{
		if (this.ps == null)
		{
			this.ps_obj = UnityEngine.Object.Instantiate<GameObject>(this.ps_obj);
			this.ps = this.ps_obj.GetComponentInChildren<ParticleSystem>();
		}
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x000578EB File Offset: 0x00055CEB
	private void Update()
	{
	}

	// Token: 0x040007F3 RID: 2035
	public GameObject ps_obj;

	// Token: 0x040007F4 RID: 2036
	private ParticleSystem ps;

	// Token: 0x040007F5 RID: 2037
	public bool debug;
}
