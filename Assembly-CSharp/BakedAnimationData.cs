using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200033F RID: 831
public class BakedAnimationData : IAnimationDataSource
{
	// Token: 0x0600119B RID: 4507 RVA: 0x00065CC0 File Offset: 0x000640C0
	public BakedAnimationData()
	{
		this.dataSets[0] = this.dataSetForward;
		this.dataSets[1] = this.dataSetLeft;
		this.dataSets[2] = this.dataSetRight;
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x0600119C RID: 4508 RVA: 0x00065D2A File Offset: 0x0006412A
	bool IAnimationDataSource.IsBoneDataAbsolute
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600119D RID: 4509 RVA: 0x00065D2D File Offset: 0x0006412D
	private Dictionary<string, AnimationFrameData> d_getDataSet(HorizontalDirection facing)
	{
		return this.dataSets[(int)facing];
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x00065D37 File Offset: 0x00064137
	private Dictionary<int, Dictionary<BodyPart, BoneFrameData>> d_getBoneData(Dictionary<string, AnimationFrameData> dataSet, string animationName)
	{
		return dataSet[animationName].boneData;
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x00065D45 File Offset: 0x00064145
	private Dictionary<BodyPart, BoneFrameData> d_getBoneFrameData(Dictionary<int, Dictionary<BodyPart, BoneFrameData>> frames, int gameFrame)
	{
		return frames[gameFrame];
	}

	// Token: 0x060011A0 RID: 4512 RVA: 0x00065D50 File Offset: 0x00064150
	public BoneFrameData GetBoneFrameData(string animationName, BodyPart bodyPart, int gameFrame, HorizontalDirection facing)
	{
		Dictionary<string, AnimationFrameData> dictionary = this.d_getDataSet(facing);
		if (!dictionary.ContainsKey(animationName))
		{
			Debug.LogWarningFormat("Missing baked data for animation {0}", new object[]
			{
				animationName
			});
			return default(BoneFrameData);
		}
		Dictionary<int, Dictionary<BodyPart, BoneFrameData>> dictionary2 = this.d_getBoneData(dictionary, animationName);
		if (!dictionary2.ContainsKey(gameFrame))
		{
			Debug.LogWarningFormat("Missing baked data for animation {0}, frame {1}", new object[]
			{
				animationName,
				gameFrame
			});
			return default(BoneFrameData);
		}
		Dictionary<BodyPart, BoneFrameData> dictionary3 = this.d_getBoneFrameData(dictionary2, gameFrame);
		if (!dictionary3.ContainsKey(bodyPart))
		{
			Debug.LogWarningFormat("Missing baked data for animation {0}, frame {1}, bodypart {2}", new object[]
			{
				animationName,
				gameFrame,
				bodyPart
			});
			return default(BoneFrameData);
		}
		return dictionary3[bodyPart];
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x00065E1C File Offset: 0x0006421C
	public bool Equals(BakedAnimationData other)
	{
		if (other.dataSetForward.Count != this.dataSetForward.Count)
		{
			return false;
		}
		foreach (string key in this.dataSetForward.Keys)
		{
			if (!other.dataSetForward.ContainsKey(key))
			{
				return false;
			}
			Dictionary<int, Vector3F> rootDeltaData = this.dataSetForward[key].rootDeltaData;
			Dictionary<int, Vector3F> rootDeltaData2 = other.dataSetForward[key].rootDeltaData;
			if (rootDeltaData == null != (rootDeltaData2 == null))
			{
				return false;
			}
			if (rootDeltaData != null && rootDeltaData2 != null)
			{
				if (rootDeltaData.Count != rootDeltaData2.Count)
				{
					return false;
				}
				foreach (int key2 in rootDeltaData.Keys)
				{
					if (!rootDeltaData2.ContainsKey(key2))
					{
						return false;
					}
					if (!rootDeltaData[key2].Equals(rootDeltaData2[key2]))
					{
						return false;
					}
				}
			}
			Dictionary<int, Dictionary<BodyPart, BoneFrameData>> boneData = this.dataSetForward[key].boneData;
			Dictionary<int, Dictionary<BodyPart, BoneFrameData>> boneData2 = other.dataSetForward[key].boneData;
			if (boneData.Count != boneData2.Count)
			{
				return false;
			}
			foreach (int key3 in boneData.Keys)
			{
				if (!boneData2.ContainsKey(key3))
				{
					return false;
				}
				Dictionary<BodyPart, BoneFrameData> dictionary = boneData[key3];
				Dictionary<BodyPart, BoneFrameData> dictionary2 = boneData2[key3];
				if (dictionary.Count != dictionary2.Count)
				{
					return false;
				}
				foreach (BodyPart key4 in dictionary.Keys)
				{
					if (!dictionary2.ContainsKey(key4))
					{
						return false;
					}
					if (!dictionary2[key4].position.Equals(dictionary[key4].position))
					{
						return false;
					}
					if (!dictionary2[key4].rotation.Equals(dictionary[key4].rotation))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x0006617C File Offset: 0x0006457C
	bool IAnimationDataSource.HasRootDeltaData(string animationName)
	{
		if (!this.dataSetForward.ContainsKey(animationName))
		{
			Debug.LogWarning("Missing baked data for animation " + animationName);
			return false;
		}
		return this.dataSetForward[animationName].HasRootDeltaData;
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x000661B2 File Offset: 0x000645B2
	Vector3F IAnimationDataSource.GetRootDelta(string animationName, int gameFrame)
	{
		if (!this.dataSetForward.ContainsKey(animationName))
		{
			Debug.LogWarning("Missing baked data for animation " + animationName);
			return Vector3F.zero;
		}
		return this.dataSetForward[animationName].GetRootDeltaData(gameFrame);
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x000661F0 File Offset: 0x000645F0
	FixedRect IAnimationDataSource.GetMaxBounds(string animationName)
	{
		if (!this.dataSetForward.ContainsKey(animationName))
		{
			Debug.LogWarning("Missing baked data for animation " + animationName);
			return default(FixedRect);
		}
		return this.dataSetForward[animationName].maxBounds;
	}

	// Token: 0x04000B40 RID: 2880
	public Dictionary<string, AnimationFrameData> dataSetForward = new Dictionary<string, AnimationFrameData>();

	// Token: 0x04000B41 RID: 2881
	public Dictionary<string, AnimationFrameData> dataSetRight = new Dictionary<string, AnimationFrameData>();

	// Token: 0x04000B42 RID: 2882
	public Dictionary<string, AnimationFrameData> dataSetLeft = new Dictionary<string, AnimationFrameData>();

	// Token: 0x04000B43 RID: 2883
	public Dictionary<string, AnimationFrameData>[] dataSets = new Dictionary<string, AnimationFrameData>[3];

	// Token: 0x04000B44 RID: 2884
	public string characterName;

	// Token: 0x02000340 RID: 832
	private class HorizontalDirectionComparer : IEqualityComparer<HorizontalDirection>
	{
		// Token: 0x060011A6 RID: 4518 RVA: 0x00066241 File Offset: 0x00064641
		public bool Equals(HorizontalDirection x, HorizontalDirection y)
		{
			return x == y;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00066247 File Offset: 0x00064647
		public int GetHashCode(HorizontalDirection obj)
		{
			return (int)obj;
		}
	}
}
