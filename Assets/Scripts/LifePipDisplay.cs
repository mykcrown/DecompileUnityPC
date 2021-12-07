// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePipDisplay : GameBehavior
{
	public GameObject pipAnchor;

	public GameObject pipPrefab;

	public Text largeLives;

	public Sprite pipSprite;

	public GameObject TextMode;

	public PlayerReference PlayerRef;

	public int TEXT_LIVES_CUTOFF = 5;

	private BattleSettings settings;

	private int lives = -1;

	private List<Image> pips = new List<Image>();

	private bool shouldDisplay
	{
		get
		{
			return this.settings.rules == GameRules.Stock;
		}
	}

	public void Initialize(BattleSettings settings)
	{
		this.settings = settings;
		this.pipAnchor.SetActive(true);
		Image[] componentsInChildren = this.pipAnchor.GetComponentsInChildren<Image>();
		Image[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Image image = array[i];
			UnityEngine.Object.Destroy(image.gameObject);
		}
	}

	private void Start()
	{
		if (base.gameManager == null)
		{
			return;
		}
		int tEXT_LIVES_CUTOFF = this.TEXT_LIVES_CUTOFF;
		for (int i = 0; i < tEXT_LIVES_CUTOFF; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pipPrefab);
			this.pips.Add(gameObject.GetComponent<Image>());
			gameObject.GetComponent<Image>().sprite = this.pipSprite;
			gameObject.transform.SetParent(this.pipAnchor.transform, false);
			gameObject.gameObject.SetActive(false);
		}
		this.lives = -1;
	}

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
}
