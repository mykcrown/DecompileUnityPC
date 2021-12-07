// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace Xft
{
	[Serializable]
	public class XWeaponTrailModel : RollbackStateTyped<XWeaponTrailModel>
	{
		public bool isExpired;

		public int fadeElapsedFrames;

		public int fadeFrames;

		public bool fading;

		public bool inited;

		[IgnoreFloatValidation]
		public float fadeT;

		[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
		[NonSerialized]
		public List<XWeaponTrail.Element> snapShotList;

		public override void CopyTo(XWeaponTrailModel target)
		{
			target.isExpired = this.isExpired;
			target.fadeElapsedFrames = this.fadeElapsedFrames;
			target.fadeFrames = this.fadeFrames;
			target.fading = this.fading;
			target.fadeT = this.fadeT;
			target.inited = this.inited;
			target.snapShotList = null;
			if (this.snapShotList != null)
			{
				target.snapShotList = new List<XWeaponTrail.Element>(this.snapShotList);
			}
		}

		public override object Clone()
		{
			XWeaponTrailModel xWeaponTrailModel = new XWeaponTrailModel();
			this.CopyTo(xWeaponTrailModel);
			return xWeaponTrailModel;
		}
	}
}
