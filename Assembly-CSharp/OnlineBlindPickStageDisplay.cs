using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009B8 RID: 2488
public class OnlineBlindPickStageDisplay : MonoBehaviour
{
	// Token: 0x17001067 RID: 4199
	// (get) Token: 0x0600453D RID: 17725 RVA: 0x001303E9 File Offset: 0x0012E7E9
	// (set) Token: 0x0600453E RID: 17726 RVA: 0x001303F1 File Offset: 0x0012E7F1
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17001068 RID: 4200
	// (get) Token: 0x0600453F RID: 17727 RVA: 0x001303FA File Offset: 0x0012E7FA
	// (set) Token: 0x06004540 RID: 17728 RVA: 0x00130402 File Offset: 0x0012E802
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17001069 RID: 4201
	// (get) Token: 0x06004541 RID: 17729 RVA: 0x0013040B File Offset: 0x0012E80B
	// (set) Token: 0x06004542 RID: 17730 RVA: 0x00130413 File Offset: 0x0012E813
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x1700106A RID: 4202
	// (get) Token: 0x06004543 RID: 17731 RVA: 0x0013041C File Offset: 0x0012E81C
	// (set) Token: 0x06004544 RID: 17732 RVA: 0x00130424 File Offset: 0x0012E824
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x06004545 RID: 17733 RVA: 0x00130430 File Offset: 0x0012E830
	public void Init(StageData stageData)
	{
		this.StagePortrait.overrideSprite = stageData.smallPortrait;
		string text = this.localization.GetText("gameData.stageSelect." + stageData.name);
		if (stageData.isTemporary)
		{
			text = text + " <#e01c1c>" + this.localization.GetText("gameData.stageSelect.temp") + "</color>";
		}
		if (stageData.worldIcon == null)
		{
			this.WithWorldIcon.SetActive(false);
			this.WithoutWorldIcon.SetActive(true);
			this.WithoutWorldIconTitle.text = text;
		}
		else
		{
			this.WithWorldIcon.SetActive(true);
			this.WithoutWorldIcon.SetActive(false);
			this.WithWorldIconTitle.text = text;
			this.Icon.sprite = stageData.worldIcon;
		}
	}

	// Token: 0x06004546 RID: 17734 RVA: 0x00130508 File Offset: 0x0012E908
	public void SetMode(OnlineBlindPickStageDisplayMode mode)
	{
		if (this.mode != mode)
		{
			this.mode = mode;
			this.DarkOverlay.gameObject.SetActive(false);
			this.StrikeIcon.gameObject.SetActive(false);
			if (mode != OnlineBlindPickStageDisplayMode.Current)
			{
				if (mode != OnlineBlindPickStageDisplayMode.Played)
				{
					if (mode == OnlineBlindPickStageDisplayMode.Upcoming)
					{
						this.DarkOverlay.gameObject.SetActive(true);
					}
				}
				else
				{
					this.DarkOverlay.gameObject.SetActive(true);
					this.StrikeIcon.gameObject.SetActive(true);
				}
			}
		}
	}

	// Token: 0x04002DFF RID: 11775
	public Image StagePortrait;

	// Token: 0x04002E00 RID: 11776
	public TextMeshProUGUI WithWorldIconTitle;

	// Token: 0x04002E01 RID: 11777
	public TextMeshProUGUI WithoutWorldIconTitle;

	// Token: 0x04002E02 RID: 11778
	public Image Icon;

	// Token: 0x04002E03 RID: 11779
	public GameObject WithWorldIcon;

	// Token: 0x04002E04 RID: 11780
	public GameObject WithoutWorldIcon;

	// Token: 0x04002E05 RID: 11781
	public GameObject DarkOverlay;

	// Token: 0x04002E06 RID: 11782
	public GameObject StrikeIcon;

	// Token: 0x04002E07 RID: 11783
	private OnlineBlindPickStageDisplayMode mode;
}
