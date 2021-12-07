// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaginatedContainer : GameBehavior
{
	public bool WrapPages;

	public Text PageCounter;

	private List<Page> Pages;

	private int currentPageIdx;

	public Page CurrentPage
	{
		get
		{
			if (this.currentPageIdx < 0 || this.currentPageIdx >= this.Pages.Count)
			{
				return null;
			}
			return this.Pages[this.CurrentPageIdx];
		}
	}

	public int CurrentPageIdx
	{
		get
		{
			return this.currentPageIdx;
		}
		set
		{
			if (value < 0 || value >= this.Pages.Count)
			{
				throw new IndexOutOfRangeException();
			}
			foreach (Page current in this.Pages)
			{
				current.gameObject.SetActive(false);
			}
			this.currentPageIdx = value;
			this.CurrentPage.gameObject.SetActive(true);
			if (this.PageCounter)
			{
				this.PageCounter.text = (this.currentPageIdx + 1).ToString();
			}
		}
	}

	public void Start()
	{
		this.Pages = new List<Page>(base.gameObject.GetComponentsInChildren<Page>());
		if (this.Pages.Count == 0)
		{
			UnityEngine.Debug.LogError("PaginatedContainer " + this + " has no pages.");
		}
		this.CurrentPageIdx = this.CurrentPageIdx;
	}

	public void NextPage()
	{
		if (this.WrapPages)
		{
			this.CurrentPageIdx = (this.CurrentPageIdx + 1) % this.Pages.Count;
		}
		else
		{
			this.CurrentPageIdx = Math.Max(0, Math.Min(this.Pages.Count - 1, this.CurrentPageIdx + 1));
		}
	}

	public void PreviousPage()
	{
		if (this.WrapPages)
		{
			this.CurrentPageIdx = (this.CurrentPageIdx + this.Pages.Count - 1) % this.Pages.Count;
		}
		else
		{
			this.CurrentPageIdx = Math.Max(0, Math.Min(this.Pages.Count - 1, this.CurrentPageIdx - 1));
		}
	}
}
