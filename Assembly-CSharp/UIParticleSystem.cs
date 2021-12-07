using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A65 RID: 2661
[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer), typeof(ParticleSystem))]
public class UIParticleSystem : MaskableGraphic
{
	// Token: 0x17001243 RID: 4675
	// (get) Token: 0x06004D25 RID: 19749 RVA: 0x00145AC9 File Offset: 0x00143EC9
	// (set) Token: 0x06004D26 RID: 19750 RVA: 0x00145AD1 File Offset: 0x00143ED1
	[Inject]
	public IUserVideoSettingsModel videoSettings { get; set; }

	// Token: 0x17001244 RID: 4676
	// (get) Token: 0x06004D27 RID: 19751 RVA: 0x00145ADA File Offset: 0x00143EDA
	// (set) Token: 0x06004D28 RID: 19752 RVA: 0x00145AE2 File Offset: 0x00143EE2
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001245 RID: 4677
	// (get) Token: 0x06004D29 RID: 19753 RVA: 0x00145AEB File Offset: 0x00143EEB
	public override Texture mainTexture
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17001246 RID: 4678
	// (get) Token: 0x06004D2A RID: 19754 RVA: 0x00145AEE File Offset: 0x00143EEE
	// (set) Token: 0x06004D2B RID: 19755 RVA: 0x00145AF6 File Offset: 0x00143EF6
	public bool IsPlaying { get; private set; }

	// Token: 0x06004D2C RID: 19756 RVA: 0x00145B00 File Offset: 0x00143F00
	private void initialize()
	{
		this.raycastTarget = false;
		this._transform = base.transform;
		this.pSystem = base.GetComponent<ParticleSystem>();
		this.parentUIPSystem = ((!(base.transform.parent != null)) ? null : base.transform.parent.GetComponent<UIParticleSystem>());
		this.mainModule = this.pSystem.main;
		this.mainModule.maxParticles = Math.Min(UIParticleSystem.MAX_UI_PARTICLES, this.mainModule.maxParticles);
		if (this.particles == null || this.particles.Length != this.mainModule.maxParticles)
		{
			this.particles = new ParticleSystem.Particle[this.mainModule.maxParticles];
		}
		ParticleSystemRenderer component = base.GetComponent<ParticleSystemRenderer>();
		if (component != null)
		{
			component.enabled = !Application.isPlaying;
			this.material = component.sharedMaterial;
		}
		this.textureSheetAnimation = this.pSystem.textureSheetAnimation;
		this.textureSheetAnimationFrames = 0;
		this.textureSheetAnimationFrameSize = Vector2.zero;
		if (this.textureSheetAnimation.enabled)
		{
			this.textureSheetAnimationFrames = this.textureSheetAnimation.numTilesX * this.textureSheetAnimation.numTilesY;
			this.textureSheetAnimationFrameSize = new Vector2(1f / (float)this.textureSheetAnimation.numTilesX, 1f / (float)this.textureSheetAnimation.numTilesY);
		}
	}

	// Token: 0x06004D2D RID: 19757 RVA: 0x00145C74 File Offset: 0x00144074
	protected override void Awake()
	{
		base.Awake();
		if (Application.isPlaying)
		{
			StaticInject.Inject(this);
			this.onUpdatedVideoSettings();
			this.signalBus.AddListener(UserVideoSettingsModel.UPDATED, new Action(this.onUpdatedVideoSettings));
		}
		this.initialize();
		this.IsPlaying = (!Application.isPlaying || this.mainModule.playOnAwake);
	}

