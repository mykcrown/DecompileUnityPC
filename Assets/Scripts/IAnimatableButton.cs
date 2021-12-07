// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IAnimatableButton
{
	Image ButtonBackgroundGet
	{
		get;
	}

	List<Image> AdditionalImagesGet
	{
		get;
	}

	TextMeshProUGUI TextFieldGet
	{
		get;
	}

	CanvasGroup FadeCanvasGet
	{
		get;
	}
}
