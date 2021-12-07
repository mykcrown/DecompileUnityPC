// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BakedAnimationData : IAnimationDataSource
{
	private class HorizontalDirectionComparer : IEqualityComparer<HorizontalDirection>
	{
		public bool Equals(HorizontalDirection x, HorizontalDirection y)
		{
			return x == y;
		}

		public int GetHashCode(HorizontalDirection obj)
		{
			return (int)obj;
		}
	}

	public Dictionary<string, AnimationFrameData> dataSetForward = new Dictionary<string, AnimationFrameData>();

	public Dictionary<string, AnimationFrameData> dataSetRight = new Dictionary<string, AnimationFrameData>();

	public Dictionary<string, AnimationFrameData> dataSetLeft = new Dictionary<string, AnimationFrameData>();

	public Dictionary<string, AnimationFrameData>[] dataSets = new Dictionary<string, AnimationFrameData>[3];

	public string characterName;

	bool IAnimationDataSource.IsBoneDataAbsolute
	{
		get
		{
			return false;
		}
	}

	public BakedAnimationData()
	{
		this.dataSets[0] = this.dataSetForward;
		this.dataSets[1] = this.dataSetLeft;
		this.dataSets[2] = this.dataSetRight;
	}

	private Dictionary<string, AnimationFrameData> d_getDataSet(HorizontalDirection facing)
	{
		return this.dataSets[(int)facing];
	}

	private Dictionary<int, Dictionary<BodyPart, BoneFrameData>> d_getBoneData(Dictionary<string, AnimationFrameData> dataSet, string animationName)
	{
		return dataSet[animationName].boneData;
	}

	private Dictionary<BodyPart, BoneFrameData> d_getBoneFrameData(Dictionary<int, Dictionary<BodyPart, BoneFrameData>> frames, int gameFrame)
	{
		return frames[gameFrame];
	}

	public BoneFrameData GetBoneFrameData(string animationName, BodyPart bodyPart, int gameFrame, HorizontalDirection facing)
	{
		Dictionary<string, AnimationFrameData> dictionary = this.d_getDataSet(facing);
		if (!dictionary.ContainsKey(animationName))
		{
			UnityEngine.Debug.LogWarningFormat("Missing baked data for animation {0}", new object[]
			{
				animationName
			});
			return default(BoneFrameData);
		}
		Dictionary<int, Dictionary<BodyPart, BoneFrameData>> dictionary2 = this.d_getBoneData(dictionary, animationName);
		if (!dictionary2.ContainsKey(gameFrame))
		{
			UnityEngine.Debug.LogWarningFormat("Missing baked data for animation {0}, frame {1}", new object[]
			{
				animationName,
				gameFrame
			});
			return default(BoneFrameData);
		}
		Dictionary<BodyPart, BoneFrameData> dictionary3 = this.d_getBoneFrameData(dictionary2, gameFrame);
		if (!dictionary3.ContainsKey(bodyPart))
		{
			UnityEngine.Debug.LogWarningFormat("Missing baked data for animation {0}, frame {1}, bodypart {2}", new object[]
			{
				animationName,
				gameFrame,
				bodyPart
			});
			return default(BoneFrameData);
		}
		return dictionary3[bodyPart];
	}

	public bool Equals(BakedAnimationData other)
	{
		if (other.dataSetForward.Count != this.dataSetForward.Count)
		{
			return false;
		}
		foreach (string current in this.dataSetForward.Keys)
		{
			if (!other.dataSetForward.ContainsKey(current))
			{
				bool result = false;
				return result;
			}
			Dictionary<int, Vector3F> rootDeltaData = this.dataSetForward[current].rootDeltaData;
			Dictionary<int, Vector3F> rootDeltaData2 = other.dataSetForward[current].rootDeltaData;
			if (rootDeltaData == null != (rootDeltaData2 == null))
			{
				bool result = false;
				return result;
			}
			if (rootDeltaData != null && rootDeltaData2 != null)
			{
				if (rootDeltaData.Count != rootDeltaData2.Count)
				{
					bool result = false;
					return result;
				}
				foreach (int current2 in rootDeltaData.Keys)
				{
					if (!rootDeltaData2.ContainsKey(current2))
					{
						bool result = false;
						return result;
					}
					if (!rootDeltaData[current2].Equals(rootDeltaData2[current2]))
					{
						bool result = false;
						return result;
					}
				}
			}
			Dictionary<int, Dictionary<BodyPart, BoneFrameData>> boneData = this.dataSetForward[current].boneData;
			Dictionary<int, Dictionary<BodyPart, BoneFrameData>> boneData2 = other.dataSetForward[current].boneData;
			if (boneData.Count != boneData2.Count)
			{
				bool result = false;
				return result;
			}
			foreach (int current3 in boneData.Keys)
			{
				if (!boneData2.ContainsKey(current3))
				{
					bool result = false;
					return result;
				}
				Dictionary<BodyPart, BoneFrameData> dictionary = boneData[current3];
				Dictionary<BodyPart, BoneFrameData> dictionary2 = boneData2[current3];
				if (dictionary.Count != dictionary2.Count)
				{
					bool result = false;
					return result;
				}
				foreach (BodyPart current4 in dictionary.Keys)
				{
					if (!dictionary2.ContainsKey(current4))
					{
						bool result = false;
						return result;
					}
					if (!dictionary2[current4].position.Equals(dictionary[current4].position))
					{
						bool result = false;
						return result;
					}
					if (!dictionary2[current4].rotation.Equals(dictionary[current4].rotation))
					{
						bool result = false;
						return result;
					}
				}
			}
		}
		return true;
	}

	bool IAnimationDataSource.HasRootDeltaData(string animationName)
	{
		if (!this.dataSetForward.ContainsKey(animationName))
		{
			UnityEngine.Debug.LogWarning("Missing baked data for animation " + animationName);
			return false;
		}
		return this.dataSetForward[animationName].HasRootDeltaData;
	}

	Vector3F IAnimationDataSource.GetRootDelta(string animationName, int gameFrame)
	{
		if (!this.dataSetForward.ContainsKey(animationName))
		{
			UnityEngine.Debug.LogWarning("Missing baked data for animation " + animationName);
			return Vector3F.zero;
		}
		return this.dataSetForward[animationName].GetRootDeltaData(gameFrame);
	}

	FixedRect IAnimationDataSource.GetMaxBounds(string animationName)
	{
		if (!this.dataSetForward.ContainsKey(animationName))
		{
			UnityEngine.Debug.LogWarning("Missing baked data for animation " + animationName);
			return default(FixedRect);
		}
		return this.dataSetForward[animationName].maxBounds;
	}
}
