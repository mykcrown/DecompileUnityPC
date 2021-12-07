using System;
using strange.extensions.command.api;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.command.impl
{
	// Token: 0x02000219 RID: 537
	public class CommandBinding : Binding, ICommandBinding, IBinding
	{
		// Token: 0x06000A61 RID: 2657 RVA: 0x00052498 File Offset: 0x00050898
		public CommandBinding()
		{
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000524A0 File Offset: 0x000508A0
		public CommandBinding(Binder.BindingResolver resolver) : base(resolver)
		{
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x000524A9 File Offset: 0x000508A9
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x000524B1 File Offset: 0x000508B1
		public bool isOneOff { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x000524BA File Offset: 0x000508BA
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x000524C2 File Offset: 0x000508C2
		public bool isSequence { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x000524CB File Offset: 0x000508CB
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x000524D3 File Offset: 0x000508D3
		public bool isPooled { get; set; }

		// Token: 0x06000A69 RID: 2665 RVA: 0x000524DC File Offset: 0x000508DC
		public ICommandBinding Once()
		{
			this.isOneOff = true;
			return this;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x000524E6 File Offset: 0x000508E6
		public ICommandBinding InParallel()
		{
			this.isSequence = false;
			return this;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000524F0 File Offset: 0x000508F0
		public ICommandBinding InSequence()
		{
			this.isSequence = true;
			return this;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000524FA File Offset: 0x000508FA
		public ICommandBinding Pooled()
		{
			this.isPooled = true;
			this.resolver(this);
			return this;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00052510 File Offset: 0x00050910
		public new ICommandBinding Bind<T>()
		{
			return base.Bind<T>() as ICommandBinding;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0005251D File Offset: 0x0005091D
		public new ICommandBinding Bind(object key)
		{
			return base.Bind(key) as ICommandBinding;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0005252B File Offset: 0x0005092B
		public new ICommandBinding To<T>()
		{
			return base.To<T>() as ICommandBinding;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00052538 File Offset: 0x00050938
		public new ICommandBinding To(object o)
		{
			return base.To(o) as ICommandBinding;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00052546 File Offset: 0x00050946
		public new ICommandBinding ToName<T>()
		{
			return base.ToName<T>() as ICommandBinding;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00052553 File Offset: 0x00050953
		public new ICommandBinding ToName(object o)
		{
			return base.ToName(o) as ICommandBinding;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00052561 File Offset: 0x00050961
		public new ICommandBinding Named<T>()
		{
			return base.Named<T>() as ICommandBinding;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0005256E File Offset: 0x0005096E
		public new ICommandBinding Named(object o)
		{
			return base.Named(o) as ICommandBinding;
		}
	}
}
