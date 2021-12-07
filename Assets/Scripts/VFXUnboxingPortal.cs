// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VFXUnboxingPortal : MonoBehaviour
{
	public GameObject Border1;

	public GameObject Border1B;

	public GameObject Border2;

	public GameObject Border2B;

	public GameObject Border1_Mouseover;

	public GameObject Border2_Mouseover;

	public GameObject NormalMode;

	public GameObject MouseoverMode;

	private Color _color;

	private bool _isMouseover;

	private Tweener scaleTween;

	public float ScaleTweenTime
	{
		get;
		set;
	}

	public Color color
	{
		get
		{
			return this._color;
		}
		set
		{
			if (this._color != value)
			{
				this._color = value;
				foreach (GameObject current in this.GetColorObjects())
				{
					ParticleSystem component = current.GetComponent<ParticleSystem>();
					component.main.startColor = this._color;
					component.GetComponent<Renderer>().material.SetColor("_TintColor", this._color);
				}
				this.NormalMode.GetComponent<Renderer>().material.color = this._color;
				this.MouseoverMode.GetComponent<Renderer>().material.color = this._color;
			}
		}
	}

	public List<GameObject> GetColorObjects()
	{
		return new List<GameObject>
		{
			this.Border1,
			this.Border2,
			this.Border1B,
			this.Border2B,
			this.Border1_Mouseover,
			this.Border2_Mouseover
		};
	}

	public void SetMouseover(bool value)
	{
		if (value != this._isMouseover)
		{
			this._isMouseover = value;
			Vector3 one;
			if (value)
			{
				this.NormalMode.GetComponent<Animator>().enabled = false;
				this.Border1B.GetComponent<ParticleSystem>().main.playOnAwake = false;
				this.Border2B.GetComponent<ParticleSystem>().main.playOnAwake = false;
				one = new Vector3(1.1f, 1.1f, 1.1f);
				this.NormalMode.SetActive(false);
				this.MouseoverMode.SetActive(true);
			}
			else
			{
				one = Vector3.one;
				this.NormalMode.SetActive(true);
				this.MouseoverMode.SetActive(false);
			}
			this.killScaleTween();
			this.scaleTween = DOTween.To(new DOGetter<Vector3>(this._SetMouseover_m__0), new DOSetter<Vector3>(this._SetMouseover_m__1), one, this.ScaleTweenTime).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killScaleTween));
		}
	}

	private void killScaleTween()
	{
		TweenUtil.Destroy(ref this.scaleTween);
	}

	private Vector3 _SetMouseover_m__0()
	{
		return base.transform.localScale;
	}

	private void _SetMouseover_m__1(Vector3 valueIn)
	{
		base.transform.localScale = valueIn;
	}
}
