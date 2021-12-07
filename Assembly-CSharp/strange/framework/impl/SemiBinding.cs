using System;
using strange.framework.api;

namespace strange.framework.impl
{
	// Token: 0x02000292 RID: 658
	public class SemiBinding : ISemiBinding, IManagedList
	{
		// Token: 0x06000DC6 RID: 3526 RVA: 0x0005757D File Offset: 0x0005597D
		public SemiBinding()
		{
			this.constraint = BindingConstraintType.ONE;
			this.uniqueValues = true;
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00057598 File Offset: 0x00055998
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x000575A0 File Offset: 0x000559A0
		public Enum constraint { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x000575A9 File Offset: 0x000559A9
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x000575B1 File Offset: 0x000559B1
		public bool uniqueValues { get; set; }

		// Token: 0x06000DCB RID: 3531 RVA: 0x000575BC File Offset: 0x000559BC
		public IManagedList Add(object o)
		{
			if (this.objectValue == null || (BindingConstraintType)this.constraint == BindingConstraintType.ONE)
			{
				this.objectValue = new object[1];
			}
			else
			{
				if (this.uniqueValues)
				{
					int num = this.objectValue.Length;
					for (int i = 0; i < num; i++)
					{
						object obj = this.objectValue[i];
						if (obj.Equals(o))
						{
							return this;
						}
					}
				}
				object[] array = this.objectValue;
				int num2 = array.Length;
				this.objectValue = new object[num2 + 1];
				array.CopyTo(this.objectValue, 0);
			}
			this.objectValue[this.objectValue.Length - 1] = o;
			return this;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00057670 File Offset: 0x00055A70
		public IManagedList Add(object[] list)
		{
			foreach (object o in list)
			{
				this.Add(o);
			}
			return this;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x000576A0 File Offset: 0x00055AA0
		public IManagedList Remove(object o)
		{
			if (o.Equals(this.objectValue) || this.objectValue == null)
			{
				this.objectValue = null;
				return this;
			}
			int num = this.objectValue.Length;
			for (int i = 0; i < num; i++)
			{
				object obj = this.objectValue[i];
				if (o.Equals(obj))
				{
					this.spliceValueAt(i);
					return this;
				}
			}
			return this;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0005770C File Offset: 0x00055B0C
		public IManagedList Remove(object[] list)
		{
			foreach (object o in list)
			{
				this.Remove(o);
			}
			return this;
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0005773C File Offset: 0x00055B3C
		public virtual object value
		{
			get
			{
				if (this.constraint.Equals(BindingConstraintType.ONE))
				{
					return (this.objectValue != null) ? this.objectValue[0] : null;
				}
				return this.objectValue;
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00057774 File Offset: 0x00055B74
		protected void spliceValueAt(int splicePos)
		{
			object[] array = new object[this.objectValue.Length - 1];
			int num = 0;
			int num2 = this.objectValue.Length;
			for (int i = 0; i < num2; i++)
			{
				if (i == splicePos)
				{
					num = -1;
				}
				else
				{
					array[i + num] = this.objectValue[i];
				}
			}
			this.objectValue = ((array.Length != 0) ? array : null);
		}

		// Token: 0x040007F0 RID: 2032
		protected object[] objectValue;
	}
}
