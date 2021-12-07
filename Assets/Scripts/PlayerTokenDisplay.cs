// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class PlayerTokenDisplay : BaseItem3DPreviewDisplay
{
	public Renderer theRenderer;

	public TextMeshPro text;

	public Transform root;

	public float punchSize = 0.5f;

	public float punchDuration = 0.5f;

	private float punchTimer;

	private Vector3 baseScale = Vector3.zero;

	private void Awake()
	{
		this.baseScale = this.root.transform.localScale;
	}

	public void SetCustom(Texture2D texture)
	{
		this.theRenderer.material.SetTexture("_MainTex", texture);
		this.text.gameObject.SetActive(false);
	}

	public void SetDefault()
	{
		this.text.gameObject.SetActive(true);
		this.text.text = "P1";
		this.text.color = PlayerUtil.GetColorFromUIColor(UIColor.Red);
	}

	protected override void Update()
	{
		base.Update();
		if (this.punchTimer > 0f)
		{
			this.punchTimer -= Time.deltaTime;
			this.punchTimer = Mathf.Max(this.punchTimer, 0f);
			float num = this.punchTimer / this.punchDuration;
			float d = -4f * this.punchSize * num * (num - 1f);
			Vector3 localScale = this.baseScale + Vector3.one * d;
			this.root.localScale = localScale;
		}
	}

	private void punch()
	{
		this.punchTimer = this.punchDuration;
	}
}
