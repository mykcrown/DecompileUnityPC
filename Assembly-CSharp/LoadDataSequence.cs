using System;
using UnityEngine;

// Token: 0x020006C3 RID: 1731
public class LoadDataSequence : ILoadDataSequence
{
	// Token: 0x17000ABA RID: 2746
	// (get) Token: 0x06002B79 RID: 11129 RVA: 0x000E2E9B File Offset: 0x000E129B
	// (set) Token: 0x06002B7A RID: 11130 RVA: 0x000E2EA3 File Offset: 0x000E12A3
	[Inject]
	public IDependencyInjection dependencyInjection { get; set; }

	// Token: 0x06002B7B RID: 11131 RVA: 0x000E2EAC File Offset: 0x000E12AC
	public void Load(DataRequirement[] list, Action<LoadSequenceResults> callback)
	{
		LoadSequenceResults sequenceResults = new LoadSequenceResults();
		this.recursiveLoad(list, callback, sequenceResults, 0);
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x000E2ECC File Offset: 0x000E12CC
	private void recursiveLoad(DataRequirement[] list, Action<LoadSequenceResults> callback, LoadSequenceResults sequenceResults, int i = 0)
	{
		if (i >= list.Length)
		{
			sequenceResults.status = DataLoadStatus.SUCCESS;
			callback(sequenceResults);
		}
		else
		{
			DataRequirement requirement = list[i];
			Type theType = requirement.theType;
			IDataDependency dataDependency = (IDataDependency)this.dependencyInjection.GetInstance(theType);
			if (dataDependency == null)
			{
				throw new UnityException("Please define the type! " + theType);
			}
			dataDependency.Load(delegate(DataLoadResult result)
			{
				if (result.status != DataLoadStatus.SUCCESS && requirement.level == DataRequirementLevel.REQUIRED)
				{
					sequenceResults.status = DataLoadStatus.FAILURE;
					callback(sequenceResults);
				}
				else
				{
					sequenceResults.AddData(theType, result.data);
					this.recursiveLoad(list, callback, sequenceResults, i + 1);
				}
			});
		}
	}
}
