// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class stageParticleManager : MonoBehaviour
{
	public GameObject ps_obj;

	private ParticleSystem ps;

	public bool debug;

	public void start_ps()
	{
		if (this.ps != null)
		{
			this.ps.playOnAwake = true;
			this.ps.enableEmission = true;
			this.ps.Play();
			if (this.debug)
			{
				UnityEngine.Debug.Log("started " + this.ps.name);
			}
		}
	}

	public void stop_ps()
	{
		if (this.ps != null)
		{
			this.ps.playOnAwake = false;
			this.ps.enableEmission = false;
			this.ps.Stop();
			if (this.debug)
			{
				UnityEngine.Debug.Log("stopped " + this.ps.name);
			}
		}
	}

	private void Start()
	{
		if (this.ps == null)
		{
			this.ps_obj = UnityEngine.Object.Instantiate<GameObject>(this.ps_obj);
			this.ps = this.ps_obj.GetComponentInChildren<ParticleSystem>();
		}
	}

	private void Update()
	{
	}
}
