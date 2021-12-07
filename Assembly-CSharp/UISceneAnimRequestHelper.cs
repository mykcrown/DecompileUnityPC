using System;
using System.Collections.Generic;

// Token: 0x02000A6C RID: 2668
public static class UISceneAnimRequestHelper
{
	// Token: 0x06004D9F RID: 19871 RVA: 0x00147A58 File Offset: 0x00145E58
	public static List<UISceneCharacterAnimRequest> GetAnimRequests(List<WavedashAnimationData> animDatas)
	{
		List<UISceneCharacterAnimRequest> list = new List<UISceneCharacterAnimRequest>();
		foreach (WavedashAnimationData animData in animDatas)
		{
			list.Add(new UISceneCharacterAnimRequest
			{
				type = UISceneCharacterAnimRequest.AnimRequestType.AnimData,
				animData = animData
			});
		}
		return list;
	}
}
