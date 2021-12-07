using System;
using TMPro;
using UnityEngine;

// Token: 0x02000772 RID: 1906
public class PlayerTokenDisplay : BaseItem3DPreviewDisplay
{
	// Token: 0x06002F32 RID: 12082 RVA: 0x000ECE24 File Offset: 0x000EB224
	private void Awake()
	{
		this.baseScale = this.root.transform.localScale;
	}

	// Token: 0x06002F33 RID: 12083 RVA: 0x000ECE3C File Offset: 0x000EB23C
	public void SetCustom(Texture2D texture)
	{
		this.theRenderer.material.SetTexture("_MainTex", texture);
		this.text.gameObject.SetActive(false);
	}

	// Token: 0x06002F34 RID: 12084 RVA: 0x000ECE65 File Offset: 0x000EB265
	public void SetDefault()
	{
		this.text.gameObject.SetActive(true);
		this.text.text = "P1";
		this.text.color = PlayerUtil.GetColorFromUIColor(UIColor.Red);
	}

	// Token: 0x06002F35 RID: 12085 RVA: 0x000ECE9C File Offset: 0x000EB29C
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

	// Token: 0x06002F36 RID: 12086 RVA: 0x000ECF2F File Offset: 0x000EB32F
	private void punch()
	{
		this.punchTimer = this.punchDuration;
	}

	// Token: 0x040020F2 RID: 8434
	public Renderer theRenderer;

	// Token: 0x040020F3 RID: 8435
	public TextMeshPro text;

	// Token: 0x040020F4 RID: 8436
	public Transform root;

	// Token: 0x040020F5 RID: 8437
	public float punchSize = 0.5f;

	// Token: 0x040020F6 RID: 8438
	public float punchDuration = 0.5f;

	// Token: 0x040020F7 RID: 8439
	private float punchTimer;

	// Token: 0x040020F8 RID: 8440
	private Vector3 baseScale = Vector3.zero;
}
