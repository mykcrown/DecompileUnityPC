using System;

namespace InControl
{
	// Token: 0x02000051 RID: 81
	public class BindingListenOptions
	{
		// Token: 0x0600029D RID: 669 RVA: 0x00013A33 File Offset: 0x00011E33
		public bool CallOnBindingFound(PlayerAction playerAction, BindingSource bindingSource)
		{
			return this.OnBindingFound == null || this.OnBindingFound(playerAction, bindingSource);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00013A4F File Offset: 0x00011E4F
		public void CallOnBindingAdded(PlayerAction playerAction, BindingSource bindingSource)
		{
			if (this.OnBindingAdded != null)
			{
				this.OnBindingAdded(playerAction, bindingSource);
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00013A69 File Offset: 0x00011E69
		public void CallOnBindingRejected(PlayerAction playerAction, BindingSource bindingSource, BindingSourceRejectionType bindingSourceRejectionType)
		{
			if (this.OnBindingRejected != null)
			{
				this.OnBindingRejected(playerAction, bindingSource, bindingSourceRejectionType);
			}
		}

		// Token: 0x040001E5 RID: 485
		public bool IncludeControllers = true;

		// Token: 0x040001E6 RID: 486
		public bool IncludeUnknownControllers;

		// Token: 0x040001E7 RID: 487
		public bool IncludeNonStandardControls = true;

		// Token: 0x040001E8 RID: 488
		public bool IncludeMouseButtons;

		// Token: 0x040001E9 RID: 489
		public bool IncludeMouseScrollWheel;

		// Token: 0x040001EA RID: 490
		public bool IncludeKeys = true;

		// Token: 0x040001EB RID: 491
		public bool IncludeModifiersAsFirstClassKeys;

		// Token: 0x040001EC RID: 492
		public uint MaxAllowedBindings;

		// Token: 0x040001ED RID: 493
		public uint MaxAllowedBindingsPerType;

		// Token: 0x040001EE RID: 494
		public bool AllowDuplicateBindingsPerSet;

		// Token: 0x040001EF RID: 495
		public bool UnsetDuplicateBindingsOnSet;

		// Token: 0x040001F0 RID: 496
		public bool RejectRedundantBindings;

		// Token: 0x040001F1 RID: 497
		public BindingSource ReplaceBinding;

		// Token: 0x040001F2 RID: 498
		public Func<PlayerAction, BindingSource, bool> OnBindingFound;

		// Token: 0x040001F3 RID: 499
		public Action<PlayerAction, BindingSource> OnBindingAdded;

		// Token: 0x040001F4 RID: 500
		public Action<PlayerAction, BindingSource, BindingSourceRejectionType> OnBindingRejected;
	}
}
