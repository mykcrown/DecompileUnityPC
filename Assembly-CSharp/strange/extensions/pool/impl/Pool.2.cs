using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.pool.api;
using strange.framework.api;

namespace strange.extensions.pool.impl
{
	// Token: 0x0200026A RID: 618
	public class Pool : IPool, IPoolable, IManagedList
	{
		// Token: 0x06000C5D RID: 3165 RVA: 0x000559E4 File Offset: 0x00053DE4
		public Pool()
		{
			this.size = 0;
			this.constraint = BindingConstraintType.POOL;
			this.uniqueValues = true;
			this.overflowBehavior = PoolOverflowBehavior.EXCEPTION;
			this.inflationType = PoolInflationType.DOUBLE;
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x00055A35 File Offset: 0x00053E35
		// (set) Token: 0x06000C5F RID: 3167 RVA: 0x00055A3D File Offset: 0x00053E3D
		[Inject]
		public IInstanceProvider instanceProvider { get; set; }

		// Token: 0x06000C60 RID: 3168 RVA: 0x00055A48 File Offset: 0x00053E48
		public virtual IManagedList Add(object value)
		{
			this.failIf(value.GetType() != this.poolType, "Pool Type mismatch. Pools must consist of a common concrete type.\n\t\tPool type: " + this.poolType.ToString() + "\n\t\tMismatch type: " + value.GetType().ToString(), PoolExceptionType.TYPE_MISMATCH);
			this._instanceCount++;
			this.instancesAvailable.Push(value);
			return this;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00055AB0 File Offset: 0x00053EB0
		public virtual IManagedList Add(object[] list)
		{
			foreach (object value in list)
			{
				this.Add(value);
			}
			return this;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00055AE0 File Offset: 0x00053EE0
		public virtual IManagedList Remove(object value)
		{
			this._instanceCount--;
			this.removeInstance(value);
			return this;
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00055AF8 File Offset: 0x00053EF8
		public virtual IManagedList Remove(object[] list)
		{
			foreach (object value in list)
			{
				this.Remove(value);
			}
			return this;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x00055B28 File Offset: 0x00053F28
		public virtual object value
		{
			get
			{
				return this.GetInstance();
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00055B30 File Offset: 0x00053F30
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x00055B38 File Offset: 0x00053F38
		public virtual bool uniqueValues { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00055B41 File Offset: 0x00053F41
		// (set) Token: 0x06000C68 RID: 3176 RVA: 0x00055B49 File Offset: 0x00053F49
		public virtual Enum constraint { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00055B52 File Offset: 0x00053F52
		// (set) Token: 0x06000C6A RID: 3178 RVA: 0x00055B5A File Offset: 0x00053F5A
		public Type poolType { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00055B63 File Offset: 0x00053F63
		public int instanceCount
		{
			get
			{
				return this._instanceCount;
			}
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00055B6C File Offset: 0x00053F6C
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

		// Token: 0x06000C6D RID: 3181 RVA: 0x00055C9C File Offset: 0x0005409C
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

		// Token: 0x06000C6E RID: 3182 RVA: 0x00055CE9 File Offset: 0x000540E9
		public virtual void Clean()
		{
			this.instancesAvailable.Clear();
			this.instancesInUse = new HashSet<object>();
			this._instanceCount = 0;
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00055D08 File Offset: 0x00054108
		public virtual int available
		{
			get
			{
				return this.instancesAvailable.Count;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00055D15 File Offset: 0x00054115
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x00055D1D File Offset: 0x0005411D
		public virtual int size { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00055D26 File Offset: 0x00054126
		// (set) Token: 0x06000C73 RID: 3187 RVA: 0x00055D2E File Offset: 0x0005412E
		public virtual PoolOverflowBehavior overflowBehavior { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x00055D37 File Offset: 0x00054137
		// (set) Token: 0x06000C75 RID: 3189 RVA: 0x00055D3F File Offset: 0x0005413F
		public virtual PoolInflationType inflationType { get; set; }

		// Token: 0x06000C76 RID: 3190 RVA: 0x00055D48 File Offset: 0x00054148
		public void Restore()
		{
			this.Clean();
			this.size = 0;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00055D57 File Offset: 0x00054157
		public void Retain()
		{
			this.retain = true;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00055D60 File Offset: 0x00054160
		public void Release()
		{
			this.retain = false;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00055D69 File Offset: 0x00054169
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x00055D71 File Offset: 0x00054171
		public bool retain { get; set; }

		// Token: 0x06000C7B RID: 3195 RVA: 0x00055D7C File Offset: 0x0005417C
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

		// Token: 0x06000C7C RID: 3196 RVA: 0x00055DF5 File Offset: 0x000541F5
		protected void failIf(bool condition, string message, PoolExceptionType type)
		{
			if (condition)
			{
				throw new PoolException(message, type);
			}
		}

		// Token: 0x0400079F RID: 1951
		protected Stack instancesAvailable = new Stack();

		// Token: 0x040007A0 RID: 1952
		protected HashSet<object> instancesInUse = new HashSet<object>();

		// Token: 0x040007A1 RID: 1953
		protected int _instanceCount;
	}
}
