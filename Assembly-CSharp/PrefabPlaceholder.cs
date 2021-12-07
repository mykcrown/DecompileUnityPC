using System;
using UnityEngine;

// Token: 0x02000B54 RID: 2900
public class PrefabPlaceholder : GameBehavior
{
	// Token: 0x06005416 RID: 21526 RVA: 0x001B0EE0 File Offset: 0x001AF2E0
	public override void Awake()
	{
		this.ReplaceWithInstance();
	}

	// Token: 0x06005417 RID: 21527 RVA: 0x001B0EE8 File Offset: 0x001AF2E8
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

	// Token: 0x04003555 RID: 13653
	public GameObject Prefab;
}
