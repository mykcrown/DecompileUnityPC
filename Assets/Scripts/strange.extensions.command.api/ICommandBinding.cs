// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.command.api
{
	public interface ICommandBinding : IBinding
	{
		bool isOneOff
		{
			get;
			set;
		}

		bool isSequence
		{
			get;
			set;
		}

		bool isPooled
		{
			get;
			set;
		}

		ICommandBinding Once();

		ICommandBinding InParallel();

		ICommandBinding InSequence();

		ICommandBinding Pooled();

		ICommandBinding Bind<T>();

		ICommandBinding Bind(object key);

		ICommandBinding To<T>();

		ICommandBinding To(object o);

		ICommandBinding ToName<T>();

		ICommandBinding ToName(object o);

		ICommandBinding Named<T>();

		ICommandBinding Named(object o);
	}
}
