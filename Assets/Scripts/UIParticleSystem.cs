// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode, RequireComponent(typeof(CanvasRenderer), typeof(ParticleSystem))]
public class UIParticleSystem : MaskableGraphic
{
	private sealed class _delayPlay_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal UIParticleSystem _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _delayPlay_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this._this.PlayDelay);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._this.Play();
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public float PlayDelay;

	private static int MAX_UI_PARTICLES = 500;

	private Transform _transform;

	private ParticleSystem pSystem;

	private UIParticleSystem parentUIPSystem;

	private ParticleSystem.Particle[] particles;

	private UIVertex[] _quad = new UIVertex[4];

	private ParticleSystem.TextureSheetAnimationModule textureSheetAnimation;

	private int textureSheetAnimationFrames;

	private Vector2 textureSheetAnimationFrameSize;

	private ParticleSystem.MainModule mainModule;

	[Inject]
	public IUserVideoSettingsModel videoSettings
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public override Texture mainTexture
	{
		get
		{
			return null;
		}
	}

	public bool IsPlaying
	{
		get;
		private set;
	}

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

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Application.isPlaying)
		{
			this.signalBus.RemoveListener(UserVideoSettingsModel.UPDATED, new Action(this.onUpdatedVideoSettings));
		}
	}

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
			float num4 = -particle.rotation * 0.0174532924f;
			float f = num4 + 1.57079637f;
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

	private void onUpdatedVideoSettings()
	{
		base.enabled = (this.videoSettings.ParticleQuality > ThreeTierQualityLevel.Low);
	}

	public void Play()
	{
		this.IsPlaying = true;
		UIParticleSystem[] componentsInChildren = base.GetComponentsInChildren<UIParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UIParticleSystem uIParticleSystem = componentsInChildren[i];
			uIParticleSystem.PlayAlone();
		}
	}

	public void PlayAlone()
	{
		this.IsPlaying = true;
	}

	public void Stop()
	{
		this.IsPlaying = false;
		UIParticleSystem[] componentsInChildren = base.GetComponentsInChildren<UIParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UIParticleSystem uIParticleSystem = componentsInChildren[i];
			uIParticleSystem.StopAlone();
		}
	}

	public void StopAlone()
	{
		this.IsPlaying = false;
	}

	public void PlayWithDelay()
	{
		if (this.pSystem == null)
		{
			UnityEngine.Debug.LogError("No particle system attached to " + base.gameObject.name);
			return;
		}
		base.StartCoroutine(this.delayPlay());
	}

	private IEnumerator delayPlay()
	{
		UIParticleSystem._delayPlay_c__Iterator0 _delayPlay_c__Iterator = new UIParticleSystem._delayPlay_c__Iterator0();
		_delayPlay_c__Iterator._this = this;
		return _delayPlay_c__Iterator;
	}
}
