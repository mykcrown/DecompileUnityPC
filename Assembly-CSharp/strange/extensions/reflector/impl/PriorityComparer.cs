using System;
using System.Collections;
using System.Reflection;

namespace strange.extensions.reflector.impl
{
	// Token: 0x02000271 RID: 625
	internal class PriorityComparer : IComparer
	{
		// Token: 0x06000CC3 RID: 3267 RVA: 0x0005639C File Offset: 0x0005479C
		int IComparer.Compare(object x, object y)
		{
			int priority = this.getPriority(x as MethodInfo);
			int priority2 = this.getPriority(y as MethodInfo);
			return (priority >= priority2) ? 1 : -1;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x000563D4 File Offset: 0x000547D4
		private int getPriority(MethodInfo methodInfo)
		{
			PostConstruct postConstruct = methodInfo.GetCustomAttributes(true)[0] as PostConstruct;
			return postConstruct.priority;
		}
	}
}
