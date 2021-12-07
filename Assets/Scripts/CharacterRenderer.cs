// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterRenderer : ICharacterRenderer, IDestroyable, IRollbackStateOwner, ITickable
{
	private IPlayerDelegate player;

	private List<Renderer> renderers;

	private CharacterRendererState state;

	private List<List<Texture>> cachedEmissionTextures = new List<List<Texture>>();

	private List<List<Color>> cachedEmissionColors = new List<List<Color>>();

	private Dictionary<int, Dictionary<int, Color>> cachedTintColors = new Dictionary<int, Dictionary<int, Color>>();

	private static string COLOR_FIELD = "_Color";

	private readonly int OUTLINE_SHADER_HASH = "Wavedash/Character-New".GetHashCode();

	private bool enableTeamOutlines;

	private Color teamOutlineColor = Color.clear;

	private float teamOutlineWidth;

	private Material lastOutlineMaterial;

	private AnimatingColor animatingEmission;

	private int animatingEmissionCurrentFrame;

	private AnimatingColor animatingColor;

	private int animatingColorCurrentFrame;

	List<Renderer> ICharacterRenderer.Renderers
	{
		get
		{
			return this.renderers;
		}
	}

	Material[] ICharacterRenderer.CurrentMaterials
	{
		get
		{
			return (this.renderers.Count <= 0) ? null : this.renderers[0].materials;
		}
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	private CharacterEffectConfig effectsData
	{
		get
		{
			return this.config.defaultCharacterEffects;
		}
	}

	private CharacterColorConfig colorData
	{
		get
		{
			return this.config.characterColorConfig;
		}
	}

	public List<Material> DefaultMaterials
	{
		get;
		private set;
	}

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

	public void Destroy()
	{
		this.events.Unsubscribe(typeof(TogglePlayerVisibilityCommand), new Events.EventHandler(this.onTogglePlayerVisibility));
	}

	private void onTogglePlayerVisibility(GameEvent message)
	{
		TogglePlayerVisibilityCommand togglePlayerVisibilityCommand = message as TogglePlayerVisibilityCommand;
		if (togglePlayerVisibilityCommand.players.Contains(this.player.PlayerNum))
		{
			this.ToggleVisibility(togglePlayerVisibilityCommand.visibility);
		}
	}

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

	private void loadAdvancedEmission(AnimatingColor emission)
	{
		if (emission.animateColor)
		{
			this.animatingEmission = emission;
			this.animatingEmissionCurrentFrame = 0;
		}
	}

	private void loadAnimatingColor(AnimatingColor color)
	{
		if (color.animateColor)
		{
			this.animatingColor = color;
			this.animatingColorCurrentFrame = 0;
		}
	}

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

	void ICharacterRenderer.OverrideColor(int frames, AnimatingColor color)
	{
		this.state.overrideColorFrameCount = frames;
		this.state.overrideColor = color;
		this.setColorMode();
	}

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

	private bool hasColorModeFlag(ColorMode flag)
	{
		return (this.state.colorModeFlags & flag) > (ColorMode)0;
	}

	void ICharacterRenderer.LerpMaterial(Material startMaterial, Material targetMaterial, float lerp)
	{
		for (int i = 0; i < this.renderers.Count; i++)
		{
			Renderer renderer = this.renderers[i];
			renderer.material.Lerp(startMaterial, targetMaterial, lerp);
		}
	}

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

	public void AddMaterialEmitter(GameObject effectPrefab, IGameVFX vfx)
	{
		if (this.state.materialEmitters.ContainsKey(effectPrefab))
		{
			UnityEngine.Debug.LogWarning("Attempted to add duplicate material emitter");
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

	public void ClearAllMaterialEmitters()
	{
		foreach (GameObject current in this.state.materialEmitters.Keys)
		{
			int count = this.state.materialEmitters[current].Count;
			for (int i = 0; i < count; i++)
			{
				this.state.materialEmitters[current][i].Expire();
			}
		}
	}

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

	public void ToggleVisibility(bool isVisible)
	{
		for (int i = 0; i < this.renderers.Count; i++)
		{
			Renderer renderer = this.renderers[i];
			this.state.IsVisible = isVisible;
			renderer.enabled = isVisible;
		}
	}

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

	public Color GetOutlineColorFromTeam(TeamNum team)
	{
		if (team >= TeamNum.All)
		{
			return Color.clear;
		}
		return this.effectsData.teamOutlineColors[(int)team];
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<CharacterRendererState>(this.state));
		return true;
	}

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

	public static bool ResetColorModeOnMoveEnd(ColorMode mode)
	{
		return mode != ColorMode.Inactive;
	}
}
