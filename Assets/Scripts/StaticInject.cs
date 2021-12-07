// Decompile from assembly: Assembly-CSharp.dll

using System;

public static class StaticInject
{
	public static IDependencyInjection staticInjector
	{
		private get;
		set;
	}

	public static bool readyToInject
	{
		get
		{
			return StaticInject.staticInjector != null;
		}
	}

	public static void Inject(object target)
	{
		if (StaticInject.readyToInject && target != null)
		{
			StaticInject.staticInjector.Inject(target);
		}
	}
}
