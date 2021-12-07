// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Basics : MonoBehaviour
{
	public Transform cubeA;

	public Transform cubeB;

	private void Start()
	{
		DOTween.Init(new bool?(false), new bool?(true), new LogBehaviour?(LogBehaviour.ErrorsOnly));
		this.cubeA.DOMove(new Vector3(-2f, 2f, 0f), 1f, false).SetRelative<Tweener>().SetLoops(-1, LoopType.Yoyo);
		DOTween.To(new DOGetter<Vector3>(this._Start_m__0), new DOSetter<Vector3>(this._Start_m__1), new Vector3(-2f, 2f, 0f), 1f).SetRelative<TweenerCore<Vector3, Vector3, VectorOptions>>().SetLoops(-1, LoopType.Yoyo);
	}

	private Vector3 _Start_m__0()
	{
		return this.cubeB.position;
	}

	private void _Start_m__1(Vector3 x)
	{
		this.cubeB.position = x;
	}
}
