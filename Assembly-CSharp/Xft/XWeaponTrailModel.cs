using System;
using System.Collections.Generic;

namespace Xft
{
	// Token: 0x0200001E RID: 30
	[Serializable]
	public class XWeaponTrailModel : RollbackStateTyped<XWeaponTrailModel>
	{
		// Token: 0x06000114 RID: 276 RVA: 0x0000B4A4 File Offset: 0x000098A4
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

		// Token: 0x06000115 RID: 277 RVA: 0x0000B51C File Offset: 0x0000991C
		public override object Clone()
		{
			XWeaponTrailModel xweaponTrailModel = new XWeaponTrailModel();
			this.CopyTo(xweaponTrailModel);
			return xweaponTrailModel;
		}

		// Token: 0x040000E8 RID: 232
		public bool isExpired;

		// Token: 0x040000E9 RID: 233
		public int fadeElapsedFrames;

		// Token: 0x040000EA RID: 234
		public int fadeFrames;

		// Token: 0x040000EB RID: 235
		public bool fading;

		// Token: 0x040000EC RID: 236
		public bool inited;

		// Token: 0x040000ED RID: 237
		[IgnoreFloatValidation]
		public float fadeT;

		// Token: 0x040000EE RID: 238
		[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
		[IgnoreCopyValidation]
		[NonSerialized]
		public List<XWeaponTrail.Element> snapShotList;
	}
}
