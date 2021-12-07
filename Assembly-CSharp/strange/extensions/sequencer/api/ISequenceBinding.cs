using System;
using strange.extensions.command.api;
using strange.framework.api;

namespace strange.extensions.sequencer.api
{
	// Token: 0x02000273 RID: 627
	public interface ISequenceBinding : ICommandBinding, IBinding
	{
		// Token: 0x06000CC9 RID: 3273
		ISequenceBinding Once();

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000CCA RID: 3274
		// (set) Token: 0x06000CCB RID: 3275
		bool isOneOff { get; set; }

		// Token: 0x06000CCC RID: 3276
		ISequenceBinding Bind<T>();

		// Token: 0x06000CCD RID: 3277
		ISequenceBinding Bind(object key);

		// Token: 0x06000CCE RID: 3278
		ISequenceBinding To<T>();

		// Token: 0x06000CCF RID: 3279
		ISequenceBinding To(object o);

		// Token: 0x06000CD0 RID: 3280
		ISequenceBinding ToName<T>();

		// Token: 0x06000CD1 RID: 3281
		ISequenceBinding ToName(object o);

		// Token: 0x06000CD2 RID: 3282
		ISequenceBinding Named<T>();

		// Token: 0x06000CD3 RID: 3283
		ISequenceBinding Named(object o);
	}
}
