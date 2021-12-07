using System;
using UnityEngine;

namespace AI
{
	// Token: 0x02000336 RID: 822
	public class LeafNodeData : ScriptableObject, IDataNode
	{
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x00065411 File Offset: 0x00063811
		public FloatDataDictionary FloatData
		{
			get
			{
				return this.floatData;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00065419 File Offset: 0x00063819
		public IntDataDictionary IntData
		{
			get
			{
				return this.intData;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x00065421 File Offset: 0x00063821
		public FixedDataDictionary FixedData
		{
			get
			{
				return this.fixedData;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x00065429 File Offset: 0x00063829
		public BoolDataDictionary BoolData
		{
			get
			{
				return this.boolData;
			}
		}

		// Token: 0x04000B22 RID: 2850
		public string typeName;

		// Token: 0x04000B23 RID: 2851
		public int frameDuration;

		// Token: 0x04000B24 RID: 2852
		public FloatDataDictionary floatData = new FloatDataDictionary();

		// Token: 0x04000B25 RID: 2853
		public IntDataDictionary intData = new IntDataDictionary();

		// Token: 0x04000B26 RID: 2854
		public FixedDataDictionary fixedData = new FixedDataDictionary();

		// Token: 0x04000B27 RID: 2855
		public BoolDataDictionary boolData = new BoolDataDictionary();
	}
}
