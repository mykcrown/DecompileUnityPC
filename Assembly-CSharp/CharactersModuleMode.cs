using System;
using UnityEngine;

// Token: 0x020009DA RID: 2522
public class CharactersModuleMode : MonoBehaviour
{
	// Token: 0x06004761 RID: 18273 RVA: 0x0013618D File Offset: 0x0013458D
	public void Init(StoreScene3D storeScene)
	{
		this.storeScene = storeScene;
		this.CanvasGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x06004762 RID: 18274 RVA: 0x001361A2 File Offset: 0x001345A2
	public void Activate()
	{
		this.storeScene.AttachCharactersTo(this.CharacterDisplay3D, this.CharacterItemDisplay3D);
	}

	// Token: 0x04002F23 RID: 12067
	private StoreScene3D storeScene;

	// Token: 0x04002F24 RID: 12068
	public Transform CharacterDisplay3D;

	// Token: 0x04002F25 RID: 12069
	public Transform CharacterItemDisplay3D;

	// Token: 0x04002F26 RID: 12070
	public CanvasGroup CanvasGroup;
}
