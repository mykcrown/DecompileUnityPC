// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class HyperlinkHandler : IHyperlinkHandler
{
	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

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

	public string GetHoveredLink(TMP_Text text, Vector2 clickPosition)
	{
		if (text == null)
		{
			return null;
		}
		int num = TMP_TextUtilities.FindIntersectingLink(text, clickPosition, null);
		if (num >= 0)
		{
			TMP_LinkInfo tMP_LinkInfo = text.textInfo.linkInfo[num];
			string empty = string.Empty;
			if (this.config.uiLinkTable.TryGetValue(tMP_LinkInfo.GetLinkID(), out empty))
			{
				return empty;
			}
		}
		return null;
	}
}
