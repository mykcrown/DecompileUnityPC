// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.framework.impl
{
	public class Binding : IBinding
	{
		public Binder.BindingResolver resolver;

		protected ISemiBinding _key;

		protected ISemiBinding _value;

		protected ISemiBinding _name;

		protected bool _isWeak;

		public object key
		{
			get
			{
				return this._key.value;
			}
		}

		public object value
		{
			get
			{
				return this._value.value;
			}
		}

		public object name
		{
			get
			{
				return (this._name.value != null) ? this._name.value : BindingConst.NULLOID;
			}
		}

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

		public bool isWeak
		{
			get
			{
				return this._isWeak;
			}
		}

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

		public Binding() : this(null)
		{
		}

		public virtual IBinding Bind<T>()
		{
			return this.Bind(typeof(T));
		}

		public virtual IBinding Bind(object o)
		{
			this._key.Add(o);
			return this;
		}

		public virtual IBinding To<T>()
		{
			return this.To(typeof(T));
		}

		public virtual IBinding To(object o)
		{
			this._value.Add(o);
			if (this.resolver != null)
			{
				this.resolver(this);
			}
			return this;
		}

		public virtual IBinding ToName<T>()
		{
			return this.ToName(typeof(T));
		}

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

		public virtual IBinding Named<T>()
		{
			return this.Named(typeof(T));
		}

		public virtual IBinding Named(object o)
		{
			return (this._name.value != o) ? null : this;
		}

		public virtual void RemoveKey(object o)
		{
			this._key.Remove(o);
		}

		public virtual void RemoveValue(object o)
		{
			this._value.Remove(o);
		}

		public virtual void RemoveName(object o)
		{
			this._name.Remove(o);
		}

		public virtual IBinding Weak()
		{
			this._isWeak = true;
			return this;
		}
	}
}
