// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.framework.impl
{
	public class SemiBinding : ISemiBinding, IManagedList
	{
		protected object[] objectValue;

		public Enum constraint
		{
			get;
			set;
		}

		public bool uniqueValues
		{
			get;
			set;
		}

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

		public SemiBinding()
		{
			this.constraint = BindingConstraintType.ONE;
			this.uniqueValues = true;
		}

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

		public IManagedList Add(object[] list)
		{
			for (int i = 0; i < list.Length; i++)
			{
				object o = list[i];
				this.Add(o);
			}
			return this;
		}

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

		public IManagedList Remove(object[] list)
		{
			for (int i = 0; i < list.Length; i++)
			{
				object o = list[i];
				this.Remove(o);
			}
			return this;
		}

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
	}
}
