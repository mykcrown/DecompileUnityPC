// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
	private class Droplet
	{
		private Vector2 position;

		private float time;

		public Droplet()
		{
			this.time = 1000f;
		}

		public void Reset()
		{
			this.position = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value);
			this.time = 0f;
		}

		public void Update()
		{
			this.time += Time.deltaTime;
		}

		public Vector4 MakeShaderParameter(float aspect)
		{
			return new Vector4(this.position.x * aspect, this.position.y, this.time, 0f);
		}
	}

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

	[Range(0.01f, 1f)]
	public float refractionStrength = 0.5f;

	public Color reflectionColor = Color.gray;

	[Range(0.01f, 1f)]
	public float reflectionStrength = 0.7f;

	[Range(1f, 3f)]
	public float waveSpeed = 1.25f;

	[Range(0f, 2f)]
	public float dropInterval = 0.5f;

	[SerializeField]
	private Shader shader;

	private RippleEffect.Droplet[] droplets;

	private Texture2D gradTexture;

	private Material material;

	private float timer;

	private int dropCount;

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
		RippleEffect.Droplet[] array = this.droplets;
		for (int i = 0; i < array.Length; i++)
		{
			RippleEffect.Droplet droplet = array[i];
			droplet.Update();
		}
		this.UpdateShaderParameters();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, this.material);
	}

	public void Emit()
	{
		this.droplets[this.dropCount++ % this.droplets.Length].Reset();
	}
}
