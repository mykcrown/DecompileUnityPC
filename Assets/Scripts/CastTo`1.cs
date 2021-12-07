// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Linq.Expressions;

public static class CastTo<T>
{
	private static class Cache<S>
	{
		public static readonly Func<S, T> caster = CastTo<T>.Cache<S>.Get();

		private static Func<S, T> Get()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(S), "t");
			UnaryExpression body = Expression.ConvertChecked(parameterExpression, typeof(T));
			return Expression.Lambda<Func<S, T>>(body, new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}
	}

	public static T From<S>(S s)
	{
		return CastTo<T>.Cache<S>.caster(s);
	}
}
