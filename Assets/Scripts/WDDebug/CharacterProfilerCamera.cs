// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace WDDebug
{
	public class CharacterProfilerCamera : MonoBehaviour
	{
		public float distance = 5f;

		public float smoothFactor = 0.5f;

		private GameObject focus;

		private Renderer[] renderers;

		private Bounds bounds;

		private Vector3 velocity;

		public GameObject Focus
		{
			get
			{
				return this.focus;
			}
			set
			{
				this.focus = value;
				if (this.focus == null)
				{
					this.renderers = new Renderer[0];
				}
				else
				{
					this.renderers = this.focus.GetComponentsInChildren<Renderer>();
				}
			}
		}

		private void Update()
		{
			if (this.focus == null)
			{
				return;
			}
			this.bounds.center = Vector3.zero;
			this.bounds.size = Vector3.zero;
			Renderer[] array = this.renderers;
			for (int i = 0; i < array.Length; i++)
			{
				Renderer renderer = array[i];
				if (renderer != null)
				{
					this.bounds.Encapsulate(renderer.bounds);
				}
			}
			Vector3 target = new Vector3(this.bounds.center.x, this.bounds.center.y, -this.distance);
			Vector3 vector = base.transform.position;
			vector = Vector3.SmoothDamp(vector, target, ref this.velocity, this.smoothFactor * Time.deltaTime);
			base.transform.position = vector;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(this.bounds.center, 0.25f);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(new Vector3(base.transform.position.x, base.transform.position.y, 0f), 0.3f);
		}
	}
}
