// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IRollbackStateOwner
{
	bool ExportState(ref RollbackStateContainer container);

	bool LoadState(RollbackStateContainer container);
}
