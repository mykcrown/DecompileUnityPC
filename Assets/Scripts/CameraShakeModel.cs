// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class CameraShakeModel : RollbackStateTyped<CameraShakeModel>
{
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public int frameCount;

	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public Fixed amplitude;

	public override void CopyTo(CameraShakeModel target)
	{
		target.frameCount = this.frameCount;
		target.amplitude = this.amplitude;
	}

	public override object Clone()
	{
		CameraShakeModel cameraShakeModel = new CameraShakeModel();
		this.CopyTo(cameraShakeModel);
		return cameraShakeModel;
	}
}
