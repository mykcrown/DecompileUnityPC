// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CharactersModuleMode : MonoBehaviour
{
	private StoreScene3D storeScene;

	public Transform CharacterDisplay3D;

	public Transform CharacterItemDisplay3D;

	public CanvasGroup CanvasGroup;

	public void Init(StoreScene3D storeScene)
	{
		this.storeScene = storeScene;
		this.CanvasGroup = base.GetComponent<CanvasGroup>();
	}

	public void Activate()
	{
		this.storeScene.AttachCharactersTo(this.CharacterDisplay3D, this.CharacterItemDisplay3D);
	}
}
