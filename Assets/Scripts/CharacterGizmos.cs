// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CharacterGizmos : MonoBehaviour
{
	public CharacterMenusData Data;

	public HorizontalDirection Facing;

	private void Awake()
	{
	}

	public void OnDrawGizmos()
	{
		if (this.Data != null)
		{
			this.Data.bounds.DrawGizmos(base.transform.position, this.Facing);
		}
	}
}
