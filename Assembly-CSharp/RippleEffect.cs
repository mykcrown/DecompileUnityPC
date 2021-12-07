using System;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class RippleEffect : MonoBehaviour
{
	// Token: 0x06001706 RID: 5894 RVA: 0x0007CA90 File Offset: 0x0007AE90
	private void UpdateShaderParameters()
	{
		Camera component = base.GetComponent<Camera>();
		this.material.SetVector("_Drop1", this.droplets[0].MakeShaderParameter(component.aspect));
		this.material.SetVector("_Drop2", this.droplets[1].MakeShaderParameter(component.aspect));
		this.material.SetVector("_Drop3", this.droplets[2].MakeShaderParameter(component.aspect));
		this.material.SetColor("_Reflection", this.reflectionColor);
		this.material.SetVector("_Params1", new Vector4(component.aspect, 1f, 1f / this.waveSpeed, 0f));
		this.material.SetVector("_Params2", new Vector4(1f, 1f / component.aspect, this.refractionStrength, this.reflectionStrength));
	}

	// Token: 0x06001707 RID: 5895 RVA: 0x0007CB88 File Offset: 0x0007AF88
	private void Awake()
	{
		this.droplets = new RippleEffect.Droplet[3];
		this.droplets[0] = new RippleEffect.Droplet();
		this.droplets[1] = new RippleEffect.Droplet();
		this.droplets[2] = new RippleEffect.Droplet();
		this.gradTexture = new Texture2D(2048, 1, TextureFormat.Alpha8, false);
		this.gradTexture.wrapMode = TextureWrapMode.Clamp;
		this.gradTexture.filterMode = FilterMode.Bilinear;
		for (int i = 0; i < this.gradTexture.width; i++)
		{
			float time = 1f / (float)this.gradTexture.width * (float)i;
			float num = this.waveform.Evaluate(time);
			this.gradTexture.SetPixel(i, 0, new Color(num, num, num, num));
		}
		this.gradTexture.Apply();
		this.material = new Material(this.shader);
		this.material.hideFlags = HideFlags.DontSave;
		this.material.SetTexture("_GradTex", this.gradTexture);
		this.UpdateShaderParameters();
	}

	// Token: 0x06001708 RID: 5896 RVA: 0x0007CC90 File Offset: 0x0007B090
	private void Update()
	{
		if (this.dropInterval > 0f)
		{
			this.timer += Time.deltaTime;
			while (this.timer > this.dropInterval)
			{
				this.Emit();
				this.timer -= this.dropInterval;
			}
		}
		foreach (RippleEffect.Droplet droplet in this.droplets)
		{
			droplet.Update();
		}
		this.UpdateShaderParameters();
	}

	// Token: 0x06001709 RID: 5897 RVA: 0x0007CD19 File Offset: 0x0007B119
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x0007CD28 File Offset: 0x0007B128
	public void Emit()
	{
		this.droplets[this.dropCount++ % this.droplets.Length].Reset();
	}

	// Token: 0x040011CE RID: 4558
	public AnimationCurve waveform = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0.5f, 0f, 0f),
		new Keyframe(0.05f, 1f, 0f, 0f),
		new Keyframe(0.15f, 0.1f, 0f, 0f),
		new Keyframe(0.25f, 0.8f, 0f, 0f),
		new Keyframe(0.35f, 0.3f, 0f, 0f),
		new Keyframe(0.45f, 0.6f, 0f, 0f),
		new Keyframe(0.55f, 0.4f, 0f, 0f),
		new Keyframe(0.65f, 0.55f, 0f, 0f),
		new Keyframe(0.75f, 0.46f, 0f, 0f),
		new Keyframe(0.85f, 0.52f, 0f, 0f),
		new Keyframe(0.99f, 0.5f, 0f, 0f)
	});

	// Token: 0x040011CF RID: 4559
	[Range(0.01f, 1f)]
	public float refractionStrength = 0.5f;

	// Token: 0x040011D0 RID: 4560
	public Color reflectionColor = Color.gray;

	// Token: 0x040011D1 RID: 4561
	[Range(0.01f, 1f)]
	public float reflectionStrength = 0.7f;

	// Token: 0x040011D2 RID: 4562
	[Range(1f, 3f)]
	public float waveSpeed = 1.25f;

	// Token: 0x040011D3 RID: 4563
	[Range(0f, 2f)]
	public float dropInterval = 0.5f;

	// Token: 0x040011D4 RID: 4564
	[SerializeField]
	private Shader shader;

	// Token: 0x040011D5 RID: 4565
	private RippleEffect.Droplet[] droplets;

	// Token: 0x040011D6 RID: 4566
	private Texture2D gradTexture;

	// Token: 0x040011D7 RID: 4567
	private Material material;

	// Token: 0x040011D8 RID: 4568
	private float timer;

	// Token: 0x040011D9 RID: 4569
	private int dropCount;

	// Token: 0x0200045B RID: 1115
	private class Droplet
	{
		// Token: 0x0600170B RID: 5899 RVA: 0x0007CD5B File Offset: 0x0007B15B
		public Droplet()
		{
			this.time = 1000f;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x0007CD6E File Offset: 0x0007B16E
		public void Reset()
		{
			this.position = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value);
			this.time = 0f;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x0007CD90 File Offset: 0x0007B190
		public void Update()
		{
			this.time += Time.deltaTime;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0007CDA4 File Offset: 0x0007B1A4
		public Vector4 MakeShaderParameter(float aspect)
		{
			return new Vector4(this.position.x * aspect, this.position.y, this.time, 0f);
		}

		// Token: 0x040011DA RID: 4570
		private Vector2 position;

		// Token: 0x040011DB RID: 4571
		private float time;
	}
}
