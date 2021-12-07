using System;
using UnityEngine;

namespace AI
{
	// Token: 0x0200032F RID: 815
	[Serializable]
	public class CompositeNodeData : ScriptableObject, IDataNode
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x000651C1 File Offset: 0x000635C1
		public FloatDataDictionary FloatData
		{
			get
			{
				return this.floatData;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x000651C9 File Offset: 0x000635C9
		public IntDataDictionary IntData
		{
			get
			{
				return this.intData;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x000651D1 File Offset: 0x000635D1
		public FixedDataDictionary FixedData
		{
			get
			{
				return this.fixedData;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x000651D9 File Offset: 0x000635D9
		public BoolDataDictionary BoolData
		{
			get
			{
				return this.boolData;
			}
		}

		// Token: 0x04000B18 RID: 2840
		public CompositeNodeData.ChildrenFileData[] childrenFileDatas = new CompositeNodeData.ChildrenFileData[0];

		// Token: 0x04000B19 RID: 2841
		public string typeName;

		// Token: 0x04000B1A RID: 2842
		public Composite.Method method;

		// Token: 0x04000B1B RID: 2843
		public bool shuffle;

		// Token: 0x04000B1C RID: 2844
		public FloatDataDictionary floatData = new FloatDataDictionary();

		// Token: 0x04000B1D RID: 2845
		public IntDataDictionary intData = new IntDataDictionary();

		// Token: 0x04000B1E RID: 2846
		public FixedDataDictionary fixedData = new FixedDataDictionary();

		// Token: 0x04000B1F RID: 2847
		public BoolDataDictionary boolData = new BoolDataDictionary();

		// Token: 0x02000330 RID: 816
		[Serializable]
		public class ChildrenFileData
		{
			// Token: 0x04000B20 RID: 2848
			public ScriptableObjectFile file;

			// Token: 0x04000B21 RID: 2849
			public int shuffleWeight = 100;
		}
	}
}
