// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LoadDataSequence : ILoadDataSequence
{
	private sealed class _recursiveLoad_c__AnonStorey1
	{
		internal LoadSequenceResults sequenceResults;

		internal Action<LoadSequenceResults> callback;

		internal DataRequirement[] list;

		internal int i;

		internal LoadDataSequence _this;
	}

	private sealed class _recursiveLoad_c__AnonStorey0
	{
		internal DataRequirement requirement;

		internal Type theType;

		internal LoadDataSequence._recursiveLoad_c__AnonStorey1 __f__ref_1;

		internal void __m__0(DataLoadResult result)
		{
			if (result.status != DataLoadStatus.SUCCESS && this.requirement.level == DataRequirementLevel.REQUIRED)
			{
				this.__f__ref_1.sequenceResults.status = DataLoadStatus.FAILURE;
				this.__f__ref_1.callback(this.__f__ref_1.sequenceResults);
			}
			else
			{
				this.__f__ref_1.sequenceResults.AddData(this.theType, result.data);
				this.__f__ref_1._this.recursiveLoad(this.__f__ref_1.list, this.__f__ref_1.callback, this.__f__ref_1.sequenceResults, this.__f__ref_1.i + 1);
			}
		}
	}

	[Inject]
	public IDependencyInjection dependencyInjection
	{
		get;
		set;
	}

	public void Load(DataRequirement[] list, Action<LoadSequenceResults> callback)
	{
		LoadSequenceResults sequenceResults = new LoadSequenceResults();
		this.recursiveLoad(list, callback, sequenceResults, 0);
	}

	private void recursiveLoad(DataRequirement[] list, Action<LoadSequenceResults> callback, LoadSequenceResults sequenceResults, int i = 0)
	{
		LoadDataSequence._recursiveLoad_c__AnonStorey1 _recursiveLoad_c__AnonStorey = new LoadDataSequence._recursiveLoad_c__AnonStorey1();
		_recursiveLoad_c__AnonStorey.sequenceResults = sequenceResults;
		_recursiveLoad_c__AnonStorey.callback = callback;
		_recursiveLoad_c__AnonStorey.list = list;
		_recursiveLoad_c__AnonStorey.i = i;
		_recursiveLoad_c__AnonStorey._this = this;
		if (_recursiveLoad_c__AnonStorey.i >= _recursiveLoad_c__AnonStorey.list.Length)
		{
			_recursiveLoad_c__AnonStorey.sequenceResults.status = DataLoadStatus.SUCCESS;
			_recursiveLoad_c__AnonStorey.callback(_recursiveLoad_c__AnonStorey.sequenceResults);
		}
		else
		{
			LoadDataSequence._recursiveLoad_c__AnonStorey0 _recursiveLoad_c__AnonStorey2 = new LoadDataSequence._recursiveLoad_c__AnonStorey0();
			_recursiveLoad_c__AnonStorey2.__f__ref_1 = _recursiveLoad_c__AnonStorey;
			_recursiveLoad_c__AnonStorey2.requirement = _recursiveLoad_c__AnonStorey.list[_recursiveLoad_c__AnonStorey.i];
			_recursiveLoad_c__AnonStorey2.theType = _recursiveLoad_c__AnonStorey2.requirement.theType;
			IDataDependency dataDependency = (IDataDependency)this.dependencyInjection.GetInstance(_recursiveLoad_c__AnonStorey2.theType);
			if (dataDependency == null)
			{
				throw new UnityException("Please define the type! " + _recursiveLoad_c__AnonStorey2.theType);
			}
			dataDependency.Load(new Action<DataLoadResult>(_recursiveLoad_c__AnonStorey2.__m__0));
		}
	}
}
