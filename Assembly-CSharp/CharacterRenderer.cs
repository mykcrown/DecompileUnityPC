using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200059A RID: 1434
public class CharacterRenderer : ICharacterRenderer, IDestroyable, IRollbackStateOwner, ITickable
{
	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x06002047 RID: 8263 RVA: 0x000A2E1B File Offset: 0x000A121B
	// (set) Token: 0x06002048 RID: 8264 RVA: 0x000A2E23 File Offset: 0x000A1223
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06002049 RID: 8265 RVA: 0x000A2E2C File Offset: 0x000A122C
	// (set) Token: 0x0600204A RID: 8266 RVA: 0x000A2E34 File Offset: 0x000A1234
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x0600204B RID: 8267 RVA: 0x000A2E3D File Offset: 0x000A123D
	// (set) Token: 0x0600204C RID: 8268 RVA: 0x000A2E45 File Offset: 0x000A1245
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x0600204D RID: 8269 RVA: 0x000A2E4E File Offset: 0x000A124E
	private CharacterEffectConfig effectsData
	{
		get
		{
			return this.config.defaultCharacterEffects;
		}
	}

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x0600204E RID: 8270 RVA: 0x000A2E5B File Offset: 0x000A125B
	private CharacterColorConfig colorData
	{
		get
		{
			return this.config.characterColorConfig;
		}
	}

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x0600204F RID: 8271 RVA: 0x000A2E68 File Offset: 0x000A1268
	List<Renderer> ICharacterRenderer.Renderers
	{
		get
		{
			return this.renderers;
		}
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000A2E70 File Offset: 0x000A1270
	public void Init(IPlayerDelegate player, List<Renderer> renderers, bool enableTeamOutlines)
	{
		this.player = player;
		this.state = new CharacterRendererState();
		this.renderers = renderers;
		this.DefaultMaterials = new List<Material>();
		for (int i = 0; i < this.renderers.Count; i++)
		{
			Renderer renderer = this.renderers[i];
			if (!renderer.gameObject.CompareTag(Tags.Debug))
			{
				this.DefaultMaterials.AddRange(renderer.materials);
				List<Texture> list = new List<Texture>();
				List<Color> list2 = new List<Color>();
				this.cachedEmissionTextures.Add(list);
				this.cachedEmissionColors.Add(list2);
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					Material material = renderer.materials[j];
					if (material != null)
					{
						list.Add(material.GetTexture("_EmissionMap"));
						list2.Add(material.GetColor("_EmissionColor"));
						if (material.HasProperty(CharacterRenderer.COLOR_FIELD))
						{
							if (!this.cachedTintColors.ContainsKey(i))
							{
								this.cachedTintColors[i] = new Dictionary<int, Color>();
							}
							this.cachedTintColors[i][j] = material.GetColor(CharacterRenderer.COLOR_FIELD);
						}
					}
				}
				if (renderer is SkinnedMeshRenderer)
				{
					(renderer as SkinnedMeshRenderer).updateWhenOffscreen = true;
					(renderer as SkinnedMeshRenderer).shadowCastingMode = ShadowCastingMode.On;
				}
			}
		}
		this.enableTeamOutlines = enableTeamOutlines;
		this.updateCharacterOutlines();
		this.events.Subscribe(typeof(TogglePlayerVisibilityCommand), new Events.EventHandler(this.onTogglePlayerVisibility));
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x000A3015 File Offset: 0x000A1415
	public void Destroy()
	{
		this.events.Unsubscribe(typeof(TogglePlayerVisibilityCommand), new Events.EventHandler(this.onTogglePlayerVisibility));
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000A3038 File Offset: 0x000A1438
	private void onTogglePlayerVisibility(GameEvent message)
	{
		TogglePlayerVisibilityCommand togglePlayerVisibilityCommand = message as TogglePlayerVisibilityCommand;
		if (togglePlayerVisibilityCommand.players.Contains(this.player.PlayerNum))
		{
			this.ToggleVisibility(togglePlayerVisibilityCommand.visibility);
		}
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000A3074 File Offset: 0x000A1474
	private void setColorMode()
	{
		Color emission = Color.black;
		Color color = Color.white;
		this.animatingEmission = null;
		this.animatingColor = null;
		if (this.state.overrideColorFrameCount > 0)
		{
			emission = this.state.overrideColor.colorStart;
			this.loadAdvancedEmission(this.state.overrideColor);
			color = this.colorData.emissionColorOffset;
		}
		else if (this.hasColorModeFlag(ColorMode.Charging))
		{
			emission = this.colorData.chargingEmission.colorStart;
			this.loadAdvancedEmission(this.colorData.chargingEmission);
			color = this.colorData.emissionColorOffset;
		}
		else if (this.hasColorModeFlag(ColorMode.Dazed))
		{
			emission = this.colorData.dazedEmission.colorStart;
			this.loadAdvancedEmission(this.colorData.dazedEmission);
			color = this.colorData.emissionColorOffset;
		}
		else if (this.hasColorModeFlag(ColorMode.Helpless))
		{
			color = this.colorData.helplessColor.colorStart;
			this.loadAnimatingColor(this.colorData.helplessColor);
		}
		else if (this.hasColorModeFlag(ColorMode.InvincibleSlow))
		{
			emission = this.colorData.invincibleEmissionSlow.colorStart;
			this.loadAdvancedEmission(this.colorData.invincibleEmissionSlow);
		}
		else if (this.hasColorModeFlag(ColorMode.InvincibleMed))
		{
			emission = this.colorData.invincibleEmissionMed.colorStart;
			this.loadAdvancedEmission(this.colorData.invincibleEmissionMed);
		}
		else if (this.hasColorModeFlag(ColorMode.Invincible))
		{
			emission = this.colorData.invincibleEmission.colorStart;
			this.loadAdvancedEmission(this.colorData.invincibleEmission);
		}
		else if (this.hasColorModeFlag(ColorMode.LedgeVulnerable))
		{
			emission = this.colorData.ledgegrabVulnerable.colorStart;
			this.loadAdvancedEmission(this.colorData.ledgegrabVulnerable);
		}
		else if (this.hasColorModeFlag(ColorMode.RegrabPrevent))
		{
			emission = this.colorData.regrabPreventionColor.colorStart;
			this.loadAdvancedEmission(this.colorData.regrabPreventionColor);
		}
		else if (this.hasColorModeFlag(ColorMode.Tumbling))
		{
			emission = this.colorData.tumblingEmission;
			color = this.colorData.emissionColorOffset;
		}
		else if (this.hasColorModeFlag(ColorMode.Inactive))
		{
			emission = this.colorData.inactiveEmission;
			color = this.colorData.inactiveColorOffset;
		}
		this.updateColorDisplays(emission, color);
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000A32F7 File Offset: 0x000A16F7
	private void loadAdvancedEmission(AnimatingColor emission)
	{
		if (emission.animateColor)
		{
			this.animatingEmission = emission;
			this.animatingEmissionCurrentFrame = 0;
		}
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x000A3312 File Offset: 0x000A1712
	private void loadAnimatingColor(AnimatingColor color)
	{
		if (color.animateColor)
		{
			this.animatingColor = color;
			this.animatingColorCurrentFrame = 0;
		}
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x000A3330 File Offset: 0x000A1730
	private void updateAnimatingColors()
	{
		if (this.animatingEmission != null || this.animatingColor != null)
		{
			Color emission = Color.black;
			Color color = Color.white;
			if (this.animatingEmission != null)
			{
				emission = this.tickAnimatingColor(ref this.animatingEmissionCurrentFrame, ref this.animatingEmission);
			}
			if (this.animatingColor != null)
			{
				color = this.tickAnimatingColor(ref this.animatingColorCurrentFrame, ref this.animatingColor);
			}
			this.updateColorDisplays(emission, color);
		}
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x000A33A4 File Offset: 0x000A17A4
	private Color tickAnimatingColor(ref int currentFrame, ref AnimatingColor thisAnimatingColor)
	{
		currentFrame++;
		float b;
		if (thisAnimatingColor.method == AnimatingColor.Method.LOOP)
		{
			int frames = thisAnimatingColor.frames;
			if (currentFrame > frames)
			{
				currentFrame = 0;
			}
			b = (float)currentFrame / (float)frames;
		}
		else
		{
			int num = thisAnimatingColor.frames * 2;
			if (currentFrame >= num)
			{
				currentFrame = 0;
			}
			if (currentFrame <= thisAnimatingColor.frames)
			{
				b = (float)currentFrame / (float)thisAnimatingColor.frames;
			}
			else
			{
				b = (float)(num - currentFrame) / (float)thisAnimatingColor.frames;
			}
		}
		return thisAnimatingColor.colorStart + (thisAnimatingColor.colorEnd - thisAnimatingColor.colorStart) * b;
	}

	// Token: 0x06002058 RID: 8280 RVA: 0x000A344C File Offset: 0x000A184C
	private void updateColorDisplays(Color emission, Color color)
	{
		for (int i = 0; i < this.renderers.Count; i++)
		{
			Renderer renderer = this.renderers[i];
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				Material material = materials[j];
				if (material.HasProperty(CharacterRenderer.COLOR_FIELD))
				{
					if (color != Color.white)
					{
						material.SetColor(CharacterRenderer.COLOR_FIELD, color);
					}
					else
					{
						material.SetColor(CharacterRenderer.COLOR_FIELD, this.cachedTintColors[i][j]);
					}
				}
				if (material.HasProperty("_EmissionColor"))
				{
					if (emission != Color.black)
					{
						if (material.HasProperty("_EmissionMap"))
						{
							material.SetTexture("_EmissionMap", null);
						}
						material.SetColor("_EmissionColor", emission);
						material.EnableKeyword("_EMISSION");
					}
					else
					{
						if (material.HasProperty("_EmissionMap"))
						{
							material.SetTexture("_EmissionMap", this.cachedEmissionTextures[i][j]);
						}
						material.SetColor("_EmissionColor", this.cachedEmissionColors[i][j]);
						if (this.cachedEmissionColors[i][j] == Color.black)
						{
							material.DisableKeyword("_EMISSION");
						}
					}
				}
			}
		}
	}

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x06002059 RID: 8281 RVA: 0x000A35CA File Offset: 0x000A19CA
	// (set) Token: 0x0600205A RID: 8282 RVA: 0x000A35D2 File Offset: 0x000A19D2
	public List<Material> DefaultMaterials { get; private set; }

	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x0600205B RID: 8283 RVA: 0x000A35DB File Offset: 0x000A19DB
	Material[] ICharacterRenderer.CurrentMaterials
	{
		get
		{
			return (this.renderers.Count <= 0) ? null : this.renderers[0].materials;
		}
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x000A3608 File Offset: 0x000A1A08
	void ICharacterRenderer.SetColorModeFlag(ColorMode flag, bool enabled)
	{
		bool flag2;
		if (enabled)
		{
			flag2 = !this.hasColorModeFlag(flag);
			this.state.colorModeFlags |= flag;
		}
		else
		{
			flag2 = this.hasColorModeFlag(flag);
			this.state.colorModeFlags &= ~flag;
		}
		if (flag2)
		{
			this.setColorMode();
		}
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x000A3668 File Offset: 0x000A1A68
	void ICharacterRenderer.OverrideColor(int frames, AnimatingColor color)
	{
		this.state.overrideColorFrameCount = frames;
		this.state.overrideColor = color;
		this.setColorMode();
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x000A3688 File Offset: 0x000A1A88
	void ITickable.TickFrame()
	{
		if (this.state.overrideColorFrameCount > 0)
		{
			this.state.overrideColorFrameCount--;
			if (this.state.overrideColorFrameCount <= 0)
			{
				this.setColorMode();
			}
		}
		if (GameClient.IsCurrentFrame)
		{
			for (int i = 0; i < this.renderers.Count; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = this.renderers[i] as SkinnedMeshRenderer;
				if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.rootBone == null))
				{
					bool flag = !MathUtil.almostEqual(skinnedMeshRenderer.rootBone.transform.localScale.sqrMagnitude, 0f, 0.01f);
					if (flag != this.renderers[i].gameObject.activeInHierarchy)
					{
						this.renderers[i].gameObject.SetActive(flag);
					}
				}
			}
			this.updateCharacterOutlines();
			this.updateAnimatingColors();
		}
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000A3794 File Offset: 0x000A1B94
	private bool hasColorModeFlag(ColorMode flag)
	{
		return (this.state.colorModeFlags & flag) > (ColorMode)0;
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000A37A8 File Offset: 0x000A1BA8
	void ICharacterRenderer.LerpMaterial(Material startMaterial, Material targetMaterial, float lerp)
	{
		for (int i = 0; i < this.renderers.Count; i++)
		{
			Renderer renderer = this.renderers[i];
			renderer.material.Lerp(startMaterial, targetMaterial, lerp);
		}
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000A37EC File Offset: 0x000A1BEC
	public void SetMaterials(List<Material> materials)
	{
		if (materials.Count == 0)
		{
			return;
		}
		bool flag = materials.Count == this.renderers.Count;
		for (int i = 0; i < this.renderers.Count; i++)
		{
			Renderer renderer = this.renderers[i];
			renderer.material = ((!flag) ? materials[0] : materials[i]);
		}
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000A3864 File Offset: 0x000A1C64
	public void AddMaterialEmitter(GameObject effectPrefab, IGameVFX vfx)
	{
		if (this.state.materialEmitters.ContainsKey(effectPrefab))
		{
			Debug.LogWarning("Attempted to add duplicate material emitter");
			return;
		}
		this.state.materialEmitters.Add(effectPrefab, new List<GeneratedEffect>());
		for (int i = 0; i < this.renderers.Count; i++)
		{
			Renderer renderer = this.renderers[i];
			if (renderer is SkinnedMeshRenderer)
			{
				GeneratedEffect generatedEffect = vfx.PlayParticle(new ParticleData
				{
					frames = 0,
					prefab = effectPrefab,
					attach = true
				}, false, TeamNum.None);
				if (generatedEffect != null)
				{
					ParticleSystem component = generatedEffect.EffectObject.GetComponent<ParticleSystem>();
					component.shape.skinnedMeshRenderer = (renderer as SkinnedMeshRenderer);
					this.state.materialEmitters[effectPrefab].Add(generatedEffect);
				}
			}
		}
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x000A3944 File Offset: 0x000A1D44
	public void ClearAllMaterialEmitters()
	{
		foreach (GameObject key in this.state.materialEmitters.Keys)
		{
			int count = this.state.materialEmitters[key].Count;
			for (int i = 0; i < count; i++)
			{
				this.state.materialEmitters[key][i].Expire();
			}
		}
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x000A39E8 File Offset: 0x000A1DE8
	public void ClearMaterialEmitter(GameObject prefab)
	{
		if (this.state.materialEmitters.ContainsKey(prefab))
		{
			int count = this.state.materialEmitters[prefab].Count;
			for (int i = 0; i < count; i++)
			{
				this.state.materialEmitters[prefab][i].Expire();
			}
		}
	}

	// Token: 0x06002065 RID: 8293 RVA: 0x000A3A50 File Offset: 0x000A1E50
	public void ToggleVisibility(bool isVisible)
	{
		for (int i = 0; i < this.renderers.Count; i++)
		{
			Renderer renderer = this.renderers[i];
			this.state.IsVisible = isVisible;
			renderer.enabled = isVisible;
		}
	}

	// Token: 0x06002066 RID: 8294 RVA: 0x000A3A9C File Offset: 0x000A1E9C
	private void updateCharacterOutlines()
	{
		if (!this.effectsData.enableTeamOutlines || !this.enableTeamOutlines)
		{
			return;
		}
		Color outlineColorFromTeam = this.GetOutlineColorFromTeam(this.player.Team);
		float num = this.effectsData.teamOutlineWidth;
		Material y = this.renderers[0].materials[0];
		if (this.teamOutlineColor != outlineColorFromTeam || this.teamOutlineWidth != num || this.lastOutlineMaterial != y)
		{
			this.teamOutlineColor = outlineColorFromTeam;
			this.teamOutlineWidth = num;
			this.lastOutlineMaterial = y;
			for (int i = 0; i < this.renderers.Count; i++)
			{
				Renderer renderer = this.renderers[i];
				if (!renderer.gameObject.CompareTag(Tags.Debug))
				{
					for (int j = 0; j < renderer.materials.Length; j++)
					{
						Material material = renderer.materials[j];
						if (material != null && material.shader.name.GetHashCode() == this.OUTLINE_SHADER_HASH)
						{
							material.EnableKeyword("_OUTLINE_ON");
							material.SetColor("_OutlineColor", outlineColorFromTeam);
							material.SetFloat("_OutlineWidth", num);
							material.SetColor("_RimColor", outlineColorFromTeam);
						}
					}
				}
			}
		}
	}

	// Token: 0x06002067 RID: 8295 RVA: 0x000A3C05 File Offset: 0x000A2005
	public Color GetOutlineColorFromTeam(TeamNum team)
	{
		if (team >= TeamNum.All)
		{
			return Color.clear;
		}
		return this.effectsData.teamOutlineColors[(int)team];
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x000A3C2A File Offset: 0x000A202A
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<CharacterRendererState>(this.state));
		return true;
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x000A3C48 File Offset: 0x000A2048
	public bool LoadState(RollbackStateContainer container)
	{
		bool isVisible = this.state.IsVisible;
		ColorMode colorModeFlags = this.state.colorModeFlags;
		container.ReadState<CharacterRendererState>(ref this.state);
		if (colorModeFlags != this.state.colorModeFlags)
		{
			this.setColorMode();
		}
		if (this.state.IsVisible != isVisible)
		{
			this.ToggleVisibility(this.state.IsVisible);
		}
		return true;
	}

	// Token: 0x0600206A RID: 8298 RVA: 0x000A3CB4 File Offset: 0x000A20B4
	public static bool ResetColorModeOnMoveEnd(ColorMode mode)
	{
		return mode != ColorMode.Inactive;
	}

	// Token: 0x040019CD RID: 6605
	private IPlayerDelegate player;

	// Token: 0x040019CE RID: 6606
	private List<Renderer> renderers;

	// Token: 0x040019CF RID: 6607
	private CharacterRendererState state;

	// Token: 0x040019D0 RID: 6608
	private List<List<Texture>> cachedEmissionTextures = new List<List<Texture>>();

	// Token: 0x040019D1 RID: 6609
	private List<List<Color>> cachedEmissionColors = new List<List<Color>>();

	// Token: 0x040019D2 RID: 6610
	private Dictionary<int, Dictionary<int, Color>> cachedTintColors = new Dictionary<int, Dictionary<int, Color>>();

	// Token: 0x040019D3 RID: 6611
	private static string COLOR_FIELD = "_Color";

	// Token: 0x040019D4 RID: 6612
	private readonly int OUTLINE_SHADER_HASH = "Wavedash/Character-New".GetHashCode();

	// Token: 0x040019D5 RID: 6613
	private bool enableTeamOutlines;

	// Token: 0x040019D6 RID: 6614
	private Color teamOutlineColor = Color.clear;

	// Token: 0x040019D7 RID: 6615
	private float teamOutlineWidth;

	// Token: 0x040019D8 RID: 6616
	private Material lastOutlineMaterial;

	// Token: 0x040019D9 RID: 6617
	private AnimatingColor animatingEmission;

	// Token: 0x040019DA RID: 6618
	private int animatingEmissionCurrentFrame;

	// Token: 0x040019DB RID: 6619
	private AnimatingColor animatingColor;

	// Token: 0x040019DC RID: 6620
	private int animatingColorCurrentFrame;
}
