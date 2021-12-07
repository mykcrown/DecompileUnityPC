using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008C2 RID: 2242
public class LifePipDisplay : GameBehavior
{
	// Token: 0x06003884 RID: 14468 RVA: 0x001090D0 File Offset: 0x001074D0
	public void Initialize(BattleSettings settings)
	{
		this.settings = settings;
		this.pipAnchor.SetActive(true);
		Image[] componentsInChildren = this.pipAnchor.GetComponentsInChildren<Image>();
		foreach (Image image in componentsInChildren)
		{
			UnityEngine.Object.Destroy(image.gameObject);
		}
	}

	// Token: 0x06003885 RID: 14469 RVA: 0x00109124 File Offset: 0x00107524
	private void Start()
	{
		if (base.gameManager == null)
		{
			return;
		}
		int text_LIVES_CUTOFF = this.TEXT_LIVES_CUTOFF;
		for (int i = 0; i < text_LIVES_CUTOFF; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pipPrefab);
			this.pips.Add(gameObject.GetComponent<Image>());
			gameObject.GetComponent<Image>().sprite = this.pipSprite;
			gameObject.transform.SetParent(this.pipAnchor.transform, false);
			gameObject.gameObject.SetActive(false);
		}
		this.lives = -1;
	}

	// Token: 0x17000DAA RID: 3498
	// (get) Token: 0x06003886 RID: 14470 RVA: 0x001091B4 File Offset: 0x001075B4
	private bool shouldDisplay
	{
		get
		{
			return this.settings.rules == GameRules.Stock;
		}
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x001091C4 File Offset: 0x001075C4
	public void TickFrame()
	{
		if (this.PlayerRef == null)
		{
			return;
		}
		if (this.PlayerRef.Lives != this.lives)
		{
			this.lives = this.PlayerRef.Lives;
			this.updateActiveMode();
			if (this.TextMode.activeSelf)
			{
				this.largeLives.text = "x" + this.lives;
			}
			else
			{
				for (int i = 0; i < this.pips.Count; i++)
				{
					this.pips[i].gameObject.SetActive(i < this.lives);
				}
			}
		}
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x0010927C File Offset: 0x0010767C
	private void updateActiveMode()
	{
		if (this.shouldDisplay)
		{
			if (this.lives > this.TEXT_LIVES_CUTOFF)
			{
				this.pipAnchor.SetActive(false);
				this.TextMode.SetActive(true);
			}
			else
			{
				this.pipAnchor.SetActive(true);
				this.TextMode.SetActive(false);
			}
		}
		else
		{
			this.pipAnchor.SetActive(false);
			this.TextMode.SetActive(false);
		}
	}

	// Token: 0x040026DC RID: 9948
	public GameObject pipAnchor;

	// Token: 0x040026DD RID: 9949
	public GameObject pipPrefab;

	// Token: 0x040026DE RID: 9950
	public Text largeLives;

	// Token: 0x040026DF RID: 9951
	public Sprite pipSprite;

	// Token: 0x040026E0 RID: 9952
	public GameObject TextMode;

	// Token: 0x040026E1 RID: 9953
	public PlayerReference PlayerRef;

	// Token: 0x040026E2 RID: 9954
	public int TEXT_LIVES_CUTOFF = 5;

	// Token: 0x040026E3 RID: 9955
	private BattleSettings settings;

	// Token: 0x040026E4 RID: 9956
	private int lives = -1;

	// Token: 0x040026E5 RID: 9957
	private List<Image> pips = new List<Image>();
}
