using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000957 RID: 2391
public class WavedashTextEntry : MonoBehaviour
{
	// Token: 0x06003F79 RID: 16249 RVA: 0x00110FCD File Offset: 0x0010F3CD
	public void Awake()
	{
		this.TargetInput.SetController(this);
	}

	// Token: 0x17000F08 RID: 3848
	// (get) Token: 0x06003F7A RID: 16250 RVA: 0x00110FDB File Offset: 0x0010F3DB
	// (set) Token: 0x06003F7B RID: 16251 RVA: 0x00110FE8 File Offset: 0x0010F3E8
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

	// Token: 0x17000F09 RID: 3849
	// (get) Token: 0x06003F7C RID: 16252 RVA: 0x00110FF6 File Offset: 0x0010F3F6
	// (set) Token: 0x06003F7D RID: 16253 RVA: 0x00111003 File Offset: 0x0010F403
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

	// Token: 0x17000F0A RID: 3850
	// (get) Token: 0x06003F7E RID: 16254 RVA: 0x00111011 File Offset: 0x0010F411
	// (set) Token: 0x06003F7F RID: 16255 RVA: 0x0011101E File Offset: 0x0010F41E
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

	// Token: 0x17000F0B RID: 3851
	// (get) Token: 0x06003F80 RID: 16256 RVA: 0x0011102C File Offset: 0x0010F42C
	// (set) Token: 0x06003F81 RID: 16257 RVA: 0x00111039 File Offset: 0x0010F439
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

	// Token: 0x06003F82 RID: 16258 RVA: 0x00111048 File Offset: 0x0010F448
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

	// Token: 0x17000F0C RID: 3852
	// (get) Token: 0x06003F83 RID: 16259 RVA: 0x001110E3 File Offset: 0x0010F4E3
	// (set) Token: 0x06003F84 RID: 16260 RVA: 0x001110F0 File Offset: 0x0010F4F0
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

	// Token: 0x04002B11 RID: 11025
	public WavedashTMProInput TargetInput;

	// Token: 0x04002B12 RID: 11026
	public bool AutoCapitalize;

	// Token: 0x04002B13 RID: 11027
	public Image HighlightImage;

	// Token: 0x04002B14 RID: 11028
	public Sprite HighlightSprite;

	// Token: 0x04002B15 RID: 11029
	public GameObject HighlightGameObject;
}
