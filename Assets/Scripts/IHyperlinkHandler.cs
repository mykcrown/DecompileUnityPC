// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public interface IHyperlinkHandler
{
	bool TryClickLink(TMP_Text text, Vector2 clickPosition);

	string GetHoveredLink(TMP_Text text, Vector2 clickPosition);
}
