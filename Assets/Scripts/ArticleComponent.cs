// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class ArticleComponent : ScriptableObject, IArticleComponent
{
	protected IArticleDelegate articleDelegate;

	protected GameManager gameManager;

	public virtual void Init(IArticleDelegate articleDelegate, GameManager manager)
	{
		this.articleDelegate = articleDelegate;
		this.gameManager = manager;
	}

	public virtual void OnArticleInstantiate()
	{
	}
}
