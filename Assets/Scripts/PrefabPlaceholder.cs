// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class PrefabPlaceholder : GameBehavior
{
	public GameObject Prefab;

	public override void Awake()
	{
		this.ReplaceWithInstance();
	}

	public void ReplaceWithInstance()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefab);
		RectTransform rectTransform = (RectTransform)gameObject.transform;
		RectTransform rectTransform2 = (RectTransform)base.transform;
		gameObject.transform.SetParent(base.transform.parent, false);
		rectTransform.anchorMax = rectTransform2.anchorMax;
		rectTransform.anchorMin = rectTransform2.anchorMin;
		rectTransform.pivot = rectTransform2.pivot;
		rectTransform.anchoredPosition3D = rectTransform2.anchoredPosition3D;
		base.gameObject.DestroySafe();
	}
}
