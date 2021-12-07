using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000933 RID: 2355
public interface IAnimatableButton
{
	// Token: 0x17000EC1 RID: 3777
	// (get) Token: 0x06003DEA RID: 15850
	Image ButtonBackgroundGet { get; }

	// Token: 0x17000EC2 RID: 3778
	// (get) Token: 0x06003DEB RID: 15851
	List<Image> AdditionalImagesGet { get; }

	// Token: 0x17000EC3 RID: 3779
	// (get) Token: 0x06003DEC RID: 15852
	TextMeshProUGUI TextFieldGet { get; }

	// Token: 0x17000EC4 RID: 3780
	// (get) Token: 0x06003DED RID: 15853
	CanvasGroup FadeCanvasGet { get; }
}
