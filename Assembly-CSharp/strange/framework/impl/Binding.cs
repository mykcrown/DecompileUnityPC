using System;
using strange.framework.api;

namespace strange.framework.impl
{
	// Token: 0x02000291 RID: 657
	public class Binding : IBinding
	{
		// Token: 0x06000DAE RID: 3502 RVA: 0x00052280 File Offset: 0x00050680
		public Binding(Binder.BindingResolver resolver)
		{
			this.resolver = resolver;
			this._key = new SemiBinding();
			this._value = new SemiBinding();
			this._name = new SemiBinding();
			this.keyConstraint = BindingConstraintType.ONE;
			this.nameConstraint = BindingConstraintType.ONE;
			this.valueConstraint = BindingConstraintType.MANY;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x000522DF File Offset: 0x000506DF
		public Binding() : this(null)
		{
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x000522E8 File Offset: 0x000506E8
		public object key
		{
			get
			{
				return this._key.value;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x000522F5 File Offset: 0x000506F5
		public object value
		{
			get
			{
				return this._value.value;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x00052302 File Offset: 0x00050702
		public object name
		{
			get
			{
				return (this._name.value != null) ? this._name.value : BindingConst.NULLOID;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0005232A File Offset: 0x0005072A
		// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x00052337 File Offset: 0x00050737
		public Enum keyConstraint
		{
			get
			{
				return this._key.constraint;
			}
			set
			{
				this._key.constraint = value;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00052345 File Offset: 0x00050745
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x00052352 File Offset: 0x00050752
		public Enum valueConstraint
		{
			get
			{
				return this._value.constraint;
			}
			set
			{
				this._value.constraint = value;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00052360 File Offset: 0x00050760
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x0005236D File Offset: 0x0005076D
		public Enum nameConstraint
		{
			get
			{
				return this._name.constraint;
			}
			set
			{
				this._name.constraint = value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0005237B File Offset: 0x0005077B
		public bool isWeak
		{
			get
			{
				return this._isWeak;
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00052383 File Offset: 0x00050783
		public virtual IBinding Bind<T>()
		{
			return this.Bind(typeof(T));
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00052395 File Offset: 0x00050795
		public virtual IBinding Bind(object o)
		{
			this._key.Add(o);
			return this;
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x000523A5 File Offset: 0x000507A5
		public virtual IBinding To<T>()
		{
			return this.To(typeof(T));
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x000523B7 File Offset: 0x000507B7
		public virtual IBinding To(object o)
		{
			this._value.Add(o);
			if (this.resolver != null)
			{
				this.resolver(this);
			}
			return this;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000523DE File Offset: 0x000507DE
		public virtual IBinding ToName<T>()
		{
			return this.ToName(typeof(T));
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x000523F0 File Offset: 0x000507F0
		public virtual IBinding ToName(object o)
		{
			object value = (o != null) ? o : BindingConst.NULLOID;
			this._name.Add(value);
			if (this.resolver != null)
			{
				this.resolver(this);
			}
			return this;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00052435 File Offset: 0x00050835
		public virtual IBinding Named<T>()
		{
			return this.Named(typeof(T));
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00052447 File Offset: 0x00050847
		public virtual IBinding Named(object o)
		{
			return (this._name.value != o) ? null : this;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00052461 File Offset: 0x00050861
		public virtual void RemoveKey(object o)
		{
			this._key.Remove(o);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00052470 File Offset: 0x00050870
		public virtual void RemoveValue(object o)
		{
			this._value.Remove(o);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0005247F File Offset: 0x0005087F
		public virtual void RemoveName(object o)
		{
			this._name.Remove(o);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0005248E File Offset: 0x0005088E
		public virtual IBinding Weak()
		{
			this._isWeak = true;
			return this;
		}

		// Token: 0x040007EB RID: 2027
		public Binder.BindingResolver resolver;

		// Token: 0x040007EC RID: 2028
		protected ISemiBinding _key;

		// Token: 0x040007ED RID: 2029
		protected ISemiBinding _value;

		// Token: 0x040007EE RID: 2030
		protected ISemiBinding _name;

		// Token: 0x040007EF RID: 2031
		protected bool _isWeak;
	}
}
