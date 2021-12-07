// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BakedAnimationSerializer
{
	private static readonly Fixed[] yawDirectionTable = new Fixed[]
	{
		0,
		-90,
		90
	};

	public static string GetCharacterBakedDataAssetPath(string characterName, bool fromResources = true)
	{
		string text = "Game/Characters/BakedAnimationData/BakedAnims_" + characterName;
		if (!fromResources)
		{
			return "Assets/Wavedash/Resources/" + text + ".bytes";
		}
		return text;
	}

	public static void Serialize(Stream stream, BakedAnimationData data)
	{
		BinaryWriter writer = new BinaryWriter(stream);
		BakedAnimationSerializer.writeDataSet(writer, data.dataSetForward);
	}

	public static BakedAnimationData Deserialize(Stream stream)
	{
		BakedAnimationData bakedAnimationData = new BakedAnimationData();
		try
		{
			BinaryReader reader = new BinaryReader(stream);
			BakedAnimationSerializer.readDataSet(reader, bakedAnimationData.dataSetForward);
			BakedAnimationSerializer.calculateDataSet(bakedAnimationData.dataSetForward, HorizontalDirection.Right, bakedAnimationData.dataSetRight);
			BakedAnimationSerializer.calculateDataSet(bakedAnimationData.dataSetForward, HorizontalDirection.Left, bakedAnimationData.dataSetLeft);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Error deserializing baked data:" + ex.ToString());
			throw ex;
		}
		return bakedAnimationData;
	}

	private static void writeDataSet(BinaryWriter writer, Dictionary<string, AnimationFrameData> dataSet)
	{
		writer.Write((short)dataSet.Count);
		foreach (string current in dataSet.Keys)
		{
			writer.Write(current);
			AnimationFrameData animationFrameData = dataSet[current];
			writer.Write(animationFrameData.reversesFacing);
			BakedAnimationSerializer.writeFixed(writer, animationFrameData.maxBounds.Top);
			BakedAnimationSerializer.writeFixed(writer, animationFrameData.maxBounds.Bottom);
			BakedAnimationSerializer.writeFixed(writer, animationFrameData.maxBounds.Left);
			BakedAnimationSerializer.writeFixed(writer, animationFrameData.maxBounds.Right);
			Dictionary<int, Vector3F> rootDeltaData = animationFrameData.rootDeltaData;
			short value = (rootDeltaData != null) ? ((short)rootDeltaData.Count) : 0;
			writer.Write(value);
			if (rootDeltaData != null)
			{
				foreach (int current2 in rootDeltaData.Keys)
				{
					writer.Write((short)current2);
					BakedAnimationSerializer.writeFixed(writer, rootDeltaData[current2].x);
					BakedAnimationSerializer.writeFixed(writer, rootDeltaData[current2].y);
					BakedAnimationSerializer.writeFixed(writer, rootDeltaData[current2].z);
				}
			}
			Dictionary<int, Dictionary<BodyPart, BoneFrameData>> boneData = animationFrameData.boneData;
			writer.Write((short)boneData.Count);
			foreach (int current3 in boneData.Keys)
			{
				writer.Write((short)current3);
				Dictionary<BodyPart, BoneFrameData> dictionary = boneData[current3];
				writer.Write((short)dictionary.Count);
				foreach (BodyPart current4 in dictionary.Keys)
				{
					writer.Write((short)current4);
					BakedAnimationSerializer.writeFixed(writer, dictionary[current4].position.x);
					BakedAnimationSerializer.writeFixed(writer, dictionary[current4].position.y);
					BakedAnimationSerializer.writeFixed(writer, dictionary[current4].position.z);
					BakedAnimationSerializer.writeFixed(writer, dictionary[current4].rotation.x);
					BakedAnimationSerializer.writeFixed(writer, dictionary[current4].rotation.y);
					BakedAnimationSerializer.writeFixed(writer, dictionary[current4].rotation.z);
					BakedAnimationSerializer.writeFixed(writer, dictionary[current4].rotation.w);
				}
			}
		}
	}

	private static Dictionary<string, AnimationFrameData> readDataSet(BinaryReader reader, Dictionary<string, AnimationFrameData> dataSetOut)
	{
		dataSetOut.Clear();
		int num = (int)reader.ReadInt16();
		for (int i = 0; i < num; i++)
		{
			string key = reader.ReadString();
			AnimationFrameData animationFrameData = new AnimationFrameData();
			dataSetOut.Add(key, animationFrameData);
			animationFrameData.reversesFacing = reader.ReadBoolean();
			FixedRect maxBounds = new FixedRect(BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader));
			animationFrameData.maxBounds = maxBounds;
			int num2 = (int)reader.ReadInt16();
			if (num2 != 0)
			{
				Dictionary<int, Vector3F> dictionary = new Dictionary<int, Vector3F>(num2);
				for (int j = 0; j < num2; j++)
				{
					int key2 = (int)reader.ReadInt16();
					Vector3F value = new Vector3F(BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader));
					dictionary.Add(key2, value);
				}
				animationFrameData.rootDeltaData = dictionary;
			}
			int num3 = (int)reader.ReadInt16();
			Dictionary<int, Dictionary<BodyPart, BoneFrameData>> dictionary2 = new Dictionary<int, Dictionary<BodyPart, BoneFrameData>>(num3);
			animationFrameData.boneData = dictionary2;
			for (int k = 0; k < num3; k++)
			{
				int key3 = (int)reader.ReadInt16();
				int num4 = (int)reader.ReadInt16();
				Dictionary<BodyPart, BoneFrameData> dictionary3 = new Dictionary<BodyPart, BoneFrameData>(num4, default(BodyPartComparer));
				dictionary2.Add(key3, dictionary3);
				for (int l = 0; l < num4; l++)
				{
					BodyPart key4 = (BodyPart)reader.ReadInt16();
					Vector3F position = new Vector3F(BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader));
					QuaternionF rotation = new QuaternionF(BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader), BakedAnimationSerializer.readFixed(reader));
					dictionary3.Add(key4, new BoneFrameData(position, rotation));
				}
			}
		}
		return dataSetOut;
	}

	private static void calculateDataSet(Dictionary<string, AnimationFrameData> source, HorizontalDirection facing, Dictionary<string, AnimationFrameData> target)
	{
		foreach (KeyValuePair<string, AnimationFrameData> current in source)
		{
			string key = current.Key;
			AnimationFrameData value = current.Value;
			AnimationFrameData animationFrameData = new AnimationFrameData();
			if (value.rootDeltaData != null)
			{
				animationFrameData.rootDeltaData = new Dictionary<int, Vector3F>(value.rootDeltaData);
			}
			animationFrameData.boneData = new Dictionary<int, Dictionary<BodyPart, BoneFrameData>>(value.boneData.Count);
			HorizontalDirection horizontalDirection = facing;
			if (value.reversesFacing)
			{
				horizontalDirection = InputUtils.GetOppositeDirection(facing);
			}
			animationFrameData.reversesFacing = value.reversesFacing;
			QuaternionF quaternionF = QuaternionF.Euler(0, BakedAnimationSerializer.yawDirectionTable[(int)horizontalDirection], 0);
			Fixed @fixed = -Fixed.MaxValue;
			Fixed fixed2 = Fixed.MaxValue;
			Fixed fixed3 = -Fixed.MaxValue;
			Fixed b = Fixed.MaxValue;
			foreach (KeyValuePair<int, Dictionary<BodyPart, BoneFrameData>> current2 in value.boneData)
			{
				int key2 = current2.Key;
				Dictionary<BodyPart, BoneFrameData> value2 = current2.Value;
				animationFrameData.boneData[key2] = new Dictionary<BodyPart, BoneFrameData>(value2.Count, default(BodyPartComparer));
				Dictionary<BodyPart, BoneFrameData> dictionary = animationFrameData.boneData[key2];
				foreach (KeyValuePair<BodyPart, BoneFrameData> current3 in value2)
				{
					BodyPart key3 = current3.Key;
					BoneFrameData value3 = current3.Value;
					Vector3F position = quaternionF * value3.position;
					QuaternionF rotation = quaternionF * value3.rotation;
					dictionary[key3] = new BoneFrameData(position, rotation);
					@fixed = FixedMath.Max(position.z, @fixed);
					fixed3 = FixedMath.Max(position.y, fixed3);
					fixed2 = FixedMath.Min(position.z, fixed2);
					b = FixedMath.Min(position.y, b);
				}
			}
			@fixed = FixedMath.Max(FixedMath.Abs(@fixed), FixedMath.Max(FixedMath.Abs(fixed2), FixedMath.Max(FixedMath.Abs(fixed3), FixedMath.Abs(fixed3))));
			@fixed *= FixedMath.SqrtTwo;
			animationFrameData.maxBounds = new FixedRect(default(Vector2F), new Vector2F(@fixed * 2, @fixed * 2));
			target[key] = animationFrameData;
		}
	}

	private static void writeFixed(BinaryWriter writer, Fixed value)
	{
		writer.Write((int)value.RawValue);
	}

	private static Fixed readFixed(BinaryReader reader)
	{
		return Fixed.Create((long)reader.ReadInt32(), false);
	}
}
