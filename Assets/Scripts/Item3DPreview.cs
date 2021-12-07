// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class Item3DPreview
{
	public GameObject obj;

	public EquipmentTypes type;

	public EquippableItem item;

	public CharacterMenusData characterMenusData;

	public bool isOffsetsInitialized;

	public float anchorOffsetFromCenterY;

	public float anchorOffsetFromBottomY;

	public float anchorOffsetFromCenterX;

	public float anchorOffsetFromBottomX;

	public Action playPreviewFn
	{
		private get;
		set;
	}

	public void PlayPreview()
	{
		if (this.playPreviewFn != null)
		{
			this.playPreviewFn();
		}
	}

	public void Cleanup()
	{
		this.playPreviewFn = null;
		this.obj = null;
	}
}
