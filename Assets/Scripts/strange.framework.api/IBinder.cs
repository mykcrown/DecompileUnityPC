// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.framework.api
{
	public interface IBinder
	{
		IBinding Bind<T>();

		IBinding Bind(object value);

		IBinding GetBinding<T>();

		IBinding GetBinding(object key);

		IBinding GetBinding<T>(object name);

		IBinding GetBinding(object key, object name);

		IBinding GetRawBinding();

		void Unbind<T>();

		void Unbind<T>(object name);

		void Unbind(object key);

		void Unbind(object key, object name);

		void Unbind(IBinding binding);

		void RemoveValue(IBinding binding, object value);

		void RemoveKey(IBinding binding, object value);

		void RemoveName(IBinding binding, object value);

		void OnRemove();

		void ResolveBinding(IBinding binding, object key);
	}
}
