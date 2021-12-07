// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialComponent : MoveComponent, IMoveTickMoveFrameComponent
{
	public MaterialChangeData[] materialChanges = new MaterialChangeData[0];

	private Material startMaterial;

	public void TickMoveFrame(InputButtonsData input)
	{
		MaterialChangeData[] array = this.materialChanges;
		for (int i = 0; i < array.Length; i++)
		{
			MaterialChangeData materialChangeData = array[i];
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
					UnityEngine.Debug.LogWarning("No material specified for ChangeMaterial component!");
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
}
