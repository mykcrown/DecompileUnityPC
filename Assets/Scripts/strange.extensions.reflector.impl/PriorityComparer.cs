// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Reflection;

namespace strange.extensions.reflector.impl
{
	internal class PriorityComparer : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			int priority = this.getPriority(x as MethodInfo);
			int priority2 = this.getPriority(y as MethodInfo);
			return (priority >= priority2) ? 1 : (-1);
		}

		private int getPriority(MethodInfo methodInfo)
		{
			PostConstruct postConstruct = methodInfo.GetCustomAttributes(true)[0] as PostConstruct;
			return postConstruct.priority;
		}
	}
}
