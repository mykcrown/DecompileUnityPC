using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class Basics : MonoBehaviour
{
	// Token: 0x06000055 RID: 85 RVA: 0x00005914 File Offset: 0x00003D14
	private void Start()
	{
		DOTween.Init(new bool?(false), new bool?(true), new LogBehaviour?(LogBehaviour.ErrorsOnly));
		this.cubeA.DOMove(new Vector3(-2f, 2f, 0f), 1f, false).SetRelative<Tweener>().SetLoops(-1, LoopType.Yoyo);
		DOTween.To(() => this.cubeB.position, delegate(Vector3 x)
		{
			this.cubeB.position = x;
		}, new Vector3(-2f, 2f, 0f), 1f).SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>().SetLoops(-1, LoopType.Yoyo);
	}

	// Token: 0x0400009D RID: 157
	public Transform cubeA;

	// Token: 0x0400009E RID: 158
	public Transform cubeB;
}
