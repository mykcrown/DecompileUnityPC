// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class TEMP_VFXTransparencyProximity : VFXBehavior
{
	[Range(0f, 1f)]
	public float maxAlpha = 1f;

	[Range(0f, 1f)]
	public float minAlpha;

	public float maxPlayerDistanceDetection;

	public float minPlayerDistanceDetection;

	private Renderer[] renderers;

	private GameManager gameManager;

	private const float heightAboveFloor = 0.68f;

	public override void OnVFXInit()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>();
		GameObject gameObject = GameObject.FindGameObjectWithTag(Tags.GameManager);
		if (gameObject != null)
		{
			this.gameManager = gameObject.GetComponent<GameManager>();
		}
	}

	public override void OnVFXStart()
	{
	}

	private void Update()
	{
		float num = 3.40282347E+38f;
		Vector3 b = base.transform.position - Vector3.up * 0.68f;
		foreach (PlayerReference current in this.gameManager.PlayerReferences)
		{
			PlayerController playerController = this.gameManager.GetPlayerController(current.PlayerNum);
			Vector3 vector = (Vector3)playerController.Position - b;
			if (vector.sqrMagnitude < num * num)
			{
				num = vector.magnitude;
			}
		}
		float num2 = Mathf.Max(num - this.minPlayerDistanceDetection, 0f) / Mathf.Max(this.maxPlayerDistanceDetection - this.minPlayerDistanceDetection, 0.0001f);
		num2 = Mathf.Min(num2, 1f);
		float a = num2 * (this.maxAlpha - this.minAlpha) + this.minAlpha;
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			Material[] materials = renderer.materials;
			Material[] array2 = materials;
			for (int j = 0; j < array2.Length; j++)
			{
				Material material = array2[j];
				material.SetColor("_Color", new Color(1f, 1f, 1f, a));
			}
		}
	}
}
