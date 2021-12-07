// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterRenderer : IDestroyable, IRollbackStateOwner, ITickable
{
	List<Material> DefaultMaterials
	{
		get;
	}

	Material[] CurrentMaterials
	{
		get;
	}

	List<Renderer> Renderers
	{
		get;
	}

	void ToggleVisibility(bool visibile);

	void SetMaterials(List<Material> material);

	Color GetOutlineColorFromTeam(TeamNum teamNum);

	void LerpMaterial(Material startMaterial, Material targetMaterial, float lerp);

	void AddMaterialEmitter(GameObject effectPrefab, IGameVFX vfx);

	void ClearAllMaterialEmitters();

	void ClearMaterialEmitter(GameObject prefab);

	void SetColorModeFlag(ColorMode flag, bool enabled);

	void OverrideColor(int frames, AnimatingColor color);
}
