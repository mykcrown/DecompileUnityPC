// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.pool.api;
using strange.framework.api;
using System;
using System.Collections;
using System.Collections.Generic;

namespace strange.extensions.pool.impl
{
	public class Pool : IPool, IPoolable, IManagedList
	{
		protected Stack instancesAvailable = new Stack();

		protected HashSet<object> instancesInUse = new HashSet<object>();

		protected int _instanceCount;

		[Inject]
		public IInstanceProvider instanceProvider
		{
			get;
			set;
		}

		public virtual object value
		{
			get
			{
				return this.GetInstance();
			}
		}

		public virtual bool uniqueValues
		{
			get;
			set;
		}

		public virtual Enum constraint
		{
			get;
			set;
		}

		public Type poolType
		{
			get;
			set;
		}

		public int instanceCount
		{
			get
			{
				return this._instanceCount;
			}
		}

		public virtual int available
		{
			get
			{
				return this.instancesAvailable.Count;
			}
		}

		public virtual int size
		{
			get;
			set;
		}

		public virtual PoolOverflowBehavior overflowBehavior
		{
			get;
			set;
		}

		public virtual PoolInflationType inflationType
		{
			get;
			set;
		}

		public bool retain
		{
			get;
			set;
		}

		public Pool()
		{
			this.size = 0;
			this.constraint = BindingConstraintType.POOL;
			this.uniqueValues = true;
			this.overflowBehavior = PoolOverflowBehavior.EXCEPTION;
			this.inflationType = PoolInflationType.DOUBLE;
		}

		public virtual IManagedList Add(object value)
		{
			this.failIf(value.GetType() != this.poolType, "Pool Type mismatch. Pools must consist of a common concrete type.\n\t\tPool type: " + this.poolType.ToString() + "\n\t\tMismatch type: " + value.GetType().ToString(), PoolExceptionType.TYPE_MISMATCH);
			this._instanceCount++;
			this.instancesAvailable.Push(value);
			return this;
		}

		public virtual IManagedList Add(object[] list)
		{
			for (int i = 0; i < list.Length; i++)
			{
				object value = list[i];
				this.Add(value);
			}
			return this;
		}

		public virtual IManagedList Remove(object value)
		{
			this._instanceCount--;
			this.removeInstance(value);
			return this;
		}

		public virtual IManagedList Remove(object[] list)
		{
			for (int i = 0; i < list.Length; i++)
			{
				object value = list[i];
				this.Remove(value);
			}
			return this;
		}

		public virtual object GetInstance()
		{
			if (this.instancesAvailable.Count > 0)
			{
				object obj = this.instancesAvailable.Pop();
				this.instancesInUse.Add(obj);
				return obj;
			}
			int num;
			if (this.size > 0)
			{
				if (this.instanceCount != 0)
				{
					this.failIf(this.overflowBehavior == PoolOverflowBehavior.EXCEPTION, "A pool has overflowed its limit.\n\t\tPool type: " + this.poolType, PoolExceptionType.OVERFLOW);
					if (this.overflowBehavior == PoolOverflowBehavior.WARNING)
					{
						Console.WriteLine("WARNING: A pool has overflowed its limit.\n\t\tPool type: " + this.poolType, PoolExceptionType.OVERFLOW);
					}
					return null;
				}
				num = this.size;
			}
			else if (this.instanceCount == 0 || this.inflationType == PoolInflationType.INCREMENT)
			{
				num = 1;
			}
			else
			{
				num = this.instanceCount;
			}
			if (num > 0)
			{
				this.failIf(this.instanceProvider == null, "A Pool of type: " + this.poolType + " has no instance provider.", PoolExceptionType.NO_INSTANCE_PROVIDER);
				for (int i = 0; i < num; i++)
				{
					object instance = this.instanceProvider.GetInstance(this.poolType);
					this.Add(instance);
				}
				return this.GetInstance();
			}
			return null;
		}

		public virtual void ReturnInstance(object value)
		{
			if (this.instancesInUse.Contains(value))
			{
				if (value is IPoolable)
				{
					(value as IPoolable).Restore();
				}
				this.instancesInUse.Remove(value);
				this.instancesAvailable.Push(value);
			}
		}

		public virtual void Clean()
		{
			this.instancesAvailable.Clear();
			this.instancesInUse = new HashSet<object>();
			this._instanceCount = 0;
		}

		public void Restore()
		{
			this.Clean();
			this.size = 0;
		}

		public void Retain()
		{
			this.retain = true;
		}

		public void Release()
		{
			this.retain = false;
		}

		protected virtual void removeInstance(object value)
		{
			this.failIf(value.GetType() != this.poolType, "Attempt to remove a instance from a pool that is of the wrong Type:\n\t\tPool type: " + this.poolType.ToString() + "\n\t\tInstance type: " + value.GetType().ToString(), PoolExceptionType.TYPE_MISMATCH);
			if (this.instancesInUse.Contains(value))
			{
				this.instancesInUse.Remove(value);
			}
			else
			{
				this.instancesAvailable.Pop();
			}
		}

		protected void failIf(bool condition, string message, PoolExceptionType type)
		{
			if (condition)
			{
				throw new PoolException(message, type);
			}
		}
	}
}