	// Token: 0x06004D2E RID: 19758 RVA: 0x00145CE0 File Offset: 0x001440E0
	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			this.initialize();
		}
		else
		{
			bool flag = this.pSystem != null && this.IsPlaying;
			bool flag2 = this.parentUIPSystem == null || this.parentUIPSystem.IsPlaying;
			if (flag && flag2)
			{
				this.pSystem.Simulate(Time.unscaledDeltaTime, false, false, true);
			}
		}
		this.SetAllDirty();
	}

	// Token: 0x06004D2F RID: 19759 RVA: 0x00145D62 File Offset: 0x00144162
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Application.isPlaying)
		{
			this.signalBus.RemoveListener(UserVideoSettingsModel.UPDATED, new Action(this.onUpdatedVideoSettings));
		}
	}

	// Token: 0x06004D30 RID: 19760 RVA: 0x00145D90 File Offset: 0x00144190
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		if (this.pSystem == null)
		{
			return;
		}
		vh.Clear();
		int num = this.pSystem.GetParticles(this.particles);
		for (int i = 0; i < num; i++)
		{
			ParticleSystem.Particle particle = this.particles[i];
			Vector2 a = (this.mainModule.simulationSpace != ParticleSystemSimulationSpace.Local) ? this._transform.InverseTransformPoint(particle.position) : particle.position;
			if (this.mainModule.scalingMode == ParticleSystemScalingMode.Shape)
			{
				a /= base.canvas.scaleFactor;
			}
			Vector4 vector = new Vector4(0f, 0f, 1f, 1f);
			if (this.textureSheetAnimation.enabled)
			{
				float num2 = 1f - particle.remainingLifetime / particle.startLifetime;
				if (this.textureSheetAnimation.frameOverTime.curveMin != null)
				{
					num2 = this.textureSheetAnimation.frameOverTime.curveMin.Evaluate(1f - particle.remainingLifetime / particle.startLifetime);
				}
				else if (this.textureSheetAnimation.frameOverTime.curve != null)
				{
					num2 = this.textureSheetAnimation.frameOverTime.curve.Evaluate(1f - particle.remainingLifetime / particle.startLifetime);
				}
				else if (this.textureSheetAnimation.frameOverTime.constant > 0f)
				{
					num2 = this.textureSheetAnimation.frameOverTime.constant - particle.remainingLifetime / particle.startLifetime;
				}
				num2 = Mathf.Repeat(num2 * (float)this.textureSheetAnimation.cycleCount, 1f);
				int num3 = 0;
				ParticleSystemAnimationType animation = this.textureSheetAnimation.animation;
				if (animation != ParticleSystemAnimationType.WholeSheet)
				{
					if (animation == ParticleSystemAnimationType.SingleRow)
					{
						num3 = Mathf.FloorToInt(num2 * (float)this.textureSheetAnimation.numTilesX);
						int rowIndex = this.textureSheetAnimation.rowIndex;
						num3 += rowIndex * this.textureSheetAnimation.numTilesX;
					}
				}
				else
				{
					num3 = Mathf.FloorToInt(num2 * (float)this.textureSheetAnimationFrames);
				}
				num3 %= this.textureSheetAnimationFrames;
				vector.x = (float)(num3 % this.textureSheetAnimation.numTilesX) * this.textureSheetAnimationFrameSize.x;
				vector.y = (float)Mathf.FloorToInt((float)(num3 / this.textureSheetAnimation.numTilesX)) * this.textureSheetAnimationFrameSize.y;
				vector.z = vector.x + this.textureSheetAnimationFrameSize.x;
				vector.w = vector.y + this.textureSheetAnimationFrameSize.y;
			}
			Color32 currentColor = particle.GetCurrentColor(this.pSystem);
			this._quad[0] = UIVertex.simpleVert;
			this._quad[0].color = currentColor;
			this._quad[0].uv0 = new Vector2(vector.x, vector.y);
			this._quad[1] = UIVertex.simpleVert;
			this._quad[1].color = currentColor;
			this._quad[1].uv0 = new Vector2(vector.x, vector.w);
			this._quad[2] = UIVertex.simpleVert;
			this._quad[2].color = currentColor;
			this._quad[2].uv0 = new Vector2(vector.z, vector.w);
			this._quad[3] = UIVertex.simpleVert;
			this._quad[3].color = currentColor;
			this._quad[3].uv0 = new Vector2(vector.z, vector.y);
			float num4 = -particle.rotation * 0.017453292f;
			float f = num4 + 1.5707964f;
			float d = particle.GetCurrentSize(this.pSystem) * 0.5f;
			Vector2 b = new Vector2(Mathf.Cos(num4), Mathf.Sin(num4)) * d;
			Vector2 b2 = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * d;
			this._quad[0].position = a - b - b2;
			this._quad[1].position = a - b + b2;
			this._quad[2].position = a + b + b2;
			this._quad[3].position = a + b - b2;
			vh.AddUIVertexQuad(this._quad);
		}
	}

	// Token: 0x06004D31 RID: 19761 RVA: 0x001462C7 File Offset: 0x001446C7
	private void onUpdatedVideoSettings()
	{
		base.enabled = (this.videoSettings.ParticleQuality > ThreeTierQualityLevel.Low);
	}

	// Token: 0x06004D32 RID: 19762 RVA: 0x001462E0 File Offset: 0x001446E0
	public void Play()
	{
		this.IsPlaying = true;
		foreach (UIParticleSystem uiparticleSystem in base.GetComponentsInChildren<UIParticleSystem>())
		{
			uiparticleSystem.PlayAlone();
		}
	}

	// Token: 0x06004D33 RID: 19763 RVA: 0x00146319 File Offset: 0x00144719
	public void PlayAlone()
	{
		this.IsPlaying = true;
	}

	// Token: 0x06004D34 RID: 19764 RVA: 0x00146324 File Offset: 0x00144724
	public void Stop()
	{
		this.IsPlaying = false;
		foreach (UIParticleSystem uiparticleSystem in base.GetComponentsInChildren<UIParticleSystem>())
		{
			uiparticleSystem.StopAlone();
		}
	}

	// Token: 0x06004D35 RID: 19765 RVA: 0x0014635D File Offset: 0x0014475D
	public void StopAlone()
	{
		this.IsPlaying = false;
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x00146366 File Offset: 0x00144766
	public void PlayWithDelay()
	{
		if (this.pSystem == null)
		{
			Debug.LogError("No particle system attached to " + base.gameObject.name);
			return;
		}
		base.StartCoroutine(this.delayPlay());
	}

	// Token: 0x06004D37 RID: 19767 RVA: 0x001463A4 File Offset: 0x001447A4
	private IEnumerator delayPlay()
	{
		yield return new WaitForSeconds(this.PlayDelay);
		this.Play();
		yield break;
	}

	// Token: 0x040032A1 RID: 12961
	public float PlayDelay;

	// Token: 0x040032A2 RID: 12962
	private static int MAX_UI_PARTICLES = 500;

	// Token: 0x040032A3 RID: 12963
	private Transform _transform;

	// Token: 0x040032A4 RID: 12964
	private ParticleSystem pSystem;

	// Token: 0x040032A5 RID: 12965
	private UIParticleSystem parentUIPSystem;

	// Token: 0x040032A6 RID: 12966
	private ParticleSystem.Particle[] particles;

	// Token: 0x040032A7 RID: 12967
	private UIVertex[] _quad = new UIVertex[4];

	// Token: 0x040032A8 RID: 12968
	private ParticleSystem.TextureSheetAnimationModule textureSheetAnimation;

	// Token: 0x040032A9 RID: 12969
	private int textureSheetAnimationFrames;

	// Token: 0x040032AA RID: 12970
	private Vector2 textureSheetAnimationFrameSize;

	// Token: 0x040032AB RID: 12971
	private ParticleSystem.MainModule mainModule;
}
