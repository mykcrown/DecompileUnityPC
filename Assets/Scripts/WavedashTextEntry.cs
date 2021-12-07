// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class WavedashTextEntry : MonoBehaviour
{
	public WavedashTMProInput TargetInput;

	public bool AutoCapitalize;

	public Image HighlightImage;

	public Sprite HighlightSprite;

	public GameObject HighlightGameObject;

	public Action EndEditCallback
	{
		get
		{
			return this.TargetInput.EndEditCallback;
		}
		set
		{
			this.TargetInput.EndEditCallback = value;
		}
	}

	public Action ValueChangedCallback
	{
		get
		{
			return this.TargetInput.ValueChangedCallback;
		}
		set
		{
			this.TargetInput.ValueChangedCallback = value;
		}
	}

	public Action TabCallback
	{
		get
		{
			return this.TargetInput.TabCallback;
		}
		set
		{
			this.TargetInput.TabCallback = value;
		}
	}

	public Action EnterCallback
	{
		get
		{
			return this.TargetInput.EnterCallback;
		}
		set
		{
			this.TargetInput.EnterCallback = value;
		}
	}

	public string Text
	{
		get
		{
			return this.TargetInput.text;
		}
		set
		{
			this.TargetInput.text = value;
		}
	}

	public void Awake()
	{
		this.TargetInput.SetController(this);
	}

	public virtual void UpdateHighlightState()
	{
		if (!this.TargetInput.Activated)
		{
			if (this.HighlightImage != null)
			{
				this.HighlightImage.overrideSprite = null;
			}
			if (this.HighlightGameObject != null)
			{
				this.HighlightGameObject.SetActive(false);
			}
		}
		else
		{
			if (this.HighlightImage != null)
			{
				this.HighlightImage.overrideSprite = this.HighlightSprite;
			}
			if (this.HighlightGameObject != null)
			{
				this.HighlightGameObject.SetActive(true);
			}
		}
	}
}
