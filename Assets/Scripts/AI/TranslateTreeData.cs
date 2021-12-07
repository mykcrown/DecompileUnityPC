// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AI
{
	public class TranslateTreeData : ITranslateTreeData
	{
		public Composite Translate(CompositeNodeData rootData)
		{
			return this.createComposite(rootData);
		}

		private Composite createComposite(CompositeNodeData data)
		{
			Type type = null;
			Composite composite;
			if (string.IsNullOrEmpty(data.typeName) || data.typeName == "base")
			{
				composite = new Composite();
			}
			else
			{
				type = Type.GetType(data.typeName);
				if (type == null)
				{
					composite = new Composite();
				}
				else
				{
					composite = (Activator.CreateInstance(type) as Composite);
				}
			}
			composite.children = new List<INode>();
			int num = 0;
			CompositeNodeData.ChildrenFileData[] childrenFileDatas = data.childrenFileDatas;
			for (int i = 0; i < childrenFileDatas.Length; i++)
			{
				CompositeNodeData.ChildrenFileData childrenFileData = childrenFileDatas[i];
				int shuffleWeight = childrenFileData.shuffleWeight;
				ScriptableObjectFile file = childrenFileData.file;
				if (file.obj is CompositeNodeData)
				{
					Composite composite2 = this.createComposite(file.obj as CompositeNodeData);
					composite2.shuffleWeight = shuffleWeight;
					composite.children.Add(composite2);
				}
				else
				{
					Leaf leaf = this.createLeafNode(file.obj as LeafNodeData);
					leaf.shuffleWeight = shuffleWeight;
					composite.children.Add(leaf);
				}
				num++;
			}
			this.addNestedData(type, data, composite);
			composite.method = data.method;
			composite.shuffle = data.shuffle;
			return composite;
		}

		private Leaf createLeafNode(LeafNodeData data)
		{
			Leaf leaf;
			if (string.IsNullOrEmpty(data.typeName))
			{
				leaf = new Leaf(0);
				UnityEngine.Debug.LogError("INVALID LEAF DATA must have a type");
			}
			else
			{
				Type type = Type.GetType(data.typeName);
				if (type == null)
				{
					leaf = new Leaf(0);
					UnityEngine.Debug.LogError("INVALID LEAF DATA, type " + data.typeName + " not found");
				}
				else
				{
					ConstructorInfo constructorInfo = null;
					ConstructorInfo[] constructors = type.GetConstructors();
					for (int i = 0; i < constructors.Length; i++)
					{
						ConstructorInfo constructorInfo2 = constructors[i];
						constructorInfo = constructorInfo2;
					}
					int num = constructorInfo.GetParameters().Length;
					if (num == 1)
					{
						leaf = (Activator.CreateInstance(type, new object[]
						{
							data.frameDuration
						}) as Leaf);
					}
					else
					{
						leaf = (Activator.CreateInstance(type) as Leaf);
					}
					this.addNestedData(type, data, leaf);
				}
			}
			return leaf;
		}

		private FieldInfo getNestedDataField(Type theType)
		{
			if (theType != null)
			{
				Type type = null;
				Type[] nestedTypes = theType.GetNestedTypes();
				for (int i = 0; i < nestedTypes.Length; i++)
				{
					Type type2 = nestedTypes[i];
					if (type2.IsSerializable)
					{
						type = type2;
						break;
					}
				}
				if (type != null)
				{
					FieldInfo[] fields = theType.GetFields();
					for (int j = 0; j < fields.Length; j++)
					{
						FieldInfo fieldInfo = fields[j];
						if (fieldInfo.FieldType == type)
						{
							return fieldInfo;
						}
					}
				}
			}
			return null;
		}

		private void addNestedData(Type theType, IDataNode data, object targetObj)
		{
			FieldInfo nestedDataField = this.getNestedDataField(theType);
			if (nestedDataField != null)
			{
				object value = nestedDataField.GetValue(targetObj);
				Type fieldType = nestedDataField.FieldType;
				foreach (KeyValuePair<string, float> current in data.FloatData)
				{
					string key = current.Key;
					float value2 = current.Value;
					FieldInfo field = fieldType.GetField(key);
					if (field != null)
					{
						field.SetValue(value, value2);
					}
				}
				foreach (KeyValuePair<string, int> current2 in data.IntData)
				{
					string key2 = current2.Key;
					int value3 = current2.Value;
					FieldInfo field2 = fieldType.GetField(key2);
					if (field2 != null)
					{
						field2.SetValue(value, value3);
					}
				}
				foreach (KeyValuePair<string, Fixed> current3 in data.FixedData)
				{
					string key3 = current3.Key;
					Fixed value4 = current3.Value;
					FieldInfo field3 = fieldType.GetField(key3);
					if (field3 != null)
					{
						field3.SetValue(value, value4);
					}
				}
				foreach (KeyValuePair<string, bool> current4 in data.BoolData)
				{
					string key4 = current4.Key;
					bool value5 = current4.Value;
					FieldInfo field4 = fieldType.GetField(key4);
					if (field4 != null)
					{
						field4.SetValue(value, value5);
					}
				}
			}
		}
	}
}
