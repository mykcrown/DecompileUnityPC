// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public static class UISceneAnimRequestHelper
{
	public static List<UISceneCharacterAnimRequest> GetAnimRequests(List<WavedashAnimationData> animDatas)
	{
		List<UISceneCharacterAnimRequest> list = new List<UISceneCharacterAnimRequest>();
		foreach (WavedashAnimationData current in animDatas)
		{
			list.Add(new UISceneCharacterAnimRequest
			{
				type = UISceneCharacterAnimRequest.AnimRequestType.AnimData,
				animData = current
			});
		}
		return list;
	}
}
