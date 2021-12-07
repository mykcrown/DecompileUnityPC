using System;

namespace Cloning
{
	// Token: 0x02000AAF RID: 2735
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class CloneAttribute : Attribute
	{
		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x0600504F RID: 20559 RVA: 0x0014E8E2 File Offset: 0x0014CCE2
		// (set) Token: 0x06005050 RID: 20560 RVA: 0x0014E8EA File Offset: 0x0014CCEA
		public CloneType CloneType
		{
			get
			{
				return this._clonetype;
			}
			set
			{
				this._clonetype = value;
			}
		}

		// Token: 0x040033B7 RID: 13239
		private CloneType _clonetype;
	}
}
