using System;
using System.Collections.Generic;

// Token: 0x0200037A RID: 890
public class CameraPresetComparer : IEqualityComparer<CameraPreset>
{
	// Token: 0x0600130E RID: 4878 RVA: 0x0006E1AC File Offset: 0x0006C5AC
	public bool Equals(CameraPreset x, CameraPreset y)
	{
		return x == y;
	}

	// Token: 0x0600130F RID: 4879 RVA: 0x0006E1B2 File Offset: 0x0006C5B2
	public int GetHashCode(CameraPreset obj)
	{
		return (int)obj;
	}
}
