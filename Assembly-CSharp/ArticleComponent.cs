using System;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class ArticleComponent : ScriptableObject, IArticleComponent
{
	// Token: 0x06001282 RID: 4738 RVA: 0x0006A958 File Offset: 0x00068D58
	public virtual void Init(IArticleDelegate articleDelegate, GameManager manager)
	{
		this.articleDelegate = articleDelegate;
		this.gameManager = manager;
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x0006A968 File Offset: 0x00068D68
	public virtual void OnArticleInstantiate()
	{
	}

	// Token: 0x04000C16 RID: 3094
	protected IArticleDelegate articleDelegate;

	// Token: 0x04000C17 RID: 3095
	protected GameManager gameManager;
}
