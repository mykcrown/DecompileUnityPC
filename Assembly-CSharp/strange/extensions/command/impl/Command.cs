using System;
using strange.extensions.command.api;
using strange.extensions.injector.api;
using strange.extensions.pool.api;

namespace strange.extensions.command.impl
{
	// Token: 0x02000217 RID: 535
	public class Command : ICommand, IPoolable
	{
		// Token: 0x06000A32 RID: 2610 RVA: 0x0005141E File Offset: 0x0004F81E
		public Command()
		{
			this.IsClean = false;
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0005142D File Offset: 0x0004F82D
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x00051435 File Offset: 0x0004F835
		[Inject]
		public ICommandBinder commandBinder { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0005143E File Offset: 0x0004F83E
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x00051446 File Offset: 0x0004F846
		[Inject]
		public IInjectionBinder injectionBinder { get; set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0005144F File Offset: 0x0004F84F
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x00051457 File Offset: 0x0004F857
		public object data { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00051460 File Offset: 0x0004F860
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x00051468 File Offset: 0x0004F868
		public bool cancelled { get; set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00051471 File Offset: 0x0004F871
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x00051479 File Offset: 0x0004F879
		public bool IsClean { get; set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00051482 File Offset: 0x0004F882
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0005148A File Offset: 0x0004F88A
		public int sequenceId { get; set; }

		// Token: 0x06000A3F RID: 2623 RVA: 0x00051493 File Offset: 0x0004F893
		public virtual void Execute()
		{
			throw new CommandException("You must override the Execute method in every Command", CommandExceptionType.EXECUTE_OVERRIDE);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x000514A0 File Offset: 0x0004F8A0
		public virtual void Retain()
		{
			this.retain = true;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x000514A9 File Offset: 0x0004F8A9
		public virtual void Release()
		{
			this.retain = false;
			if (this.commandBinder != null)
			{
				this.commandBinder.ReleaseCommand(this);
			}
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x000514C9 File Offset: 0x0004F8C9
		public virtual void Restore()
		{
			this.injectionBinder.injector.Uninject(this);
			this.IsClean = true;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x000514E3 File Offset: 0x0004F8E3
		public virtual void Fail()
		{
			if (this.commandBinder != null)
			{
				this.commandBinder.Stop(this);
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x000514FC File Offset: 0x0004F8FC
		public void Cancel()
		{
			this.cancelled = true;
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x00051505 File Offset: 0x0004F905
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x0005150D File Offset: 0x0004F90D
		public bool retain { get; set; }
	}
}
