using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200059B RID: 1435
public interface ICharacterRenderer : IDestroyable, IRollbackStateOwner, ITickable
{
	// Token: 0x0600206C RID: 8300
	void ToggleVisibility(bool visibile);

	// Token: 0x0600206D RID: 8301
	void SetMaterials(List<Material> material);

	// Token: 0x0600206E RID: 8302
	Color GetOutlineColorFromTeam(TeamNum teamNum);

	// Token: 0x0600206F RID: 8303
	void LerpMaterial(Material startMaterial, Material targetMaterial, float lerp);

	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x06002070 RID: 8304
	List<Material> DefaultMaterials { get; }

	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06002071 RID: 8305
	Material[] CurrentMaterials { get; }

	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06002072 RID: 8306
	List<Renderer> Renderers { get; }

	// Token: 0x06002073 RID: 8307
	void AddMaterialEmitter(GameObject effectPrefab, IGameVFX vfx);

	// Token: 0x06002074 RID: 8308
	void ClearAllMaterialEmitters();

	// Token: 0x06002075 RID: 8309
	void ClearMaterialEmitter(GameObject prefab);

	// Token: 0x06002076 RID: 8310
	void SetColorModeFlag(ColorMode flag, bool enabled);

	// Token: 0x06002077 RID: 8311
	void OverrideColor(int frames, AnimatingColor color);
}
