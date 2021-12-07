using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class Sequences : MonoBehaviour
{
	// Token: 0x06000059 RID: 89 RVA: 0x000059D4 File Offset: 0x00003DD4
	private void Start()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(this.target.DOMoveY(2f, 1f, false));
		sequence.Join(this.target.DORotate(new Vector3(0f, 135f, 0f), 1f, RotateMode.Fast));
		sequence.Append(this.target.DOScaleY(0.2f, 1f));
		sequence.Insert(0f, this.target.DOMoveX(4f, sequence.Duration(true), false).SetRelative<Tweener>());
		sequence.SetLoops(4, LoopType.Yoyo);
	}

	// Token: 0x0400009F RID: 159
	public Transform target;
}
