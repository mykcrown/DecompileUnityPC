using System;
using System.Collections.Generic;
using System.Reflection;
using FixedPoint;
using UnityEngine;

namespace AI
{
	// Token: 0x0200033B RID: 827
	public class TranslateTreeData : ITranslateTreeData
	{
		// Token: 0x06001192 RID: 4498 RVA: 0x000656A0 File Offset: 0x00063AA0
		public Composite Translate(CompositeNodeData rootData)
		{
			return this.createComposite(rootData);
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x000656AC File Offset: 0x00063AAC
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
			foreach (CompositeNodeData.ChildrenFileData childrenFileData in data.childrenFileDatas)
			{
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

		// Token: 0x06001194 RID: 4500 RVA: 0x000657F0 File Offset: 0x00063BF0
		private Leaf createLeafNode(LeafNodeData data)
		{
			Leaf leaf;
			if (string.IsNullOrEmpty(data.typeName))
			{
				leaf = new Leaf(0);
				Debug.LogError("INVALID LEAF DATA must have a type");
			}
			else
			{
				Type type = Type.GetType(data.typeName);
				if (type == null)
				{
					leaf = new Leaf(0);
					Debug.LogError("INVALID LEAF DATA, type " + data.typeName + " not found");
				}
				else
				{
					ConstructorInfo constructorInfo = null;
					foreach (ConstructorInfo constructorInfo2 in type.GetConstructors())
					{
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

		// Token: 0x06001195 RID: 4501 RVA: 0x000658DC File Offset: 0x00063CDC
		private FieldInfo getNestedDataField(Type theType)
		{
			if (theType != null)
			{
				Type type = null;
				foreach (Type type2 in theType.GetNestedTypes())
				{
					if (type2.IsSerializable)
					{
						type = type2;
						break;
					}
				}
				if (type != null)
				{
					foreach (FieldInfo fieldInfo in theType.GetFields())
					{
						if (fieldInfo.FieldType == type)
						{
							return fieldInfo;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00065974 File Offset: 0x00063D74
		private void addNestedData(Type theType, IDataNode data, object targetObj)
		{
			FieldInfo nestedDataField = this.getNestedDataField(theType);
			if (nestedDataField != null)
			{
				object value = nestedDataField.GetValue(targetObj);
				Type fieldType = nestedDataField.FieldType;
				foreach (KeyValuePair<string, float> keyValuePair in data.FloatData)
				{
					string key = keyValuePair.Key;
					float value2 = keyValuePair.Value;
					FieldInfo field = fieldType.GetField(key);
					if (field != null)
					{
						field.SetValue(value, value2);
					}
				}
				foreach (KeyValuePair<string, int> keyValuePair2 in data.IntData)
				{
					string key2 = keyValuePair2.Key;
					int value3 = keyValuePair2.Value;
					FieldInfo field2 = fieldType.GetField(key2);
					if (field2 != null)
					{
						field2.SetValue(value, value3);
					}
				}
				foreach (KeyValuePair<string, Fixed> keyValuePair3 in data.FixedData)
				{
					string key3 = keyValuePair3.Key;
					Fixed value4 = keyValuePair3.Value;
					FieldInfo field3 = fieldType.GetField(key3);
					if (field3 != null)
					{
						field3.SetValue(value, value4);
					}
				}
				foreach (KeyValuePair<string, bool> keyValuePair4 in data.BoolData)
				{
					string key4 = keyValuePair4.Key;
					bool value5 = keyValuePair4.Value;
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
