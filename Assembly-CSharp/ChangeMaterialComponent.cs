using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public class ChangeMaterialComponent : MoveComponent, IMoveTickMoveFrameComponent
{
	// Token: 0x06001B1D RID: 6941 RVA: 0x0008A428 File Offset: 0x00088828
	public void TickMoveFrame(InputButtonsData input)
	{
		foreach (MaterialChangeData materialChangeData in this.materialChanges)
		{
			if (this.moveDelegate.Model.internalFrame >= materialChangeData.startFrame)
			{
				this.startMaterial = this.playerDelegate.Renderer.CurrentMaterials[0];
				Material material = materialChangeData.targetMaterial;
				if (materialChangeData.restoreDefaultMaterial)
				{
					material = this.playerDelegate.Renderer.DefaultMaterials[0];
				}
				else if (material == null)
				{
					Debug.LogWarning("No material specified for ChangeMaterial component!");
					material = this.playerDelegate.Renderer.DefaultMaterials[0];
				}
				if (this.moveDelegate.Model.internalFrame < materialChangeData.startFrame + materialChangeData.lerpFrames)
				{
					float lerp = (float)(this.moveDelegate.Model.internalFrame - materialChangeData.startFrame) / (float)Mathf.Max(1, materialChangeData.lerpFrames);
					this.playerDelegate.Renderer.LerpMaterial(this.startMaterial, material, lerp);
				}
				else if (this.moveDelegate.Model.internalFrame == materialChangeData.startFrame + materialChangeData.lerpFrames)
				{
					this.playerDelegate.Renderer.SetMaterials(new List<Material>
					{
						material
					});
				}
			}
		}
	}

	// Token: 0x0400146E RID: 5230
	public MaterialChangeData[] materialChanges = new MaterialChangeData[0];

	// Token: 0x0400146F RID: 5231
	private Material startMaterial;
}
