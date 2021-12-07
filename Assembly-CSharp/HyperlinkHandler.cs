using System;
using TMPro;
using UnityEngine;

// Token: 0x0200093F RID: 2367
public class HyperlinkHandler : IHyperlinkHandler
{
	// Token: 0x17000EDC RID: 3804
	// (get) Token: 0x06003E8F RID: 16015 RVA: 0x0011D178 File Offset: 0x0011B578
	// (set) Token: 0x06003E90 RID: 16016 RVA: 0x0011D180 File Offset: 0x0011B580
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x06003E91 RID: 16017 RVA: 0x0011D18C File Offset: 0x0011B58C
	public bool TryClickLink(TMP_Text text, Vector2 clickPosition)
	{
		string hoveredLink = this.GetHoveredLink(text, clickPosition);
		if (hoveredLink != null)
		{
			Application.OpenURL(hoveredLink);
			return true;
		}
		return false;
	}

	// Token: 0x06003E92 RID: 16018 RVA: 0x0011D1B4 File Offset: 0x0011B5B4
	public string GetHoveredLink(TMP_Text text, Vector2 clickPosition)
	{
		if (text == null)
		{
			return null;
		}
		int num = TMP_TextUtilities.FindIntersectingLink(text, clickPosition, null);
		if (num >= 0)
		{
			TMP_LinkInfo tmp_LinkInfo = text.textInfo.linkInfo[num];
			string empty = string.Empty;
			if (this.config.uiLinkTable.TryGetValue(tmp_LinkInfo.GetLinkID(), out empty))
			{
				return empty;
			}
		}
		return null;
	}
}
