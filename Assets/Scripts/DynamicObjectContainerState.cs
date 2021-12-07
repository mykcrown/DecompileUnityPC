// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class DynamicObjectContainerState : RollbackStateTyped<DynamicObjectContainerState>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public List<ITickable> tickables = new List<ITickable>(512);

	public override void CopyTo(DynamicObjectContainerState target)
	{
		base.copyList<ITickable>(this.tickables, target.tickables);
	}

	public override object Clone()
	{
		DynamicObjectContainerState dynamicObjectContainerState = new DynamicObjectContainerState();
		this.CopyTo(dynamicObjectContainerState);
		return dynamicObjectContainerState;
	}

	public override void Clear()
	{
		base.Clear();
		this.tickables.Clear();
	}
}
