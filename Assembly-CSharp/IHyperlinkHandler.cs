using System;
using TMPro;
using UnityEngine;

// Token: 0x02000940 RID: 2368
public interface IHyperlinkHandler
{
	// Token: 0x06003E93 RID: 16019
	bool TryClickLink(TMP_Text text, Vector2 clickPosition);

	// Token: 0x06003E94 RID: 16020
	string GetHoveredLink(TMP_Text text, Vector2 clickPosition);
}
