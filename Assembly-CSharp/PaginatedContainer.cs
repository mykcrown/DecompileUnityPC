using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000908 RID: 2312
public class PaginatedContainer : GameBehavior
{
	// Token: 0x17000E5F RID: 3679
	// (get) Token: 0x06003C1A RID: 15386 RVA: 0x001169E8 File Offset: 0x00114DE8
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

	// Token: 0x17000E60 RID: 3680
	// (get) Token: 0x06003C1B RID: 15387 RVA: 0x00116A1F File Offset: 0x00114E1F
	// (set) Token: 0x06003C1C RID: 15388 RVA: 0x00116A28 File Offset: 0x00114E28
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
			foreach (Page page in this.Pages)
			{
				page.gameObject.SetActive(false);
			}
			this.currentPageIdx = value;
			this.CurrentPage.gameObject.SetActive(true);
			if (this.PageCounter)
			{
				this.PageCounter.text = (this.currentPageIdx + 1).ToString();
			}
		}
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x00116AF0 File Offset: 0x00114EF0
	public void Start()
	{
		this.Pages = new List<Page>(base.gameObject.GetComponentsInChildren<Page>());
		if (this.Pages.Count == 0)
		{
			Debug.LogError("PaginatedContainer " + this + " has no pages.");
		}
		this.CurrentPageIdx = this.CurrentPageIdx;
	}

	// Token: 0x06003C1E RID: 15390 RVA: 0x00116B44 File Offset: 0x00114F44
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

	// Token: 0x06003C1F RID: 15391 RVA: 0x00116BA4 File Offset: 0x00114FA4
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

	// Token: 0x04002933 RID: 10547
	public bool WrapPages;

	// Token: 0x04002934 RID: 10548
	public Text PageCounter;

	// Token: 0x04002935 RID: 10549
	private List<Page> Pages;

	// Token: 0x04002936 RID: 10550
	private int currentPageIdx;
}
