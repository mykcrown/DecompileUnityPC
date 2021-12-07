// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AI
{
	public class LeafNodeData : ScriptableObject, IDataNode
	{
		public string typeName;

		public int frameDuration;

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
