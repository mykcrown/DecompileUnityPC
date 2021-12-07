// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AI
{
	[Serializable]
	public class CompositeNodeData : ScriptableObject, IDataNode
	{
		[Serializable]
		public class ChildrenFileData
		{
			public ScriptableObjectFile file;

			public int shuffleWeight = 100;
		}

		public CompositeNodeData.ChildrenFileData[] childrenFileDatas = new CompositeNodeData.ChildrenFileData[0];

		public string typeName;

		public Composite.Method method;

		public bool shuffle;

		public FloatDataDictionary floatData = new FloatDataDictionary();

		public IntDataDictionary intData = new IntDataDictionary();

		public FixedDataDictionary fixedData = new FixedDataDictionary();

		public BoolDataDictionary boolData = new BoolDataDictionary();

		public FloatDataDictionary FloatData
		{
			get
			{
				return this.floatData;
			}
		}

		public IntDataDictionary IntData
		{
			get
			{
				return this.intData;
			}
		}

		public FixedDataDictionary FixedData
		{
			get
			{
				return this.fixedData;
			}
		}

		public BoolDataDictionary BoolData
		{
			get
			{
				return this.boolData;
			}
		}
	}
}
